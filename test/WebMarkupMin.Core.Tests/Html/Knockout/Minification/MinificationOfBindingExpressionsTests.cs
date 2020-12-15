using Xunit;

namespace WebMarkupMin.Core.Tests.Html.Knockout.Minification
{
	public class MinificationOfBindingExpressionsTests
	{
		/// <summary>
		/// Minification of Knockout binding expressions in <code>data-bind</code> attributes
		/// </summary>
		[Fact]
		public void MinificationOfBindingExpressionsInDataBindAttributesIsCorrect()
		{
			// Arrange
			var keepingDataBindAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyKnockoutBindingExpressions = false });
			var minifyingDataBindAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyKnockoutBindingExpressions = true });

			const string input1 = "<select data-bind=\"\n" +
				"	options: availableCountries,\n" +
				"	optionsText: 'countryName',\n" +
				"	value: selectedCountry,\n" +
				"	optionsCaption: 'Choose...'\"></select>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<select data-bind=\"" +
				"options:availableCountries," +
				"optionsText:'countryName'," +
				"value:selectedCountry," +
				"optionsCaption:'Choose...'\"></select>"
				;

			const string input2 = "<div data-bind=\"visible: shouldShowMessage\">...</div>";
			const string targetOutput2A = input2;
			const string targetOutput2B = "<div data-bind=\"visible:shouldShowMessage\">...</div>";

			const string input3 = "<div data-bind=\"{ visible: shouldShowMessage }\">...</div>";
			const string targetOutput3A = input3;
			const string targetOutput3B = "<div data-bind=\"visible:shouldShowMessage\">...</div>";

			const string input4 = "The item is <span data-bind=\"text: price() > 50 ? 'expensive' : 'cheap'\"></span>.";
			const string targetOutput4A = input4;
			const string targetOutput4B = "The item is <span data-bind=\"text:price()>50?'expensive':'cheap'\"></span>.";

			const string input5 = "<button data-bind=\"enable: parseAreaCode(cellphoneNumber()) != '555'\">...</button>";
			const string targetOutput5A = input5;
			const string targetOutput5B = "<button data-bind=\"enable:parseAreaCode(cellphoneNumber())!='555'\">...</button>";

			const string input6 = "<div data-bind=\"click: function (data) { myFunction('param1', data) }\">...</div>";
			const string targetOutput6A = input6;
			const string targetOutput6B = "<div data-bind=\"click:function(data){myFunction('param1',data)}\">...</div>";

			const string input7 = "<div data-bind=\"with: {emotion: 'happy', 'facial-expression': 'smile'}\">...</div>";
			const string targetOutput7A = input7;
			const string targetOutput7B = "<div data-bind=\"with:{emotion:'happy','facial-expression':'smile'}\">...</div>";

			const string input8 = "<span data-bind=\"text\">Text that will be cleared when bindings are applied.</span>";
			const string targetOutput8A = input8;
			const string targetOutput8B = input8;

			// Act
			string output1A = keepingDataBindAttributesMinifier.Minify(input1).MinifiedContent;
			string output1B = minifyingDataBindAttributesMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingDataBindAttributesMinifier.Minify(input2).MinifiedContent;
			string output2B = minifyingDataBindAttributesMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingDataBindAttributesMinifier.Minify(input3).MinifiedContent;
			string output3B = minifyingDataBindAttributesMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingDataBindAttributesMinifier.Minify(input4).MinifiedContent;
			string output4B = minifyingDataBindAttributesMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingDataBindAttributesMinifier.Minify(input5).MinifiedContent;
			string output5B = minifyingDataBindAttributesMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingDataBindAttributesMinifier.Minify(input6).MinifiedContent;
			string output6B = minifyingDataBindAttributesMinifier.Minify(input6).MinifiedContent;

			string output7A = keepingDataBindAttributesMinifier.Minify(input7).MinifiedContent;
			string output7B = minifyingDataBindAttributesMinifier.Minify(input7).MinifiedContent;

			string output8A = keepingDataBindAttributesMinifier.Minify(input8).MinifiedContent;
			string output8B = minifyingDataBindAttributesMinifier.Minify(input8).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);

			Assert.Equal(targetOutput6A, output6A);
			Assert.Equal(targetOutput6B, output6B);

			Assert.Equal(targetOutput7A, output7A);
			Assert.Equal(targetOutput7B, output7B);

			Assert.Equal(targetOutput8A, output8A);
			Assert.Equal(targetOutput8B, output8B);
		}

		/// <summary>
		/// Minification of Knockout binding expressions in containerless comments
		/// </summary>
		[Fact]
		public void MinificationOfBindingExpressionsInContainerlessCommentsIsCorrect()
		{
			// Arrange
			var keepingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyKnockoutBindingExpressions = false });
			var minifyingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyKnockoutBindingExpressions = true });

			const string input1 = "<ul>\n" +
				"	<li>This item always appears</li>\n" +
				"	<!--ko if: someExpressionGoesHere-->\n" +
				"		<li>I want to make this item present/absent dynamically</li>\n" +
				"	<!--/ko-->\n" +
				"</ul>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<ul>\n" +
				"	<li>This item always appears</li>\n" +
				"	<!--ko if:someExpressionGoesHere-->\n" +
				"		<li>I want to make this item present/absent dynamically</li>\n" +
				"	<!--/ko-->\n" +
				"</ul>"
				;

			const string input2 = "<ul>\n" +
				"	<li>This item always appears</li>\n" +
				"	<!--ko { if: someExpressionGoesHere }-->\n" +
				"		<li>I want to make this item present/absent dynamically</li>\n" +
				"	<!--/ko-->\n" +
				"</ul>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = "<ul>\n" +
				"	<li>This item always appears</li>\n" +
				"	<!--ko if:someExpressionGoesHere-->\n" +
				"		<li>I want to make this item present/absent dynamically</li>\n" +
				"	<!--/ko-->\n" +
				"</ul>"
				;

			const string input3 = "<!--ko compose: { view: 'myView.html',\n" +
				"	mode: 'templated' }-->\n" +
				"	<div data-part=\"content\">This is a view part override of the default content....</div>\n" +
				"<!--/ko-->"
				;
			const string targetOutput3A = input3;
			const string targetOutput3B = "<!--ko compose:{view:'myView.html'," +
				"mode:'templated'}-->\n" +
				"	<div data-part=\"content\">This is a view part override of the default content....</div>\n" +
				"<!--/ko-->"
				;

			// Act
			string output1A = keepingExpressionsMinifier.Minify(input1).MinifiedContent;
			string output1B = minifyingExpressionsMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingExpressionsMinifier.Minify(input2).MinifiedContent;
			string output2B = minifyingExpressionsMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingExpressionsMinifier.Minify(input3).MinifiedContent;
			string output3B = minifyingExpressionsMinifier.Minify(input3).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
		}
	}
}