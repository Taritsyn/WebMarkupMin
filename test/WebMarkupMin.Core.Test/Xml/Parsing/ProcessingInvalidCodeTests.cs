using System.Collections.Generic;

using Xunit;

namespace WebMarkupMin.Core.Test.Xml.Parsing
{
	public class ProcessingInvalidCodeTests
	{
		[Fact]
		public void ProcessingInvalidCharactersInXmlDeclarationIsCorrect()
		{
			// Arrange
			var minifier = new XmlMinifier(new XmlMinificationSettings(true));

			const string input1 = "<?xml version=\"1.0\" encoding=\"UTF-8 standalone=\"yes\"?>";
			const string input2 = "<?xml version=\"1.1\" ,@$! encoding=\"UTF-8\" ?>";

			// Act
			IList<MinificationErrorInfo> errors1 = minifier.Minify(input1).Errors;
			IList<MinificationErrorInfo> errors2 = minifier.Minify(input2).Errors;

			// Assert
			Assert.Equal(1, errors1.Count);
			Assert.Equal(1, errors1[0].LineNumber);
			Assert.Equal(49, errors1[0].ColumnNumber);

			Assert.Equal(1, errors2.Count);
			Assert.Equal(1, errors2[0].LineNumber);
			Assert.Equal(21, errors2[0].ColumnNumber);
		}

		[Fact]
		public void ProcessingInvalidCharactersInProcessingInstructionIsCorrect()
		{
			// Arrange
			var minifier = new XmlMinifier(new XmlMinificationSettings(true));

			const string input1 = "<?xml-stylesheet type=\"text/css\" href=\"myStyleSheet.css?>";
			const string input2 = "<?xml-stylesheet type=\"text/xsl\" rel=stylesheet href=\"transform.xsl\"?>";

			// Act
			IList<MinificationErrorInfo> errors1 = minifier.Minify(input1).Errors;
			IList<MinificationErrorInfo> errors2 = minifier.Minify(input2).Errors;

			// Assert
			Assert.Equal(1, errors1.Count);
			Assert.Equal(1, errors1[0].LineNumber);
			Assert.Equal(34, errors1[0].ColumnNumber);

			Assert.Equal(1, errors2.Count);
			Assert.Equal(1, errors2[0].LineNumber);
			Assert.Equal(34, errors2[0].ColumnNumber);
		}

		[Fact]
		public void ProcessingInvalidDoctypeIsCorrect()
		{
			// Arrange
			var minifier = new XmlMinifier(new XmlMinificationSettings(true));

			const string input1 = "<!DOCTYPErecipe>";
			const string input2 = "<!doctype recipe>";
			const string input3 = "<!doctyperecipe>";

			// Act
			IList<MinificationErrorInfo> errors1 = minifier.Minify(input1).Errors;
			IList<MinificationErrorInfo> errors2 = minifier.Minify(input2).Errors;
			IList<MinificationErrorInfo> errors3 = minifier.Minify(input3).Errors;

			// Assert
			Assert.Equal(1, errors1.Count);
			Assert.Equal(1, errors1[0].LineNumber);
			Assert.Equal(1, errors1[0].ColumnNumber);

			Assert.Equal(1, errors2.Count);
			Assert.Equal(1, errors2[0].LineNumber);
			Assert.Equal(1, errors2[0].ColumnNumber);

			Assert.Equal(1, errors3.Count);
			Assert.Equal(1, errors3[0].LineNumber);
			Assert.Equal(1, errors3[0].ColumnNumber);
		}

