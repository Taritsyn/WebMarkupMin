using Xunit;

using WebMarkupMin.Core.Parsers;

namespace WebMarkupMin.Tests.Xhtml.Common.Parsing
{
	public class ParsingDoctypesTests
	{
		private static HtmlDoctype ParseDoctype(string doctypeString)
		{
			HtmlDoctype result = null;
			var parser = new HtmlParser(new HtmlParsingHandlers
			{
				Doctype = (MarkupParsingContext context, HtmlDoctype doctype) => {
					result = doctype;
				}
			});
			parser.Parse(doctypeString);

			return result;
		}

		[Fact]
		public void ParsingXhtml10StrictDoctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\"" +
				" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "XHTML 1.0 Strict", "EN"),
				new HtmlSystemId("http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd")
			);

			const string input2 = "<!DOCTYPE  html  PUBLIC  '-//W3C//DTD XHTML 1.0 Strict//EN'" +
				"  'http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd' >";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "XHTML 1.0 Strict", "EN", '\''),
				new HtmlSystemId("http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd", '\'')
			);

			// Act
			HtmlDoctype output1 = ParseDoctype(input1);
			HtmlDoctype output2 = ParseDoctype(input2);

			// Assert
			Assert.Equivalent(targetOutput1, output1, true);
			Assert.Equivalent(targetOutput2, output2, true);
		}

		[Fact]
		public void ParsingXhtml10TransitionalDoctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\"" +
				" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "XHTML 1.0 Transitional", "EN"),
				new HtmlSystemId("http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd")
			);

			const string input2 = "<!DOCTYPE\thtml\tPUBLIC\t\"-//W3C//DTD XHTML 1.0 Transitional//EN\"" +
				"\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"\t>";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "XHTML 1.0 Transitional", "EN"),
				new HtmlSystemId("http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd")
			);

			// Act
			HtmlDoctype output1 = ParseDoctype(input1);
			HtmlDoctype output2 = ParseDoctype(input2);

			// Assert
			Assert.Equivalent(targetOutput1, output1, true);
			Assert.Equivalent(targetOutput2, output2, true);
		}

		[Fact]
		public void ParsingXhtml10FramesetDoctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Frameset//EN\"" +
				" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "XHTML 1.0 Frameset", "EN"),
				new HtmlSystemId("http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd")
			);

			const string input2 = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Frameset//EN'" +
				"\t\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd\"" +
				"\t>";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "XHTML 1.0 Frameset", "EN", '\''),
				new HtmlSystemId("http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd")
			);

			// Act
			HtmlDoctype output1 = ParseDoctype(input1);
			HtmlDoctype output2 = ParseDoctype(input2);

			// Assert
			Assert.Equivalent(targetOutput1, output1, true);
			Assert.Equivalent(targetOutput2, output2, true);
		}

		[Fact]
		public void ParsingXhtmlBasic10Doctype()
		{
			// Arrange
			const string input = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML Basic 1.0//EN\"" +
				" \"http://www.w3.org/TR/xhtml-basic/xhtml-basic10.dtd\">";
			HtmlDoctype targetOutput = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "XHTML Basic 1.0", "EN"),
				new HtmlSystemId("http://www.w3.org/TR/xhtml-basic/xhtml-basic10.dtd")
			);

			// Act
			HtmlDoctype output = ParseDoctype(input);

			// Assert
			Assert.Equivalent(targetOutput, output, true);
		}

		[Fact]
		public void ParsingXhtmlMobile10Doctype()
		{
			// Arrange
			const string input = "<!DOCTYPE html PUBLIC \"-//WAPFORUM//DTD XHTML Mobile 1.0//EN\"" +
				" \"http://www.wapforum.org/DTD/xhtml-mobile10.dtd\">";
			HtmlDoctype targetOutput = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "WAPFORUM", "DTD", "XHTML Mobile 1.0", "EN"),
				new HtmlSystemId("http://www.wapforum.org/DTD/xhtml-mobile10.dtd")
			);

			// Act
			HtmlDoctype output = ParseDoctype(input);

			// Assert
			Assert.Equivalent(targetOutput, output, true);
		}

		[Fact]
		public void ParsingXhtmlPrint10Doctype()
		{
			// Arrange
			const string input = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML-Print 1.0//EN\"" +
				" \"http://www.w3.org/TR/xhtml-print/xhtml-print10.dtd\">";
			HtmlDoctype targetOutput = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "XHTML-Print 1.0", "EN"),
				new HtmlSystemId("http://www.w3.org/TR/xhtml-print/xhtml-print10.dtd")
			);

			// Act
			HtmlDoctype output = ParseDoctype(input);

			// Assert
			Assert.Equivalent(targetOutput, output, true);
		}

		[Fact]
		public void ParsingXhtml11Doctype()
		{
			// Arrange
			const string input = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\"" +
				" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">";
			HtmlDoctype targetOutput = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "XHTML 1.1", "EN"),
				new HtmlSystemId("http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd")
			);

			// Act
			HtmlDoctype output = ParseDoctype(input);

			// Assert
			Assert.Equivalent(targetOutput, output, true);
		}

		[Fact]
		public void ParsingXhtml11AriaDoctype()
		{
			// Arrange
			const string input = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML+ARIA 1.0//EN\"" +
				" \"http://www.w3.org/WAI/ARIA/schemata/xhtml-aria-1.dtd\">";
			HtmlDoctype targetOutput = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "XHTML+ARIA 1.0", "EN"),
				new HtmlSystemId("http://www.w3.org/WAI/ARIA/schemata/xhtml-aria-1.dtd")
			);

			// Act
			HtmlDoctype output = ParseDoctype(input);

			// Assert
			Assert.Equivalent(targetOutput, output, true);
		}

		[Fact]
		public void ParsingXhtml11RdfaDoctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML+RDFa 1.0//EN\"" +
				" \"http://www.w3.org/MarkUp/DTD/xhtml-rdfa-1.dtd\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "XHTML+RDFa 1.0", "EN"),
				new HtmlSystemId("http://www.w3.org/MarkUp/DTD/xhtml-rdfa-1.dtd")
			);

			const string input2 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML+RDFa 1.1//EN\"" +
				" \"http://www.w3.org/MarkUp/DTD/xhtml-rdfa-2.dtd\">";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "XHTML+RDFa 1.1", "EN"),
				new HtmlSystemId("http://www.w3.org/MarkUp/DTD/xhtml-rdfa-2.dtd")
			);

			// Act
			HtmlDoctype output1 = ParseDoctype(input1);
			HtmlDoctype output2 = ParseDoctype(input2);

			// Assert
			Assert.Equivalent(targetOutput1, output1, true);
			Assert.Equivalent(targetOutput2, output2, true);
		}

		[Fact]
		public void ParsingXhtmlBasic11Doctype()
		{
			// Arrange
			const string input = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML Basic 1.1//EN\"" +
				" \"http://www.w3.org/TR/xhtml-basic/xhtml-basic11.dtd\">";
			HtmlDoctype targetOutput = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "XHTML Basic 1.1", "EN"),
				new HtmlSystemId("http://www.w3.org/TR/xhtml-basic/xhtml-basic11.dtd")
			);

			// Act
			HtmlDoctype output = ParseDoctype(input);

			// Assert
			Assert.Equivalent(targetOutput, output, true);
		}

		[Fact]
		public void ParsingXhtmlMobile11Doctype()
		{
			// Arrange
			const string input = "<!DOCTYPE html PUBLIC \"-//WAPFORUM//DTD XHTML Mobile 1.1//EN\"" +
				" \"http://www.openmobilealliance.org/tech/DTD/xhtml-mobile11.dtd\">";
			HtmlDoctype targetOutput = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "WAPFORUM", "DTD", "XHTML Mobile 1.1", "EN"),
				new HtmlSystemId("http://www.openmobilealliance.org/tech/DTD/xhtml-mobile11.dtd")
			);

			// Act
			HtmlDoctype output = ParseDoctype(input);

			// Assert
			Assert.Equivalent(targetOutput, output, true);
		}

		[Fact]
		public void ParsingXhtmlMobile12Doctype()
		{
			// Arrange
			const string input = "<!DOCTYPE html PUBLIC \"-//WAPFORUM//DTD XHTML Mobile 1.2//EN\"" +
				" \"http://www.openmobilealliance.org/tech/DTD/xhtml-mobile12.dtd\">";
			HtmlDoctype targetOutput = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "WAPFORUM", "DTD", "XHTML Mobile 1.2", "EN"),
				new HtmlSystemId("http://www.openmobilealliance.org/tech/DTD/xhtml-mobile12.dtd")
			);

			// Act
			HtmlDoctype output = ParseDoctype(input);

			// Assert
			Assert.Equivalent(targetOutput, output, true);
		}

		[Fact]
		public void ParsingXhtml20Doctype()
		{
			// Arrange
			const string input = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 2.0//EN\"" +
				" \"http://www.w3.org/TR/xhtml2/DTD/xhtml2.dtd\">";
			HtmlDoctype targetOutput = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "XHTML 2.0", "EN"),
				new HtmlSystemId("http://www.w3.org/TR/xhtml2/DTD/xhtml2.dtd")
			);

			// Act
			HtmlDoctype output = ParseDoctype(input);

			// Assert
			Assert.Equivalent(targetOutput, output, true);
		}

		[Fact]
		public void ParsingXhtml5Doctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE html>";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "html");

			const string input2 = "<!DOCTYPE html SYSTEM \"about:legacy-compat\">";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "html", "SYSTEM",
				new HtmlSystemId("about:legacy-compat")
			);

			const string input3 = "<!DOCTYPE html SYSTEM 'about:legacy-compat'>";
			HtmlDoctype targetOutput3 = new HtmlDoctype("DOCTYPE", true, "html", "SYSTEM",
				new HtmlSystemId("about:legacy-compat", '\'')
			);

			// Act
			HtmlDoctype output1 = ParseDoctype(input1);
			HtmlDoctype output2 = ParseDoctype(input2);
			HtmlDoctype output3 = ParseDoctype(input3);

			// Assert
			Assert.Equivalent(targetOutput1, output1, true);
			Assert.Equivalent(targetOutput2, output2, true);
			Assert.Equivalent(targetOutput3, output3, true);
		}

		[Fact]
		public void ParsingOldXhtmlVendorDoctype()
		{
			// Arrange
			const string input = "<!DOCTYPE html SYSTEM" +
				" \"http://www.ibm.com/data/dtd/v11/ibmxhtml1-transitional.dtd\">";
			HtmlDoctype targetOutput = new HtmlDoctype("DOCTYPE", true, "html", "SYSTEM",
				new HtmlSystemId("http://www.ibm.com/data/dtd/v11/ibmxhtml1-transitional.dtd")
			);

			// Act
			HtmlDoctype output = ParseDoctype(input);

			// Assert
			Assert.Equivalent(targetOutput, output, true);
		}
	}
}