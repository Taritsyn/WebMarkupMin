using System.IO;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

using WebMarkupMin.Core;

namespace WebMarkupMin.Benchmarks
{
	[MemoryDiagnoser]
	public class StatisticsGenerationBenchmark
	{
		private static readonly HtmlMinifier s_minifier = new HtmlMinifier();
		private static readonly string s_content = null;


		static StatisticsGenerationBenchmark()
		{
			string absoluteDirectoryPath = Utils.GetAbsoluteDirectoryPath("../../../Files/html");
			string absoluteFilePath = Path.Combine(absoluteDirectoryPath, "josh-ducks-periodic-table.html");

			s_content = File.ReadAllText(absoluteFilePath);
		}


		[Benchmark(Baseline = true)]
		public void StatisticsDisabled()
		{
			Process(false);
		}

		[Benchmark]
		public void StatisticsEnabled()
		{
			Process(true);
		}

		private static void Process(bool generateStatistics)
		{
			string minifiedContent = s_minifier.Minify(s_content, generateStatistics).MinifiedContent;
		}
	}
}