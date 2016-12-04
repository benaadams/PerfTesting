using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace BoxTest
{
    public static class Int32Extensions
    {
        private static readonly object[] BoxedIntegers = CreateCache();

        private static object[] CreateCache()
        {
            object[] ret = new object[256];
            for (int i = -128; i < 128; i++)
            {
                ret[i + 128] = i;
            }

            return ret;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object BoxExplict(int i)
        {
            return (i >= -128 && i < 128) ? BoxedIntegers[i + 128] : (object)i;
        }

        public static object Box(this int i)
        {
            return (i >= -128 && i < 128) ? BoxedIntegers[i + 128] : (object)i;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]		
        public static object NoInlineBox(int i)
        {
            return (i >= -128 && i < 128) ? BoxedIntegers[i + 128] : (object)i;
        }
    }

    [Config(typeof(CoreConfig))]
    public class IntBoxTesting
    {

        private const int InnerLoopCount = 256 * 2000;
        object[] Boxes = new object[256];

        [Benchmark(Baseline = true, OperationsPerInvoke = InnerLoopCount)]
        public void Int32UncachedBoxing()
        {
            for (var loop = 0; loop < 2000; loop++)
            {
                var boxes = Boxes;
                for (var i = 0; i < boxes.Length; i++)
                {
                    boxes[i] = i - 128;
                }
            }
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void Int32CachedBoxing()
        {
            for (var loop = 0; loop < 2000; loop++)
            {
                var boxes = Boxes;
                for (var i = 0; i < boxes.Length; i++)
                {
                    boxes[i] = Int32Extensions.BoxExplict(i - 128);
                }
            }
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void Int32CachedNoInlineBoxing()
        {
            for (var loop = 0; loop < 2000; loop++)
            {
                var boxes = Boxes;
                for (var i = 0; i < boxes.Length; i++)
                {
                    boxes[i] = (i - 128).Box();
                }
            }
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void Int32CachedBoxExtenstion()
        {
            for (var loop = 0; loop < 2000; loop++)
            {
                var boxes = Boxes;
                for (var i = 0; i < boxes.Length; i++)
                {
                    boxes[i] = Int32Extensions.NoInlineBox(i - 128);
                }
            }
        }
		
        [Setup]
        public void Setup()
        {
            Int32Extensions.BoxExplict(1);
            1.Box();
            Int32Extensions.NoInlineBox(1);
        }
    }

}
