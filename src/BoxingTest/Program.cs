using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;

namespace BoxTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<IntBoxTesting>();
            BenchmarkRunner.Run<BoolBoxTesting>();
        }
    }
}
