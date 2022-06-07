using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class ProcessingMetaTagsTests
	{
		[Fact]
		public void UpgradingToMetaCharsetTag()
		{
			// Arrange
			var originalMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { UseMetaCharsetTag = false });
			var upgradingMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { UseMetaCharsetTag = true });

			const string input1 = "<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\">";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<meta charset=\"utf-8\">";

			const string input2 = "<meta http-equiv=\"Content-Type\" content=\"text/html;charset=windows-1251\">";
			const string targetOutput2A = input2;
			const string targetOutput2B = "<meta charset=\"windows-1251\">";

			const string input3 = "<META http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\">";
			const string targetOutput3A = "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\">";
			const string targetOutput3B = "<meta charset=\"iso-8859-1\">";

			const string input4 = "<meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml;charset=UTF-8\"/>";
			const string targetOutput4A = "<meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml;charset=UTF-8\">";
			const string targetOutput4B = "<meta charset=\"UTF-8\">";

			// Act
			string output1A = originalMinifier.Minify(input1).MinifiedContent;
			string output1B = upgradingMinifier.Minify(input1).MinifiedContent;

			string output2A = originalMinifier.Minify(input2).MinifiedContent;
			string output2B = upgradingMinifier.Minify(input2).MinifiedContent;

			string output3A = originalMinifier.Minify(input3).MinifiedContent;
			string output3B = upgradingMinifier.Minify(input3).MinifiedContent;

			string output4A = originalMinifier.Minify(input4).MinifiedContent;
			string output4B = upgradingMinifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
		}
	}
}