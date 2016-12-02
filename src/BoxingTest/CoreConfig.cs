using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Validators;

namespace BoxTest
{
    public class CoreConfig : ManualConfig
    {
        public CoreConfig()
        {
            Add(JitOptimizationsValidator.FailOnError);
            Add(new RpsColumn());

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
