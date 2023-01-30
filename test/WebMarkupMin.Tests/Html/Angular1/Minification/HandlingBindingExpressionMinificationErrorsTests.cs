using System.Collections.Generic;

using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Angular1.Minification
{
	public class HandlingBindingExpressionMinificationErrorsTests
	{
		[Fact]
		public void HandlingMinificationErrorsInMustacheStyleTagsContainedInText()
		{
			// Arrange
			var minifyingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyAngularBindingExpressions = true });

			const string input = "<ul>\n" +
				"	<li data-ng-repeat=\"customer in customers\">{{ customer.name\" }} - {{ customer.city/* }}</li>\n" +
				"</ul>"
				;

			// Act
			IList<MinificationErrorInfo> errors = minifyingExpressionsMinifier.Minify(input).Errors;

			// Assert
			Assert.Equal(2, errors.Count);
			Assert.Equal(2, errors[0].LineNumber);
			Assert.Equal(45, errors[0].ColumnNumber);
			Assert.Equal(2, errors[1].LineNumber);
			Assert.Equal(68, errors[1].ColumnNumber);
		}

		[Fact]
		public void HandlingMinificationErrorsInMustacheStyleTagsContainedInAttributeValues()
		{
			// Arrange
			var minifyingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyAngularBindingExpressions = true });

			const string input1 = "<label for=\"txtPrice\">Price:</label>\n" +
				"<input type=\"text\" id=\"txtPrice\" name=\"price\" value=\"{{ price ' currency }}\">"
				;
			const string input2 = "<li>\n" +
				"    <img src=\"/Content/images/icons/{{ iconName + '.png }}\">\n" +
				"</li>"
				;
			const string input3 = "<pre ng-bind-template=\"{{ /*/ salutation }} {{ ' name }}!\"></pre>";

			// Act
			IList<MinificationErrorInfo> errors1 = minifyingExpressionsMinifier.Minify(input1).Errors;
			IList<MinificationErrorInfo> errors2 = minifyingExpressionsMinifier.Minify(input2).Errors;
			IList<MinificationErrorInfo> errors3 = minifyingExpressionsMinifier.Minify(input3).Errors;

			// Assert
			Assert.Equal(1, errors1.Count);
			Assert.Equal(2, errors1[0].LineNumber);
			Assert.Equal(54, errors1[0].ColumnNumber);

			Assert.Equal(1, errors2.Count);
			Assert.Equal(2, errors2[0].LineNumber);
			Assert.Equal(37, errors2[0].ColumnNumber);

			Assert.Equal(2, errors3.Count);
			Assert.Equal(1, errors3[0].LineNumber);
			Assert.Equal(24, errors3[0].ColumnNumber);
			Assert.Equal(1, errors3[1].LineNumber);
			Assert.Equal(45, errors3[1].ColumnNumber);
		}

		[Fact]
		public void HandlingMinificationErrorsInElementDirectives()
		{
			// Arrange
			var minifyingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyAngularBindingExpressions = true });

			const string input = "<ng-pluralize count=\" /*/personCount \"" +
				" when=\"{ '0': 'Nobody is viewing.',\n" +
				"			'one: '1 person is viewing.',\n" +
				"			'other': '{} people are viewing.'}\">\n" +
				"</ng-pluralize>"
				;

			// Act
			IList<MinificationErrorInfo> errors = minifyingExpressionsMinifier.Minify(input).Errors;

			// Assert
			Assert.Equal(2, errors.Count);
			Assert.Equal(1, errors[0].LineNumber);
			Assert.Equal(22, errors[0].ColumnNumber);
			Assert.Equal(1, errors[1].LineNumber);
			Assert.Equal(46, errors[1].ColumnNumber);
		}

		[Fact]
		public void HandlingMinificationErrorsInAttributeDirectives()
		{
			// Arrange
			var minifyingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyAngularBindingExpressions = true });

			const string input = "<input ng-model=\" user.name /*\"" +
				" ng-model-options=\"{ updateOn: 'default blur', debounce: {'default: 500, 'blur': 0} }\">";

			// Act
			IList<MinificationErrorInfo> errors = minifyingExpressionsMinifier.Minify(input).Errors;

			// Assert
			Assert.Equal(2, errors.Count);
			Assert.Equal(1, errors[0].LineNumber);
			Assert.Equal(18, errors[0].ColumnNumber);
			Assert.Equal(1, errors[1].LineNumber);
			Assert.Equal(51, errors[1].ColumnNumber);
		}

		[Fact]
		public void HandlingMinificationErrorsInClassDirectives()
		{
			// Arrange
			var minifyingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyAngularBindingExpressions = true });

			const string input = "<p class=\"ng-class: { 'strike': deleted, 'bold: important, 'red': error }; " +
				"ng-bind: 'Mr.  + name\">Map Syntax Example</p>";

			// Act
			IList<MinificationErrorInfo> errors = minifyingExpressionsMinifier.Minify(input).Errors;

			// Assert
			Assert.Equal(2, errors.Count);
			Assert.Equal(1, errors[0].LineNumber);
			Assert.Equal(20, errors[0].ColumnNumber);
			Assert.Equal(1, errors[1].LineNumber);
			Assert.Equal(84, errors[1].ColumnNumber);
		}

		[Fact]
		public void HandlingMinificationErrorsInCommentDirectives()
		{
			// Arrange
			var minifyingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					MinifyAngularBindingExpressions = true,
					CustomAngularDirectiveList = "myDirective"
				});

			const string input = "<!-- directive: my-directive 'a' + 'b + 'c' -->";

			// Act
			IList<MinificationErrorInfo> errors = minifyingExpressionsMinifier.Minify(input).Errors;

			// Assert
			Assert.Equal(1, errors.Count);
			Assert.Equal(1, errors[0].LineNumber);
			Assert.Equal(30, errors[0].ColumnNumber);
		}
	}
}