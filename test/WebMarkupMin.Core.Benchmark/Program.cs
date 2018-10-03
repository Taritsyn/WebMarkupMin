using BenchmarkDotNet.Running;

namespace WebMarkupMin.Core.Benchmark
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			BenchmarkRunner.Run<HtmlMinificationBenchmark>();
		}
	}
}