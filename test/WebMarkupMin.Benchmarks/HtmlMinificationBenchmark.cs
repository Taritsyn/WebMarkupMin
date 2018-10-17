using System.Collections.Generic;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

#if HTML_MINIFIER_COMPARISON
using NuMinifier = NUglify.Uglify;
using NuSettings = NUglify.Html.HtmlSettings;
#endif
using WmmMinifier = WebMarkupMin.Core.HtmlMinifier;
using WmmNullCssMinifier = WebMarkupMin.Core.NullCssMinifier;
using WmmNullJsMinifier = WebMarkupMin.Core.NullJsMinifier;
using WmmSettings = WebMarkupMin.Core.HtmlMinificationSettings;
#if HTML_MINIFIER_COMPARISON && NET461
using ZphcMinifier = ZetaProducerHtmlCompressor.HtmlContentCompressor;
#endif

namespace WebMarkupMin.Benchmarks
{
	[MemoryDiagnoser]
	public class HtmlMinificationBenchmark
	{
		private static readonly Dictionary<string, Document> s_documents = new Dictionary<string, Document>
		{
			{ "afisha.ru", new Document("https://www.afisha.ru/") },
			{ "dotnet.ru", new Document("http://dotnet.ru/") },
			{ "html50-specification", new Document("https://www.w3.org/TR/html50/single-page.html") },
			{ "moskva.mts.ru", new Document("https://moskva.mts.ru/") },
			{ "ozon.ru", new Document("https://www.ozon.ru/") }
		};

		[ParamsSource(nameof(DocumentNames))]
		public string DocumentName { get; set; }


		static HtmlMinificationBenchmark()
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
#if HTML_MINIFIER_COMPARISON

		[Benchmark]
		public void NUglify()
		{
			var settings = new NuSettings()
			{
				MinifyCss = false,
				MinifyJs = false
			};
			string minifiedContent = NuMinifier.Html(s_documents[DocumentName].Content, settings).Code;
		}
#endif

		[Benchmark]
		public void WebMarkupMin()
		{
			var settings = new WmmSettings
			{
				MinifyEmbeddedCssCode = false,
				MinifyInlineCssCode = false,
				MinifyEmbeddedJsCode = false,
				MinifyInlineJsCode = false
			};
			var minifier = new WmmMinifier(settings, new WmmNullCssMinifier(), new WmmNullJsMinifier());
			string minifiedContent = minifier.Minify(s_documents[DocumentName].Content).MinifiedContent;
		}
#if HTML_MINIFIER_COMPARISON && NET461

		[Benchmark]
		public void ZetaProducerHtmlCompressor()
		{
			var minifier = new ZphcMinifier();
			string minifiedContent = minifier.Compress(s_documents[DocumentName].Content);
		}
#endif
	}
}