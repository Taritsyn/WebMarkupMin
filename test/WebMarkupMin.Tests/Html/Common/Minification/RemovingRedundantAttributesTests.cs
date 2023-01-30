using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class RemovingRedundantAttributesTests
	{
		[Fact]
		public void RemovingRedundantAnchorTagAttributes()
		{
			// Arrange
			var keepingAllRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = false });
			var removingRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = true });
			var keepingSomeRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					RemoveRedundantAttributes = true,
					PreservableAttributeList = "A[NAME]"
				}
			);

			const string input1 = "<a id=\"_toppage\" name=\"_toppage\"></a>";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<a id=\"_toppage\"></a>";
			const string targetOutput1C = input1;

			const string input2 = "<a name=\"_toppage\"></a>";
			const string targetOutput2A = input2;
			const string targetOutput2B = input2;
			const string targetOutput2C = input2;

			const string input3 = "<a href=\"http://example.com/\" id=\"lnkExample\" name=\"lnkExample\"></a>";
			const string targetOutput3A = input3;
			const string targetOutput3B = "<a href=\"http://example.com/\" id=\"lnkExample\"></a>";
			const string targetOutput3C = input3;

			const string input4 = "<input id=\"txtEmail\" name=\"email\">";
			const string targetOutput4A = input4;
			const string targetOutput4B = input4;
			const string targetOutput4C = input4;

			const string input5 = "<A HREF=\"http://example.com/\" ID=\"lnkExample\" NAME=\"lnkexample\"></A>";
			const string targetOutput5A = "<a href=\"http://example.com/\" id=\"lnkExample\" name=\"lnkexample\"></a>";
			const string targetOutput5B = targetOutput5A;
			const string targetOutput5C = targetOutput5A;

			// Act
			string output1A = keepingAllRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output1B = removingRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output1C = keepingSomeRedundantAttributesMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingAllRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output2B = removingRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output2C = keepingSomeRedundantAttributesMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingAllRedundantAttributesMinifier.Minify(input3).MinifiedContent;
			string output3B = removingRedundantAttributesMinifier.Minify(input3).MinifiedContent;
			string output3C = keepingSomeRedundantAttributesMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingAllRedundantAttributesMinifier.Minify(input4).MinifiedContent;
			string output4B = removingRedundantAttributesMinifier.Minify(input4).MinifiedContent;
			string output4C = keepingSomeRedundantAttributesMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingAllRedundantAttributesMinifier.Minify(input5).MinifiedContent;
			string output5B = removingRedundantAttributesMinifier.Minify(input5).MinifiedContent;
			string output5C = keepingSomeRedundantAttributesMinifier.Minify(input5).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);
		}

		[Fact]
		public void RemovingRedundantAreaTagAttributes()
		{
			// Arrange
			var keepingAllRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = false });
			var removingRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = true });
			var keepingSomeRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					RemoveRedundantAttributes = true,
					PreservableAttributeList = "area[shape=rect i]"
				}
			);

			const string input1 = "<area shape=\"rect\" coords=\"0,0,82,126\"" +
				" href=\"http://example.com/\" title=\"Some title…\">";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<area coords=\"0,0,82,126\"" +
				" href=\"http://example.com/\" title=\"Some title…\">";
			const string targetOutput1C = input1;

			const string input2 = "<AREA SHAPE=\"RECT\" COORDS=\"0,0,82,126\"" +
				" HREF=\"http://example.com/\" TITLE=\"Some title…\">";
			const string targetOutput2A = "<area shape=\"RECT\" coords=\"0,0,82,126\"" +
				" href=\"http://example.com/\" title=\"Some title…\">";
			const string targetOutput2B = "<area coords=\"0,0,82,126\"" +
				" href=\"http://example.com/\" title=\"Some title…\">";
			const string targetOutput2C = targetOutput2A;

			const string input3 = "<area shape=\"  rect  \" coords=\"0,0,82,126\"" +
				" href=\"http://example.com/\" title=\"Some title…\">";
			const string targetOutput3A = input3;
			const string targetOutput3B = input3;
			const string targetOutput3C = input3;

			const string input4 = "<area shape=\"circle\" coords=\"90,58,3\"" +
				" href=\"http://example.com/\" alt=\"Some title…\">";
			const string targetOutput4A = input4;
			const string targetOutput4B = input4;
			const string targetOutput4C = input4;

			// Act
			string output1A = keepingAllRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output1B = removingRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output1C = keepingSomeRedundantAttributesMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingAllRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output2B = removingRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output2C = keepingSomeRedundantAttributesMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingAllRedundantAttributesMinifier.Minify(input3).MinifiedContent;
			string output3B = removingRedundantAttributesMinifier.Minify(input3).MinifiedContent;
			string output3C = keepingSomeRedundantAttributesMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingAllRedundantAttributesMinifier.Minify(input4).MinifiedContent;
			string output4B = removingRedundantAttributesMinifier.Minify(input4).MinifiedContent;
			string output4C = keepingSomeRedundantAttributesMinifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);
		}

		[Fact]
		public void RemovingRedundantButtonTagAttributes()
		{
			// Arrange
			var keepingAllRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = false });
			var removingRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = true });
			var keepingSomeRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					RemoveRedundantAttributes = true,
					PreservableAttributeList = "button[type=submit i]"
				}
			);

			const string input1 = "<button type=\"submit\">Click Me!</button>";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<button>Click Me!</button>";
			const string targetOutput1C = input1;

			const string input2 = "<BUTTON TYPE=\"SUBMIT\">Click Me!</BUTTON>";
			const string targetOutput2A = "<button type=\"SUBMIT\">Click Me!</button>";
			const string targetOutput2B = "<button>Click Me!</button>";
			const string targetOutput2C = targetOutput2A;

			const string input3 = "<button type=\"  submit  \">Click Me!</button>";
			const string targetOutput3A = input3;
			const string targetOutput3B = input3;
			const string targetOutput3C = input3;

			const string input4 = "<button type=\"button\">Click Me!</button>";
			const string targetOutput4A = input4;
			const string targetOutput4B = input4;
			const string targetOutput4C = input4;

			// Act
			string output1A = keepingAllRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output1B = removingRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output1C = keepingSomeRedundantAttributesMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingAllRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output2B = removingRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output2C = keepingSomeRedundantAttributesMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingAllRedundantAttributesMinifier.Minify(input3).MinifiedContent;
			string output3B = removingRedundantAttributesMinifier.Minify(input3).MinifiedContent;
			string output3C = keepingSomeRedundantAttributesMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingAllRedundantAttributesMinifier.Minify(input4).MinifiedContent;
			string output4B = removingRedundantAttributesMinifier.Minify(input4).MinifiedContent;
			string output4C = keepingSomeRedundantAttributesMinifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);
		}

		[Fact]
		public void RemovingRedundantFormTagAttributes()
		{
			// Arrange
			var keepingAllRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = false });
			var removingRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = true });
			var keepingSomeRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					RemoveRedundantAttributes = true,
					PreservableAttributeList = "form[autocomplete], form[enctype], form[method]"
				}
			);

			const string input1 = "<form autocomplete=\"on\">Some controls…</form>";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<form>Some controls…</form>";
			const string targetOutput1C = input1;

			const string input2 = "<FORM AUTOCOMPLETE=\"ON\">Some controls…</FORM>";
			const string targetOutput2A = "<form autocomplete=\"ON\">Some controls…</form>";
			const string targetOutput2B = "<form>Some controls…</form>";
			const string targetOutput2C = targetOutput2A;

			const string input3 = "<form autocomplete=\"  on  \">Some controls…</form>";
			const string targetOutput3A = input3;
			const string targetOutput3B = input3;
			const string targetOutput3C = input3;

			const string input4 = "<form autocomplete=\"off\">Some controls…</form>";
			const string targetOutput4A = input4;
			const string targetOutput4B = input4;
			const string targetOutput4C = input4;

			const string input5 = "<form enctype=\"application/x-www-form-urlencoded\">Some controls…</form>";
			const string targetOutput5A = input5;
			const string targetOutput5B = "<form>Some controls…</form>";
			const string targetOutput5C = input5;

			const string input6 = "<FORM ENCTYPE=\"APPLICATION/X-WWW-FORM-URLENCODED\">Some controls…</FORM>";
			const string targetOutput6A = "<form enctype=\"APPLICATION/X-WWW-FORM-URLENCODED\">Some controls…</form>";
			const string targetOutput6B = "<form>Some controls…</form>";
			const string targetOutput6C = targetOutput6A;

			const string input7 = "<form enctype=\"  application/x-www-form-urlencoded  \">Some controls…</form>";
			const string targetOutput7A = input7;
			const string targetOutput7B = input7;
			const string targetOutput7C = input7;

			const string input8 = "<form enctype=\"multipart/form-data\">Some controls…</form>";
			const string targetOutput8A = input8;
			const string targetOutput8B = input8;
			const string targetOutput8C = input8;

			const string input9 = "<form method=\"get\">Some controls…</form>";
			const string targetOutput9A = input9;
			const string targetOutput9B = "<form>Some controls…</form>";
			const string targetOutput9C = input9;

			const string input10 = "<FORM METHOD=\"GET\">Some controls…</FORM>";
			const string targetOutput10A = "<form method=\"GET\">Some controls…</form>";
			const string targetOutput10B = "<form>Some controls…</form>";
			const string targetOutput10C = targetOutput10A;

			const string input11 = "<form method=\"  get  \">Some controls…</form>";
			const string targetOutput11A = input11;
			const string targetOutput11B = input11;
			const string targetOutput11C = input11;

			const string input12 = "<form method=\"post\">Some controls…</form>";
			const string targetOutput12A = input12;
			const string targetOutput12B = input12;
			const string targetOutput12C = input12;

			// Act
			string output1A = keepingAllRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output1B = removingRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output1C = keepingSomeRedundantAttributesMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingAllRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output2B = removingRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output2C = keepingSomeRedundantAttributesMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingAllRedundantAttributesMinifier.Minify(input3).MinifiedContent;
			string output3B = removingRedundantAttributesMinifier.Minify(input3).MinifiedContent;
			string output3C = keepingSomeRedundantAttributesMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingAllRedundantAttributesMinifier.Minify(input4).MinifiedContent;
			string output4B = removingRedundantAttributesMinifier.Minify(input4).MinifiedContent;
			string output4C = keepingSomeRedundantAttributesMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingAllRedundantAttributesMinifier.Minify(input5).MinifiedContent;
			string output5B = removingRedundantAttributesMinifier.Minify(input5).MinifiedContent;
			string output5C = keepingSomeRedundantAttributesMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingAllRedundantAttributesMinifier.Minify(input6).MinifiedContent;
			string output6B = removingRedundantAttributesMinifier.Minify(input6).MinifiedContent;
			string output6C = keepingSomeRedundantAttributesMinifier.Minify(input6).MinifiedContent;

			string output7A = keepingAllRedundantAttributesMinifier.Minify(input7).MinifiedContent;
			string output7B = removingRedundantAttributesMinifier.Minify(input7).MinifiedContent;
			string output7C = keepingSomeRedundantAttributesMinifier.Minify(input7).MinifiedContent;

			string output8A = keepingAllRedundantAttributesMinifier.Minify(input8).MinifiedContent;
			string output8B = removingRedundantAttributesMinifier.Minify(input8).MinifiedContent;
			string output8C = keepingSomeRedundantAttributesMinifier.Minify(input8).MinifiedContent;

			string output9A = keepingAllRedundantAttributesMinifier.Minify(input9).MinifiedContent;
			string output9B = removingRedundantAttributesMinifier.Minify(input9).MinifiedContent;
			string output9C = keepingSomeRedundantAttributesMinifier.Minify(input9).MinifiedContent;

			string output10A = keepingAllRedundantAttributesMinifier.Minify(input10).MinifiedContent;
			string output10B = removingRedundantAttributesMinifier.Minify(input10).MinifiedContent;
			string output10C = keepingSomeRedundantAttributesMinifier.Minify(input10).MinifiedContent;

			string output11A = keepingAllRedundantAttributesMinifier.Minify(input11).MinifiedContent;
			string output11B = removingRedundantAttributesMinifier.Minify(input11).MinifiedContent;
			string output11C = keepingSomeRedundantAttributesMinifier.Minify(input11).MinifiedContent;

			string output12A = keepingAllRedundantAttributesMinifier.Minify(input12).MinifiedContent;
			string output12B = removingRedundantAttributesMinifier.Minify(input12).MinifiedContent;
			string output12C = keepingSomeRedundantAttributesMinifier.Minify(input12).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);

			Assert.Equal(targetOutput6A, output6A);
			Assert.Equal(targetOutput6B, output6B);
			Assert.Equal(targetOutput6C, output6C);

			Assert.Equal(targetOutput7A, output7A);
			Assert.Equal(targetOutput7B, output7B);
			Assert.Equal(targetOutput7C, output7C);

			Assert.Equal(targetOutput8A, output8A);
			Assert.Equal(targetOutput8B, output8B);
			Assert.Equal(targetOutput8C, output8C);

			Assert.Equal(targetOutput9A, output9A);
			Assert.Equal(targetOutput9B, output9B);
			Assert.Equal(targetOutput9C, output9C);

			Assert.Equal(targetOutput10A, output10A);
			Assert.Equal(targetOutput10B, output10B);
			Assert.Equal(targetOutput10C, output10C);

			Assert.Equal(targetOutput11A, output11A);
			Assert.Equal(targetOutput11B, output11B);
			Assert.Equal(targetOutput11C, output11C);

			Assert.Equal(targetOutput12A, output12A);
			Assert.Equal(targetOutput12B, output12B);
			Assert.Equal(targetOutput12C, output12C);
		}

		[Fact]
		public void RemovingRedundantImageTagAttributes()
		{
			// Arrange
			var keepingAllRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = false });
			var removingRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = true });
			var keepingSomeRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					RemoveRedundantAttributes = true,
					PreservableAttributeList = "img[decoding=auto i]"
				}
			);

			const string input1 = "<img decoding=\"auto\" src=\"space-pizza.jpg\">";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<img src=\"space-pizza.jpg\">";
			const string targetOutput1C = input1;

			const string input2 = "<IMG DECODING=\"AUTO\" SRC=\"space-pizza.jpg\">";
			const string targetOutput2A = "<img decoding=\"AUTO\" src=\"space-pizza.jpg\">";
			const string targetOutput2B = "<img src=\"space-pizza.jpg\">";
			const string targetOutput2C = targetOutput2A;

			const string input3 = "<img decoding=\"  auto  \" src=\"space-pizza.jpg\">";
			const string targetOutput3A = input3;
			const string targetOutput3B = input3;
			const string targetOutput3C = input3;

			const string input4 = "<img decoding=\"async\" src=\"space-cats.jpg\">";
			const string targetOutput4A = input4;
			const string targetOutput4B = input4;
			const string targetOutput4C = input4;

			// Act
			string output1A = keepingAllRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output1B = removingRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output1C = keepingSomeRedundantAttributesMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingAllRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output2B = removingRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output2C = keepingSomeRedundantAttributesMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingAllRedundantAttributesMinifier.Minify(input3).MinifiedContent;
			string output3B = removingRedundantAttributesMinifier.Minify(input3).MinifiedContent;
			string output3C = keepingSomeRedundantAttributesMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingAllRedundantAttributesMinifier.Minify(input4).MinifiedContent;
			string output4B = removingRedundantAttributesMinifier.Minify(input4).MinifiedContent;
			string output4C = keepingSomeRedundantAttributesMinifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);
		}

		[Fact]
		public void RemovingRedundantInputTagAttributes()
		{
			// Arrange
			var keepingAllRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = false });
			var removingRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = true });
			var keepingSomeRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					RemoveRedundantAttributes = true,
					PreservableAttributeList = "input[type=text i]"
				}
			);

			const string input1 = "<input type=\"text\" value=\"Some value…\">";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<input value=\"Some value…\">";
			const string targetOutput1C = input1;

			const string input2 = "<INPUT TYPE=\"TEXT\" VALUE=\"Some value…\">";
			const string targetOutput2A = "<input type=\"TEXT\" value=\"Some value…\">";
			const string targetOutput2B = "<input value=\"Some value…\">";
			const string targetOutput2C = targetOutput2A;

			const string input3 = "<input type=\"  text  \" value=\"Some value…\">";
			const string targetOutput3A = input3;
			const string targetOutput3B = input3;
			const string targetOutput3C = input3;

			const string input4 = "<input type=\"checkbox\">";
			const string targetOutput4A = input4;
			const string targetOutput4B = input4;
			const string targetOutput4C = input4;

			// Act
			string output1A = keepingAllRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output1B = removingRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output1C = keepingSomeRedundantAttributesMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingAllRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output2B = removingRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output2C = keepingSomeRedundantAttributesMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingAllRedundantAttributesMinifier.Minify(input3).MinifiedContent;
			string output3B = removingRedundantAttributesMinifier.Minify(input3).MinifiedContent;
			string output3C = keepingSomeRedundantAttributesMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingAllRedundantAttributesMinifier.Minify(input4).MinifiedContent;
			string output4B = removingRedundantAttributesMinifier.Minify(input4).MinifiedContent;
			string output4C = keepingSomeRedundantAttributesMinifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);
		}

		[Fact]
		public void RemovingRedundantScriptTagAttributes()
		{
			// Arrange
			var keepingAllRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = false });
			var removingRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = true });
			var keepingSomeRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					RemoveRedundantAttributes = true,
					PreservableAttributeList = "[charset], script[language]"
				}
			);

			const string input1 = "<script type=\"text/javascript\" charset=\"UTF-8\">alert(\"Hooray!\");</script>";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<script type=\"text/javascript\">alert(\"Hooray!\");</script>";
			const string targetOutput1C = input1;

			const string input2 = "<script type=\"text/javascript\"" +
				" src=\"http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js\"" +
				" charset=\"UTF-8\"></script>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = input2;
			const string targetOutput2C = input2;

			const string input3 = "<script charset=\"UTF-8\">alert(\"Hooray!\");</script>";
			const string targetOutput3A = input3;
			const string targetOutput3B = "<script>alert(\"Hooray!\");</script>";
			const string targetOutput3C = input3;

			const string input4 = "<script language=\"JavaScript\">var a = 1, b = 3;</script>";
			const string targetOutput4A = input4;
			const string targetOutput4B = "<script>var a = 1, b = 3;</script>";
			const string targetOutput4C = input4;

			const string input5 = "<SCRIPT LANGUAGE=\"JAVASCRIPT\">var a = 1, b = 3;</SCRIPT>";
			const string targetOutput5A = "<script language=\"JAVASCRIPT\">var a = 1, b = 3;</script>";
			const string targetOutput5B = "<script>var a = 1, b = 3;</script>";
			const string targetOutput5C = targetOutput5A;

			const string input6 = "<script language=\"  JavaScript  \">var a = 1, b = 3;</script>";
			const string targetOutput6A = input6;
			const string targetOutput6B = input6;
			const string targetOutput6C = input6;

			// Act
			string output1A = keepingAllRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output1B = removingRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output1C = keepingSomeRedundantAttributesMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingAllRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output2B = removingRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output2C = keepingSomeRedundantAttributesMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingAllRedundantAttributesMinifier.Minify(input3).MinifiedContent;
			string output3B = removingRedundantAttributesMinifier.Minify(input3).MinifiedContent;
			string output3C = keepingSomeRedundantAttributesMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingAllRedundantAttributesMinifier.Minify(input4).MinifiedContent;
			string output4B = removingRedundantAttributesMinifier.Minify(input4).MinifiedContent;
			string output4C = keepingSomeRedundantAttributesMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingAllRedundantAttributesMinifier.Minify(input5).MinifiedContent;
			string output5B = removingRedundantAttributesMinifier.Minify(input5).MinifiedContent;
			string output5C = keepingSomeRedundantAttributesMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingAllRedundantAttributesMinifier.Minify(input6).MinifiedContent;
			string output6B = removingRedundantAttributesMinifier.Minify(input6).MinifiedContent;
			string output6C = keepingSomeRedundantAttributesMinifier.Minify(input6).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);

			Assert.Equal(targetOutput6A, output6A);
			Assert.Equal(targetOutput6B, output6B);
			Assert.Equal(targetOutput6C, output6C);
		}

		[Fact]
		public void RemovingRedundantTextareaTagAttributes()
		{
			// Arrange
			var keepingAllRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = false });
			var removingRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = true });
			var keepingSomeRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					RemoveRedundantAttributes = true,
					PreservableAttributeList = "textarea[wrap=soft i]"
				}
			);

			const string input1 = "<textarea rows=\"10\" cols=\"20\" wrap=\"soft\"></textarea>";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<textarea rows=\"10\" cols=\"20\"></textarea>";
			const string targetOutput1C = input1;

			const string input2 = "<TEXTAREA ROWS=\"10\" COLS=\"20\" WRAP=\"SOFT\"></TEXTAREA>";
			const string targetOutput2A = "<textarea rows=\"10\" cols=\"20\" wrap=\"SOFT\"></textarea>";
			const string targetOutput2B = "<textarea rows=\"10\" cols=\"20\"></textarea>";
			const string targetOutput2C = targetOutput2A;

			const string input3 = "<textarea rows=\"10\" cols=\"20\" wrap=\"  soft  \"></textarea>";
			const string targetOutput3A = input3;
			const string targetOutput3B = input3;
			const string targetOutput3C = input3;

			const string input4 = "<textarea rows=\"10\" cols=\"20\" wrap=\"hard\"></textarea>";
			const string targetOutput4A = input4;
			const string targetOutput4B = input4;
			const string targetOutput4C = input4;

			// Act
			string output1A = keepingAllRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output1B = removingRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output1C = keepingSomeRedundantAttributesMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingAllRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output2B = removingRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output2C = keepingSomeRedundantAttributesMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingAllRedundantAttributesMinifier.Minify(input3).MinifiedContent;
			string output3B = removingRedundantAttributesMinifier.Minify(input3).MinifiedContent;
			string output3C = keepingSomeRedundantAttributesMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingAllRedundantAttributesMinifier.Minify(input4).MinifiedContent;
			string output4B = removingRedundantAttributesMinifier.Minify(input4).MinifiedContent;
			string output4C = keepingSomeRedundantAttributesMinifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);
		}

		[Fact]
		public void RemovingRedundantTrackTagAttributes()
		{
			// Arrange
			var keepingAllRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = false });
			var removingRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = true });
			var keepingSomeRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					RemoveRedundantAttributes = true,
					PreservableAttributeList = "track[kind=subtitles i]"
				}
			);

			const string input1 = "<video width=\"640\" height=\"360\">\n" +
				"	<source src=\"pirates_of_silicon_valley.mp4\" type=\"video/mp4\">\n" +
				"	<source src=\"pirates_of_silicon_valley.ogg\" type=\"video/ogg\">\n" +
				"	<track src=\"subtitles_en.vtt\" kind=\"subtitles\" srclang=\"en\" label=\"English\">\n" +
				"	<track src=\"subtitles_ru.vtt\" kind=\"subtitles\" srclang=\"ru\" label=\"Russian\">\n" +
				"</video>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<video width=\"640\" height=\"360\">\n" +
				"	<source src=\"pirates_of_silicon_valley.mp4\" type=\"video/mp4\">\n" +
				"	<source src=\"pirates_of_silicon_valley.ogg\" type=\"video/ogg\">\n" +
				"	<track src=\"subtitles_en.vtt\" srclang=\"en\" label=\"English\">\n" +
				"	<track src=\"subtitles_ru.vtt\" srclang=\"ru\" label=\"Russian\">\n" +
				"</video>"
				;
			const string targetOutput1C = input1;

			const string input2 = "<VIDEO WIDTH=\"640\" HEIGHT=\"360\">\n" +
				"	<SOURCE SRC=\"pirates_of_silicon_valley.mp4\" TYPE=\"video/mp4\">\n" +
				"	<SOURCE SRC=\"pirates_of_silicon_valley.ogg\" TYPE=\"video/ogg\">\n" +
				"	<TRACK SRC=\"subtitles_en.vtt\" KIND=\"SUBTITLES\" SRCLANG=\"en\" LABEL=\"English\">\n" +
				"	<TRACK SRC=\"subtitles_ru.vtt\" KIND=\"SUBTITLES\" SRCLANG=\"ru\" LABEL=\"Russian\">\n" +
				"</VIDEO>"
				;
			const string targetOutput2A = "<video width=\"640\" height=\"360\">\n" +
				"	<source src=\"pirates_of_silicon_valley.mp4\" type=\"video/mp4\">\n" +
				"	<source src=\"pirates_of_silicon_valley.ogg\" type=\"video/ogg\">\n" +
				"	<track src=\"subtitles_en.vtt\" kind=\"SUBTITLES\" srclang=\"en\" label=\"English\">\n" +
				"	<track src=\"subtitles_ru.vtt\" kind=\"SUBTITLES\" srclang=\"ru\" label=\"Russian\">\n" +
				"</video>"
				;
			const string targetOutput2B = "<video width=\"640\" height=\"360\">\n" +
				"	<source src=\"pirates_of_silicon_valley.mp4\" type=\"video/mp4\">\n" +
				"	<source src=\"pirates_of_silicon_valley.ogg\" type=\"video/ogg\">\n" +
				"	<track src=\"subtitles_en.vtt\" srclang=\"en\" label=\"English\">\n" +
				"	<track src=\"subtitles_ru.vtt\" srclang=\"ru\" label=\"Russian\">\n" +
				"</video>";
			const string targetOutput2C = targetOutput2A;

			const string input3 = "<video width=\"640\" height=\"360\">\n" +
				"	<source src=\"pirates_of_silicon_valley.mp4\" type=\"video/mp4\">\n" +
				"	<source src=\"pirates_of_silicon_valley.ogg\" type=\"video/ogg\">\n" +
				"	<track src=\"subtitles_en.vtt\" kind=\"  subtitles  \" srclang=\"en\" label=\"English\">\n" +
				"	<track src=\"subtitles_ru.vtt\" kind=\"  subtitles  \" srclang=\"ru\" label=\"Russian\">\n" +
				"</video>"
				;
			const string targetOutput3A = input3;
			const string targetOutput3B = input3;
			const string targetOutput3C = input3;

			const string input4 = "<video width=\"640\" height=\"360\">\n" +
				"	<source src=\"pirates_of_silicon_valley.mp4\" type=\"video/mp4\">\n" +
				"	<source src=\"pirates_of_silicon_valley.ogg\" type=\"video/ogg\">\n" +
				"	<track src=\"captions_en.vtt\" kind=\"captions\" srclang=\"en\" label=\"English\">\n" +
				"	<track src=\"captions_ru.vtt\" kind=\"captions\" srclang=\"ru\" label=\"Russian\">\n" +
				"</video>"
				;
			const string targetOutput4A = input4;
			const string targetOutput4B = input4;
			const string targetOutput4C = input4;

			// Act
			string output1A = keepingAllRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output1B = removingRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output1C = keepingSomeRedundantAttributesMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingAllRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output2B = removingRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output2C = keepingSomeRedundantAttributesMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingAllRedundantAttributesMinifier.Minify(input3).MinifiedContent;
			string output3B = removingRedundantAttributesMinifier.Minify(input3).MinifiedContent;
			string output3C = keepingSomeRedundantAttributesMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingAllRedundantAttributesMinifier.Minify(input4).MinifiedContent;
			string output4B = removingRedundantAttributesMinifier.Minify(input4).MinifiedContent;
			string output4C = keepingSomeRedundantAttributesMinifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);
		}
	}
}