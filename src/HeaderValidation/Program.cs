using BenchmarkDotNet.Running;

namespace HeaderValidation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<HeaderValidation>();
        }
    }
}
