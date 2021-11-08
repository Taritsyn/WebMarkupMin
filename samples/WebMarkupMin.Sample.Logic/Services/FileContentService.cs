using System;
using System.IO;
#if NET452 || NETSTANDARD
using HostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
#elif NETCOREAPP3_1_OR_GREATER
using HostingEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;
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
#if NET452 || NETSTANDARD || NETCOREAPP
		private readonly HostingEnvironment _hostingEnvironment;
#endif


#if NET452 || NETSTANDARD || NETCOREAPP
		public FileContentService(
			string textContentDirectoryPath,
			HostingEnvironment hostingEnvironment
		)
		{
			_textContentDirectoryPath = textContentDirectoryPath;
			_hostingEnvironment = hostingEnvironment;
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
					nameof(filePath)
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
#if NET452 || NETSTANDARD || NETCOREAPP
			string applicationDirectoryPath = _hostingEnvironment.ContentRootPath;
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