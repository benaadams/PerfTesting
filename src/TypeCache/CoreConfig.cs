using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Validators;

namespace TypeCacheTest
{
    public class CoreConfig : ManualConfig
    {
        public CoreConfig()
        {
            Add(JitOptimizationsValidator.FailOnError);
            Add(new RpsColumn());
            Add(MemoryDiagnoser.Default);

            Add(Job.Default.
                With(BenchmarkDotNet.Environments.Runtime.Core).
                WithRemoveOutliers(false).
                With(new GcMode() { Server = true }).
                With(RunStrategy.Throughput).
                WithLaunchCount(3).
                WithWarmupCount(5).
                WithTargetCount(10));
        }
    }
}
