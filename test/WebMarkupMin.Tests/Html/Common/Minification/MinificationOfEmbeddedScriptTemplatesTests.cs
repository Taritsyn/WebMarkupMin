using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class MinificationOfEmbeddedScriptTemplatesTests
	{
		[Fact]
		public void MinificationOfEmbeddedScriptTemplatesIsCorrect()
		{
			// Arrange
			var keepingEmbeddedJsTemplatesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					ProcessableScriptTypeList = "",
					WhitespaceMinificationMode = WhitespaceMinificationMode.Medium
				});
			var minifyingEmbeddedJsTemplatesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					ProcessableScriptTypeList = "text/html,text/x-kendo-template,text/ng-template",
					WhitespaceMinificationMode = WhitespaceMinificationMode.Medium
				});

			const string input1 = "<script type=\"text/html\" id=\"person-template\">\n" +
				"	<h3 data-bind=\"text: name\"></h3>\n" +
				"	<p>Credits: <span data-bind=\"text: credits\"></span></p>\n" +
				"</script>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<script type=\"text/html\" id=\"person-template\">" +
				"<h3 data-bind=\"text: name\"></h3>" +
				"<p>Credits: <span data-bind=\"text: credits\"></span></p>" +
				"</script>"
				;

			const string input2 = "<script id=\"ul-template\" type=\"text/x-kendo-template\">\n" +
				"	<li> id: <span data-bind=\"text: id\"></span> </li>\n" +
				"	<li> details: <span data-bind=\"text: id\"></span> </li>\n" +
				"</script>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = "<script id=\"ul-template\" type=\"text/x-kendo-template\">" +
				"<li>id: <span data-bind=\"text: id\"></span></li>" +
				"<li>details: <span data-bind=\"text: id\"></span></li>" +
				"</script>"
				;

			const string input3 = "<script id=\"entry-template\" type=\"text/x-handlebars-template\">\n" +
				"	<div class=\"entry\">\n" +
				"		<h1>{{title}}</h1>\n" +
				"		<div class=\"body\">\n" +
				"			{{{body}}}\n" +
				"		</div>\n" +
				"	</div>\n" +
				"</script>"
				;

			const string input4 = "<script type=\"text/ng-template\" id=\"questionnaire\">\n" +
				"	<div class=\"question\">\n" +
				"		<span>{{question.number}}.</span>&nbsp;\n" +
				"		<span>{{question.question}}</span> -&nbsp;\n" +
				"		<span>{{question.type}}</span>\n" +
				"		<div class=\"answer\" ng-include=\"'templates/'+question.type+'.html'\">\n" +
				"		</div>\n" +
				"	</div>\n" +
				"</script>"
				;
			const string targetOutput4A = input4;
			const string targetOutput4B = "<script type=\"text/ng-template\" id=\"questionnaire\">" +
				"<div class=\"question\">" +
				"<span>{{question.number}}.</span>&nbsp; " +
				"<span>{{question.question}}</span> -&nbsp; " +
				"<span>{{question.type}}</span>" +
				"<div class=\"answer\" ng-include=\"'templates/'+question.type+'.html'\">" +
				"</div>" +
				"</div>" +
				"</script>"
				;

			// Act
			string output1A = keepingEmbeddedJsTemplatesMinifier.Minify(input1).MinifiedContent;
			string output1B = minifyingEmbeddedJsTemplatesMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingEmbeddedJsTemplatesMinifier.Minify(input2).MinifiedContent;
			string output2B = minifyingEmbeddedJsTemplatesMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingEmbeddedJsTemplatesMinifier.Minify(input3).MinifiedContent;
			string output3B = minifyingEmbeddedJsTemplatesMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingEmbeddedJsTemplatesMinifier.Minify(input4).MinifiedContent;
			string output4B = minifyingEmbeddedJsTemplatesMinifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);

			Assert.Equal(input3, output3A);
			Assert.Equal(input3, output3B);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
		}
	}
}