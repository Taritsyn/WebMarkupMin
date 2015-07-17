using System;
using System.IO;

#if DNXCORE50 || DNX451
using Microsoft.Framework.Runtime;
#elif NET40
using System.Web;
#else
#error No implementation for this target
#endif

using WebMarkupMin.Sample.Resources;

namespace WebMarkupMin.Sample.Logic.Services
{
	public sealed class FileContentService
	{
		private readonly string _textContentDirectoryPath;
#if DNXCORE50 || DNX451
		private IApplicationEnvironment _applicationEnvironment { get; set; }
#endif


#if DNXCORE50 || DNX451
		public FileContentService(
			string textContentDirectoryPath,
			IApplicationEnvironment applicationEnvironment
		)
		{
			_textContentDirectoryPath = textContentDirectoryPath;
			_applicationEnvironment = applicationEnvironment;
		}
#elif NET40
		public FileContentService(string textContentDirectoryPath)
		{
			_textContentDirectoryPath = textContentDirectoryPath;
		}
#else
#error No implementation for this target
#endif

		public string GetFileContent(string filePath)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				throw new ArgumentException(
					string.Format(CommonStrings.ErrorMessage_FilePathNotSpecified, filePath),
					"filePath"
				);
			}

			string content;
			string fullFilePath = _textContentDirectoryPath.TrimEnd('/') + "/" + filePath;
			string physicalFilePath = GetPhysicalFilePath(fullFilePath);

			try
			{
				using (FileStream fileStream = File.OpenRead(physicalFilePath))
				using (var reader = new StreamReader(fileStream))
				{
					content = reader.ReadToEnd();
				}
			}
			catch (FileNotFoundException)
			{
				throw new FileNotFoundException(
					string.Format(CommonStrings.ErrorMessage_FileNotFound, filePath));
			}
			catch (DirectoryNotFoundException)
			{
				throw new FileNotFoundException(
					string.Format(CommonStrings.ErrorMessage_FileNotFound, filePath));
			}

			return content;
		}

		private string GetPhysicalFilePath(string filePath)
		{
#if DNXCORE50 || DNX451
			string applicationDirectoryPath = _applicationEnvironment.ApplicationBasePath;
#elif NET40
			HttpContext context = HttpContext.Current;
			string applicationDirectoryPath = context.Server.MapPath("~/");
#else
#error No implementation for this target
#endif
			string physicalFilePath = Path.Combine(
				applicationDirectoryPath,
				filePath.Replace('/', '\\').TrimStart(new char[] { '\\' })
			);

			return physicalFilePath;
		}
	}
}