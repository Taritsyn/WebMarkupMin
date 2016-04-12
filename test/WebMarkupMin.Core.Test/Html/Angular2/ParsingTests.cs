using Xunit;

namespace WebMarkupMin.Core.Test.Html.Angular2
{
	public class ParsingTests
	{
		[Fact]
		public void ParsingIsCorrect()
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
			const string input15 = "<button (click)=\"\t  onSave()  \t\">Save</button>";
			const string input16 = "<div (^click)=\"\t  onSave()  \t\">\n" +
				"	<span>Save</span>\n" +
				"</div>"
				;
			const string input17 = "<button on-click=\"\t  onSave()  \t\">Save</button>";
			const string input18 = "<div (myClick)=\"\t  clickMessage = $event;  \t\">click with myClick</div>";
			const string input19 = "<div on-myClick=\"\t  clickMessage = $event;  \t\">click with myClick</div>";
			const string input20 = "<hero-detail (deleteRequest)=\"\t  deleteHero($event);  \t\"></hero-detail>";
			const string input21 = "<hero-detail on-deleteRequest=\"\t  deleteHero($event);  \t\"></hero-detail>";
			const string input22 = "<input [value]=\"\t  currentHero.firstName  \t\"" +
				" (input)=\"\t  currentHero.firstName = $event.target.value;  \t\">"
				;
			const string input23 = "<input bind-value=\"\t  currentHero.firstName  \t\"" +
				" on-input=\"\t  currentHero.firstName = $event.target.value;  \t\">"
				;
			const string input24 = "<input [ngModel]=\"\t  currentHero.firstName  \t\"" +
				" (ngModelChange)=\"\t  currentHero.firstName = $event;  \t\">"
				;
			const string input25 = "<input bind-ngModel=\"\t  currentHero.firstName  \t\"" +
				" on-ngModelChange=\"\t  currentHero.firstName = $event;  \t\">"
				;
			const string input26 = "<input [(ngModel)]=\"\t  currentHero.firstName  \t\">";
			const string input27 = "<input bindon-ngModel=\"\t  currentHero.firstName  \t\">";
			const string input28 = "<hero-detail *ngIf=\"currentHero\" [hero]=\"currentHero\"></hero-detail>";
			const string input29 = "<hero-detail template=\"ngIf:currentHero\" [hero]=\"currentHero\"></hero-detail>";
			const string input30 = "<video #moviePlayer>\n" +
				"	<button (click)=\"moviePlayer.play()\"></button>\n" +
				"</video>"
				;
			const string input31 = "<video var-moviePlayer>\n" +
				"	<button (click)=\"moviePlayer.play()\"></button>\n" +
				"</video>"
				;
			const string input32 = "<p>Employer: {{\t  employer?.companyName  \t}}</p>";
			const string input33 = "<p>My birthday is {{\t  birthday  |  date:\"MM/dd/yy\"  \t}}</p>";
			const string input34 = "<p>Message: {{\t  delayedMessage  |  async  \t}}</p>";

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
			string output29 = minifier.Minify(input29).MinifiedContent;
			string output30 = minifier.Minify(input30).MinifiedContent;
			string output31 = minifier.Minify(input31).MinifiedContent;
			string output32 = minifier.Minify(input32).MinifiedContent;
			string output33 = minifier.Minify(input33).MinifiedContent;
			string output34 = minifier.Minify(input34).MinifiedContent;

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
			Assert.Equal(input29, output29);
			Assert.Equal(input30, output30);
			Assert.Equal(input31, output31);
			Assert.Equal(input32, output32);
			Assert.Equal(input33, output33);
			Assert.Equal(input34, output34);
		}
	}
}