using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Xunit;

namespace WebMarkupMin.Core.Test.Html
{
	public class JavaScriptPerfomanceMinificationIssue : FileSystemTestsBase
	{
		private readonly string _htmlFilesDirectoryPath;


		public JavaScriptPerfomanceMinificationIssue()
		{
			_htmlFilesDirectoryPath = Path.GetFullPath(Path.Combine(_baseDirectoryPath, @"../SharedFiles/html/"));
		}

		[Fact]
		public void MinifyingHTMLWithWronglyPlacedHtmlCommentInsideJavasScriptBlockIsNotSlowerThanUsual()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			string inputFilePath = Path.Combine(_htmlFilesDirectoryPath, "html-document-with-large-JSON-and-correct-JavasScript-comment.html");
			byte[] inputBytes = File.ReadAllBytes(inputFilePath);
			string inputContent = Encoding.UTF8.GetString(inputBytes);
			Stopwatch inputStopWatch = new Stopwatch();

			string targetFilePath = Path.Combine(_htmlFilesDirectoryPath, "html-document-with-large-JSON-and-HTML-comment-inside-JavasScript-block.html");
			byte[] targetBytes = File.ReadAllBytes(targetFilePath);
			string targetContent = Encoding.UTF8.GetString(targetBytes);
			Stopwatch targetStopWatch = new Stopwatch();

			//Act
			inputStopWatch.Start();
			string outputContent = minifier.Minify(inputContent).MinifiedContent;
			inputStopWatch.Stop();

			targetStopWatch.Start();
			string targetOutputContent = minifier.Minify(targetContent).MinifiedContent;
			targetStopWatch.Stop();

			// Assert
			Assert.Equal(inputStopWatch.ElapsedMilliseconds, targetStopWatch.ElapsedMilliseconds);
		}
		
	}
}
