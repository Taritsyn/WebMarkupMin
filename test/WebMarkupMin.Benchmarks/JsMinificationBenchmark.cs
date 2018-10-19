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
	public class JsMinificationBenchmark
	{
		private static HtmlMinificationSettings _sharedSettings = new HtmlMinificationSettings
		{
			MinifyEmbeddedCssCode = false,
			MinifyInlineCssCode = false,
			MinifyEmbeddedJsCode = true,
			MinifyInlineJsCode = true
		};

		private static readonly Dictionary<string, Document> s_documents = new Dictionary<string, Document>
		{
			{ "finmarket.ru", new Document("http://www.finmarket.ru/") },
			{ "moskva.mts.ru", new Document("https://moskva.mts.ru/") },
			{ "pochtabank.ru", new Document("https://www.pochtabank.ru/") }
		};

		[ParamsSource(nameof(DocumentNames))]
		public string DocumentName { get; set; }


		static JsMinificationBenchmark()
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
		public void Crockford()
		{
			var minifier = new HtmlMinifier(_sharedSettings, new NullCssMinifier(), new CrockfordJsMinifier());
			string minifiedContent = minifier.Minify(s_documents[DocumentName].Content).MinifiedContent;
		}

		[Benchmark()]
		public void MsAjax()
		{
			var minifier = new HtmlMinifier(_sharedSettings, new NullCssMinifier(), new MsAjaxJsMinifier());
			string minifiedContent = minifier.Minify(s_documents[DocumentName].Content).MinifiedContent;
		}

		[Benchmark()]
		public void NUglify()
		{
			var minifier = new HtmlMinifier(_sharedSettings, new NullCssMinifier(), new NUglifyJsMinifier());
			string minifiedContent = minifier.Minify(s_documents[DocumentName].Content).MinifiedContent;
		}

		[Benchmark()]
		public void Yui()
		{
			var minifier = new HtmlMinifier(_sharedSettings, new NullCssMinifier(), new YuiJsMinifier());
			string minifiedContent = minifier.Minify(s_documents[DocumentName].Content).MinifiedContent;
		}
	}
}
