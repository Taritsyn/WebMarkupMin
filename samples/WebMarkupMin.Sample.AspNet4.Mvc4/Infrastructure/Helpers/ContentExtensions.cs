using System.IO;
using System.Web;
using System.Web.Mvc;

using WebMarkupMin.Sample.Resources;

namespace WebMarkupMin.Sample.AspNet4.Mvc4.Infrastructure.Helpers
{
	public static class ContentExtensions
	{
		public static MvcHtmlString RenderContent(this HtmlHelper htmlHelper, string filePath)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				return RenderErrorMessage(
					string.Format(CommonStrings.ErrorMessage_FilePathNotSpecified, filePath));
			}

			string content;
			HttpContextBase context = htmlHelper.ViewContext.HttpContext;
			StreamReader fileStream = null;
			string physicalFilePath = context.Server.MapPath(filePath); // Physical file path

			try
			{
				fileStream = new StreamReader(physicalFilePath);
				content = fileStream.ReadToEnd();
			}
			catch (FileNotFoundException)
			{
				return RenderErrorMessage(string.Format(CommonStrings.ErrorMessage_FileNotFound, filePath));
			}
			catch (DirectoryNotFoundException)
			{
				return RenderErrorMessage(string.Format(CommonStrings.ErrorMessage_FileNotFound, filePath));
			}
			catch
			{
				return RenderErrorMessage(string.Format(CommonStrings.ErrorMessage_FileReadingFailed, filePath));
			}
			finally
			{
				if (fileStream != null)
				{
					// Close the file
					fileStream.Close();
				}
			}

			return MvcHtmlString.Create(content);
		}

		private static MvcHtmlString RenderErrorMessage(string errorMessage)
		{
			const string errorMessageTemplate = @"<p class=""text-error"">{0}</p>";

			return MvcHtmlString.Create(string.Format(errorMessageTemplate, errorMessage));
		}
	}
}