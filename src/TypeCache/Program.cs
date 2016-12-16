using BenchmarkDotNet.Running;

namespace TypeCacheTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<TypeCacheTesting>();
        }
    }
}
