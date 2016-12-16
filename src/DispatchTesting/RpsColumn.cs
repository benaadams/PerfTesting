using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace DispatchTest
{
    public class RpsColumn : IColumn
    {
        private static int NanosPerSecond = 1000 * 1000 * 1000;

        public string GetValue(Summary summary, Benchmark benchmark)
        {
            var totalNanos = summary.Reports.First(r => r.Benchmark == benchmark).ResultStatistics.Mean;
            // Make sure we don't divide by zero!!
            return Math.Abs(totalNanos) > 0.0 ? (NanosPerSecond / totalNanos).ToString("N2") : "N/A";
        }

        public bool IsDefault(Summary summary, Benchmark benchmark) => false;
        public bool IsAvailable(Summary summary) => true;
        public string Id => "RPS-Column";
        public string ColumnName => "RPS";
        public bool AlwaysShow => true;
        public ColumnCategory Category => ColumnCategory.Custom;
        public int PriorityInCategory => 1;
    }
}
