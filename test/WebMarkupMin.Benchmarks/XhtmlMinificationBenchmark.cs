using System.Collections.Generic;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

using WmmMinifier = WebMarkupMin.Core.XhtmlMinifier;
using WmmNullCssMinifier = WebMarkupMin.Core.NullCssMinifier;
using WmmNullJsMinifier = WebMarkupMin.Core.NullJsMinifier;
using WmmSettings = WebMarkupMin.Core.XhtmlMinificationSettings;

namespace WebMarkupMin.Benchmarks
{
	[MemoryDiagnoser]
	public class XhtmlMinificationBenchmark
	{
		private static readonly Dictionary<string, Document> s_documents = new Dictionary<string, Document>
		{
			{ "javascript.ru", new Document("https://javascript.ru/") },
			{ "prettydiff.com", new Document("https://prettydiff.com/tool.xhtml") },
			{ "ubl.xml.org", new Document("https://ubl.xml.org/") },
			{ "webmascon.com", new Document("https://webmascon.com/") },
			{ "webstandards.org", new Document("https://www.webstandards.org/") },
		};

		[ParamsSource(nameof(DocumentNames))]
		public string DocumentName { get; set; }


		static XhtmlMinificationBenchmark()
		{
			Utils.PopulateTestData("../../../Files/xhtml", s_documents, ".html");
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
				MinifyInlineJsCode = false,
				RemoveRedundantAttributes = true
			};
			var minifier = new WmmMinifier(settings, new WmmNullCssMinifier(), new WmmNullJsMinifier());
			string minifiedContent = minifier.Minify(s_documents[DocumentName].Content).MinifiedContent;
		}
	}
}
