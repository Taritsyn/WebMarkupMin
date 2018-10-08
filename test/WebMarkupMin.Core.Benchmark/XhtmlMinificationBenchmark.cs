using System.Collections.Generic;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

using WmmMinifier = WebMarkupMin.Core.XhtmlMinifier;
using WmmNullCssMinifier = WebMarkupMin.Core.NullCssMinifier;
using WmmNullJsMinifier = WebMarkupMin.Core.NullJsMinifier;
using WmmSettings = WebMarkupMin.Core.XhtmlMinificationSettings;

namespace WebMarkupMin.Core.Benchmark
{
	[MemoryDiagnoser]
	public class XhtmlMinificationBenchmark
	{
		private static readonly Dictionary<string, Document> s_documents = new Dictionary<string, Document>
		{
			{ "ibm.com", new Document("https://www.ibm.com/ru-ru/") },
			{ "microformats.org", new Document("http://microformats.org/") },
			{ "moskva.beeline.ru", new Document("https://moskva.beeline.ru/") },
			{ "ozon.ru", new Document("https://www.ozon.ru/") },
			{ "prettydiff.com", new Document("https://prettydiff.com/") }
		};

		[ParamsSource(nameof(DocumentNames))]
		public string DocumentName { get; set; }


		static XhtmlMinificationBenchmark()
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
	}
}
