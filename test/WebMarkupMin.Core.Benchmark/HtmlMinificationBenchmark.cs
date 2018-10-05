using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

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

namespace WebMarkupMin.Core.Benchmark
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
			string documentsDirectoryPath = Path.GetFullPath(
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../Files"));
			List <Document> nonExistentDocuments = null;

			foreach (string documentName in s_documents.Keys)
			{
				Document document = s_documents[documentName];
				if (document.Content == null)
				{
					string path = Path.Combine(documentsDirectoryPath, $"{documentName}.html");
					document.Path = path;
	
					if (File.Exists(path))
					{
						document.Content = File.ReadAllText(path);
					}
					else
					{
						if (nonExistentDocuments == null)
						{
							nonExistentDocuments = new List<Document>();
						}
						nonExistentDocuments.Add(document);
					}
				}
			}

			if (nonExistentDocuments == null || nonExistentDocuments.Count == 0)
			{
				return;
			}

			WebClient webClient = null;

			try
			{
				foreach (Document document in nonExistentDocuments)
				{
					if (webClient == null)
					{
						webClient = new WebClient();
					}

					string url = document.Url;
					string path = document.Path;
					string content;

					Console.WriteLine($"Downloading content from {url}...");
					content = webClient.DownloadString(url);

					if (!Directory.Exists(documentsDirectoryPath))
					{
						Directory.CreateDirectory(documentsDirectoryPath);
					}
					File.WriteAllText(path, content);

					document.Content = content;
				}
			}
			finally
			{
				webClient?.Dispose();
			}
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
			NuMinifier.Html(s_documents[DocumentName].Content, settings);
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
			minifier.Minify(s_documents[DocumentName].Content);
		}
#if HTML_MINIFIER_COMPARISON && NET461

		[Benchmark]
		public void ZetaProducerHtmlCompressor()
		{
			var minifier = new ZphcMinifier();
			minifier.Compress(s_documents[DocumentName].Content);
		}
#endif
	}
}