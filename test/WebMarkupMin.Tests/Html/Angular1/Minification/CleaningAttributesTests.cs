using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Angular1.Minification
{
	public class CleaningAttributesTests
	{
		[Fact]
		public void CleaningClassAttributes()
		{
			// Arrange
			var cleaningAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<span class=\" label  done-{{\t  todo.done  \t}} \">{{todo.text}}</span>";
			const string targetOutput1 = "<span class=\"label done-{{\t  todo.done  \t}}\">{{todo.text}}</span>";

			const string input2 = "<span class=\"ng-bind: 'Mr. ' + name;\"></span>";
			const string targetOutput2 = "<span class=\"ng-bind:'Mr. ' + name\"></span>";

			const string input3 = "<p class=\"ng-class: { strike: deleted, bold: important, red: error };\">" +
				"Map Syntax Example</p>";
			const string targetOutput3 = "<p class=\"ng-class:{ strike: deleted, bold: important, red: error }\">" +
				"Map Syntax Example</p>";

			const string input4 = "<div class=\"ng-cloak\">{{'hello IE7'}}</div>";
			const string targetOutput4 = input4;

			const string input5 = "<div class=\"ng-form: 'frmMain';\"></div>";
			const string targetOutput5 = "<div class=\"ng-form:'frmMain'\"></div>";

			const string input6 = "<div class=\"ng-include: 'myPartialTemplate.html'; onload: childOnLoad();" +
				" autoscroll;\"></div>";
			const string targetOutput6 = "<div class=\"ng-include:'myPartialTemplate.html';onload:childOnLoad();" +
				"autoscroll\"></div>";

			const string input7 = "<span class=\"data-ng-bind: name; ng-cloak; " +
				"ng-style: { 'background-color': 'lime' };\"></span>";
			const string targetOutput7 = "<span class=\"data-ng-bind:name;ng-cloak;" +
				"ng-style:{ 'background-color': 'lime' }\"></span>";

			const string input8 = "<button class=\"btn  data-ng-bind: buttonText;  btn-primary  btn-lg\"></button>";
			const string targetOutput8 = "<button class=\"btn data-ng-bind:buttonText; btn-primary btn-lg\"></button>";

			// Act
			string output1 = cleaningAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = cleaningAttributesMinifier.Minify(input2).MinifiedContent;
			string output3 = cleaningAttributesMinifier.Minify(input3).MinifiedContent;
			string output4 = cleaningAttributesMinifier.Minify(input4).MinifiedContent;
			string output5 = cleaningAttributesMinifier.Minify(input5).MinifiedContent;
			string output6 = cleaningAttributesMinifier.Minify(input6).MinifiedContent;
			string output7 = cleaningAttributesMinifier.Minify(input7).MinifiedContent;
			string output8 = cleaningAttributesMinifier.Minify(input8).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
			Assert.Equal(targetOutput6, output6);
			Assert.Equal(targetOutput7, output7);
			Assert.Equal(targetOutput8, output8);
		}

		[Fact]
		public void CleaningStyleAttributes()
		{
			// Arrange
			var cleaningAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input = "<div style=\"\t  height: 25px; width: 25px; background-color: {{\t  color  \t}};  \t\"></div>";
			const string targetOutput = "<div style=\"height: 25px; width: 25px; background-color: {{\t  color  \t}}\"></div>";

			// Act
			string output = cleaningAttributesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput, output);
		}

		[Fact]
		public void CleaningUriBasedAttributes()
		{
			// Arrange
			var cleaningAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input = "<img src=\"  /Content/images/icons/{{\t  iconName  \t}}.png  \">";
			const string targetOutput = "<img src=\"/Content/images/icons/{{\t  iconName  \t}}.png\">";

			// Act
			string output = cleaningAttributesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput, output);
		}

		[Fact]
		public void CleaningNumericAttributes()
		{
			// Arrange
			var cleaningAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input = "<select size=\"  {{\t  listSize  \t}}  \"></select>";
			const string targetOutput = "<select size=\"{{\t  listSize  \t}}\"></select>";

			// Act
			string output = cleaningAttributesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput, output);
		}

		[Fact]
		public void CleaningEventAttributes()
		{
			// Arrange
			var cleaningAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input = "<button onclick=\"  showMessageBox('Error', '\t  {{\t  message  \t}}  \t');  \">" +
				"Show message</button>";
			const string targetOutput = "<button onclick=\"showMessageBox('Error', '\t  {{\t  message  \t}}  \t')\">" +
				"Show message</button>";

			// Act
			string output = cleaningAttributesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput, output);
		}

		[Fact]
		public void CleaningOtherAttributes()
		{
			// Arrange
			var cleaningAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<input type=\"text\" value=\"  {{\t  text  \t}}  \">";
			const string targetOutput1 = input1;

			const string input2 = "<pre ng-bind-template=\"  {{\t  salutation  \t}} {{\t  name  \t}}!  \"></pre>";
			const string targetOutput2 = input2;

			// Act
			string output1 = cleaningAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = cleaningAttributesMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
		}
	}
}