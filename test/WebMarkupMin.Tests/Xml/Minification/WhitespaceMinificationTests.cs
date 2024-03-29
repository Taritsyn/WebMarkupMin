﻿using System;

using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Xml.Minification
{
	public class WhitespaceMinificationTests : IDisposable
	{
		private XmlMinifier _keepingWhitespaceMinifier;
		private XmlMinifier _keepingWhitespaceAndNewLinesMinifier;
		private XmlMinifier _removingWhitespaceMinifier;
		private XmlMinifier _removingWhitespaceExceptForNewLinesMinifier;


		public WhitespaceMinificationTests()
		{
			_keepingWhitespaceMinifier = new XmlMinifier(
				new XmlMinificationSettings(true)
				{
					MinifyWhitespace = false,
					PreserveNewLines = false
				});
			_keepingWhitespaceAndNewLinesMinifier = new XmlMinifier(
				new XmlMinificationSettings(true)
				{
					MinifyWhitespace = false,
					PreserveNewLines = true
				});
			_removingWhitespaceMinifier = new XmlMinifier(
				new XmlMinificationSettings(true)
				{
					MinifyWhitespace = true,
					PreserveNewLines = false
				});
			_removingWhitespaceExceptForNewLinesMinifier = new XmlMinifier(
				new XmlMinificationSettings(true)
				{
					MinifyWhitespace = true,
					PreserveNewLines = true
				});
		}


		[Fact]
		public void WhitespaceMinificationInRssDocument()
		{
			// Arrange
			const string input = " \n   <?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
				"<?xml-stylesheet type=\"text/xsl\" href=\"http://feeds.example.com/feed-rss.xslt\"?>\n" +
				"<rss version=\"2.0\">\n" +
				"	<channel>\n" +
				"		<!-- Channel properties -->\n" +
				"		<title>    RSS          Title          </title>\n" +
				"		<description>	This    is  an example   of an   RSS feed	</description>\n" +
				"		<link>http://www.example.com/rss</link>\n" +
				"		<lastBuildDate>Mon, 07 Sep 2013 00:01:00 +0000</lastBuildDate>\n" +
				"		<pubDate>Mon, 07 Sep 2012 16:45:00 +0000</pubDate>\n" +
				"		<ttl>1800</ttl>\n" +
				"		<!-- /Channel properties -->\n" +
				"		<!-- Item list -->\n" +
				"		<item>\n" +
				"			<title>		Example    entry  </title>\n" +
				"			<description>\n" +
				"			<![CDATA[\n" +
				"			<p>Here is some text containing an description.</p>\n" +
				"			]]>\n" +
				"			</description>\n" +
				"			<link>http://www.example.com/2012/09/01/my-article</link>\n" +
				"			<guid>97357194-40ea-4a19-9941-bc208521b8ce</guid>\n" +
				"			<pubDate>Mon, 07 Sep 2012 16:45:00 +0000</pubDate>\n" +
				"		</item>\n" +
				"		<!-- /Item list -->\n" +
				"	</channel>\n" +
				"</rss>\t   \n "
				;
			const string targetOutputA = input;
			const string targetOutputB = input;
			const string targetOutputC = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
				"<?xml-stylesheet type=\"text/xsl\" href=\"http://feeds.example.com/feed-rss.xslt\"?>" +
				"<rss version=\"2.0\">" +
				"<channel>\n" +
				"		<!-- Channel properties -->\n" +
				"		<title>    RSS          Title          </title>" +
				"<description>	This    is  an example   of an   RSS feed	</description>" +
				"<link>http://www.example.com/rss</link>" +
				"<lastBuildDate>Mon, 07 Sep 2013 00:01:00 +0000</lastBuildDate>" +
				"<pubDate>Mon, 07 Sep 2012 16:45:00 +0000</pubDate>" +
				"<ttl>1800</ttl>\n" +
				"		<!-- /Channel properties -->\n" +
				"		<!-- Item list -->\n" +
				"		<item>" +
				"<title>		Example    entry  </title>" +
				"<description>\n" +
				"			<![CDATA[\n" +
				"			<p>Here is some text containing an description.</p>\n" +
				"			]]>\n" +
				"			</description>" +
				"<link>http://www.example.com/2012/09/01/my-article</link>" +
				"<guid>97357194-40ea-4a19-9941-bc208521b8ce</guid>" +
				"<pubDate>Mon, 07 Sep 2012 16:45:00 +0000</pubDate>" +
				"</item>\n" +
				"		<!-- /Item list -->\n" +
				"	</channel>" +
				"</rss>"
				;
			const string targetOutputD = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
				"<?xml-stylesheet type=\"text/xsl\" href=\"http://feeds.example.com/feed-rss.xslt\"?>\n" +
				"<rss version=\"2.0\">\n" +
				"<channel>\n" +
				"		<!-- Channel properties -->\n" +
				"		<title>    RSS          Title          </title>\n" +
				"<description>	This    is  an example   of an   RSS feed	</description>\n" +
				"<link>http://www.example.com/rss</link>\n" +
				"<lastBuildDate>Mon, 07 Sep 2013 00:01:00 +0000</lastBuildDate>\n" +
				"<pubDate>Mon, 07 Sep 2012 16:45:00 +0000</pubDate>\n" +
				"<ttl>1800</ttl>\n" +
				"		<!-- /Channel properties -->\n" +
				"		<!-- Item list -->\n" +
				"		<item>\n" +
				"<title>		Example    entry  </title>\n" +
				"<description>\n" +
				"			<![CDATA[\n" +
				"			<p>Here is some text containing an description.</p>\n" +
				"			]]>\n" +
				"			</description>\n" +
				"<link>http://www.example.com/2012/09/01/my-article</link>\n" +
				"<guid>97357194-40ea-4a19-9941-bc208521b8ce</guid>\n" +
				"<pubDate>Mon, 07 Sep 2012 16:45:00 +0000</pubDate>\n" +
				"</item>\n" +
				"		<!-- /Item list -->\n" +
				"	</channel>\n" +
				"</rss>"
				;

			// Act
			string outputA = _keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = _keepingWhitespaceAndNewLinesMinifier.Minify(input).MinifiedContent;
			string outputC = _removingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = _removingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
		}

		[Fact]
		public void WhitespaceMinificationInAtomDocument()
		{
			// Arrange
			const string input = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n\n" +
				"<feed xmlns=\"http://www.w3.org/2005/Atom\">\n\n" +
				"	<title>Some feed title...</title>\n" +
				"	<subtitle> 	 </subtitle>\n" +
				"	<link href=\"http://example.org/feed/\" rel=\"self\"/>\n" +
				"	<link href=\"http://example.org/\"/>\n" +
				"	<id>urn:uuid:447d7106-3653-407c-ba8b-776f001b9d30</id>\n" +
				"	<updated>2013-04-12T19:35:07Z</updated>\n\n" +
				"	<entry>\n" +
				"		<title>Some entry title...</title>\n" +
				"		<link href=\"http://example.org/2003/12/13/atom03\"/>\n" +
				"		<link rel=\"alternate\" type=\"text/html\" href=\"http://example.org/2003/12/13/atom03.html\"/>\n" +
				"		<link rel=\"edit\" href=\"http://example.org/2003/12/13/atom03/edit\"/>\n" +
				"		<id>urn:uuid:af2cb1de-6a68-4af2-aa87-f575898a96e2</id>\n" +
				"		<updated>2013-04-12T19:35:07Z</updated>\n" +
				"		<summary>Some text...</summary>\n" +
				"		<author>\n" +
				"			<name>Vasya Pupkin</name>\n" +
				"			<email>vasya.pupkin@example.com</email>\n" +
				"		</author>\n" +
				"	</entry>\n" +
				"</feed>\n"
				;
			const string targetOutputA = input;
			const string targetOutputB = input;
			const string targetOutputC = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
				"<feed xmlns=\"http://www.w3.org/2005/Atom\">" +
				"<title>Some feed title...</title>" +
				"<subtitle> 	 </subtitle>" +
				"<link href=\"http://example.org/feed/\" rel=\"self\"/>" +
				"<link href=\"http://example.org/\"/>" +
				"<id>urn:uuid:447d7106-3653-407c-ba8b-776f001b9d30</id>" +
				"<updated>2013-04-12T19:35:07Z</updated>" +
				"<entry>" +
				"<title>Some entry title...</title>" +
				"<link href=\"http://example.org/2003/12/13/atom03\"/>" +
				"<link rel=\"alternate\" type=\"text/html\" href=\"http://example.org/2003/12/13/atom03.html\"/>" +
				"<link rel=\"edit\" href=\"http://example.org/2003/12/13/atom03/edit\"/>" +
				"<id>urn:uuid:af2cb1de-6a68-4af2-aa87-f575898a96e2</id>" +
				"<updated>2013-04-12T19:35:07Z</updated>" +
				"<summary>Some text...</summary>" +
				"<author>" +
				"<name>Vasya Pupkin</name>" +
				"<email>vasya.pupkin@example.com</email>" +
				"</author>" +
				"</entry>" +
				"</feed>"
				;
			const string targetOutputD = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
				"<feed xmlns=\"http://www.w3.org/2005/Atom\">\n" +
				"<title>Some feed title...</title>\n" +
				"<subtitle> 	 </subtitle>\n" +
				"<link href=\"http://example.org/feed/\" rel=\"self\"/>\n" +
				"<link href=\"http://example.org/\"/>\n" +
				"<id>urn:uuid:447d7106-3653-407c-ba8b-776f001b9d30</id>\n" +
				"<updated>2013-04-12T19:35:07Z</updated>\n" +
				"<entry>\n" +
				"<title>Some entry title...</title>\n" +
				"<link href=\"http://example.org/2003/12/13/atom03\"/>\n" +
				"<link rel=\"alternate\" type=\"text/html\" href=\"http://example.org/2003/12/13/atom03.html\"/>\n" +
				"<link rel=\"edit\" href=\"http://example.org/2003/12/13/atom03/edit\"/>\n" +
				"<id>urn:uuid:af2cb1de-6a68-4af2-aa87-f575898a96e2</id>\n" +
				"<updated>2013-04-12T19:35:07Z</updated>\n" +
				"<summary>Some text...</summary>\n" +
				"<author>\n" +
				"<name>Vasya Pupkin</name>\n" +
				"<email>vasya.pupkin@example.com</email>\n" +
				"</author>\n" +
				"</entry>\n" +
				"</feed>"
				;

			// Act
			string outputA = _keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = _keepingWhitespaceAndNewLinesMinifier.Minify(input).MinifiedContent;
			string outputC = _removingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = _removingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
		}

		[Fact]
		public void WhitespaceMinificationInSoapDocument()
		{
			// Arrange
			const string input = "  \n\n  <?xml version=\"1.0\"?>\n" +
				"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\">\n" +
				"	<soap:Header>\n" +
				"	</soap:Header>\n" +
				"	<soap:Body>\n" +
				"		<m:GetStockPrice xmlns:m=\"http://www.example.com/stock\">\n" +
				"			<m:StockName>MSFT</m:StockName>\n" +
				"		</m:GetStockPrice>\n" +
				"	</soap:Body>\n" +
				"</soap:Envelope>	   \n  "
				;
			const string targetOutputA = input;
			const string targetOutputB = input;
			const string targetOutputC = "<?xml version=\"1.0\"?>" +
				"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\">" +
				"<soap:Header>\n" +
				"	</soap:Header>" +
				"<soap:Body>" +
				"<m:GetStockPrice xmlns:m=\"http://www.example.com/stock\">" +
				"<m:StockName>MSFT</m:StockName>" +
				"</m:GetStockPrice>" +
				"</soap:Body>" +
				"</soap:Envelope>"
				;
			const string targetOutputD = "<?xml version=\"1.0\"?>\n" +
				"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\">\n" +
				"<soap:Header>\n" +
				"	</soap:Header>\n" +
				"<soap:Body>\n" +
				"<m:GetStockPrice xmlns:m=\"http://www.example.com/stock\">\n" +
				"<m:StockName>MSFT</m:StockName>\n" +
				"</m:GetStockPrice>\n" +
				"</soap:Body>\n" +
				"</soap:Envelope>"
				;

			// Act
			string outputA = _keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = _keepingWhitespaceAndNewLinesMinifier.Minify(input).MinifiedContent;
			string outputC = _removingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = _removingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
		}

		[Fact]
		public void WhitespaceMinificationInMathMlDocument()
		{
			// Arrange
			const string input1 = "<?xml version=\"1.0\" standalone=\"no\"?>\n" +
				"<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" " +
				"\"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">\n" +
				"<math xmlns=\"http://www.w3.org/1998/Math/MathML\">\n" +
				"	<mrow>\n" +
				"		<mi>a</mi>\n" +
				"		<mo>&InvisibleTimes;</mo>\n" +
				"		<msup>\n" +
				"			<mi>x</mi>\n" +
				"			<mn>2</mn>\n" +
				"		</msup>\n" +
				"		<mo>+</mo>\n" +
				"		<mi>b</mi>\n" +
				"		<mo>&InvisibleTimes; </mo>\n" +
				"		<mi>x</mi>\n" +
				"		<mo>+</mo>\n" +
				"		<mi>c</mi>\n" +
				"	</mrow>\n" +
				"</math>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = input1;
			const string targetOutput1C = "<?xml version=\"1.0\" standalone=\"no\"?>" +
				"<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" " +
				"\"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">" +
				"<math xmlns=\"http://www.w3.org/1998/Math/MathML\">" +
				"<mrow>" +
				"<mi>a</mi>" +
				"<mo>&InvisibleTimes;</mo>" +
				"<msup>" +
				"<mi>x</mi>" +
				"<mn>2</mn>" +
				"</msup>" +
				"<mo>+</mo>" +
				"<mi>b</mi>" +
				"<mo>&InvisibleTimes; </mo>" +
				"<mi>x</mi>" +
				"<mo>+</mo>" +
				"<mi>c</mi>" +
				"</mrow>" +
				"</math>"
				;
			const string targetOutput1D = "<?xml version=\"1.0\" standalone=\"no\"?>\n" +
				"<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" " +
				"\"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">\n" +
				"<math xmlns=\"http://www.w3.org/1998/Math/MathML\">\n" +
				"<mrow>\n" +
				"<mi>a</mi>\n" +
				"<mo>&InvisibleTimes;</mo>\n" +
				"<msup>\n" +
				"<mi>x</mi>\n" +
				"<mn>2</mn>\n" +
				"</msup>\n" +
				"<mo>+</mo>\n" +
				"<mi>b</mi>\n" +
				"<mo>&InvisibleTimes; </mo>\n" +
				"<mi>x</mi>\n" +
				"<mo>+</mo>\n" +
				"<mi>c</mi>\n" +
				"</mrow>\n" +
				"</math>"
				;

			const string input2 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
				"<!DOCTYPE math PUBLIC \"-//W3C//DTD MathML 2.0//EN\" " +
				"\"http://www.w3.org/Math/DTD/mathml2/mathml2.dtd\">\n" +
				"<mrow>\n" +
				"	a &InvisibleTimes; <msup>x 2</msup>\n" +
				"	+ b &InvisibleTimes; x\n" +
				"	+ c\n" +
				"</mrow>\n"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = input2;
			const string targetOutput2C = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
				"<!DOCTYPE math PUBLIC \"-//W3C//DTD MathML 2.0//EN\" " +
				"\"http://www.w3.org/Math/DTD/mathml2/mathml2.dtd\">" +
				"<mrow>\n" +
				"	a &InvisibleTimes; <msup>x 2</msup>\n" +
				"	+ b &InvisibleTimes; x\n" +
				"	+ c\n" +
				"</mrow>"
				;
			const string targetOutput2D = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
				"<!DOCTYPE math PUBLIC \"-//W3C//DTD MathML 2.0//EN\" " +
				"\"http://www.w3.org/Math/DTD/mathml2/mathml2.dtd\">\n" +
				"<mrow>\n" +
				"	a &InvisibleTimes; <msup>x 2</msup>\n" +
				"	+ b &InvisibleTimes; x\n" +
				"	+ c\n" +
				"</mrow>";

			// Act
			string output1A = _keepingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1B = _keepingWhitespaceAndNewLinesMinifier.Minify(input1).MinifiedContent;
			string output1C = _removingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1D = _removingWhitespaceExceptForNewLinesMinifier.Minify(input1).MinifiedContent;

			string output2A = _keepingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2B = _keepingWhitespaceAndNewLinesMinifier.Minify(input2).MinifiedContent;
			string output2C = _removingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2D = _removingWhitespaceExceptForNewLinesMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);
			Assert.Equal(targetOutput1D, output1D);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);
			Assert.Equal(targetOutput2D, output2D);
		}

		#region IDisposable implementation

		public void Dispose()
		{
			_keepingWhitespaceMinifier = null;
			_keepingWhitespaceAndNewLinesMinifier = null;
			_removingWhitespaceMinifier = null;
			_removingWhitespaceExceptForNewLinesMinifier = null;
		}

		#endregion
	}
}