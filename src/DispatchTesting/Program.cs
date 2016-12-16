using BenchmarkDotNet.Running;

namespace DispatchTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<DispatchTesting>();
        }
    }
}
