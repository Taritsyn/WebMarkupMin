using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Polymer.Parsing
{
	public class ParsingTemplatesTests
	{
		[Fact]
		public void ParsingTemplates()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<dom-module id=\"host-element\">\n" +
				"	<template>\n" +
				"		<child-element name=\"{{myName}}\"></child-element>\n" +
				"	</template>\n" +
				"</dom-module>"
				;
			const string input2 = "<span>Name: {{lastname}}, {{firstname}}</span>";
			const string input3 = "<div>{{user.manager.name}}</div>";
			const string input4 = "<div class=\"result\">\n" +
				"	{{ amount * fromCurrencyRelativeValue / toCurrencyRelativeValue }}\n" +
				"</div>"
				;
			const string input5 = "<span>{{doThisOnce()}}</span>";
			const string input6 = "My name is <span>{{computeFullName(first, last)}}</span>";
			const string input7 = "<span>{{translate('Hello\\, nice to meet you', first, last)}}</span>";
			const string input8 = "<span>{{array.0}}</span>";
			const string input9 = "<div>[[arrayItem(myArray.*, 0, 'name')]]</div>";
			const string input10 = "<custom-element some-prop=\"{{value}}\"></custom-element>";
			const string input11 = "<custom-element some-prop=\"[[value]]\"></custom-element>";
			const string input12 = "<user-view first=\"{{user.first}}\" last=\"{{user.last}}\"></user-view>";
			const string input13 = "<input value=\"{{hostValue::input}}\">";
			const string input14 = "<input value=\"{{user.name.first::input}}\">";
			const string input15 = "<input type=\"checkbox\" checked=\"{{hostChecked::change}}\">";
			const string input16 = "<video url=\"/video/movie.mp4\" current-time=\"{{hostTime::timeupdate}}\"></video>";
			const string input17 = "<my-element value=\"{{hostValue::value-changed}}\"></my-element>";
			const string input18 = "<div hidden=\"{{!enabled}}\"></div>";
			const string input19 = "<div class$=\"{{foo}}\"></div>";
			const string input20 = "<div style$=\"{{background}}\"></div>";
			const string input21 = "<img src$=\"https://www.example.com/profiles/{{userId}}.jpg\">";
			const string input22 = "<label for$=\"{{bar}}\"></label>";
			const string input23 = "<div data-bar$=\"{{baz}}\"></div>";
			const string input24 = "<my-element selected$=\"{{value}}\"></my-element>";
			const string input25 = "<template is=\"dom-repeat\" items=\"{{employees}}\">\n" +
				"	<div># <span>{{index}}</span></div>\n" +
				"	<div>First name: <span>{{item.first}}</span></div>\n" +
				"	<div>Last name: <span>{{item.last}}</span></div>\n" +
				"</template>"
				;
			const string input26 = "<template is=\"dom-repeat\" items=\"{{employees}}\" as=\"employee\" filter=\"{{computeFilter(searchString)}}\">\n" +
				"	<div>{{employee.lastname}}, {{employee.firstname}}</div>\n" +
				"</template>"
				;
			const string input27 = "<array-selector id=\"selector\" items=\"{{employees}}\" selected=\"{{selected}}\" multi toggle></array-selector>";
			const string input28 = "<template is=\"dom-if\" if=\"{{user.isAdmin}}\">\n"+
				"	Only admins will see this.\n" +
				"	<div>{{user.secretAdminStuff}}</div>\n" +
				"</template>"
				;

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;
			string output3 = minifier.Minify(input3).MinifiedContent;
			string output4 = minifier.Minify(input4).MinifiedContent;
			string output5 = minifier.Minify(input5).MinifiedContent;
			string output6 = minifier.Minify(input6).MinifiedContent;
			string output7 = minifier.Minify(input7).MinifiedContent;
			string output8 = minifier.Minify(input8).MinifiedContent;
			string output9 = minifier.Minify(input9).MinifiedContent;
			string output10 = minifier.Minify(input10).MinifiedContent;
			string output11 = minifier.Minify(input11).MinifiedContent;
			string output12 = minifier.Minify(input12).MinifiedContent;
			string output13 = minifier.Minify(input13).MinifiedContent;
			string output14 = minifier.Minify(input14).MinifiedContent;
			string output15 = minifier.Minify(input15).MinifiedContent;
			string output16 = minifier.Minify(input16).MinifiedContent;
			string output17 = minifier.Minify(input17).MinifiedContent;
			string output18 = minifier.Minify(input18).MinifiedContent;
			string output19 = minifier.Minify(input19).MinifiedContent;
			string output20 = minifier.Minify(input20).MinifiedContent;
			string output21 = minifier.Minify(input21).MinifiedContent;
			string output22 = minifier.Minify(input22).MinifiedContent;
			string output23 = minifier.Minify(input23).MinifiedContent;
			string output24 = minifier.Minify(input24).MinifiedContent;
			string output25 = minifier.Minify(input25).MinifiedContent;
			string output26 = minifier.Minify(input26).MinifiedContent;
			string output27 = minifier.Minify(input27).MinifiedContent;
			string output28 = minifier.Minify(input28).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(input2, output2);
			Assert.Equal(input3, output3);
			Assert.Equal(input4, output4);
			Assert.Equal(input5, output5);
			Assert.Equal(input6, output6);
			Assert.Equal(input7, output7);
			Assert.Equal(input8, output8);
			Assert.Equal(input9, output9);
			Assert.Equal(input10, output10);
			Assert.Equal(input11, output11);
			Assert.Equal(input12, output12);
			Assert.Equal(input13, output13);
			Assert.Equal(input14, output14);
			Assert.Equal(input15, output15);
			Assert.Equal(input16, output16);
			Assert.Equal(input17, output17);
			Assert.Equal(input18, output18);
			Assert.Equal(input19, output19);
			Assert.Equal(input20, output20);
			Assert.Equal(input21, output21);
			Assert.Equal(input22, output22);
			Assert.Equal(input23, output23);
			Assert.Equal(input24, output24);
			Assert.Equal(input25, output25);
			Assert.Equal(input26, output26);
			Assert.Equal(input27, output27);
			Assert.Equal(input28, output28);
		}
	}
}