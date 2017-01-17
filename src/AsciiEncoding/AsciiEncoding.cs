#define BIT64

using System;
using BenchmarkDotNet.Attributes;
using System.Text;
using System.Diagnostics;

namespace AsciiEncoding
{

    internal static class EncodingForwarder
    {
        const int Shift16Shift24 = 256 * 256 * 256 + 256 * 256;
        const int Shift8Identity = 256 + 1;

        // Ascii fast-paths
        public unsafe static byte[] GetBytesAsciiFastPath(Encoding encoding, String s)
        {
            // Fast path for pure ASCII data for ASCII and UTF8 encoding
            Debug.Assert(encoding != null);
            if (s == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s, ExceptionResource.ArgumentNull_String);
            //Contract.EndContractBlock();

            int charCount = s.Length;

            byte[] bytes;
            if (charCount > 0)
            {
                fixed (char* input = s)
                    bytes = GetBytesAsciiFastPath(encoding, input, charCount);
            }
            else
            {
                bytes = Array.Empty<byte>();
            }

            return bytes;
        }

        internal unsafe static byte[] GetBytesAsciiFastPath(Encoding encoding, char[] chars, int index, int count)
        {
            // Fast path for pure ASCII data for ASCII and UTF8 encoding
            if (chars == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.chars, ExceptionResource.ArgumentNull_Array);
            if (index < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.charIndex, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            if (count < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.charCount, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            if (chars.Length - index < count)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.chars, ExceptionResource.ArgumentOutOfRange_IndexCountBuffer);
            //Contract.EndContractBlock();

            byte[] bytes;
            if (count > 0)
            {
                fixed (char* input = chars)
                    bytes = GetBytesAsciiFastPath(encoding, input + index, count);
            }
            else
            {
                bytes = Array.Empty<byte>();
            }

            return bytes;

        }

        public unsafe static int GetBytesAsciiFastPath(Encoding encoding, char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            // Fast path for pure ASCII data for ASCII and UTF8 encoding
            Debug.Assert(encoding != null);
            if (chars == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.chars, ExceptionResource.ArgumentNull_Array);
            if (bytes == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.bytes, ExceptionResource.ArgumentNull_Array);
            if (charIndex < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.charIndex, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            if (charCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.charCount, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            if (chars.Length - charIndex < charCount)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.chars, ExceptionResource.ArgumentOutOfRange_IndexCountBuffer);
            if (byteIndex < 0 || byteIndex > bytes.Length)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.byteIndex, ExceptionResource.ArgumentOutOfRange_Index);
            //Contract.EndContractBlock();

            // Note that byteCount is the # of bytes to decode, not the size of the array
            int byteCount = bytes.Length - byteIndex;
            int lengthEncoded;
            if (charCount > 0 && byteCount > 0)
            {
                fixed (char* input = chars)
                fixed (byte* output = &bytes[0])
                {
                    lengthEncoded = GetBytesAsciiFastPath(input + charIndex, output + byteIndex, Math.Min(charCount, byteCount));
                    if (lengthEncoded < byteCount)
                    {
                        // Not all ASCII, use encoding's GetBytes for remaining conversion
                        //lengthEncoded += encoding.GetBytesFallback(input + lengthEncoded, charCount - lengthEncoded, output + lengthEncoded, byteCount - lengthEncoded, null);
                        lengthEncoded += encoding.GetBytes(input + lengthEncoded, charCount - lengthEncoded, output + lengthEncoded, byteCount - lengthEncoded);
                    }
                }
            }
            else
            {
                // Nothing to encode
                lengthEncoded = 0;
            }

            return lengthEncoded;
        }

        public unsafe static int GetBytesAsciiFastPath(Encoding encoding, char* chars, int charCount, byte* bytes, int byteCount)
        {
            // Fast path for pure ASCII data for ASCII and UTF8 encoding
            Debug.Assert(encoding != null);
            if (bytes == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.bytes, ExceptionResource.ArgumentNull_Array);
            if (chars == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.chars, ExceptionResource.ArgumentNull_Array);
            if (charCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.charCount, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            if (byteCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.byteCount, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            //Contract.EndContractBlock();

            int lengthEncoded;
            if (charCount > 0 && byteCount > 0)
            {
                lengthEncoded = GetBytesAsciiFastPath(chars, bytes, Math.Min(charCount, byteCount));
                if (lengthEncoded < byteCount)
                {
                    // Not all ASCII, use encoding's GetBytes for remaining conversion
                    //lengthEncoded += encoding.GetBytesFallback(chars + lengthEncoded, charCount - lengthEncoded, bytes + lengthEncoded, byteCount - lengthEncoded, null);
                    lengthEncoded += encoding.GetBytes(chars + lengthEncoded, charCount - lengthEncoded, bytes + lengthEncoded, byteCount - lengthEncoded);
                }
            }
            else
            {
                // Nothing to encode
                lengthEncoded = 0;
            }

            return lengthEncoded;
        }

        //public unsafe static int GetBytesAsciiFastPath(Encoding encoding, char* chars, int charCount, byte* bytes, int byteCount, EncoderNLS encoder)
        //{
        //    // Fast path for pure ASCII data for ASCII and UTF8 encoding
        //    // Just need to Assert, this is called by internal EncoderNLS and parameters should already be checked
        //    Debug.Assert(encoding != null);
        //    Debug.Assert(bytes != null);
        //    Debug.Assert(chars != null);
        //    Debug.Assert(charCount >= 0);
        //    Debug.Assert(byteCount >= 0);

        //    int lengthEncoded;
        //    if ((encoder?.InternalHasFallbackBuffer ?? false) &&
        //        (encoder.FallbackBuffer.InternalGetNextChar()) != 0)
        //    {
        //        // Non-ASCII data already in Fallback buffer, so straight to encoder's version
        //        lengthEncoded = encoding.GetBytesFallback(chars, charCount, bytes, byteCount, encoder);
        //    }
        //    else if (charCount > 0 && byteCount > 0)
        //    {
        //        lengthEncoded = GetBytesAsciiFastPath(chars, bytes, Math.Min(charCount, byteCount));
        //        if (lengthEncoded < charCount)
        //        {
        //            // Not all ASCII, use encoding's GetBytes for remaining conversion
        //            lengthEncoded += encoding.GetBytesFallback(chars + lengthEncoded, charCount - lengthEncoded, bytes + lengthEncoded, byteCount - lengthEncoded, encoder);
        //        }
        //    }
        //    else
        //    {
        //        // Nothing to encode
        //        lengthEncoded = 0;
        //    }

        //    return lengthEncoded;
        //}

        public unsafe static int GetBytesAsciiFastPath(Encoding encoding, String s, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            // Fast path for pure ASCII data for ASCII and UTF8 encoding
            Debug.Assert(encoding != null);
            if (s == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s, ExceptionResource.ArgumentNull_String);
            if (bytes == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.bytes, ExceptionResource.ArgumentNull_Array);
            if (charIndex < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.charIndex, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            if (charCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.charCount, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            if (s.Length - charIndex < charCount)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.chars, ExceptionResource.ArgumentOutOfRange_IndexCountBuffer);
            if (byteIndex < 0 || byteIndex > bytes.Length)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.byteIndex, ExceptionResource.ArgumentOutOfRange_Index);
            //Contract.EndContractBlock();

            // Note that byteCount is the # of bytes to decode, not the size of the array
            int byteCount = bytes.Length - byteIndex;
            int lengthEncoded;
            if (charCount > 0 && byteCount > 0)
            {
                fixed (char* input = s)
                fixed (byte* output = &bytes[0])
                {
                    lengthEncoded = GetBytesAsciiFastPath(input + charIndex, output + byteIndex, Math.Min(charCount, byteCount));
                    if (lengthEncoded < byteCount)
                    {
                        // Not all ASCII, use encoding's GetBytes for remaining conversion
                        // lengthEncoded += encoding.GetBytesFallback(input + lengthEncoded, charCount - lengthEncoded, output + lengthEncoded, byteCount - lengthEncoded, null);
                        lengthEncoded += encoding.GetBytes(input + lengthEncoded, charCount - lengthEncoded, output + lengthEncoded, byteCount - lengthEncoded);
                    }
                }
            }
            else
            {
                // Nothing to encode
                lengthEncoded = 0;
            }

            return lengthEncoded;
        }

        private unsafe static byte[] GetBytesAsciiFastPath(Encoding encoding, char* input, int charCount)
        {
            // Fast path for pure ASCII data for ASCII and UTF8 encoding
            int asciiLength;
            int remaining = 0;
            // Assume string is all ASCII and size array for that
            byte[] bytes = new byte[charCount];

            fixed (byte* output = &bytes[0])
            {
                asciiLength = GetBytesAsciiFastPath(input, output, charCount);
                if (asciiLength < charCount)
                {
                    // Not all ASCII, get the byte count for the remaining encoded conversion
                    // remaining = encoding.GetByteCount(input + asciiLength, charCount - asciiLength, null);
                    remaining = encoding.GetByteCount(input + asciiLength, charCount - asciiLength);
                }
            }

            if (remaining > 0)
            {
                // Not all ASCII, fallback to slower path for remaining encoding
                var encoded = ResizeGetRemainingBytes(encoding, input, charCount, ref bytes, asciiLength, remaining);
                Debug.Assert(encoded == remaining);
            }

            return bytes;
        }

        internal unsafe static int ResizeGetRemainingBytes(Encoding encoding, char* chars, int charCount, ref byte[] bytes, int alreadyEncoded, int remaining)
        {
            // Resize the array to the correct size
            Array.Resize(ref bytes, alreadyEncoded + remaining);

            int encoded;
            fixed (byte* output = &bytes[0])
            {
                // Use encoding's GetBytes for remaining conversion
                //encoded = encoding.GetBytesFallback(chars + alreadyEncoded, charCount - alreadyEncoded, output + alreadyEncoded, remaining, null);
                encoded = encoding.GetBytes(chars + alreadyEncoded, charCount - alreadyEncoded, output + alreadyEncoded, remaining);
            }

            return encoded;
        }

        internal unsafe static int GetBytesAsciiFastPath(char* input, byte* output, int byteCount)
        {
            // Encode as bytes upto the first non-ASCII byte and return count encoded
            int i = 0;
#if BIT64
            if (byteCount < 4) goto trailing;

            int unaligned = (unchecked(-(int)input) >> 1) & 0x3;
            // Unaligned chars
            for (; i < unaligned; i++)
            {
                char ch = *(input + i);
                if (ch > 0x7F)
                {
                    goto exit; // Found non-ASCII, bail
                }
                else
                {
                    *(output + i) = (byte)ch; // Cast convert
                }
            }

            // Aligned
            int ulongDoubleCount = (byteCount - i) & ~0x7;
            for (; i < ulongDoubleCount; i += 8)
            {
                ulong inputUlong0 = *(ulong*)(input + i);
                ulong inputUlong1 = *(ulong*)(input + i + 4);
                if (((inputUlong0 | inputUlong1) & 0x8080808080808080) != 0)
                {
                    goto exit; // Found non-ASCII, bail
                }
                // Pack 16 ASCII chars into 16 bytes
                *(uint*)(output + i) =
                    ((uint)((inputUlong0 * Shift16Shift24) >> 24) & 0xffff) |
                    ((uint)((inputUlong0 * Shift8Identity) >> 24) & 0xffff0000);
                *(uint*)(output + i + 4) =
                    ((uint)((inputUlong1 * Shift16Shift24) >> 24) & 0xffff) |
                    ((uint)((inputUlong1 * Shift8Identity) >> 24) & 0xffff0000);
            }
            if (byteCount - 4 > i)
            {
                ulong inputUlong = *(ulong*)(input + i);
                if ((inputUlong & 0x8080808080808080) != 0)
                {
                    goto exit; // Found non-ASCII, bail
                }
                // Pack 8 ASCII chars into 8 bytes
                *(uint*)(output + i) =
                    ((uint)((inputUlong * Shift16Shift24) >> 24) & 0xffff) |
                    ((uint)((inputUlong * Shift8Identity) >> 24) & 0xffff0000);
                i += 4;
            }

            trailing:
            for (; i < byteCount; i++)
            {
                char ch = *(input + i);
                if (ch > 0x7F)
                {
                    goto exit; // Found non-ASCII, bail
                }
                else
                {
                    *(output + i) = (byte)ch; // Cast convert
                }
            }
#else
            // Unaligned chars
            if ((unchecked((int)input) & 0x2) != 0) {
                char ch = *input;
                if (ch > 0x7F) {
                    goto exit; // Found non-ASCII, bail
                } else {
                    i = 1;
                    *(output) = (byte)ch; // Cast convert
                }
            }

            // Aligned
            int uintCount = (byteCount - i) & ~0x3;
            for (; i < uintCount; i += 4) {
                uint inputUint0 = *(uint*)(input + i);
                uint inputUint1 = *(uint*)(input + i + 2);
                if (((inputUint0 | inputUint1) & 0x80808080) != 0) {
                    goto exit; // Found non-ASCII, bail
                }
                // Pack 4 ASCII chars into 4 bytes
                *(ushort*)(output + i) = (ushort)(inputUint0 | (inputUint0 >> 8));
                *(ushort*)(output + i + 2) = (ushort)(inputUint1 | (inputUint1 >> 8));
            }
            if (byteCount - 1 > i) {
                uint inputUint = *(uint*)(input + i);
                if ((inputUint & 0x80808080) != 0) {
                    goto exit; // Found non-ASCII, bail
                }
                // Pack 2 ASCII chars into 2 bytes
                *(ushort*)(output + i) = (ushort)(inputUint | (inputUint >> 8));
                i += 2;
            }

            if (i < byteCount) {
                char ch = *(input + i);
                if (ch > 0x7F) {
                    goto exit; // Found non-ASCII, bail
                } else {
                    *(output + i) = (byte)ch; // Cast convert
                    i = byteCount;
                }
            }
#endif // BIT64
            exit:
            return i;
        }

        //public static unsafe int GetBytesCountAsciiFastPath(Encoding encoding, string s)
        //{
        //    // Fast path for pure ASCII data for ASCII and UTF8 encoding
        //    Debug.Assert(encoding != null);
        //    if (s == null)
        //        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s, ExceptionResource.ArgumentNull_String);
        //    //Contract.EndContractBlock();

        //    int charCount = s.Length;
        //    int byteCount = 0;
        //    if (String.IsInterned(s) && s.IsAscii()) {
        //        // If string is Interned its worth using IsAscii cache for repeated use
        //        byteCount = s.Length;
        //    }

        //    if (byteCount < charCount) {
        //        fixed (char* input = s)
        //            byteCount = GetBytesCountAsciiFastPath(encoding, input, charCount);
        //    }

        //    return byteCount;
        //}

        //        private unsafe static int GetBytesCountAsciiFastPath(Encoding encoding, char* input, int charCount)
        //        {
        //            // Fast path for pure ASCII data for ASCII and UTF8 encoding
        //            int length;

        //            length = GetByteCountAsciiFastPath(input, charCount);
        //            if (length < charCount) {
        //                // Not all ASCII, get the byte count for the remaining encoded conversion
        //                //length += encoding.GetByteCount(input + length, charCount - length, null);
        //                length += encoding.GetByteCount(input + length, charCount - length);
        //            }

        //            return length;
        //        }

        //        internal unsafe static int GetByteCountAsciiFastPath(char* input, int byteCount)
        //        {
        //            // Counting sequence length that is ASCII, bail if non-ASCII char is found
        //            int i = 0;
        //#if BIT64
        //            if (byteCount < 4) goto trailing;

        //            int unaligned = (unchecked(-(int)input) >> 1) & 0x3;
        //            // Unaligned chars
        //            for (; i < unaligned; i++)
        //            {
        //                char ch = *(input + i);
        //                if (ch > 0x7F) {
        //                    goto exit; // Found non-ASCII, bail
        //                }
        //            }

        //            // Aligned
        //            int ulongDoubleCount = (byteCount - i) & ~0x7;
        //            for (; i < ulongDoubleCount; i += 8)
        //            {
        //                ulong inputUlong0 = *(ulong*)(input + i);
        //                ulong inputUlong1 = *(ulong*)(input + i + 4);
        //                if (((inputUlong0 | inputUlong1) & 0x8080808080808080) != 0) {
        //                    goto exit; // Found non-ASCII, bail
        //                }
        //            }
        //            if (byteCount - 4 > i)
        //            {
        //                ulong inputUlong = *(ulong*)(input + i);
        //                if ((inputUlong & 0x8080808080808080) != 0) {
        //                    goto exit; // Found non-ASCII, bail
        //                }
        //                i += 4;
        //            }

        //        trailing:
        //            for (; i < byteCount; i++)
        //            {
        //                char ch = *(input + i);
        //                if (ch > 0x7F) {
        //                    goto exit; // Found non-ASCII, bail
        //                }
        //            }
        //#else
        //            // Unaligned chars
        //            if ((unchecked((int)input) & 0x2) != 0) {
        //                char ch = *input;
        //                if (ch > 0x7F) {
        //                    goto exit; // Found non-ASCII, bail
        //                } else {
        //                    i = 1;
        //                }
        //            }

        //            // Aligned
        //            int uintCount = (byteCount - i) & ~0x3;
        //            for (; i < uintCount; i += 4) {
        //                uint inputUint0 = *(uint*)(input + i);
        //                uint inputUint1 = *(uint*)(input + i + 2);
        //                if (((inputUint0 | inputUint1) & 0x80808080) != 0) {
        //                    goto exit; // Found non-ASCII, bail
        //                }
        //            }
        //            if (byteCount - 1 > i) {
        //                uint inputUint = *(uint*)(input + i);
        //                if ((inputUint & 0x80808080) != 0) {
        //                    goto exit; // Found non-ASCII, bail
        //                }
        //                i += 2;
        //            }

        //            if (i < byteCount) {
        //                char ch = *(input + i);
        //                if (ch > 0x7F) {
        //                    goto exit; // Found non-ASCII, bail
        //                } else {
        //                    i = byteCount;
        //                }
        //            }
        //#endif // BIT64
        //        exit:
        //            return i;
        //        }
    }

    public class AltAsciiEncoding : Encoding
    {
        const int Shift16Shift24 = 256 * 256 * 256 + 256 * 256;
        const int Shift8Identity = 256 + 1;

        // Used by Encoding.ASCII for lazy initialization
        // The initialization code will not be run until a static member of the class is referenced
        internal static readonly AltAsciiEncoding s_default = new AltAsciiEncoding();


        // Returns an encoding for the ASCII character set. The returned encoding
        // will be an instance of the ASCIIEncoding class.

        public static Encoding AltASCII => s_default;

        public override byte[] GetBytes(String s)
            => EncodingForwarder.GetBytesAsciiFastPath(this, s);

        public override int GetBytes(String s, int charIndex, int charCount, byte[] bytes, int byteIndex)
            => EncodingForwarder.GetBytesAsciiFastPath(this, s, charIndex, charCount, bytes, byteIndex);

        public override byte[] GetBytes(char[] chars)
            => EncodingForwarder.GetBytesAsciiFastPath(this, chars, 0, chars.Length);

        public override byte[] GetBytes(char[] chars, int index, int count)
            => EncodingForwarder.GetBytesAsciiFastPath(this, chars, index, count);

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
            => EncodingForwarder.GetBytesAsciiFastPath(this, chars, charIndex, charCount, bytes, byteIndex);

        public override unsafe int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
            => EncodingForwarder.GetBytesAsciiFastPath(this, chars, charCount, bytes, byteCount);

        //internal override unsafe int GetBytes(char* chars, int charCount, byte* bytes, int byteCount, EncoderNLS encoder)
        //    => EncodingForwarder.GetBytesAsciiFastPath(this, chars, charCount, bytes, byteCount, encoder);

        //public override int GetByteCount(String s)
        //    => EncodingForwarder.GetBytesCountAsciiFastPath(this, s);

        //public int GetByteCount(string s, int index, int count) { }


        //[CLSCompliant(false)]
        //[System.Runtime.InteropServices.ComVisible(false)]
        //public override unsafe int GetByteCount(char* chars, int count) { }
        //public override int GetByteCount(char[] chars) { }
        //public override int GetByteCount(char[] chars, int index, int count);

        //internal override unsafe int GetByteCount(char* chars, int count, EncoderNLS encoder) { }

        public override int GetByteCount(char[] chars, int index, int count)
        {
            throw new NotImplementedException();
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            throw new NotImplementedException();
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            throw new NotImplementedException();
        }

        public override int GetMaxByteCount(int charCount)
        {
            throw new NotImplementedException();
        }

        public override int GetMaxCharCount(int byteCount)
        {
            throw new NotImplementedException();
        }
    }

    [Config(typeof(CoreConfig))]
    public class AsciiEncoding
    {
        private const int InnerLoopCount = 20;

        [Params(
            0,
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            14,
            15,
            16,
            17,
            18,
            32,
            63,
            64,
            65,
            128,
            256,
            512,
            1024
        )]
        public int StringLength { get; set; }
        public string data;

        public Encoding ASCII;
        public Encoding UTF8;
        public Encoding AltASCII;

        [Setup]
        public unsafe void Setup()
        {
            ASCII = Encoding.ASCII;
            AltASCII = AltAsciiEncoding.AltASCII;
            UTF8 = Encoding.UTF8;

            data = new string('\0', StringLength);
            fixed (char* pData = data)
            {
                for (var i = 0; i < data.Length; i++)
                {
                    // ascii chars 32 - 126
                    pData[i] = (char)((i % (126 - 32)) + 32);
                }
            }
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount, Baseline =true)]
        public void ASCIIGetBytes()
        {
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                ASCII.GetBytes(data);
            }
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void UTF8GetBytes()
        {
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                UTF8.GetBytes(data);
            }
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void FastPathGetBytes()
        {
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                AltASCII.GetBytes(data);
            }
        }
    }
}
