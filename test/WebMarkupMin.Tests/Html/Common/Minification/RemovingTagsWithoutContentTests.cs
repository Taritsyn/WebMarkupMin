using System;

using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class RemovingTagsWithoutContentTests : IDisposable
	{
		private HtmlMinifier _keepingTagsWithoutContentMinifier;
		private HtmlMinifier _removingTagsWithoutContentMinifier;


		public RemovingTagsWithoutContentTests()
		{
			_keepingTagsWithoutContentMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveTagsWithoutContent = false }
			);
			_removingTagsWithoutContentMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveTagsWithoutContent = true }
			);
		}


		[Fact]
		public void RemovingAnchorTagsWithoutContent()
		{
			// Arrange
			const string input1 = "<a href=\"http://example/com\" title=\"Some title...\"></a>";
			const string targetOutput1A = input1;
			const string targetOutput1B = "";

			const string input2 = "<p>Some text...<a title=\"Some title...\" name=\"anchor\"></a></p>";

			const string input3 = "<p>Some text...<a title=\"Some title...\" name=\"\"></a></p>";
			const string targetOutput3A = input3;
			const string targetOutput3B = "<p>Some text...</p>";

			// Act
			string outputA = _keepingTagsWithoutContentMinifier.Minify(input1).MinifiedContent;
			string outputB = _removingTagsWithoutContentMinifier.Minify(input1).MinifiedContent;

			string output2A = _keepingTagsWithoutContentMinifier.Minify(input2).MinifiedContent;
			string output2B = _removingTagsWithoutContentMinifier.Minify(input2).MinifiedContent;

			string output3A = _keepingTagsWithoutContentMinifier.Minify(input3).MinifiedContent;
			string output3B = _removingTagsWithoutContentMinifier.Minify(input3).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, outputA);
			Assert.Equal(targetOutput1B, outputB);

			Assert.Equal(input2, output2A);
			Assert.Equal(input2, output2B);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
		}

		[Fact]
		public void RemovingSpanTagsWithoutContent()
		{
			// Arrange
			const string input1 = "<p>Some text...<span>Some other text...</span><span></span></p>";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<p>Some text...<span>Some other text...</span></p>";

			const string input2 = "<div>Hello, <span>%Username%</span>!</div>";

			const string input3 = "<p>Some text...<span title=\"Some title...\" class=\"caret\"></span></p>";

			const string input4 = "<p>Some text...<span title=\"Some title...\" class=\"\"></span></p>";
			const string targetOutput4A = input4;
			const string targetOutput4B = "<p>Some text...</p>";

			const string input5 = "<p>Some text...<span title=\"Some title...\" id=\"label\"></span></p>";

			const string input6 = "<p>Some text...<span title=\"Some title...\" id=\"\"></span></p>";
			const string targetOutput6A = input6;
			const string targetOutput6B = "<p>Some text...</p>";

			const string input7 = "<p>Some text...<span title=\"Some title...\" data-description=\"Some description...\"></span></p>";

			const string input8 = "<p>Some text...<span title=\"Some title...\" data-description=\"\"></span></p>";

			// Act
			string output1A = _keepingTagsWithoutContentMinifier.Minify(input1).MinifiedContent;
			string output1B = _removingTagsWithoutContentMinifier.Minify(input1).MinifiedContent;

			string output2A = _keepingTagsWithoutContentMinifier.Minify(input2).MinifiedContent;
			string output2B = _removingTagsWithoutContentMinifier.Minify(input2).MinifiedContent;

			string output3A = _keepingTagsWithoutContentMinifier.Minify(input3).MinifiedContent;
			string output3B = _removingTagsWithoutContentMinifier.Minify(input3).MinifiedContent;

			string output4A = _keepingTagsWithoutContentMinifier.Minify(input4).MinifiedContent;
			string output4B = _removingTagsWithoutContentMinifier.Minify(input4).MinifiedContent;

			string output5A = _keepingTagsWithoutContentMinifier.Minify(input5).MinifiedContent;
			string output5B = _removingTagsWithoutContentMinifier.Minify(input5).MinifiedContent;

			string output6A = _keepingTagsWithoutContentMinifier.Minify(input6).MinifiedContent;
			string output6B = _removingTagsWithoutContentMinifier.Minify(input6).MinifiedContent;

			string output7A = _keepingTagsWithoutContentMinifier.Minify(input7).MinifiedContent;
			string output7B = _removingTagsWithoutContentMinifier.Minify(input7).MinifiedContent;

			string output8A = _keepingTagsWithoutContentMinifier.Minify(input8).MinifiedContent;
			string output8B = _removingTagsWithoutContentMinifier.Minify(input8).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(input2, output2A);
			Assert.Equal(input2, output2B);

			Assert.Equal(input3, output3A);
			Assert.Equal(input3, output3B);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);

			Assert.Equal(input5, output5A);
			Assert.Equal(input5, output5B);

			Assert.Equal(targetOutput6A, output6A);
			Assert.Equal(targetOutput6B, output6B);

			Assert.Equal(input7, output7A);
			Assert.Equal(input7, output7B);

			Assert.Equal(input8, output8A);
			Assert.Equal(input8, output8B);
		}

		[Fact]
		public void RemovingImageTagsWithoutContent()
		{
			// Arrange
			const string input1 = "<img>";

			const string input2 = "<img src=\"\">";

			// Act
			string output1A = _keepingTagsWithoutContentMinifier.Minify(input1).MinifiedContent;
			string output1B = _removingTagsWithoutContentMinifier.Minify(input1).MinifiedContent;

			string output2A = _keepingTagsWithoutContentMinifier.Minify(input2).MinifiedContent;
			string output2B = _removingTagsWithoutContentMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1A);
			Assert.Equal(input1, output1B);

			Assert.Equal(input2, output2A);
			Assert.Equal(input2, output2B);
		}

		[Fact]
		public void RemovingParagraphTagsWithoutContent()
		{
			// Arrange
			const string input1 = "<p>Some text...</p>";

			const string input2 = "<p></p>";
			const string targetOutput2A = input2;
			const string targetOutput2B = "";

			const string input3 = "<p><!-- Some comment... --></p>";

			const string input4 = "<div>\n" +
				"	<p>	\n  </p>\n" +
				"</div>"
				;
			const string targetOutput4A = input4;
			const string targetOutput4B = "<div>\n" +
				"	\n" +
				"</div>"
				;

			// Act
			string output1A = _keepingTagsWithoutContentMinifier.Minify(input1).MinifiedContent;
			string output1B = _removingTagsWithoutContentMinifier.Minify(input1).MinifiedContent;

			string output2A = _keepingTagsWithoutContentMinifier.Minify(input2).MinifiedContent;
			string output2B = _removingTagsWithoutContentMinifier.Minify(input2).MinifiedContent;

			string output3A = _keepingTagsWithoutContentMinifier.Minify(input3).MinifiedContent;
			string output3B = _removingTagsWithoutContentMinifier.Minify(input3).MinifiedContent;

			string output4A = _keepingTagsWithoutContentMinifier.Minify(input4).MinifiedContent;
			string output4B = _removingTagsWithoutContentMinifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1A);
			Assert.Equal(input1, output1B);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);

			Assert.Equal(input3, output3A);
			Assert.Equal(input3, output3B);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
		}

		[Fact]
		public void RemovingDivTagsWithoutContent()
		{
			// Arrange
			const string input1 = "<div>one<div>two <div>three</div><div></div>four</div>five</div>";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<div>one<div>two <div>three</div>four</div>five</div>";

			const string input2 = "<div role=\"contentinfo\"></div>";

			const string input3 = "<div role=\"\"></div>";
			const string targetOutput3A = input3;
			const string targetOutput3B = "";

			const string input4 = "<div custom-attribute=\"\"></div>";

			// Act
			string output1A = _keepingTagsWithoutContentMinifier.Minify(input1).MinifiedContent;
			string output1B = _removingTagsWithoutContentMinifier.Minify(input1).MinifiedContent;

			string output2A = _keepingTagsWithoutContentMinifier.Minify(input2).MinifiedContent;
			string output2B = _removingTagsWithoutContentMinifier.Minify(input2).MinifiedContent;

			string output3A = _keepingTagsWithoutContentMinifier.Minify(input3).MinifiedContent;
			string output3B = _removingTagsWithoutContentMinifier.Minify(input3).MinifiedContent;

			string output4B = _removingTagsWithoutContentMinifier.Minify(input4).MinifiedContent;
			string output4A = _keepingTagsWithoutContentMinifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(input2, output2A);
			Assert.Equal(input2, output2B);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);

			Assert.Equal(input4, output4A);
			Assert.Equal(input4, output4B);
		}

		[Fact]
		public void RemovingTagsWithoutContentInForms()
		{
			// Arrange
			const string input1 = "<textarea cols=\"80\" rows=\"10\"></textarea>";
			const string input2 = "<select name=\"city\">\n" +
				"	<option value=\"\"></option>\n" +
				"	<option value=\"msk\">Moscow</option>\n" +
				"	<option value=\"spb\">St. Petersburg</option>\n" +
				"	<option value=\"ekb\">Yekaterinburg</option>\n" +
				"</select>"
				;
			const string input3 = "<input list=\"browsers\" id=\"browser\" name=\"browser\">\n" +
				"<datalist id=\"browsers\">\n" +
				"	<option value=\"Chrome\"></option>\n" +
				"	<option value=\"Edge\"></option>\n" +
				"	<option value=\"Firefox\"></option>\n" +
				"	<option value=\"Opera\"></option>\n" +
				"	<option value=\"Safari\"></option>\n" +
				"</datalist>"
				;

			// Act
			string output1A = _keepingTagsWithoutContentMinifier.Minify(input1).MinifiedContent;
			string output1B = _removingTagsWithoutContentMinifier.Minify(input1).MinifiedContent;

			string output2A = _keepingTagsWithoutContentMinifier.Minify(input2).MinifiedContent;
			string output2B = _removingTagsWithoutContentMinifier.Minify(input2).MinifiedContent;

			string output3A = _keepingTagsWithoutContentMinifier.Minify(input3).MinifiedContent;
			string output3B = _removingTagsWithoutContentMinifier.Minify(input3).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1A);
			Assert.Equal(input1, output1B);

			Assert.Equal(input2, output2A);
			Assert.Equal(input2, output2B);

			Assert.Equal(input3, output3A);
			Assert.Equal(input3, output3B);
		}

		[Fact]
		public void RemovingStyleTagsWithoutContent()
		{
			// Arrange
			const string input = "<style></style>";
			const string targetOutputA = input;
			const string targetOutputB = "";

			// Act
			string outputA = _keepingTagsWithoutContentMinifier.Minify(input).MinifiedContent;
			string outputB = _removingTagsWithoutContentMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
		}

		[Fact]
		public void RemovingScriptTagsWithoutContent()
		{
			// Arrange
			const string input1 = "<script></script>";
			const string targetOutput1A = input1;
			const string targetOutput1B = "";

			const string input2 = "<script src=\"/scripts/common.js\"></script>";

			// Act
			string output1A = _keepingTagsWithoutContentMinifier.Minify(input1).MinifiedContent;
			string output1B = _removingTagsWithoutContentMinifier.Minify(input1).MinifiedContent;

			string output2A = _keepingTagsWithoutContentMinifier.Minify(input2).MinifiedContent;
			string output2B = _removingTagsWithoutContentMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(input2, output2A);
			Assert.Equal(input2, output2B);
		}

		[Fact]
		public void RemovingXmlBasedTagsWithoutContent()
		{
			// Arrange
			const string input1 = "<div>\n" +
				"	<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"100\" height=\"50\"></svg>\n" +
				"</div>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<div>\n" +
				"	\n" +
				"</div>"
				;

			const string input2 = "<div>\n" +
				"	<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"100\" height=\"50\">\n" +
				"		<text x=\"10\" y=\"20\" transform=\"rotate(30 20,40)\"></text>\n" +
				"	</svg>\n" +
				"</div>"
				;

			const string input3 = "<div>" +
				"<svg width=\"220\" height=\"220\">" +
				"<circle cx=\"110\" cy=\"110\" r=\"100\" fill=\"#fafaa2\" stroke=\"#000\" />" +
				"</svg>" +
				"</div>"
				;

			const string input4 = "<div>" +
				"<math><infinity /></math>" +
				"</div>"
				;

			// Act
			string output1A = _keepingTagsWithoutContentMinifier.Minify(input1).MinifiedContent;
			string output1B = _removingTagsWithoutContentMinifier.Minify(input1).MinifiedContent;

			string output2A = _keepingTagsWithoutContentMinifier.Minify(input2).MinifiedContent;
			string output2B = _removingTagsWithoutContentMinifier.Minify(input2).MinifiedContent;

			string output3A = _keepingTagsWithoutContentMinifier.Minify(input3).MinifiedContent;
			string output3B = _removingTagsWithoutContentMinifier.Minify(input3).MinifiedContent;

			string output4A = _keepingTagsWithoutContentMinifier.Minify(input4).MinifiedContent;
			string output4B = _removingTagsWithoutContentMinifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(input2, output2A);
			Assert.Equal(input2, output2B);

			Assert.Equal(input3, output3A);
			Assert.Equal(input3, output3B);

			Assert.Equal(input4, output4A);
			Assert.Equal(input4, output4B);
		}

		#region IDisposable implementation

		public void Dispose()
		{
			_keepingTagsWithoutContentMinifier = null;
			_removingTagsWithoutContentMinifier = null;
		}

		#endregion
	}
}