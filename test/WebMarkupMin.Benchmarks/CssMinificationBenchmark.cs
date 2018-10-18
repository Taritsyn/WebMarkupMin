using System.Collections.Generic;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

using WebMarkupMin.Core;
using WebMarkupMin.MsAjax;
using WebMarkupMin.NUglify;
using WebMarkupMin.Yui;

namespace WebMarkupMin.Benchmarks
{
	[MemoryDiagnoser]
	public class CssMinificationBenchmark
	{
		private static HtmlMinificationSettings _sharedSettings = new HtmlMinificationSettings
		{
			MinifyEmbeddedCssCode = true,
			MinifyInlineCssCode = true,
			MinifyEmbeddedJsCode = false,
			MinifyInlineJsCode = false
		};

		private static readonly Dictionary<string, Document> s_documents = new Dictionary<string, Document>
		{
			{ "finmarket.ru", new Document("http://www.finmarket.ru/") },
			{ "hitfm.ru", new Document("http://hitfm.ru/") },
			{ "moskva.beeline.ru", new Document("https://moskva.beeline.ru/") }
		};

		[ParamsSource(nameof(DocumentNames))]
		public string DocumentName { get; set; }


		static CssMinificationBenchmark()
		{
			Utils.PopulateTestData("../../../Files/html", s_documents, ".html");
		}


		public IEnumerable<string> DocumentNames()
		{
			foreach (string key in s_documents.Keys)
			{
				yield return key;
			}
		}

		[Benchmark(Baseline = true)]
		public void Null()
		{
			var minifier = new HtmlMinifier(_sharedSettings, new NullCssMinifier(), new NullJsMinifier());
			string minifiedContent = minifier.Minify(s_documents[DocumentName].Content).MinifiedContent;
		}

		[Benchmark()]
		public void Kristensen()
		{
			var minifier = new HtmlMinifier(_sharedSettings, new KristensenCssMinifier(), new NullJsMinifier());
			string minifiedContent = minifier.Minify(s_documents[DocumentName].Content).MinifiedContent;
		}

		[Benchmark()]
		public void MsAjax()
		{
			var minifier = new HtmlMinifier(_sharedSettings, new MsAjaxCssMinifier(), new NullJsMinifier());
			string minifiedContent = minifier.Minify(s_documents[DocumentName].Content).MinifiedContent;
		}

		[Benchmark()]
		public void NUglify()
		{
			var minifier = new HtmlMinifier(_sharedSettings, new NUglifyCssMinifier(), new NullJsMinifier());
			string minifiedContent = minifier.Minify(s_documents[DocumentName].Content).MinifiedContent;
		}

		[Benchmark()]
		public void Yui()
		{
			var minifier = new HtmlMinifier(_sharedSettings, new YuiCssMinifier(), new NullJsMinifier());
			string minifiedContent = minifier.Minify(s_documents[DocumentName].Content).MinifiedContent;
		}
	}
}
