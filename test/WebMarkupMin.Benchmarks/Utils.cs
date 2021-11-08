using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace WebMarkupMin.Benchmarks
{
	internal static class Utils
	{
		public static string GetAbsoluteDirectoryPath(string directoryPath)
		{
			string baseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
			string absoluteDirectoryPath = Path.GetFullPath(Path.Combine(baseDirectoryPath, directoryPath));
#if NETCOREAPP

			if (!Directory.Exists(absoluteDirectoryPath))
			{
				absoluteDirectoryPath = Path.GetFullPath(
					Path.Combine(baseDirectoryPath, "../../../../", directoryPath));
			}
#endif

			return absoluteDirectoryPath;
		}

		public static void PopulateTestData(string directoryPath, Dictionary<string, Document> documents,
			string fileExtension)
		{
			string absoluteDirectoryPath = GetAbsoluteDirectoryPath(directoryPath);
			List<Document> nonExistentDocuments = null;

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

			HttpClient httpClient = null;

			try
			{
				foreach (Document document in nonExistentDocuments)
				{
					if (httpClient == null)
					{
						httpClient = new HttpClient();
					}

					string url = document.Url;
					string path = document.Path;
					string content;

					Console.WriteLine($"Downloading content from {url}...");
					content = httpClient.GetStringAsync(url)
						.ConfigureAwait(false)
						.GetAwaiter()
						.GetResult()
						;

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
				httpClient?.Dispose();
			}
		}
	}
}