using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class RemovingAttributeQuotesTests
	{
		[Fact]
		public void RemovingAttributeQuotes()
		{
			// Arrange
			var keepingAttributeQuotesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { AttributeQuotesRemovalMode = HtmlAttributeQuotesRemovalMode.KeepQuotes });
			var html4RemovingAttributeQuotesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { AttributeQuotesRemovalMode = HtmlAttributeQuotesRemovalMode.Html4 });
			var html5RemovingAttributeQuotesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { AttributeQuotesRemovalMode = HtmlAttributeQuotesRemovalMode.Html5 });

			const string input1 = "<input value=\"Minifier\">";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<input value=Minifier>";
			const string targetOutput1C = "<input value=Minifier>";

			const string input2 = "<input value=\"Минимизатор\">";
			const string targetOutput2A = input2;
			const string targetOutput2B = input2;
			const string targetOutput2C = "<input value=Минимизатор>";

			const string input3 = "<input value=\"極小\">";
			const string targetOutput3A = input3;
			const string targetOutput3B = input3;
			const string targetOutput3C = "<input value=極小>";

			const string input4 = "<input value=\"HTML Minifier\">";

			const string input5 = "<div class=\"l-constrained\"></div>";
			const string targetOutput5A = input5;
			const string targetOutput5B = "<div class=l-constrained></div>";
			const string targetOutput5C = "<div class=l-constrained></div>";

			const string input6 = "<input class=\"search__button\">";
			const string targetOutput6A = input6;
			const string targetOutput6B = "<input class=search__button>";
			const string targetOutput6C = "<input class=search__button>";

			const string input7 = "<link href=\"favicon.ico\" type=\"image/x-icon\">";
			const string targetOutput7A = input7;
			const string targetOutput7B = "<link href=favicon.ico type=\"image/x-icon\">";
			const string targetOutput7C = "<link href=favicon.ico type=image/x-icon>";

			const string input8 = "<a href=\"\\\\sun\">Intranet portal</a>";
			const string targetOutput8A = input8;
			const string targetOutput8B = input8;
			const string targetOutput8C = "<a href=\\\\sun>Intranet portal</a>";

			const string input9 = "<input value=\"a&lt;0\">";
			const string input10 = "<input value=\"a=0\">";
			const string input11 = "<input value=\"a>0\">";

			const string input12 = "<a href=\"#forms\" title=\"Form`s\"></a>";
			const string targetOutput12A = input12;
			const string targetOutput12B = input12;
			const string targetOutput12C = "<a href=#forms title=\"Form`s\"></a>";

			const string input13 = "<a href=\"?forms\" title=\"Form's\"></a>";
			const string targetOutput13A = input13;
			const string targetOutput13B = input13;
			const string targetOutput13C = "<a href=?forms title=\"Form's\"></a>";

			const string input14 = "<a href=\"/\">Home page</a>";

			const string input15 = "<a href=\"#/\">Home page</a>";

			const string input16 = "<input value=\"localhost:86\">";
			const string targetOutput16A = input16;
			const string targetOutput16B = "<input value=localhost:86>";
			const string targetOutput16C = "<input value=localhost:86>";

			const string input17 = "<svg width=\"220\" height=\"220\">" +
				"<line x1=\"6\" y1=\"14\" x2=\"177\" y2=\"198\" stroke=\"#000\" />" +
				"</svg>"
				;
			const string targetOutput17A = input17;
			const string targetOutput17B = "<svg width=220 height=220>" +
				"<line x1=6 y1=14 x2=177 y2=198 stroke=\"#000\" />" +
				"</svg>"
				;
			const string targetOutput17C = "<svg width=220 height=220>" +
				"<line x1=6 y1=14 x2=177 y2=198 stroke=#000 />" +
				"</svg>"
				;

			const string input18 = "<math>" +
				"<apply>" +
				"<in />" +
				"<cn type=\"complex-cartesian\">17<sep />29</cn>" +
				"<complexes />" +
				"</apply>" +
				"</math>"
				;
			const string targetOutput18A = input18;
			const string targetOutput18B = "<math>" +
				"<apply>" +
				"<in />" +
				"<cn type=complex-cartesian>17<sep />29</cn>" +
				"<complexes />" +
				"</apply>" +
				"</math>"
				;
			const string targetOutput18C = targetOutput18B;

			const string input19 = "<input data-options=\"\">";

			// Act
			string output1A = keepingAttributeQuotesMinifier.Minify(input1).MinifiedContent;
			string output1B = html4RemovingAttributeQuotesMinifier.Minify(input1).MinifiedContent;
			string output1C = html5RemovingAttributeQuotesMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingAttributeQuotesMinifier.Minify(input2).MinifiedContent;
			string output2B = html4RemovingAttributeQuotesMinifier.Minify(input2).MinifiedContent;
			string output2C = html5RemovingAttributeQuotesMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingAttributeQuotesMinifier.Minify(input3).MinifiedContent;
			string output3B = html4RemovingAttributeQuotesMinifier.Minify(input3).MinifiedContent;
			string output3C = html5RemovingAttributeQuotesMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingAttributeQuotesMinifier.Minify(input4).MinifiedContent;
			string output4B = html4RemovingAttributeQuotesMinifier.Minify(input4).MinifiedContent;
			string output4C = html5RemovingAttributeQuotesMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingAttributeQuotesMinifier.Minify(input5).MinifiedContent;
			string output5B = html4RemovingAttributeQuotesMinifier.Minify(input5).MinifiedContent;
			string output5C = html5RemovingAttributeQuotesMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingAttributeQuotesMinifier.Minify(input6).MinifiedContent;
			string output6B = html4RemovingAttributeQuotesMinifier.Minify(input6).MinifiedContent;
			string output6C = html5RemovingAttributeQuotesMinifier.Minify(input6).MinifiedContent;

			string output7A = keepingAttributeQuotesMinifier.Minify(input7).MinifiedContent;
			string output7B = html4RemovingAttributeQuotesMinifier.Minify(input7).MinifiedContent;
			string output7C = html5RemovingAttributeQuotesMinifier.Minify(input7).MinifiedContent;

			string output8A = keepingAttributeQuotesMinifier.Minify(input8).MinifiedContent;
			string output8B = html4RemovingAttributeQuotesMinifier.Minify(input8).MinifiedContent;
			string output8C = html5RemovingAttributeQuotesMinifier.Minify(input8).MinifiedContent;

			string output9A = keepingAttributeQuotesMinifier.Minify(input9).MinifiedContent;
			string output9B = html4RemovingAttributeQuotesMinifier.Minify(input9).MinifiedContent;
			string output9C = html5RemovingAttributeQuotesMinifier.Minify(input9).MinifiedContent;

			string output10A = keepingAttributeQuotesMinifier.Minify(input10).MinifiedContent;
			string output10B = html4RemovingAttributeQuotesMinifier.Minify(input10).MinifiedContent;
			string output10C = html5RemovingAttributeQuotesMinifier.Minify(input10).MinifiedContent;

			string output11A = keepingAttributeQuotesMinifier.Minify(input11).MinifiedContent;
			string output11B = html4RemovingAttributeQuotesMinifier.Minify(input11).MinifiedContent;
			string output11C = html5RemovingAttributeQuotesMinifier.Minify(input11).MinifiedContent;

			string output12A = keepingAttributeQuotesMinifier.Minify(input12).MinifiedContent;
			string output12B = html4RemovingAttributeQuotesMinifier.Minify(input12).MinifiedContent;
			string output12C = html5RemovingAttributeQuotesMinifier.Minify(input12).MinifiedContent;

			string output13A = keepingAttributeQuotesMinifier.Minify(input13).MinifiedContent;
			string output13B = html4RemovingAttributeQuotesMinifier.Minify(input13).MinifiedContent;
			string output13C = html5RemovingAttributeQuotesMinifier.Minify(input13).MinifiedContent;

			string output14A = keepingAttributeQuotesMinifier.Minify(input14).MinifiedContent;
			string output14B = html4RemovingAttributeQuotesMinifier.Minify(input14).MinifiedContent;
			string output14C = html5RemovingAttributeQuotesMinifier.Minify(input14).MinifiedContent;

			string output15A = keepingAttributeQuotesMinifier.Minify(input15).MinifiedContent;
			string output15B = html4RemovingAttributeQuotesMinifier.Minify(input15).MinifiedContent;
			string output15C = html5RemovingAttributeQuotesMinifier.Minify(input15).MinifiedContent;

			string output16A = keepingAttributeQuotesMinifier.Minify(input16).MinifiedContent;
			string output16B = html4RemovingAttributeQuotesMinifier.Minify(input16).MinifiedContent;
			string output16C = html5RemovingAttributeQuotesMinifier.Minify(input16).MinifiedContent;

			string output17A = keepingAttributeQuotesMinifier.Minify(input17).MinifiedContent;
			string output17B = html4RemovingAttributeQuotesMinifier.Minify(input17).MinifiedContent;
			string output17C = html5RemovingAttributeQuotesMinifier.Minify(input17).MinifiedContent;

			string output18A = keepingAttributeQuotesMinifier.Minify(input18).MinifiedContent;
			string output18B = html4RemovingAttributeQuotesMinifier.Minify(input18).MinifiedContent;
			string output18C = html5RemovingAttributeQuotesMinifier.Minify(input18).MinifiedContent;

			string output19A = keepingAttributeQuotesMinifier.Minify(input19).MinifiedContent;
			string output19B = html4RemovingAttributeQuotesMinifier.Minify(input19).MinifiedContent;
			string output19C = html5RemovingAttributeQuotesMinifier.Minify(input19).MinifiedContent;

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

			Assert.Equal(input4, output4A);
			Assert.Equal(input4, output4B);
			Assert.Equal(input4, output4C);

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

			Assert.Equal(input9, output9A);
			Assert.Equal(input9, output9B);
			Assert.Equal(input9, output9C);

			Assert.Equal(input10, output10A);
			Assert.Equal(input10, output10B);
			Assert.Equal(input10, output10C);

			Assert.Equal(input11, output11A);
			Assert.Equal(input11, output11B);
			Assert.Equal(input11, output11C);

			Assert.Equal(targetOutput12A, output12A);
			Assert.Equal(targetOutput12B, output12B);
			Assert.Equal(targetOutput12C, output12C);

			Assert.Equal(targetOutput13A, output13A);
			Assert.Equal(targetOutput13B, output13B);
			Assert.Equal(targetOutput13C, output13C);

			Assert.Equal(input14, output14A);
			Assert.Equal(input14, output14B);
			Assert.Equal(input14, output14C);

			Assert.Equal(input15, output15A);
			Assert.Equal(input15, output15B);
			Assert.Equal(input15, output15C);

			Assert.Equal(targetOutput16A, output16A);
			Assert.Equal(targetOutput16B, output16B);
			Assert.Equal(targetOutput16C, output16C);

			Assert.Equal(targetOutput17A, output17A);
			Assert.Equal(targetOutput17B, output17B);
			Assert.Equal(targetOutput17C, output17C);

			Assert.Equal(targetOutput18A, output18A);
			Assert.Equal(targetOutput18B, output18B);
			Assert.Equal(targetOutput18C, output18C);

			Assert.Equal(input19, output19A);
			Assert.Equal(input19, output19B);
			Assert.Equal(input19, output19C);
		}
	}
}