		[Fact]
		public void ProcessingInvalidTagDeclaration()
		{
			// Arrange
			var minifier = new XmlMinifier(new XmlMinificationSettings(true));

			const string input1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
				"	<to>Vasya</to>\n" +
				"	<from>Petya</from>\n" +
				"	<subject>Meeting of graduates</subject>\n" +
				"	<body>Hi! How are you?</body>\n" +
				"</message>"
				;
			const string input2 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
				"<message>\n" +
				"	<to>Vasya</to>\n" +
				"	<from>Petya</from>\n" +
				"	<subject>Meeting of graduates</subject>\n" +
				"	<body>Hi! How are you?</body>"
				;
			const string input3 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
				"<message>\n" +
				"	<to>Vasya</to>\n" +
				"	<from>Petya</from>\n" +
				"	<subject>Meeting of graduates" +
				"	<body>Hi! How are you?</body>\n" +
				"</message>"
				;
			const string input4 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
				"<message>\n" +
				"	<to>Vasya</to>\n" +
				"	Petya</from>\n" +
				"	<subject>Meeting of graduates</subject>\n" +
				"	<body>Hi! How are you?</body>\n" +
				"</message>"
				;

			// Act
			IList<MinificationErrorInfo> errors1 = minifier.Minify(input1).Errors;
			IList<MinificationErrorInfo> errors2 = minifier.Minify(input2).Errors;
			IList<MinificationErrorInfo> errors3 = minifier.Minify(input3).Errors;
			IList<MinificationErrorInfo> errors4 = minifier.Minify(input4).Errors;

			// Assert
			Assert.Equal(1, errors1.Count);
			Assert.Equal(6, errors1[0].LineNumber);
			Assert.Equal(1, errors1[0].ColumnNumber);

			Assert.Equal(1, errors2.Count);
			Assert.Equal(2, errors2[0].LineNumber);
			Assert.Equal(10, errors2[0].ColumnNumber);

			Assert.Equal(1, errors3.Count);
			Assert.Equal(5, errors3[0].LineNumber);
			Assert.Equal(11, errors3[0].ColumnNumber);

			Assert.Equal(1, errors4.Count);
			Assert.Equal(4, errors4[0].LineNumber);
			Assert.Equal(7, errors4[0].ColumnNumber);
		}

		[Fact]
		public void ProcessingInvalidCharactersInStartTagIsCorrect()
		{
			// Arrange
			var minifier = new XmlMinifier(new XmlMinificationSettings(true));

			const string input1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
				"<recipe type=\"dessert\">\n" +
				"	<recipename cuisine=\"american servings=\"1\">Ice Cream Sundae</recipename>\n" +
				"	<preptime>5 minutes</preptime>\n" +
				"</recipe>"
				;
			const string input2 = "<query key=\"1178517\" enable-multiple-hits=\"true\" " +
				"secondary-query=\"author-title\">\n" +
				"	<journal_title match=\"fuzzy\">American Journal of Bioethics</journal_title>\n" +
				"	<author match=\"fuzzy\" '  search-all-authors=\"true\">Agich</author>\n" +
				"	<volume match=\"fuzzy\">1</volume>\n" +
				"	<issue>1</issue>\n" +
				"	<first_page>50</first_page>\n" +
				"	<year>2001</year>\n" +
				"	<article_title>The Salience of Narrative for Bioethics</article_title>\n" +
				"</query>"
				;

			// Act
			IList<MinificationErrorInfo> errors1 = minifier.Minify(input1).Errors;
			IList<MinificationErrorInfo> errors2 = minifier.Minify(input2).Errors;

			// Assert
			Assert.Equal(1, errors1.Count);
			Assert.Equal(3, errors1[0].LineNumber);
			Assert.Equal(42, errors1[0].ColumnNumber);

			Assert.Equal(1, errors2.Count);
			Assert.Equal(3, errors2[0].LineNumber);
			Assert.Equal(24, errors2[0].ColumnNumber);
		}

		[Fact]
		public void ProcessingInvalidXmlCommentsIsCorrect()
		{
			// Arrange
			var minifier = new XmlMinifier(new XmlMinificationSettings(true));

			const string input1 = "<!-->";

			// Act
			IList<MinificationErrorInfo> errors1 = minifier.Minify(input1).Errors;

			// Assert
			Assert.Equal(1, errors1.Count);
			Assert.Equal(1, errors1[0].LineNumber);
			Assert.Equal(1, errors1[0].ColumnNumber);
		}
	}
}