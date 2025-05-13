using System;

using Xunit;

using WebMarkupMin.Core.Parsers;

namespace WebMarkupMin.Tests.Html.Common.Parsing
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
		public void ParsingHtml20Doctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML//EN\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML", "EN")
			);

			const string input2 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML//EN//2.0\">";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML", "EN", "2.0")
			);

			const string input3 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML Level 0//EN\">";
			HtmlDoctype targetOutput3 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML Level 0", "EN")
			);

			const string input4 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML Level 0//EN//2.0\">";
			HtmlDoctype targetOutput4 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML Level 0", "EN", "2.0")
			);

			const string input5 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML Level 1//EN\">";
			HtmlDoctype targetOutput5 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML Level 1", "EN")
			);

			const string input6 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML Level 1//EN//2.0\">";
			HtmlDoctype targetOutput6 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML Level 1", "EN", "2.0")
			);

			const string input7 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML Level 2//EN\">";
			HtmlDoctype targetOutput7 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML Level 2", "EN")
			);

			const string input8 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML Level 2//EN//2.0\">";
			HtmlDoctype targetOutput8 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML Level 2", "EN", "2.0")
			);

			const string input9 = "<!DOCTYPE html PUBLIC \"-//IETF//DTD HTML 2.0//EN\">";
			HtmlDoctype targetOutput9 = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML 2.0", "EN")
			);

			const string input10 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML 2.0//EN\">";
			HtmlDoctype targetOutput10 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML 2.0", "EN")
			);

			const string input11 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML 2.0 Level 1//EN\">";
			HtmlDoctype targetOutput11 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML 2.0 Level 1", "EN")
			);

			const string input12 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML 2.0 Level 2//EN\">";
			HtmlDoctype targetOutput12 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML 2.0 Level 2", "EN")
			);

			// Act
			HtmlDoctype output1 = ParseDoctype(input1);
			HtmlDoctype output2 = ParseDoctype(input2);
			HtmlDoctype output3 = ParseDoctype(input3);
			HtmlDoctype output4 = ParseDoctype(input4);
			HtmlDoctype output5 = ParseDoctype(input5);
			HtmlDoctype output6 = ParseDoctype(input6);
			HtmlDoctype output7 = ParseDoctype(input7);
			HtmlDoctype output8 = ParseDoctype(input8);
			HtmlDoctype output9 = ParseDoctype(input9);
			HtmlDoctype output10 = ParseDoctype(input10);
			HtmlDoctype output11 = ParseDoctype(input11);
			HtmlDoctype output12 = ParseDoctype(input12);

			// Assert
			Assert.Equivalent(targetOutput1, output1, true);
			Assert.Equivalent(targetOutput2, output2, true);
			Assert.Equivalent(targetOutput3, output3, true);
			Assert.Equivalent(targetOutput4, output4, true);
			Assert.Equivalent(targetOutput5, output5, true);
			Assert.Equivalent(targetOutput6, output6, true);
			Assert.Equivalent(targetOutput7, output7, true);
			Assert.Equivalent(targetOutput8, output8, true);
			Assert.Equivalent(targetOutput9, output9, true);
			Assert.Equivalent(targetOutput10, output10, true);
			Assert.Equivalent(targetOutput11, output11, true);
			Assert.Equivalent(targetOutput12, output12, true);

		}

		[Fact]
		public void ParsingHtml20StrictDoctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML Strict//EN\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML Strict", "EN")
			);

			const string input2 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML Strict//EN//2.0\">";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML Strict", "EN", "2.0")
			);

			const string input3 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML Strict Level 0//EN\">";
			HtmlDoctype targetOutput3 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML Strict Level 0", "EN")
			);

			const string input4 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML Strict Level 0//EN//2.0\">";
			HtmlDoctype targetOutput4 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML Strict Level 0", "EN", "2.0")
			);

			const string input5 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML Strict Level 1//EN\">";
			HtmlDoctype targetOutput5 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML Strict Level 1", "EN")
			);

			const string input6 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML Strict Level 1//EN//2.0\">";
			HtmlDoctype targetOutput6 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML Strict Level 1", "EN", "2.0")
			);

			const string input7 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML Strict Level 2//EN\">";
			HtmlDoctype targetOutput7 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML Strict Level 2", "EN")
			);

			const string input8 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML Strict Level 2//EN//2.0\">";
			HtmlDoctype targetOutput8 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML Strict Level 2", "EN", "2.0")
			);

			const string input9 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML 2.0 Strict//EN\">";
			HtmlDoctype targetOutput9 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML 2.0 Strict", "EN")
			);

			const string input10 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML 2.0 Strict Level 1//EN\">";
			HtmlDoctype targetOutput10 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML 2.0 Strict Level 1", "EN")
			);

			const string input11 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML 2.0 Strict Level 2//EN\">";
			HtmlDoctype targetOutput11 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML 2.0 Strict Level 2", "EN")
			);

			// Act
			HtmlDoctype output1 = ParseDoctype(input1);
			HtmlDoctype output2 = ParseDoctype(input2);
			HtmlDoctype output3 = ParseDoctype(input3);
			HtmlDoctype output4 = ParseDoctype(input4);
			HtmlDoctype output5 = ParseDoctype(input5);
			HtmlDoctype output6 = ParseDoctype(input6);
			HtmlDoctype output7 = ParseDoctype(input7);
			HtmlDoctype output8 = ParseDoctype(input8);
			HtmlDoctype output9 = ParseDoctype(input9);
			HtmlDoctype output10 = ParseDoctype(input10);
			HtmlDoctype output11 = ParseDoctype(input11);

			// Assert
			Assert.Equivalent(targetOutput1, output1, true);
			Assert.Equivalent(targetOutput2, output2, true);
			Assert.Equivalent(targetOutput3, output3, true);
			Assert.Equivalent(targetOutput4, output4, true);
			Assert.Equivalent(targetOutput5, output5, true);
			Assert.Equivalent(targetOutput6, output6, true);
			Assert.Equivalent(targetOutput7, output7, true);
			Assert.Equivalent(targetOutput8, output8, true);
			Assert.Equivalent(targetOutput9, output9, true);
			Assert.Equivalent(targetOutput10, output10, true);
			Assert.Equivalent(targetOutput11, output11, true);
		}

		[Fact]
		public void ParsingHtml21Doctype()
		{
			// Arrange
			const string input = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML 2.1E//EN\">";
			HtmlDoctype targetOutput = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML 2.1E", "EN")
			);

			// Act
			HtmlDoctype output = ParseDoctype(input);

			// Assert
			Assert.Equivalent(targetOutput, output, true);
		}

		[Fact]
		public void ParsingHtml30Doctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML//EN//3.0\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML", "EN", "3.0")
			);

			const string input2 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML Level 3//EN\">";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML Level 3", "EN")
			);

			const string input3 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML Level 3//EN//3.0\">";
			HtmlDoctype targetOutput3 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML Level 3", "EN", "3.0")
			);

			const string input4 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML 3//EN\">";
			HtmlDoctype targetOutput4 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML 3", "EN")
			);

			const string input5 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML 3.0//EN\">";
			HtmlDoctype targetOutput5 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML 3.0", "EN")
			);

			const string input6 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML 3.0//EN//\">";
			HtmlDoctype targetOutput6 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML 3.0", "EN")
			);

			const string input7 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 3 1995-03-24//EN\">";
			HtmlDoctype targetOutput7 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 3 1995-03-24", "EN")
			);

			const string input8 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD W3 HTML//EN\">";
			HtmlDoctype targetOutput8 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "W3 HTML", "EN")
			);

			const string input9 = "<!DOCTYPE HTML PUBLIC \"-//W3O//DTD W3 HTML 3.0//EN\">";
			HtmlDoctype targetOutput9 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3O", "DTD", "W3 HTML 3.0", "EN")
			);

			const string input10 = "<!DOCTYPE HTML PUBLIC \"-//W3O//DTD W3 HTML 3.0//EN//\">";
			HtmlDoctype targetOutput10 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3O", "DTD", "W3 HTML 3.0", "EN")
			);

			// Act
			HtmlDoctype output1 = ParseDoctype(input1);
			HtmlDoctype output2 = ParseDoctype(input2);
			HtmlDoctype output3 = ParseDoctype(input3);
			HtmlDoctype output4 = ParseDoctype(input4);
			HtmlDoctype output5 = ParseDoctype(input5);
			HtmlDoctype output6 = ParseDoctype(input6);
			HtmlDoctype output7 = ParseDoctype(input7);
			HtmlDoctype output8 = ParseDoctype(input8);
			HtmlDoctype output9 = ParseDoctype(input9);
			HtmlDoctype output10 = ParseDoctype(input10);

			// Assert
			Assert.Equivalent(targetOutput1, output1, true);
			Assert.Equivalent(targetOutput2, output2, true);
			Assert.Equivalent(targetOutput3, output3, true);
			Assert.Equivalent(targetOutput4, output4, true);
			Assert.Equivalent(targetOutput5, output5, true);
			Assert.Equivalent(targetOutput6, output6, true);
			Assert.Equivalent(targetOutput7, output7, true);
			Assert.Equivalent(targetOutput8, output8, true);
			Assert.Equivalent(targetOutput9, output9, true);
			Assert.Equivalent(targetOutput10, output10, true);
		}

		[Fact]
		public void ParsingHtml30StrictDoctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML Strict//EN//3.0\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML Strict", "EN", "3.0")
			);

			const string input2 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML Strict Level 3//EN\">";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML Strict Level 3", "EN")
			);

			const string input3 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML Strict Level 3//EN//3.0\">";
			HtmlDoctype targetOutput3 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML Strict Level 3", "EN", "3.0")
			);

			const string input4 = "<!DOCTYPE HTML PUBLIC \"-//W3O//DTD W3 HTML Strict 3.0//EN//\">";
			HtmlDoctype targetOutput4 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3O", "DTD", "W3 HTML Strict 3.0", "EN")
			);

			// Act
			HtmlDoctype output1 = ParseDoctype(input1);
			HtmlDoctype output2 = ParseDoctype(input2);
			HtmlDoctype output3 = ParseDoctype(input3);
			HtmlDoctype output4 = ParseDoctype(input4);

			// Assert
			Assert.Equivalent(targetOutput1, output1, true);
			Assert.Equivalent(targetOutput2, output2, true);
			Assert.Equivalent(targetOutput3, output3, true);
			Assert.Equivalent(targetOutput4, output4, true);
		}

		[Fact]
		public void ParsingHtml30ExperimentalDoctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML i18n//EN\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML i18n", "EN")
			);

			const string input2 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML Experimental 19960712//EN\">";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML Experimental 19960712", "EN")
			);

			// Act
			HtmlDoctype output1 = ParseDoctype(input1);
			HtmlDoctype output2 = ParseDoctype(input2);

			// Assert
			Assert.Equivalent(targetOutput1, output1, true);
			Assert.Equivalent(targetOutput2, output2, true);
		}

		[Fact]
		public void ParsingHtml32Doctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML 3.2//EN\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML 3.2", "EN")
			);

			const string input2 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 3.2//EN\">";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 3.2", "EN")
			);

			const string input3 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 3.2//EN\">";
			HtmlDoctype targetOutput3 = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 3.2", "EN")
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
		public void ParsingHtml32DraftDoctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 3.2 Draft//EN\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 3.2 Draft", "EN")
			);

			const string input2 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 3.2S Draft//EN\">";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 3.2S Draft", "EN")
			);

			// Act
			HtmlDoctype output1 = ParseDoctype(input1);
			HtmlDoctype output2 = ParseDoctype(input2);

			// Assert
			Assert.Equivalent(targetOutput1, output1, true);
			Assert.Equivalent(targetOutput2, output2, true);
		}

		[Fact]
		public void ParsingHtml32FinalDoctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML 3.2 Final//EN\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "IETF", "DTD", "HTML 3.2 Final", "EN")
			);

			const string input2 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 3.2 Final//EN\">";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 3.2 Final", "EN")
			);

			// Act
			HtmlDoctype output1 = ParseDoctype(input1);
			HtmlDoctype output2 = ParseDoctype(input2);

			// Assert
			Assert.Equivalent(targetOutput1, output1, true);
			Assert.Equivalent(targetOutput2, output2, true);
		}

		[Fact]
		public void ParsingHtml40StrictDoctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0//EN\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 4.0", "EN")
			);

			const string input2 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0//EN\"" +
				" \"http://www.w3.org/TR/REC-html40/strict.dtd\">";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 4.0", "EN"),
				new HtmlSystemId("http://www.w3.org/TR/REC-html40/strict.dtd")
			);

			const string input3 = "<!DOCTYPE HTML SYSTEM \"http://www.w3.org/TR/REC-html40/strict.dtd\">";
			HtmlDoctype targetOutput3 = new HtmlDoctype("DOCTYPE", true, "HTML", "SYSTEM",
				new HtmlSystemId("http://www.w3.org/TR/REC-html40/strict.dtd")
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
		public void ParsingHtml40TransitionalDoctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 4.0 Transitional", "EN")
			);

			const string input2 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\"" +
				" \"http://www.w3.org/TR/REC-html40/loose.dtd\">";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 4.0 Transitional", "EN"),
				new HtmlSystemId("http://www.w3.org/TR/REC-html40/loose.dtd")
			);

			// Act
			HtmlDoctype output1 = ParseDoctype(input1);
			HtmlDoctype output2 = ParseDoctype(input2);

			// Assert
			Assert.Equivalent(targetOutput1, output1, true);
			Assert.Equivalent(targetOutput2, output2, true);
		}

		[Fact]
		public void ParsingHtml40FramesetDoctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Frameset//EN\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 4.0 Frameset", "EN")
			);

			const string input2 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Frameset//EN\"" +
				" \"http://www.w3.org/TR/REC-html40/frameset.dtd\">";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 4.0 Frameset", "EN"),
				new HtmlSystemId("http://www.w3.org/TR/REC-html40/frameset.dtd")
			);

			// Act
			HtmlDoctype output1 = ParseDoctype(input1);
			HtmlDoctype output2 = ParseDoctype(input2);

			// Assert
			Assert.Equivalent(targetOutput1, output1, true);
			Assert.Equivalent(targetOutput2, output2, true);
		}

		[Fact]
		public void ParsingHtml40ExperimentalDoctype()
		{
			// Arrange
			const string input = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML Experimental 970421//EN\">";
			HtmlDoctype targetOutput = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML Experimental 970421", "EN")
			);

			// Act
			HtmlDoctype output = ParseDoctype(input);

			// Assert
			Assert.Equivalent(targetOutput, output, true);
		}

		[Fact]
		public void ParsingIsoIec15445DraftDoctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE HTML PUBLIC \"ISO/IEC 15445:1999//DTD HTML//EN\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId(string.Empty, "ISO/IEC 15445:1999", "DTD", "HTML", "EN")
			);

			const string input2 = "<!DOCTYPE HTML PUBLIC \"ISO/IEC 15445:1999//DTD HyperText Markup Language//EN\">";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId(string.Empty, "ISO/IEC 15445:1999", "DTD", "HyperText Markup Language", "EN")
			);

			// Act
			HtmlDoctype output1 = ParseDoctype(input1);
			HtmlDoctype output2 = ParseDoctype(input2);

			// Assert
			Assert.Equivalent(targetOutput1, output1, true);
			Assert.Equivalent(targetOutput2, output2, true);
		}

		[Fact]
		public void ParsingHtml401StrictDoctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 4.01", "EN")
			);

			const string input2 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\"" +
				" \"http://www.w3.org/TR/html4/strict.dtd\">";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 4.01", "EN"),
				new HtmlSystemId("http://www.w3.org/TR/html4/strict.dtd")
			);

			const string input3 = "<!DOCTYPE  html  public  \"-//W3C//DTD HTML 4.01//EN\"" +
				"\"http://www.w3.org/TR/html4/strict.dtd\"  >";
			HtmlDoctype targetOutput3 = new HtmlDoctype("DOCTYPE", true, "html", "public",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 4.01", "EN"),
				new HtmlSystemId("http://www.w3.org/TR/html4/strict.dtd")
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
		public void ParsingHtml401TransitionalDoctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 4.01 Transitional", "EN")
			);

			const string input2 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\"" +
				" \"http://www.w3.org/TR/html4/loose.dtd\">";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 4.01 Transitional", "EN"),
				new HtmlSystemId("http://www.w3.org/TR/html4/loose.dtd")
			);

			const string input3 = "<!DOCTYPE\thtml\tPUBLIC\t'-//W3C//DTD HTML 4.01 Transitional//EN'\n" +
				"'http://www.w3.org/TR/html4/loose.dtd'\t>";
			HtmlDoctype targetOutput3 = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 4.01 Transitional", "EN", '\''),
				new HtmlSystemId("http://www.w3.org/TR/html4/loose.dtd", '\'')
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
		public void ParsingHtml401FramesetDoctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Frameset//EN\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 4.01 Frameset", "EN")
			);

			const string input2 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Frameset//EN\"" +
				" \"http://www.w3.org/TR/html4/frameset.dtd\">";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 4.01 Frameset", "EN"),
				new HtmlSystemId("http://www.w3.org/TR/html4/frameset.dtd")
			);

			const string input3 = "<!DOCTYPE html PubLic \"-//W3C//DTD HTML 4.01 Frameset//EN\"\r\n" +
				"   'http://www.w3.org/TR/html4/frameset.dtd'\r\n" +
				"   >";
			HtmlDoctype targetOutput3 = new HtmlDoctype("DOCTYPE", true, "html", "PubLic",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 4.01 Frameset", "EN"),
				new HtmlSystemId("http://www.w3.org/TR/html4/frameset.dtd", '\'')
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
		public void ParsingHtml401AriaDoctype()
		{
			// Arrange
			const string input = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML+ARIA 1.0//EN\"" +
				" \"http://www.w3.org/WAI/ARIA/schemata/html4-aria-1.dtd\">";
			HtmlDoctype targetOutput = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML+ARIA 1.0", "EN"),
				new HtmlSystemId("http://www.w3.org/WAI/ARIA/schemata/html4-aria-1.dtd")
			);

			// Act
			HtmlDoctype output = ParseDoctype(input);

			// Assert
			Assert.Equivalent(targetOutput, output, true);
		}

		[Fact]
		public void ParsingHtml401RdfaDoctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01+RDFa 1.0//EN\"" +
				" \"http://www.w3.org/MarkUp/DTD/html401-rdfa-1.dtd\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 4.01+RDFa 1.0", "EN"),
				new HtmlSystemId("http://www.w3.org/MarkUp/DTD/html401-rdfa-1.dtd")
			);

			const string input2 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01+RDFa 1.1//EN\"" +
				" \"http://www.w3.org/MarkUp/DTD/html401-rdfa11-1.dtd\">";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 4.01+RDFa 1.1", "EN"),
				new HtmlSystemId("http://www.w3.org/MarkUp/DTD/html401-rdfa11-1.dtd")
			);

			const string input3 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01+RDFa Lite 1.1//EN\"" +
				" \"http://www.w3.org/MarkUp/DTD/html401-rdfalite11-1.dtd\">";
			HtmlDoctype targetOutput3 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "W3C", "DTD", "HTML 4.01+RDFa Lite 1.1", "EN"),
				new HtmlSystemId("http://www.w3.org/MarkUp/DTD/html401-rdfalite11-1.dtd")
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
		public void ParsingIsoIec15445FinalDoctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE HTML PUBLIC \"ISO/IEC 15445:2000//DTD HTML//EN\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId(string.Empty, "ISO/IEC 15445:2000", "DTD", "HTML", "EN")
			);

			const string input2 = "<!DOCTYPE HTML PUBLIC \"ISO/IEC 15445:2000//DTD HyperText Markup Language//EN\">";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId(string.Empty, "ISO/IEC 15445:2000", "DTD", "HyperText Markup Language", "EN")
			);

			// Act
			HtmlDoctype output1 = ParseDoctype(input1);
			HtmlDoctype output2 = ParseDoctype(input2);

			// Assert
			Assert.Equivalent(targetOutput1, output1, true);
			Assert.Equivalent(targetOutput2, output2, true);
		}

		[Fact]
		public void ParsingHtml5Doctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE HTML>";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "HTML");

			const string input2 = "<!doctype HTML>";
			HtmlDoctype targetOutput2 = new HtmlDoctype("doctype", true, "HTML");

			const string input3 = "<!doctype html>";
			HtmlDoctype targetOutput3 = new HtmlDoctype("doctype", true, "html");

			const string input4 = "<!DoCtYPe hTMl>";
			HtmlDoctype targetOutput4 = new HtmlDoctype("DoCtYPe", true, "hTMl");

			const string input5 = "<!doctypehtml>";
			HtmlDoctype targetOutput5 = new HtmlDoctype("doctype", false, "html");

			const string input6 = "<!DOCTYPE HTML SYSTEM \"about:legacy-compat\">";
			HtmlDoctype targetOutput6 = new HtmlDoctype("DOCTYPE", true, "HTML", "SYSTEM",
				new HtmlSystemId("about:legacy-compat")
			);

			const string input7 = "<!DOCTYPE HTML SYSTEM 'about:legacy-compat'>";
			HtmlDoctype targetOutput7 = new HtmlDoctype("DOCTYPE", true, "HTML", "SYSTEM",
				new HtmlSystemId("about:legacy-compat", '\'')
			);

			// Act
			HtmlDoctype output1 = ParseDoctype(input1);
			HtmlDoctype output2 = ParseDoctype(input2);
			HtmlDoctype output3 = ParseDoctype(input3);
			HtmlDoctype output4 = ParseDoctype(input4);
			HtmlDoctype output5 = ParseDoctype(input5);
			HtmlDoctype output6 = ParseDoctype(input6);
			HtmlDoctype output7 = ParseDoctype(input7);

			// Assert
			Assert.Equivalent(targetOutput1, output1, true);
			Assert.Equivalent(targetOutput2, output2, true);
			Assert.Equivalent(targetOutput3, output3, true);
			Assert.Equivalent(targetOutput4, output4, true);
			Assert.Equivalent(targetOutput5, output5, true);
			Assert.Equivalent(targetOutput6, output6, true);
			Assert.Equivalent(targetOutput7, output7, true);
		}

		[Fact]
		public void ParsingOldHtmlVendorDoctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE HTML PUBLIC \"+//Silmaril//DTD HTML Pro v0r11 19970101//EN\">";
			HtmlDoctype targetOutput1 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("+", "Silmaril", "DTD", "HTML Pro v0r11 19970101", "EN")
			);

			const string input2 = "<!DOCTYPE HTML PUBLIC \"-//AdvaSoft Ltd//DTD HTML 3.0 asWedit + extensions//EN\">";
			HtmlDoctype targetOutput2 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "AdvaSoft Ltd", "DTD", "HTML 3.0 asWedit + extensions", "EN")
			);

			const string input3 = "<!DOCTYPE HTML PUBLIC \"-//AS//DTD HTML 3.0 asWedit + extensions//EN\">";
			HtmlDoctype targetOutput3 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "AS", "DTD", "HTML 3.0 asWedit + extensions", "EN")
			);

			const string input4 = "<!DOCTYPE html PUBLIC \"-//Metrius//DTD Metrius Presentational//EN\">";
			HtmlDoctype targetOutput4 = new HtmlDoctype("DOCTYPE", true, "html", "PUBLIC",
				new HtmlFormalPublicId("-", "Metrius", "DTD", "Metrius Presentational", "EN")
			);

			const string input5 = "<!DOCTYPE HTML PUBLIC \"-//Microsoft//DTD Internet Explorer 2.0 HTML//EN\">";
			HtmlDoctype targetOutput5 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "Microsoft", "DTD", "Internet Explorer 2.0 HTML", "EN")
			);

			const string input6 = "<!DOCTYPE HTML PUBLIC \"-//Microsoft//DTD Internet Explorer 2.0 HTML Strict//EN\">";
			HtmlDoctype targetOutput6 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "Microsoft", "DTD", "Internet Explorer 2.0 HTML Strict", "EN")
			);

			const string input7 = "<!DOCTYPE HTML PUBLIC \"-//Microsoft//DTD Internet Explorer 2.0 Tables//EN\">";
			HtmlDoctype targetOutput7 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "Microsoft", "DTD", "Internet Explorer 2.0 Tables", "EN")
			);

			const string input8 = "<!DOCTYPE HTML PUBLIC \"-//Microsoft//DTD Internet Explorer 3.0 HTML//EN\">";
			HtmlDoctype targetOutput8 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "Microsoft", "DTD", "Internet Explorer 3.0 HTML", "EN")
			);

			const string input9 = "<!DOCTYPE HTML PUBLIC \"-//Microsoft//DTD Internet Explorer 3.0 HTML Strict//EN\">";
			HtmlDoctype targetOutput9 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "Microsoft", "DTD", "Internet Explorer 3.0 HTML Strict", "EN")
			);

			const string input10 = "<!DOCTYPE HTML PUBLIC \"-//Microsoft//DTD Internet Explorer 3.0 Tables//EN\">";
			HtmlDoctype targetOutput10 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "Microsoft", "DTD", "Internet Explorer 3.0 Tables", "EN")
			);

			const string input11 = "<!DOCTYPE HTML PUBLIC \"-//Netscape Comm. Corp.//DTD HTML//EN\">";
			HtmlDoctype targetOutput11 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "Netscape Comm. Corp.", "DTD", "HTML", "EN")
			);

			const string input12 = "<!DOCTYPE HTML PUBLIC \"-//Netscape Comm. Corp.//DTD Strict HTML//EN\">";
			HtmlDoctype targetOutput12 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "Netscape Comm. Corp.", "DTD", "Strict HTML", "EN")
			);

			const string input13 = "<!DOCTYPE HTML PUBLIC \"-//O'Reilly and Associates//DTD HTML Extended 1.0//EN\">";
			HtmlDoctype targetOutput13 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "O'Reilly and Associates", "DTD", "HTML Extended 1.0", "EN")
			);

			const string input14 = "<!DOCTYPE HTML PUBLIC \"-//O'Reilly and Associates//DTD HTML Extended Relaxed 1.0//EN\">";
			HtmlDoctype targetOutput14 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "O'Reilly and Associates", "DTD", "HTML Extended Relaxed 1.0", "EN")
			);

			const string input15 = "<!DOCTYPE HTML PUBLIC \"-//O'Reilly and Associates//DTD HTML 2.0//EN\">";
			HtmlDoctype targetOutput15 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "O'Reilly and Associates", "DTD", "HTML 2.0", "EN")
			);

			const string input16 = "<!DOCTYPE HTML PUBLIC \"-//SoftQuad//DTD HoTMetaL PRO 4.0::19971010::extensions to HTML 4.0//EN\">";
			HtmlDoctype targetOutput16 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "SoftQuad", "DTD", "HoTMetaL PRO 4.0::19971010::extensions to HTML 4.0", "EN")
			);

			const string input17 = "<!DOCTYPE HTML PUBLIC \"-//SoftQuad Software//DTD HoTMetaL PRO 6.0::19990601::extensions to HTML 4.0//EN\">";
			HtmlDoctype targetOutput17 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "SoftQuad Software", "DTD", "HoTMetaL PRO 6.0::19990601::extensions to HTML 4.0", "EN")
			);

			const string input18 = "<!DOCTYPE HTML PUBLIC \"-//Spyglass//DTD HTML 2.0 Extended//EN\">";
			HtmlDoctype targetOutput18 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "Spyglass", "DTD", "HTML 2.0 Extended", "EN")
			);

			const string input19 = "<!DOCTYPE HTML PUBLIC \"-//SQ//DTD HTML 2.0 HoTMetaL + extensions//EN\">";
			HtmlDoctype targetOutput19 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "SQ", "DTD", "HTML 2.0 HoTMetaL + extensions", "EN")
			);

			const string input20 = "<!DOCTYPE HTML PUBLIC \"-//Sun Microsystems Corp.//DTD HotJava HTML//EN\">";
			HtmlDoctype targetOutput20 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "Sun Microsystems Corp.", "DTD", "HotJava HTML", "EN")
			);

			const string input21 = "<!DOCTYPE HTML PUBLIC \"-//Sun Microsystems Corp.//DTD HotJava Strict HTML//EN\">";
			HtmlDoctype targetOutput21 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "Sun Microsystems Corp.", "DTD", "HotJava Strict HTML", "EN")
			);

			const string input22 = "<!DOCTYPE HTML PUBLIC \"-//WebTechs//DTD Mozilla HTML//EN\">";
			HtmlDoctype targetOutput22 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "WebTechs", "DTD", "Mozilla HTML", "EN")
			);

			const string input23 = "<!DOCTYPE HTML PUBLIC \"-//WebTechs//DTD Mozilla HTML 2.0//EN\">";
			HtmlDoctype targetOutput23 = new HtmlDoctype("DOCTYPE", true, "HTML", "PUBLIC",
				new HtmlFormalPublicId("-", "WebTechs", "DTD", "Mozilla HTML 2.0", "EN")
			);

			// Act
			HtmlDoctype output1 = ParseDoctype(input1);
			HtmlDoctype output2 = ParseDoctype(input2);
			HtmlDoctype output3 = ParseDoctype(input3);
			HtmlDoctype output4 = ParseDoctype(input4);
			HtmlDoctype output5 = ParseDoctype(input5);
			HtmlDoctype output6 = ParseDoctype(input6);
			HtmlDoctype output7 = ParseDoctype(input7);
			HtmlDoctype output8 = ParseDoctype(input8);
			HtmlDoctype output9 = ParseDoctype(input9);
			HtmlDoctype output10 = ParseDoctype(input10);
			HtmlDoctype output11 = ParseDoctype(input11);
			HtmlDoctype output12 = ParseDoctype(input12);
			HtmlDoctype output13 = ParseDoctype(input13);
			HtmlDoctype output14 = ParseDoctype(input14);
			HtmlDoctype output15 = ParseDoctype(input15);
			HtmlDoctype output16 = ParseDoctype(input16);
			HtmlDoctype output17 = ParseDoctype(input17);
			HtmlDoctype output18 = ParseDoctype(input18);
			HtmlDoctype output19 = ParseDoctype(input19);
			HtmlDoctype output20 = ParseDoctype(input20);
			HtmlDoctype output21 = ParseDoctype(input21);
			HtmlDoctype output22 = ParseDoctype(input22);
			HtmlDoctype output23 = ParseDoctype(input23);

			// Assert
			Assert.Equivalent(targetOutput1, output1, true);
			Assert.Equivalent(targetOutput2, output2, true);
			Assert.Equivalent(targetOutput3, output3, true);
			Assert.Equivalent(targetOutput4, output4, true);
			Assert.Equivalent(targetOutput5, output5, true);
			Assert.Equivalent(targetOutput6, output6, true);
			Assert.Equivalent(targetOutput7, output7, true);
			Assert.Equivalent(targetOutput8, output8, true);
			Assert.Equivalent(targetOutput9, output9, true);
			Assert.Equivalent(targetOutput10, output10, true);
			Assert.Equivalent(targetOutput11, output11, true);
			Assert.Equivalent(targetOutput12, output12, true);
			Assert.Equivalent(targetOutput13, output13, true);
			Assert.Equivalent(targetOutput14, output14, true);
			Assert.Equivalent(targetOutput15, output15, true);
			Assert.Equivalent(targetOutput16, output16, true);
			Assert.Equivalent(targetOutput17, output17, true);
			Assert.Equivalent(targetOutput18, output18, true);
			Assert.Equivalent(targetOutput19, output19, true);
			Assert.Equivalent(targetOutput20, output20, true);
			Assert.Equivalent(targetOutput21, output21, true);
			Assert.Equivalent(targetOutput22, output22, true);
			Assert.Equivalent(targetOutput23, output23, true);
		}

		[Fact]
		public void ParsingIncompleteDoctypes()
		{
			// Arrange
			const string input1 = "<!DOCTYPE";
			const string input2 = "<!DOCTYPE>";
			const string input3 = "<!DOCTYPE  >";
			const string input4 = "<!DOCTYPE HTML NOT UNDERSTOOD>";
			const string input5 = "<!DOCTYPE HTML PUBLIC>";
			const string input6 = "<!DOCTYPE HTML SYSTEM >";

			// Act
			var exception1 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input1));
			var exception2 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input2));
			var exception3 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input3));
			var exception4 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input4));
			var exception5 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input5));
			var exception6 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input6));

			// Assert
			Assert.Equal("Doctype does not contain a root element.", exception1.Message);
			Assert.Equal(1, exception1.LineNumber);
			Assert.Equal(10, exception1.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE" + Environment.NewLine +
				"-----------------^",
				exception1.SourceFragment
			);

			Assert.Equal("Doctype does not contain a root element.", exception2.Message);
			Assert.Equal(1, exception2.LineNumber);
			Assert.Equal(10, exception2.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE>" + Environment.NewLine +
				"-----------------^",
				exception2.SourceFragment
			);

			Assert.Equal("Doctype does not contain a root element.", exception3.Message);
			Assert.Equal(1, exception3.LineNumber);
			Assert.Equal(12, exception3.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE  >" + Environment.NewLine +
				"-------------------^",
				exception3.SourceFragment
			);

			Assert.Equal("Bogus doctype.", exception4.Message);
			Assert.Equal(1, exception4.LineNumber);
			Assert.Equal(16, exception4.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML NOT UNDERSTOOD>" + Environment.NewLine +
				"-----------------------^",
				exception4.SourceFragment
			);

			Assert.Equal("Expected a formal public identifier but the doctype ended.", exception5.Message);
			Assert.Equal(1, exception5.LineNumber);
			Assert.Equal(22, exception5.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML PUBLIC>" + Environment.NewLine +
				"-----------------------------^",
				exception5.SourceFragment
			);

			Assert.Equal("Expected a system identifier but the doctype ended.", exception6.Message);
			Assert.Equal(1, exception6.LineNumber);
			Assert.Equal(23, exception6.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML SYSTEM >" + Environment.NewLine +
				"------------------------------^",
				exception6.SourceFragment
			);
		}

		[Fact]
		public void ParsingInvalidDoctypesWithPublicId()
		{
			// Arrange
			const string input1 = "<!DOCTYPEHTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
			const string input2 = "<!DOCTYPE HTMLPUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
			const string input3 = "<!DOCTYPE HTML PUBLIC\"-//W3C//DTD HTML 4.0 Transitional//EN\">";
			const string input4 = "<!DOCTYPE HTML PUBLIC'-//W3C//DTD HTML 4.0 Transitional//EN'>";
			const string input5 = "<!DOCTYPE HTML PUBLIC \"\">";
			const string input6 = "<!DOCTYPE HTML PUBLIC ''>";
			const string input7 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN>";
			const string input8 = "<!DOCTYPE HTML PUBLIC '-//W3C//DTD HTML 4.0 Transitional//EN >";
			const string input9 = "<!DOCTYPE HTML PUBLIC \"HTML\">";
			const string input10 = "<!DOCTYPE HTML PUBLIC 'HTML'>";

			// Act
			var exception1 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input1));
			var exception2 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input2));
			var exception3 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input3));
			var exception4 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input4));
			var exception5 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input5));
			var exception6 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input6));
			var exception7 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input7));
			var exception8 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input8));
			var exception9 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input9));
			var exception10 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input10));

			// Assert
			Assert.Equal("Missing space before the doctype root element.", exception1.Message);
			Assert.Equal(1, exception1.LineNumber);
			Assert.Equal(10, exception1.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPEHTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" + Environment.NewLine +
				"-----------------^",
				exception1.SourceFragment
			);

			Assert.Equal("Bogus doctype.", exception2.Message);
			Assert.Equal(1, exception2.LineNumber);
			Assert.Equal(22, exception2.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTMLPUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" + Environment.NewLine +
				"-----------------------------^",
				exception2.SourceFragment
			);

			Assert.Equal("No space between the doctype `PUBLIC` keyword and the quote.", exception3.Message);
			Assert.Equal(1, exception3.LineNumber);
			Assert.Equal(22, exception3.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML PUBLIC\"-//W3C//DTD HTML 4.0 Transitional//EN\">" + Environment.NewLine +
				"-----------------------------^",
				exception3.SourceFragment
			);

			Assert.Equal("No space between the doctype `PUBLIC` keyword and the quote.", exception4.Message);
			Assert.Equal(1, exception4.LineNumber);
			Assert.Equal(22, exception4.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML PUBLIC'-//W3C//DTD HTML 4.0 Transitional//EN'>" + Environment.NewLine +
				"-----------------------------^",
				exception4.SourceFragment
			);

			Assert.Equal("Empty formal public identifier.", exception5.Message);
			Assert.Equal(1, exception5.LineNumber);
			Assert.Equal(24, exception5.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML PUBLIC \"\">" + Environment.NewLine +
				"-------------------------------^",
				exception5.SourceFragment
			);

			Assert.Equal("Empty formal public identifier.", exception6.Message);
			Assert.Equal(1, exception6.LineNumber);
			Assert.Equal(24, exception6.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML PUBLIC ''>" + Environment.NewLine +
				"-------------------------------^",
				exception6.SourceFragment
			);

			Assert.Equal("Invalid formal public identifier.", exception7.Message);
			Assert.Equal(1, exception7.LineNumber);
			Assert.Equal(24, exception7.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN>" + Environment.NewLine +
				"-------------------------------^",
				exception7.SourceFragment
			);

			Assert.Equal("Invalid formal public identifier.", exception8.Message);
			Assert.Equal(1, exception8.LineNumber);
			Assert.Equal(24, exception8.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML PUBLIC '-//W3C//DTD HTML 4.0 Transitional//EN >" + Environment.NewLine +
				"-------------------------------^",
				exception8.SourceFragment
			);

			Assert.Equal("Invalid formal public identifier.", exception9.Message);
			Assert.Equal(1, exception9.LineNumber);
			Assert.Equal(24, exception9.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML PUBLIC \"HTML\">" + Environment.NewLine +
				"-------------------------------^",
				exception9.SourceFragment
			);

			Assert.Equal("Invalid formal public identifier.", exception10.Message);
			Assert.Equal(1, exception10.LineNumber);
			Assert.Equal(24, exception10.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML PUBLIC 'HTML'>" + Environment.NewLine +
				"-------------------------------^",
				exception10.SourceFragment
			);
		}

		[Fact]
		public void ParsingInvalidDoctypesWithSystemId()
		{
			// Arrange
			const string input1 = "<!DOCTYPE HTMLSYSTEM \"about:legacy-compat\">";
			const string input2 = "<!DOCTYPE HTML SYSTEM\"about:legacy-compat\">";
			const string input3 = "<!DOCTYPE HTML SYSTEM'about:legacy-compat'>";
			const string input4 = "<!DOCTYPE HTML SYSTEM \"about:legacy-compat >";
			const string input5 = "<!DOCTYPE HTML SYSTEM 'about:legacy-compat>";
			const string input6 = "<!DOCTYPE HTML SYSTEM \"\">";
			const string input7 = "<!DOCTYPE HTML SYSTEM ''>";
			const string input8 = "<!DOCTYPE HTML SYSTEM \"123\">";
			const string input9 = "<!DOCTYPE HTML SYSTEM '123'>";
			const string input10 = "<!DOCTYPE HTML SYSTEM \"about:legacy-compat\"" +
				" \"http://www.somedomain.org/html5/normal.dtd\">";
			const string input11 = "<!DOCTYPE HTML SYSTEM 'about:legacy-compat'" +
				" 'http://www.somedomain.org/html5/normal.dtd'>";

			// Act
			var exception1 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input1));
			var exception2 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input2));
			var exception3 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input3));
			var exception4 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input4));
			var exception5 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input5));
			var exception6 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input6));
			var exception7 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input7));
			var exception8 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input8));
			var exception9 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input9));
			var exception10 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input10));
			var exception11 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input11));

			// Assert
			Assert.Equal("Bogus doctype.", exception1.Message);
			Assert.Equal(1, exception1.LineNumber);
			Assert.Equal(22, exception1.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTMLSYSTEM \"about:legacy-compat\">" + Environment.NewLine +
				"-----------------------------^",
				exception1.SourceFragment
			);

			Assert.Equal("No space between the doctype `SYSTEM` keyword and the quote.", exception2.Message);
			Assert.Equal(1, exception2.LineNumber);
			Assert.Equal(22, exception2.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML SYSTEM\"about:legacy-compat\">" + Environment.NewLine +
				"-----------------------------^",
				exception2.SourceFragment
			);

			Assert.Equal("No space between the doctype `SYSTEM` keyword and the quote.", exception3.Message);
			Assert.Equal(1, exception3.LineNumber);
			Assert.Equal(22, exception3.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML SYSTEM'about:legacy-compat'>" + Environment.NewLine +
				"-----------------------------^",
				exception3.SourceFragment
			);

			Assert.Equal("Invalid system identifier.", exception4.Message);
			Assert.Equal(1, exception4.LineNumber);
			Assert.Equal(24, exception4.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML SYSTEM \"about:legacy-compat >" + Environment.NewLine +
				"-------------------------------^",
				exception4.SourceFragment
			);

			Assert.Equal("Invalid system identifier.", exception5.Message);
			Assert.Equal(1, exception5.LineNumber);
			Assert.Equal(24, exception5.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML SYSTEM 'about:legacy-compat>" + Environment.NewLine +
				"-------------------------------^",
				exception5.SourceFragment
			);

			Assert.Equal("Empty system identifier.", exception6.Message);
			Assert.Equal(1, exception6.LineNumber);
			Assert.Equal(24, exception6.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML SYSTEM \"\">" + Environment.NewLine +
				"-------------------------------^",
				exception6.SourceFragment
			);

			Assert.Equal("Empty system identifier.", exception7.Message);
			Assert.Equal(1, exception7.LineNumber);
			Assert.Equal(24, exception7.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML SYSTEM ''>" + Environment.NewLine +
				"-------------------------------^",
				exception7.SourceFragment
			);

			Assert.Equal("Invalid system identifier.", exception8.Message);
			Assert.Equal(1, exception8.LineNumber);
			Assert.Equal(24, exception8.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML SYSTEM \"123\">" + Environment.NewLine +
				"-------------------------------^",
				exception8.SourceFragment
			);

			Assert.Equal("Invalid system identifier.", exception9.Message);
			Assert.Equal(1, exception9.LineNumber);
			Assert.Equal(24, exception9.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML SYSTEM '123'>" + Environment.NewLine +
				"-------------------------------^",
				exception9.SourceFragment
			);

			Assert.Equal("Bogus doctype.", exception10.Message);
			Assert.Equal(1, exception10.LineNumber);
			Assert.Equal(45, exception10.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML SYSTEM \"about:legacy-compat\" \"http://www.somedomain.org/html5/normal.dtd\">" + Environment.NewLine +
				"----------------------------------------------------^",
				exception10.SourceFragment
			);

			Assert.Equal("Bogus doctype.", exception11.Message);
			Assert.Equal(1, exception11.LineNumber);
			Assert.Equal(45, exception11.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE HTML SYSTEM 'about:legacy-compat' 'http://www.somedomain.org/html5/normal.dtd'>" + Environment.NewLine +
				"----------------------------------------------------^",
				exception11.SourceFragment
			);
		}

		[Fact]
		public void ParsingInvalidDoctypesWithPublicIdAndSystemId()
		{
			// Arrange
			const string input1 = "<!DOCTYPE htmlPUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\"" +
				" \"http://www.w3.org/TR/html4/loose.dtd\">";
			const string input2 = "<!DOCTYPE html PUBLIC\"-//W3C//DTD HTML 4.01 Transitional//EN\"" +
				" \"http://www.w3.org/TR/html4/loose.dtd\">";
			const string input3 = "<!DOCTYPE html PUBLIC'-//W3C//DTD HTML 4.01 Transitional//EN'" +
				" 'http://www.w3.org/TR/html4/loose.dtd'>";
			const string input4 = "<!DOCTYPE html PUBLIC \"\" \"http://www.w3.org/TR/html4/loose.dtd\">";
			const string input5 = "<!DOCTYPE html PUBLIC '' \"http://www.w3.org/TR/html4/loose.dtd\">";
			const string input6 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"\">";
			const string input7 = "<!DOCTYPE html PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN' ''>";
			const string input8 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN" +
				" \"http://www.w3.org/TR/html4/loose.dtd\">";
			const string input9 = "<!DOCTYPE html PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN" +
				" 'http://www.w3.org/TR/html4/loose.dtd' >";
			const string input10 = "<!DOCTYPE html PUBLIC \"HTML\" \"http://www.w3.org/TR/html4/loose.dtd\">";
			const string input11 = "<!DOCTYPE html PUBLIC 'HTML' 'http://www.w3.org/TR/html4/loose.dtd'>";
			const string input12 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\"" +
				" \"http://www.w3.org/TR/html4/loose.dtd>";
			const string input13 = "<!DOCTYPE html PUBLIC '-//W3C//DTD HTML 4.01" +
				" Transitional//EN' 'http://www.w3.org/TR/html4/loose.dtd >";
			const string input14 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"123\">";
			const string input15 = "<!DOCTYPE html PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN' '123'>";

			// Act
			var exception1 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input1));
			var exception2 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input2));
			var exception3 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input3));
			var exception4 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input4));
			var exception5 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input5));
			var exception6 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input6));
			var exception7 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input7));
			var exception8 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input8));
			var exception9 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input9));
			var exception10 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input10));
			var exception11 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input11));
			var exception12 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input12));
			var exception13 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input13));
			var exception14 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input14));
			var exception15 = Assert.Throws<MarkupParsingException>(() => ParseDoctype(input15));

			// Assert
			Assert.Equal("Bogus doctype.", exception1.Message);
			Assert.Equal(1, exception1.LineNumber);
			Assert.Equal(22, exception1.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE htmlPUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loo…" + Environment.NewLine +
				"-----------------------------^",
				exception1.SourceFragment
			);

			Assert.Equal("No space between the doctype `PUBLIC` keyword and the quote.", exception2.Message);
			Assert.Equal(1, exception2.LineNumber);
			Assert.Equal(22, exception2.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE html PUBLIC\"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loo…" + Environment.NewLine +
				"-----------------------------^",
				exception2.SourceFragment
			);

			Assert.Equal("No space between the doctype `PUBLIC` keyword and the quote.", exception3.Message);
			Assert.Equal(1, exception3.LineNumber);
			Assert.Equal(22, exception3.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE html PUBLIC'-//W3C//DTD HTML 4.01 Transitional//EN' 'http://www.w3.org/TR/html4/loo…" + Environment.NewLine +
				"-----------------------------^",
				exception3.SourceFragment
			);

			Assert.Equal("Empty formal public identifier.", exception4.Message);
			Assert.Equal(1, exception4.LineNumber);
			Assert.Equal(24, exception4.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE html PUBLIC \"\" \"http://www.w3.org/TR/html4/loose.dtd\">" + Environment.NewLine +
				"-------------------------------^",
				exception4.SourceFragment
			);

			Assert.Equal("Empty formal public identifier.", exception5.Message);
			Assert.Equal(1, exception5.LineNumber);
			Assert.Equal(24, exception5.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE html PUBLIC '' \"http://www.w3.org/TR/html4/loose.dtd\">" + Environment.NewLine +
				"-------------------------------^",
				exception5.SourceFragment
			);

			Assert.Equal("Empty system identifier.", exception6.Message);
			Assert.Equal(1, exception6.LineNumber);
			Assert.Equal(65, exception6.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"\">" + Environment.NewLine +
				"------------------------------------------------------------------------^",
				exception6.SourceFragment
			);

			Assert.Equal("Empty system identifier.", exception7.Message);
			Assert.Equal(1, exception7.LineNumber);
			Assert.Equal(65, exception7.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE html PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN' ''>" + Environment.NewLine +
				"------------------------------------------------------------------------^",
				exception7.SourceFragment
			);

			Assert.Equal("Invalid formal public identifier.", exception8.Message);
			Assert.Equal(1, exception8.LineNumber);
			Assert.Equal(24, exception8.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN \"http://www.w3.org/TR/html4/loo…" + Environment.NewLine +
				"-------------------------------^",
				exception8.SourceFragment
			);

			Assert.Equal("Invalid formal public identifier.", exception9.Message);
			Assert.Equal(1, exception9.LineNumber);
			Assert.Equal(24, exception9.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE html PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN 'http://www.w3.org/TR/html4/loo…" + Environment.NewLine +
				"-------------------------------^",
				exception9.SourceFragment
			);

			Assert.Equal("Invalid formal public identifier.", exception10.Message);
			Assert.Equal(1, exception10.LineNumber);
			Assert.Equal(24, exception10.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE html PUBLIC \"HTML\" \"http://www.w3.org/TR/html4/loose.dtd\">" + Environment.NewLine +
				"-------------------------------^",
				exception10.SourceFragment
			);

			Assert.Equal("Invalid formal public identifier.", exception11.Message);
			Assert.Equal(1, exception11.LineNumber);
			Assert.Equal(24, exception11.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE html PUBLIC 'HTML' 'http://www.w3.org/TR/html4/loose.dtd'>" + Environment.NewLine +
				"-------------------------------^",
				exception11.SourceFragment
			);

			Assert.Equal("Invalid system identifier.", exception12.Message);
			Assert.Equal(1, exception12.LineNumber);
			Assert.Equal(65, exception12.ColumnNumber);
			Assert.Equal(
				"Line 1: …BLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd>" + Environment.NewLine +
				"--------------------------------------------------------^",
				exception12.SourceFragment
			);

			Assert.Equal("Invalid system identifier.", exception13.Message);
			Assert.Equal(1, exception13.LineNumber);
			Assert.Equal(65, exception13.ColumnNumber);
			Assert.Equal(
				"Line 1: …BLIC '-//W3C//DTD HTML 4.01 Transitional//EN' 'http://www.w3.org/TR/html4/loose.dtd >" + Environment.NewLine +
				"--------------------------------------------------------^",
				exception13.SourceFragment
			);

			Assert.Equal("Invalid system identifier.", exception14.Message);
			Assert.Equal(1, exception14.LineNumber);
			Assert.Equal(65, exception14.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"123\">" + Environment.NewLine +
				"------------------------------------------------------------------------^",
				exception14.SourceFragment
			);

			Assert.Equal("Invalid system identifier.", exception15.Message);
			Assert.Equal(1, exception15.LineNumber);
			Assert.Equal(65, exception15.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE html PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN' '123'>" + Environment.NewLine +
				"------------------------------------------------------------------------^",
				exception15.SourceFragment
			);
		}
	}
}