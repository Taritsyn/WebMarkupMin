using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace WebMarkupMin.Benchmarks
{
	internal static class Utils
	{
		public static void PopulateTestData(string directoryPath, Dictionary<string, Document> documents,
			string fileExtension)
		{
			string baseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
			string absoluteDirectoryPath = Path.GetFullPath(Path.Combine(baseDirectoryPath, directoryPath));
			List<Document> nonExistentDocuments = null;
#if NETCOREAPP2_0

			if (!Directory.Exists(absoluteDirectoryPath))
			{
				absoluteDirectoryPath = Path.GetFullPath(
					Path.Combine(baseDirectoryPath, "../../../../", directoryPath));
			}
#endif

			foreach (string documentName in documents.Keys)
			{
				Document document = documents[documentName];
				if (document.Content == null)
				{
					string path = Path.Combine(absoluteDirectoryPath, $"{documentName}{fileExtension}");
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

					if (!Directory.Exists(absoluteDirectoryPath))
					{
						Directory.CreateDirectory(absoluteDirectoryPath);
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
	}
}