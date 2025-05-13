using System.Collections.Generic;
#if XML_MINIFIER_COMPARISON
using System.IO;
#endif

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

#if XML_MINIFIER_COMPARISON
using ClassicXmlDocument = System.Xml.XmlDocument;
using LinqXmlDocument = System.Xml.Linq.XDocument;
using LinqXmlSaveOptions = System.Xml.Linq.SaveOptions;
#endif
using WmmMinifier = WebMarkupMin.Core.XmlMinifier;

namespace WebMarkupMin.Benchmarks
{
	[MemoryDiagnoser]
	public class XmlMinificationBenchmark
	{
		private static readonly Dictionary<string, Document> s_documents = new Dictionary<string, Document>
		{
			{ "commerce-ml-sample", new Document() },
			{ "lea-verou-atom", new Document("https://lea.verou.me/feed.xml") },
			{ "meyerweb-rss2", new Document("https://meyerweb.com/eric/thoughts/category/tech/rss2/full/") },
			{ "software2cents-sitemap", new Document("https://software2cents.wordpress.com/sitemap.xml") },
			{ "yandex-ml-sample", new Document() }
		};

		[ParamsSource(nameof(DocumentNames))]
		public string DocumentName { get; set; }


		static XmlMinificationBenchmark()
		{
			Utils.PopulateTestData("../../../Files/xml", s_documents, ".xml");
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
			var minifier = new WmmMinifier();
			string minifiedContent = minifier.Minify(s_documents[DocumentName].Content).MinifiedContent;
		}
#if XML_MINIFIER_COMPARISON

		[Benchmark]
		public void XDocument()
		{
			LinqXmlDocument document = LinqXmlDocument.Parse(s_documents[DocumentName].Content);
			string minifiedContent;

			using (var writer = new StringWriter())
			{
				document.Save(writer, LinqXmlSaveOptions.DisableFormatting);
				minifiedContent = writer.ToString();
			}
		}

		[Benchmark]
		public void XmlDocument()
		{
			var document = new ClassicXmlDocument();
			document.LoadXml(s_documents[DocumentName].Content);
			string minifiedContent = document.OuterXml;
		}
#endif
	}
}