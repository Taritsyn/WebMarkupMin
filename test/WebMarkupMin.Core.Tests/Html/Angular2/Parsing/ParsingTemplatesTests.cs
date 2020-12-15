using Xunit;

namespace WebMarkupMin.Core.Tests.Html.Angular2.Parsing
{
	public class ParsingTemplatesTests
	{
		[Fact]
		public void ParsingOfPropertyBindingsIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true){ PreserveCase = true });

			const string input1 = "<input [value]=\"\t  firstName  \t\" [placeholder]=\"\t  firstNamePlaceholder  \t\">";
			const string input2 = "<input bind-value=\"\t  firstName  \t\" bind-placeholder=\"\t  firstNamePlaceholder  \t\">";
			const string input3 = "<div [ngClass]=\"{ selected: isSelected }\"></div>";
			const string input4 = "<div bind-ngClass=\"{ selected: isSelected }\"></div>";
			const string input5 = "<hero-detail prefix=\"You are my\" [hero]=\"\t  currentHero  \t\"></hero-detail>";
			const string input6 = "<hero-detail prefix=\"You are my\" bind-hero=\"\t  currentHero  \t\"></hero-detail>";
			const string input7 = "<div [textContent]=\"'News has the following title: ' + title\"></div>";
			const string input8 = "<div bind-textContent=\"'News has the following title: ' + title\"></div>";
			const string input9 = "<button [attr.aria-label]=\"\t  actionName  \t\">Perform action</button>";
			const string input10 = "<button bind-attr.aria-label=\"\t  actionName  \t\">Perform action</button>";
			const string input11 = "<div class=\"special\" [class.special]=\"\t  !isSpecial  \t\"></div>";
			const string input12 = "<div class=\"special\" bind-class.special=\"\t  !isSpecial  \t\"></div>";
			const string input13 = "<button [style.fontSize.%]=\"\t  !isSpecial ? 150  :  50  \t\">Small</button>";
			const string input14 = "<button bind-style.fontSize.%=\"\t  !isSpecial ? 150  :  50  \t\">Small</button>";

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
		}

		[Fact]
		public void ParsingOfEventBindingsIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true) { PreserveCase = true });

			const string input1 = "<button (click)=\"\t  onSave()  \t\">Save</button>";
			const string input2 = "<button on-click=\"\t  onSave()  \t\">Save</button>";
			const string input3 = "<div (myClick)=\"\t  clickMessage = $event;  \t\">click with myClick</div>";
			const string input4 = "<div on-myClick=\"\t  clickMessage = $event;  \t\">click with myClick</div>";
			const string input5 = "<hero-detail (deleteRequest)=\"\t  deleteHero($event);  \t\"></hero-detail>";
			const string input6 = "<hero-detail on-deleteRequest=\"\t  deleteHero($event);  \t\"></hero-detail>";

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;
			string output3 = minifier.Minify(input3).MinifiedContent;
			string output4 = minifier.Minify(input4).MinifiedContent;
			string output5 = minifier.Minify(input5).MinifiedContent;
			string output6 = minifier.Minify(input6).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(input2, output2);
			Assert.Equal(input3, output3);
			Assert.Equal(input4, output4);
			Assert.Equal(input5, output5);
			Assert.Equal(input6, output6);
		}

		[Fact]
		public void ParsingOfBubblingEventBindingsIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true) { PreserveCase = true });

			const string input = "<div (^click)=\"\t  onSave()  \t\">\n" +
				"	<span>Save</span>\n" +
				"</div>"
				;

			// Act
			string output = minifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(input, output);
		}

		[Fact]
		public void ParsingOfTwoWayDataBindingsIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true) { PreserveCase = true });

			const string input1 = "<input [value]=\"\t  currentHero.firstName  \t\"" +
				" (input)=\"\t  currentHero.firstName = $event.target.value;  \t\">"
				;
			const string input2 = "<input bind-value=\"\t  currentHero.firstName  \t\"" +
				" on-input=\"\t  currentHero.firstName = $event.target.value;  \t\">"
				;
			const string input3 = "<input [ngModel]=\"\t  currentHero.firstName  \t\"" +
				" (ngModelChange)=\"\t  currentHero.firstName = $event;  \t\">"
				;
			const string input4 = "<input bind-ngModel=\"\t  currentHero.firstName  \t\"" +
				" on-ngModelChange=\"\t  currentHero.firstName = $event;  \t\">"
				;
			const string input5 = "<input [(ngModel)]=\"\t  currentHero.firstName  \t\">";
			const string input6 = "<input bindon-ngModel=\"\t  currentHero.firstName  \t\">";

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;
			string output3 = minifier.Minify(input3).MinifiedContent;
			string output4 = minifier.Minify(input4).MinifiedContent;
			string output5 = minifier.Minify(input5).MinifiedContent;
			string output6 = minifier.Minify(input6).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(input2, output2);
			Assert.Equal(input3, output3);
			Assert.Equal(input4, output4);
			Assert.Equal(input5, output5);
			Assert.Equal(input6, output6);
		}

		[Fact]
		public void ParsingOfTemplateBindingsIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true) { PreserveCase = true });

			const string input1 = "<hero-detail *ngIf=\"currentHero\" [hero]=\"currentHero\"></hero-detail>";
			const string input2 = "<hero-detail template=\"ngIf:currentHero\" [hero]=\"currentHero\"></hero-detail>";

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(input2, output2);
		}

		[Fact]
		public void ParsingOfLocalVariablesIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true) { PreserveCase = true });

			const string input1 = "<video #moviePlayer>\n" +
				"	<button (click)=\"moviePlayer.play()\"></button>\n" +
				"</video>"
				;
			const string input2 = "<video var-moviePlayer>\n" +
				"	<button (click)=\"moviePlayer.play()\"></button>\n" +
				"</video>"
				;

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(input2, output2);
		}

		[Fact]
		public void ParsingOfMustacheStyleTagsIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true) { PreserveCase = true });

			const string input1 = "<p>Employer: {{\t  employer?.companyName  \t}}</p>";
			const string input2 = "<p>My birthday is {{\t  birthday  |  date:\"MM/dd/yy\"  \t}}</p>";
			const string input3 = "<p>Message: {{\t  delayedMessage  |  async  \t}}</p>";

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;
			string output3 = minifier.Minify(input3).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(input2, output2);
			Assert.Equal(input3, output3);
		}
	}
}