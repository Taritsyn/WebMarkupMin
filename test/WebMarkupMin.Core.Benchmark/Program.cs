using System.Reflection;

using BenchmarkDotNet.Running;

namespace WebMarkupMin.Core.Benchmark
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			BenchmarkSwitcher.FromAssembly(typeof(Program).GetTypeInfo().Assembly).Run(args);
		}
	}
}