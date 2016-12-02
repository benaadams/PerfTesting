using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace BoxTest
{
    public static class BoolExtensions
    {
        private static readonly object BoxedTrue = true;
        private static readonly object BoxedFalse = false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object BoxExplict(bool b)
        {
            return b ? BoxedTrue : BoxedFalse;
        }

        public static object Box(this bool b)
        {
            return b ? BoxedTrue : BoxedFalse;
        }
    }

    [Config(typeof(CoreConfig))]
    public class BoolBoxTesting
    {

        private const int InnerLoopCount = 256 * 2000;
        object[] Boxes = new object[256];

        [Benchmark(Baseline = true, OperationsPerInvoke = InnerLoopCount)]
        public void BoolUncachedBoxing()
        {
            for (var loop = 0; loop < 2000; loop++)
            {
                bool state = false;
                var boxes = Boxes;
                for (var i = 0; i < boxes.Length; i++)
                {
                    boxes[i] = state;
                    state = !state;
                }
            }
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void BoolCachedBoxing()
        {
            for (var loop = 0; loop < 2000; loop++)
            {
                bool state = false;
                var boxes = Boxes;
                for (var i = 0; i < boxes.Length; i++)
                {
                    boxes[i] = BoolExtensions.BoxExplict(state);
                    state = !state;
                }
            }
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void BoolCachedBoxExtenstion()
        {
            for (var loop = 0; loop < 2000; loop++)
            {
                bool state = false;
                var boxes = Boxes;
                for (var i = 0; i < boxes.Length; i++)
                {
                    boxes[i] = state.Box();
                    state = !state;
                }
            }
        }

        [Setup]
        public void Setup()
        {
            BoolExtensions.BoxExplict(false);
            false.Box();
        }
    }

}
