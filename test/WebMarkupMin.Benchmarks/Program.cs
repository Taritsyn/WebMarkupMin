using System.Reflection;

using BenchmarkDotNet.Running;

namespace WebMarkupMin.Benchmarks
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			BenchmarkSwitcher.FromAssembly(typeof(Program).GetTypeInfo().Assembly).Run(args);
		}
	}
}