using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using System.Numerics;

namespace HeaderValidation
{

    [Config(typeof(CoreConfig))]
    public class HeaderValidation
    {
        private const int InnerLoopCount = 2000;

        [Params(
            "13", 
            "text/plain",
            "Content-Security-Policy",
            "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36",
            "ZGVmYXVsdC1zcmMgJ25vbmUnOyBiYXNlLXVyaSAnc2VsZic7IGJsb2NrLWFsbC1taXhlZC1jb250ZW50OyBjaGlsZC1zcmMgcmVuZGVyLmdpdGh1YnVzZXJjb250ZW50LmNvbTsgY29ubmVjdC1zcmMgJ3NlbGYnIHVwbG9hZHMuZ2l0aHViLmNvbSBzdGF0dXMuZ2l0aHViLmNvbSBhcGkuZ2l0aHViLmNvbSB3d3cuZ29vZ2xlLWFuYWx5dGljcy5jb20gZ2l0aHViLWNsb3VkLnMzLmFtYXpvbmF3cy5jb20gYXBpLmJyYWludHJlZWdhdGV3YXkuY29tIGNsaWVudC1hbmFseXRpY3MuYnJhaW50cmVlZ2F0ZXdheS5jb20gd3NzOi8vbGl2ZS5naXRodWIuY29tOyBmb250LXNyYyBhc3NldHMtY2RuLmdpdGh1Yi5jb207IGZvcm0tYWN0aW9uICdzZWxmJyBnaXRodWIuY29tIGdpc3QuZ2l0aHViLmNvbTsgZnJhbWUtYW5jZXN0b3JzICdub25lJzsgZnJhbWUtc3JjIHJlbmRlci5naXRodWJ1c2VyY29udGVudC5jb207IGltZy1zcmMgJ3NlbGYnIGRhdGE6IGFzc2V0cy1jZG4uZ2l0aHViLmNvbSBpZGVudGljb25zLmdpdGh1Yi5jb20gd3d3Lmdvb2dsZS1hbmFseXRpY3MuY29tIGNvbGxlY3Rvci5naXRodWJhcHAuY29tICouZ3JhdmF0YXIuY29tICoud3AuY29tIGNoZWNrb3V0LnBheXBhbC5jb20gKi5naXRodWJ1c2VyY29udGVudC5jb207IG1lZGlhLXNyYyAnbm9uZSc7IG9iamVjdC1zcmMgYXNzZXRzLWNkbi5naXRodWIuY29tOyBwbHVnaW4tdHlwZXMgYXBwbGljYXRpb24veC1zaG9ja3dhdmUtZmxhc2g7IHNjcmlwdC1zcmMgYXNzZXRzLWNkbi5naXRodWIuY29tOyBzdHlsZS1zcmMgJ3Vuc2FmZS1pbmxpbmUnIGFzc2V0cy1jZG4uZ2l0aHViLmNvbTsgcmVwb3J0LXVyaSBodHRwczovL2FwaS5naXRodWIuY29tL19wcml2YXRlL2Jyb3dzZXIvZXJyb3Jz"
        )]
        public string Value { get; set; }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void ValidateHeaderIter()
        {
            var value = Value;

            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                ValidateHeaderCharactersIter(value);
            }
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void ValidateHeaderVector()
        {
            var value = Value;

            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                ValidateHeaderCharactersVector(value);
            }
        }

        public static void ValidateHeaderCharactersIter(string headerCharacters)
        {
            if (headerCharacters != null)
            {
                foreach (var ch in headerCharacters)
                {
                    if (ch < 0x20 || ch > 0x7E)
                    {
                        ThrowInvalidHeaderCharacter(ch);
                    }
                }
            }
        }

        private static void ThrowInvalidHeaderCharacter(char ch)
        {
            throw new InvalidOperationException(string.Format("Invalid non-ASCII or control character in header: 0x{0:X4}", (ushort)ch));
        }

        public static unsafe void ValidateHeaderCharactersVector(string headerCharacters)
        {
            if (headerCharacters != null)
            {
                fixed (char* header = headerCharacters)
                {
                    // offsets and lengths are handled in byte* sizes
                    var pHeader = (byte*)header;
                    var offset = 0;
                    var length = headerCharacters.Length * 2;

                    if (Vector.IsHardwareAccelerated && Vector<byte>.Count <= length)
                    {
                        var vSub = Vector.AsVectorUInt16(new Vector<uint>(0x00200020u));
                        // 0x7e as highest ascii (don't include DEL)
                        // 0x20 as lowerest ascii (space)
                        var vTest = Vector.AsVectorUInt16(new Vector<uint>(0x007e007eu - 0x00200020u));

                        do
                        {
                            var stringVector = Unsafe.Read<Vector<ushort>>(pHeader + offset);
                            offset += Vector<byte>.Count;
                            if (Vector.GreaterThanAny(stringVector - vSub, vTest))
                            {
                                ThrowInvalidHeaderCharacter(pHeader + offset, Vector<byte>.Count);
                            }
                        } while (offset + Vector<byte>.Count <= length);
                    }

                    // Non-vector testing:
                    // Flag 0x7f     => Add 0x0001 to each char so DEL (0x7f) will set a high bit
                    // Flag < 0x20   => Sub 0x0020 from each char so high bit will be set in previous char bit
                    // Flag > 0x007f => All but highest bit picked up by 0x7f flagging, highest bit picked up by < 0x20 flagging
                    // Bitwise | or the above three together
                    // Bitwise & and each char with 0xff80; result should be 0 if all tests pass
                    if (offset + sizeof(ulong) <= length)
                    {
                        do
                        {
                            var stringUlong = (ulong*)(pHeader + offset);
                            offset += sizeof(ulong);
                            if ((((*stringUlong + 0x0001000100010001UL) | (*stringUlong - 0x0020002000200020UL)) & 0xff80ff80ff80ff80UL) != 0)
                            {
                                ThrowInvalidHeaderCharacter(pHeader + offset, sizeof(ulong));
                            }
                        } while (offset + sizeof(ulong) <= length);
                    }
                    if (offset + sizeof(uint) <= length)
                    {
                        var stringUint = (uint*)(pHeader + offset);
                        offset += sizeof(uint);
                        if ((((*stringUint + 0x00010001u) | (*stringUint - 0x00200020u)) & 0xff80ff80u) != 0)
                        {
                            ThrowInvalidHeaderCharacter(pHeader + offset, sizeof(uint));
                        }
                    }
                    if (offset + sizeof(ushort) <= length)
                    {
                        var stringUshort = (ushort*)(pHeader + offset);
                        offset += sizeof(ushort);
                        if ((((*stringUshort + 0x0001u) | (*stringUshort - 0x0020u)) & 0xff80u) != 0)
                        {
                            ThrowInvalidHeaderCharacter(pHeader + offset, sizeof(ushort));
                        }
                    }
                }
            }
        }

        private unsafe static Exception GetInvalidHeaderCharacterException(byte* end, int byteCount)
        {
            var start = end - byteCount;
            while (start < end)
            {
                var ch = *(char*)start;
                if (ch < 0x20 || ch >= 0x7f)
                {
                    return new InvalidOperationException(string.Format("Invalid non-ASCII or control character in header: 0x{0:X4}", (ushort)ch));
                }
                start += sizeof(char);
            }

            // Should never be reached, use different exception type so unit tests can pick it up
            return new ArgumentException("Invalid non-ASCII or control character in header");
        }

        private unsafe static void ThrowInvalidHeaderCharacter(byte* end, int byteCount)
        {
            throw GetInvalidHeaderCharacterException(end, byteCount);
        }
    }
}
