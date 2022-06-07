using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Xml.Minification
{
	public class ProcessingXmlCommentsTests
	{
		[Fact]
		public void RemovingXmlCommentsIsCorrect()
		{
			// Arrange
			var keepingXmlCommentsMinifier = new XmlMinifier(
				new XmlMinificationSettings(true) { RemoveXmlComments = false });
			var removingXmlCommentsMinifier = new XmlMinifier(
				new XmlMinificationSettings(true) { RemoveXmlComments = true });

			const string input1 = "<!---->";
			const string targetOutput1A = input1;
			const string targetOutput1B = "";

			const string input2 = "<!-- -->";
			const string targetOutput2A = input2;
			const string targetOutput2B = "";

			const string input3 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
				"<rss version=\"2.0\">" +
				"<channel>" +
				"<!-- Channel properties -->" +
				"<title>RSS Title</title>" +
				"<description>This is an example of an RSS feed</description>" +
				"<link>http://www.example.com/rss</link>" +
				"<lastBuildDate>Mon, 07 Sep 2013 00:01:00 +0000</lastBuildDate>" +
				"<pubDate>Mon, 07 Sep 2012 16:45:00 +0000</pubDate>" +
				"<ttl>1800</ttl>" +
				"<!-- /Channel properties -->" +
				"<!-- Item list -->" +
				"<item>" +
				"<title>Example <!-- Somme comment... -->entry</title>" +
				"<description>" +
				"<![CDATA[" +
				"<p>Here is some text<!-- Somme other comment... --> containing an description.</p>" +
				"]]>" +
				"</description>" +
				"<link>http://www.example.com/2012/09/01/my-article</link>" +
				"<guid>97357194-40ea-4a19-9941-bc208521b8ce</guid>" +
				"<pubDate>Mon, 07 Sep 2012 16:45:00 +0000</pubDate>" +
				"</item>" +
				"<!-- /Item list -->" +
				"</channel>" +
				"</rss>"
				;
			const string targetOutput3A = input3;
			const string targetOutput3B = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
				"<rss version=\"2.0\">" +
				"<channel>" +
				"<title>RSS Title</title>" +
				"<description>This is an example of an RSS feed</description>" +
				"<link>http://www.example.com/rss</link>" +
				"<lastBuildDate>Mon, 07 Sep 2013 00:01:00 +0000</lastBuildDate>" +
				"<pubDate>Mon, 07 Sep 2012 16:45:00 +0000</pubDate>" +
				"<ttl>1800</ttl>" +
				"<item>" +
				"<title>Example entry</title>" +
				"<description>" +
				"<![CDATA[" +
				"<p>Here is some text<!-- Somme other comment... --> containing an description.</p>" +
				"]]>" +
				"</description>" +
				"<link>http://www.example.com/2012/09/01/my-article</link>" +
				"<guid>97357194-40ea-4a19-9941-bc208521b8ce</guid>" +
				"<pubDate>Mon, 07 Sep 2012 16:45:00 +0000</pubDate>" +
				"</item>" +
				"</channel>" +
				"</rss>"
				;

			const string input4 = "<!--wmm:ignore-->Some text...<!--/wmm:ignore-->";
			const string targetOutput4A = "Some text...";
			const string targetOutput4B = targetOutput4A;

			// Act
			string output1A = keepingXmlCommentsMinifier.Minify(input1).MinifiedContent;
			string output1B = removingXmlCommentsMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingXmlCommentsMinifier.Minify(input2).MinifiedContent;
			string output2B = removingXmlCommentsMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingXmlCommentsMinifier.Minify(input3).MinifiedContent;
			string output3B = removingXmlCommentsMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingXmlCommentsMinifier.Minify(input4).MinifiedContent;
			string output4B = removingXmlCommentsMinifier.Minify(input4).MinifiedContent;

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