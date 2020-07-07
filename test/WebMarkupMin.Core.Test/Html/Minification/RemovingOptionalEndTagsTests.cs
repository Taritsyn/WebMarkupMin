using Xunit;

namespace WebMarkupMin.Core.Test.Html.Minification
{
	public class RemovingOptionalEndTagsTests
	{
		[Fact]
		public void RemovingStructuralOptionalEndTagsIsCorrect()
		{
			// Arrange
			var keepingOptionalEndTagsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveOptionalEndTags = false });
			var removingOptionalEndTagsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveOptionalEndTags = true });
			var keepingTopLevelOptionalEndTagsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					RemoveOptionalEndTags = true,
					PreservableOptionalTagList = "html,head,body"
				}
			);

			const string input1 = "<html>\n" +
				"	<head>\n" +
				"		<title>Some title…</title>\n" +
				"	</head>\n" +
				"	<body>\n" +
				"		<div><strong>Welcome!</strong></div>\n" +
				"	</body>\n" +
				"</html>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<html>\n" +
				"	<head>\n" +
				"		<title>Some title…</title>\n" +
				"	\n" +
				"	<body>\n" +
				"		<div><strong>Welcome!</strong></div>\n" +
				"	\n"
				;
			const string targetOutput1C = input1;

			// Act
			string output1A = keepingOptionalEndTagsMinifier.Minify(input1).MinifiedContent;
			string output1B = removingOptionalEndTagsMinifier.Minify(input1).MinifiedContent;
			string output1C = keepingTopLevelOptionalEndTagsMinifier.Minify(input1).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);
		}

		[Fact]
		public void RemovingTypographicalOptionalEndTags()
		{
			// Arrange
			var keepingOptionalEndTagsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveOptionalEndTags = false });
			var removingOptionalEndTagsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveOptionalEndTags = true });
			var keepingUnsafeOptionalEndTagsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					RemoveOptionalEndTags = true,
					PreservableOptionalTagList = "li"
				}
			);

			const string input1 = "<form>" +
				"<p>Some text 1…</p>" +
				"<p>Some text 2…</p>" +
				"<textarea name=\"txtBody\" rows=\"8\" cols=\"40\"></textarea>" +
				"<br>" +
				"<button type=\"button\">Save</button>" +
				"<p>Some text 3…</p>" +
				"<p>Some text 4…</p>" +
				"<div>Some content…</div>" +
				"<p>Some text 5…</p>" +
				"</form>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<form>" +
				"<p>Some text 1…" +
				"<p>Some text 2…</p>" +
				"<textarea name=\"txtBody\" rows=\"8\" cols=\"40\"></textarea>" +
				"<br>" +
				"<button type=\"button\">Save</button>" +
				"<p>Some text 3…" +
				"<p>Some text 4…" +
				"<div>Some content…</div>" +
				"<p>Some text 5…" +
				"</form>"
				;
			const string targetOutput1C = targetOutput1B;


			const string input2 = "<div>\n" +
				"	<p>Some text…</p>\n" +
				"</div>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = "<div>\n" +
				"	<p>Some text…\n" +
				"</div>"
				;
			const string targetOutput2C = targetOutput2B;


			const string input3 = "<div>\n" +
				"	<p>Some text…</p>\n" +
				"	Some other text…\n" +
				"</div>"
				;


			const string input4 = "<a href=\"http://www.example.com/\">\n" +
				"	<p>Some text…</p>\n" +
				"</a>"
				;


			const string input5 = "<ul>\n" +
				"	<li>Item 1</li>\n" +
				"	<li>Item 2\n" +
				"		<ul>\n" +
				"			<li>Item 21</li>\n" +
				"			<li>Item 22</li>\n" +
				"		</ul>\n" +
				"	</li>\n" +
				"	<li>Item 3</li>\n" +
				"</ul>"
				;
			const string targetOutput5A = input5;
			const string targetOutput5B = "<ul>\n" +
				"	<li>Item 1\n" +
				"	<li>Item 2\n" +
				"		<ul>\n" +
				"			<li>Item 21\n" +
				"			<li>Item 22\n" +
				"		</ul>\n" +
				"	\n" +
				"	<li>Item 3\n" +
				"</ul>"
				;
			const string targetOutput5C = input5;


			const string input6 = "<dl>\n" +
				"	<dt>CoffeeScript</dt>\n" +
				"	<dd>Little language that compiles into JavaScript</dd>\n" +
				"	<dt>TypeScript</dt>\n" +
				"	<dd>Typed superset of JavaScript that compiles to plain JavaScript</dd>\n" +
				"</dl>"
				;
			const string targetOutput6A = input6;
			const string targetOutput6B = "<dl>\n" +
				"	<dt>CoffeeScript\n" +
				"	<dd>Little language that compiles into JavaScript\n" +
				"	<dt>TypeScript\n" +
				"	<dd>Typed superset of JavaScript that compiles to plain JavaScript\n" +
				"</dl>"
				;
			const string targetOutput6C = targetOutput6B;


			const string input7 = "<dl>\n" +
				"	<dt>LESS</dt>\n" +
				"	<dd><img src=\"/images/less-logo.png\" width=\"199\" height=\"81\" alt=\"LESS logo\"></dd>\n" +
				"	<dd>The dynamic stylesheet language</dd>\n" +
				"	<dt>Sass</dt>\n" +
				"	<dd><img src=\"/images/sass-logo.gif\" width=\"217\" height=\"238\" alt=\"Sass logo\"></dd>\n" +
				"	<dd>Extension of CSS3, adding nested rules, variables, mixins, selector inheritance, and more</dd>\n" +
				"</dl>"
				;
			const string targetOutput7A = input7;
			const string targetOutput7B = "<dl>\n" +
				"	<dt>LESS\n" +
				"	<dd><img src=\"/images/less-logo.png\" width=\"199\" height=\"81\" alt=\"LESS logo\">\n" +
				"	<dd>The dynamic stylesheet language\n" +
				"	<dt>Sass\n" +
				"	<dd><img src=\"/images/sass-logo.gif\" width=\"217\" height=\"238\" alt=\"Sass logo\">\n" +
				"	<dd>Extension of CSS3, adding nested rules, variables, mixins, selector inheritance, and more\n" +
				"</dl>"
				;
			const string targetOutput7C = targetOutput7B;


			const string input8 = "<ruby>\n" +
				"	攻殻\n" +
				"	<rt>こうかく</rt>\n" +
				"	機動隊\n" +
				"	<rt>きどうたい</rt>\n" +
				"</ruby>"
				;
			const string targetOutput8A = input8;
			const string targetOutput8B = "<ruby>\n" +
				"	攻殻\n" +
				"	<rt>こうかく</rt>\n" +
				"	機動隊\n" +
				"	<rt>きどうたい\n" +
				"</ruby>"
				;
			const string targetOutput8C = targetOutput8B;


			const string input9 = "<ruby>\n" +
				"	攻殻\n" +
				"	<rp>（</rp>\n" +
				"	<rt>こうかく</rt>\n" +
				"	<rp>）</rp>\n" +
				"	機動隊\n" +
				"	<rp>（</rp>\n" +
				"	<rt>きどうたい</rt>\n" +
				"	<rp>）</rp>\n" +
				"</ruby>"
				;
			const string targetOutput9A = input9;
			const string targetOutput9B = "<ruby>\n" +
				"	攻殻\n" +
				"	<rp>（\n" +
				"	<rt>こうかく\n" +
				"	<rp>）</rp>\n" +
				"	機動隊\n" +
				"	<rp>（\n" +
				"	<rt>きどうたい\n" +
				"	<rp>）\n" +
				"</ruby>"
				;
			const string targetOutput9C = targetOutput9B;


			const string input10 = "<ruby>\n" +
				"	<ruby>\n" +
				"		攻\n" +
				"		<rp>（</rp>\n" +
				"		<rt>こう</rt>\n" +
				"		<rp>）</rp>\n" +
				"		殻\n" +
				"		<rp>（</rp>\n" +
				"		<rt>かく</rt>\n" +
				"		<rp>）</rp>\n" +
				"		機\n" +
				"		<rp>（</rp>\n" +
				"		<rt>き</rt>\n" +
				"		<rp>）</rp>\n" +
				"		動\n" +
				"		<rp>（</rp>\n" +
				"		<rt>どう</rt>\n" +
				"		<rp>）</rp>\n" +
				"		隊\n" +
				"		<rp>（</rp>\n" +
				"		<rt>たい</rt>\n" +
				"		<rp>）</rp>\n" +
				"	</ruby>\n" +
				"	<rp>（</rp>\n" +
				"	<rt>Kōkakukidōtai</rt>\n" +
				"	<rp>）</rp>\n" +
				"</ruby>"
				;
			const string targetOutput10A = input10;
			const string targetOutput10B = "<ruby>\n" +
				"	<ruby>\n" +
				"		攻\n" +
				"		<rp>（\n" +
				"		<rt>こう\n" +
				"		<rp>）</rp>\n" +
				"		殻\n" +
				"		<rp>（\n" +
				"		<rt>かく\n" +
				"		<rp>）</rp>\n" +
				"		機\n" +
				"		<rp>（\n" +
				"		<rt>き\n" +
				"		<rp>）</rp>\n" +
				"		動\n" +
				"		<rp>（\n" +
				"		<rt>どう\n" +
				"		<rp>）</rp>\n" +
				"		隊\n" +
				"		<rp>（\n" +
				"		<rt>たい\n" +
				"		<rp>）\n" +
				"	</ruby>\n" +
				"	<rp>（\n" +
				"	<rt>Kōkakukidōtai\n" +
				"	<rp>）\n" +
				"</ruby>"
				;
			const string targetOutput10C = targetOutput10B;


			const string input11 = "<ruby>\n" +
				"	<rb>家辺 勝文</rb>\n" +
				"	<rt>liaison</rt>\n" +
				"</ruby>"
				;
			const string targetOutput11A = input11;
			const string targetOutput11B = "<ruby>\n" +
				"	<rb>家辺 勝文\n" +
				"	<rt>liaison\n" +
				"</ruby>"
				;
			const string targetOutput11C = targetOutput11B;


			const string input12 = "<ruby>\n" +
				"	♥<rp>: </rp><rt>Heart</rt><rp>, </rp><rtc><rt lang=\"fr\">Cœur</rt></rtc><rp>.</rp>\n" +
				"	☘<rp>: </rp><rt>Shamrock</rt><rp>, </rp><rtc><rt lang=\"fr\">Trèfle</rt></rtc><rp>.</rp>\n" +
				"	✶<rp>: </rp><rt>Star</rt><rp>, </rp><rtc><rt lang=\"fr\">Étoile</rt></rtc><rp>.</rp>\n" +
				"</ruby>"
				;
			const string targetOutput12A = input12;
			const string targetOutput12B = "<ruby>\n" +
				"	♥<rp>: <rt>Heart<rp>, <rtc><rt lang=\"fr\">Cœur<rp>.</rp>\n" +
				"	☘<rp>: <rt>Shamrock<rp>, <rtc><rt lang=\"fr\">Trèfle<rp>.</rp>\n" +
				"	✶<rp>: <rt>Star<rp>, <rtc><rt lang=\"fr\">Étoile<rp>.\n" +
				"</ruby>"
				;
			const string targetOutput12C = targetOutput12B;


			const string input13 = "<ruby>\n" +
				"	<rb>旧</rb>\n" +
				"	<rb>金</rb>\n" +
				"	<rb>山</rb>\n" +
				"	<rt>jiù</rt>\n" +
				"	<rt>jīn</rt>\n" +
				"	<rt>shān</rt>\n" +
				"	<rtc>San Francisco</rtc>\n" +
				"</ruby>"
				;
			const string targetOutput13A = input13;
			const string targetOutput13B = "<ruby>\n" +
				"	<rb>旧\n" +
				"	<rb>金\n" +
				"	<rb>山\n" +
				"	<rt>jiù\n" +
				"	<rt>jīn\n" +
				"	<rt>shān\n" +
				"	<rtc>San Francisco\n" +
				"</ruby>"
				;
			const string targetOutput13C = targetOutput13B;

			// Act
			string output1A = keepingOptionalEndTagsMinifier.Minify(input1).MinifiedContent;
			string output1B = removingOptionalEndTagsMinifier.Minify(input1).MinifiedContent;
			string output1C = keepingUnsafeOptionalEndTagsMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingOptionalEndTagsMinifier.Minify(input2).MinifiedContent;
			string output2B = removingOptionalEndTagsMinifier.Minify(input2).MinifiedContent;
			string output2C = keepingUnsafeOptionalEndTagsMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingOptionalEndTagsMinifier.Minify(input3).MinifiedContent;
			string output3B = removingOptionalEndTagsMinifier.Minify(input3).MinifiedContent;
			string output3C = keepingUnsafeOptionalEndTagsMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingOptionalEndTagsMinifier.Minify(input4).MinifiedContent;
			string output4B = removingOptionalEndTagsMinifier.Minify(input4).MinifiedContent;
			string output4C = keepingUnsafeOptionalEndTagsMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingOptionalEndTagsMinifier.Minify(input5).MinifiedContent;
			string output5B = removingOptionalEndTagsMinifier.Minify(input5).MinifiedContent;
			string output5C = keepingUnsafeOptionalEndTagsMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingOptionalEndTagsMinifier.Minify(input6).MinifiedContent;
			string output6B = removingOptionalEndTagsMinifier.Minify(input6).MinifiedContent;
			string output6C = keepingUnsafeOptionalEndTagsMinifier.Minify(input6).MinifiedContent;

			string output7A = keepingOptionalEndTagsMinifier.Minify(input7).MinifiedContent;
			string output7B = removingOptionalEndTagsMinifier.Minify(input7).MinifiedContent;
			string output7C = keepingUnsafeOptionalEndTagsMinifier.Minify(input7).MinifiedContent;

			string output8A = keepingOptionalEndTagsMinifier.Minify(input8).MinifiedContent;
			string output8B = removingOptionalEndTagsMinifier.Minify(input8).MinifiedContent;
			string output8C = keepingUnsafeOptionalEndTagsMinifier.Minify(input8).MinifiedContent;

			string output9A = keepingOptionalEndTagsMinifier.Minify(input9).MinifiedContent;
			string output9B = removingOptionalEndTagsMinifier.Minify(input9).MinifiedContent;
			string output9C = keepingUnsafeOptionalEndTagsMinifier.Minify(input9).MinifiedContent;

			string output10A = keepingOptionalEndTagsMinifier.Minify(input10).MinifiedContent;
			string output10B = removingOptionalEndTagsMinifier.Minify(input10).MinifiedContent;
			string output10C = keepingUnsafeOptionalEndTagsMinifier.Minify(input10).MinifiedContent;

			string output11A = keepingOptionalEndTagsMinifier.Minify(input11).MinifiedContent;
			string output11B = removingOptionalEndTagsMinifier.Minify(input11).MinifiedContent;
			string output11C = keepingUnsafeOptionalEndTagsMinifier.Minify(input11).MinifiedContent;

			string output12A = keepingOptionalEndTagsMinifier.Minify(input12).MinifiedContent;
			string output12B = removingOptionalEndTagsMinifier.Minify(input12).MinifiedContent;
			string output12C = keepingUnsafeOptionalEndTagsMinifier.Minify(input12).MinifiedContent;

			string output13A = keepingOptionalEndTagsMinifier.Minify(input13).MinifiedContent;
			string output13B = removingOptionalEndTagsMinifier.Minify(input13).MinifiedContent;
			string output13C = keepingUnsafeOptionalEndTagsMinifier.Minify(input13).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(input3, output3A);
			Assert.Equal(input3, output3B);
			Assert.Equal(input3, output3C);

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

			Assert.Equal(targetOutput13A, output13A);
			Assert.Equal(targetOutput13B, output13B);
			Assert.Equal(targetOutput13C, output13C);
		}

		[Fact]
		public void RemovingOptionalEndTagsInTablesIsCorrect()
		{
			// Arrange
			var keepingOptionalEndTagsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveOptionalEndTags = false });
			var removingOptionalEndTagsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveOptionalEndTags = true });

			const string input1 = "<table border=\"1\">\n" +
				"	<caption>Books</caption>\n" +
				"	<colgroup>\n" +
				"		<col span=\"2\" style=\"background-color: #ccc\">\n" +
				"		<col style=\"background-color: #fff\">\n" +
				"	</colgroup>\n" +
				"	<colgroup style=\"background-color: #ccc\"></colgroup>\n" +
				"	<tr>\n" +
				"		<th>Title</th>\n" +
				"		<th>Author</th>\n" +
				"		<th>Description</th>\n" +
				"		<th>Price</th>\n" +
				"	</tr>\n" +
				"	<tr>\n" +
				"		<td>HTML5 for pupils</td>\n" +
				"		<td>John Smith</td>\n" +
				"		<td>A flexible guide to developing small sites.</td>\n" +
				"		<td>$1.75</td>\n" +
				"	</tr>\n" +
				"</table>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<table border=\"1\">\n" +
				"	<caption>Books</caption>\n" +
				"	<colgroup>\n" +
				"		<col span=\"2\" style=\"background-color: #ccc\">\n" +
				"		<col style=\"background-color: #fff\">\n" +
				"	\n" +
				"	<colgroup style=\"background-color: #ccc\">\n" +
				"	<tr>\n" +
				"		<th>Title\n" +
				"		<th>Author\n" +
				"		<th>Description\n" +
				"		<th>Price\n" +
				"	\n" +
				"	<tr>\n" +
				"		<td>HTML5 for pupils\n" +
				"		<td>John Smith\n" +
				"		<td>A flexible guide to developing small sites.\n" +
				"		<td>$1.75\n" +
				"	\n" +
				"</table>"
				;


			const string input2 = "<table class=\"table\">\n" +
				"	<thead>\n" +
				"		<tr>\n" +
				"			<th>Month</th>\n" +
				"			<th>Savings</th>\n" +
				"		</tr>\n" +
				"	</thead>\n" +
				"	<tbody>\n" +
				"		<tr>\n" +
				"			<td>Jan</td>\n" +
				"			<td>$2800</td>\n" +
				"		</tr>\n" +
				"		<tr>\n" +
				"			<td>Feb</td>\n" +
				"			<td>$3000</td>\n" +
				"		</tr>\n" +
				"	</tbody>\n" +
				"</table>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = "<table class=\"table\">\n" +
				"	<thead>\n" +
				"		<tr>\n" +
				"			<th>Month\n" +
				"			<th>Savings\n" +
				"		\n" +
				"	\n" +
				"	<tbody>\n" +
				"		<tr>\n" +
				"			<td>Jan\n" +
				"			<td>$2800\n" +
				"		\n" +
				"		<tr>\n" +
				"			<td>Feb\n" +
				"			<td>$3000\n" +
				"		\n" +
				"	\n" +
				"</table>"
				;


			const string input3 = "<table class=\"table\">\n" +
				"	<thead>\n" +
				"		<tr>\n" +
				"			<th>Month</th>\n" +
				"			<th>Savings</th>\n" +
				"		</tr>\n" +
				"	</thead>\n" +
				"	<tfoot>\n" +
				"		<tr>\n" +
				"			<td>Total</td>\n" +
				"			<td>$6250</td>\n" +
				"		</tr>\n" +
				"	</tfoot>\n" +
				"	<tbody>\n" +
				"		<tr>\n" +
				"			<td>Sep</td>\n" +
				"			<td>$3100</td>\n" +
				"		</tr>\n" +
				"		<tr>\n" +
				"			<td>Oct</td>\n" +
				"			<td>$3150</td>\n" +
				"		</tr>\n" +
				"	</tbody>\n" +
				"</table>"
				;
			const string targetOutput3A = input3;
			const string targetOutput3B = "<table class=\"table\">\n" +
				"	<thead>\n" +
				"		<tr>\n" +
				"			<th>Month\n" +
				"			<th>Savings\n" +
				"		\n" +
				"	\n" +
				"	<tfoot>\n" +
				"		<tr>\n" +
				"			<td>Total\n" +
				"			<td>$6250\n" +
				"		\n" +
				"	\n" +
				"	<tbody>\n" +
				"		<tr>\n" +
				"			<td>Sep\n" +
				"			<td>$3100\n" +
				"		\n" +
				"		<tr>\n" +
				"			<td>Oct\n" +
				"			<td>$3150\n" +
				"		\n" +
				"	\n" +
				"</table>"
				;


			const string input4 = "<table class=\"table\">\n" +
				"	<thead>\n" +
				"		<tr>\n" +
				"			<th>Month</th>\n" +
				"			<th>Savings</th>\n" +
				"		</tr>\n" +
				"	</thead>\n" +
				"	<tbody id=\"firstQuarter\">\n" +
				"		<tr>\n" +
				"			<td>Jan</td>\n" +
				"			<td>$2800</td>\n" +
				"		</tr>\n" +
				"		<tr>\n" +
				"			<td>Feb</td>\n" +
				"			<td>$3000</td>\n" +
				"		</tr>\n" +
				"		<tr>\n" +
				"			<td>Mar</td>\n" +
				"			<td>$2950</td>\n" +
				"		</tr>\n" +
				"	</tbody>\n" +
				"	<tbody id=\"secondQuarter\">\n" +
				"		<tr>\n" +
				"			<td>Apr</td>\n" +
				"			<td>$2900</td>\n" +
				"		</tr>\n" +
				"		<tr>\n" +
				"			<td>May</td>\n" +
				"			<td>$3050</td>\n" +
				"		</tr>\n" +
				"		<tr>\n" +
				"			<td>Jun</td>\n" +
				"			<td>$3010</td>\n" +
				"		</tr>\n" +
				"	</tbody>\n" +
				"</table>"
				;
			const string targetOutput4A = input4;
			const string targetOutput4B = "<table class=\"table\">\n" +
				"	<thead>\n" +
				"		<tr>\n" +
				"			<th>Month\n" +
				"			<th>Savings\n" +
				"		\n" +
				"	\n" +
				"	<tbody id=\"firstQuarter\">\n" +
				"		<tr>\n" +
				"			<td>Jan\n" +
				"			<td>$2800\n" +
				"		\n" +
				"		<tr>\n" +
				"			<td>Feb\n" +
				"			<td>$3000\n" +
				"		\n" +
				"		<tr>\n" +
				"			<td>Mar\n" +
				"			<td>$2950\n" +
				"		\n" +
				"	\n" +
				"	<tbody id=\"secondQuarter\">\n" +
				"		<tr>\n" +
				"			<td>Apr\n" +
				"			<td>$2900\n" +
				"		\n" +
				"		<tr>\n" +
				"			<td>May\n" +
				"			<td>$3050\n" +
				"		\n" +
				"		<tr>\n" +
				"			<td>Jun\n" +
				"			<td>$3010\n" +
				"		\n" +
				"	\n" +
				"</table>"
				;


			const string input5 = "<table class=\"table\">\n" +
				"	<thead>\n" +
				"		<tr>\n" +
				"			<th>Month</th>\n" +
				"			<th>Savings</th>\n" +
				"		</tr>\n" +
				"	</thead>\n" +
				"	<tbody>\n" +
				"		<tr>\n" +
				"			<td>Jul</td>\n" +
				"			<td>$2900</td>\n" +
				"		</tr>\n" +
				"		<tr>\n" +
				"			<td>Oct</td>\n" +
				"			<td>$3120</td>\n" +
				"		</tr>\n" +
				"	</tbody>\n" +
				"	<tfoot>\n" +
				"		<tr>\n" +
				"			<td>Total</td>\n" +
				"			<td>$6250</td>\n" +
				"		</tr>\n" +
				"	</tfoot>\n" +
				"</table>"
				;
			const string targetOutput5A = input5;
			const string targetOutput5B = "<table class=\"table\">\n" +
				"	<thead>\n" +
				"		<tr>\n" +
				"			<th>Month\n" +
				"			<th>Savings\n" +
				"		\n" +
				"	\n" +
				"	<tbody>\n" +
				"		<tr>\n" +
				"			<td>Jul\n" +
				"			<td>$2900\n" +
				"		\n" +
				"		<tr>\n" +
				"			<td>Oct\n" +
				"			<td>$3120\n" +
				"		\n" +
				"	\n" +
				"	<tfoot>\n" +
				"		<tr>\n" +
				"			<td>Total\n" +
				"			<td>$6250\n" +
				"		\n" +
				"	\n" +
				"</table>"
				;


			const string input6 = "<table class=\"table\">\n" +
				"	<tr>\n" +
				"		<th>Some header 1…</th>\n" +
				"		<td>Some text 1…</td>\n" +
				"		<th>Some other header 1…</th>\n" +
				"		<td>Some other text 1…</td>\n" +
				"	</tr>\n" +
				"	<tr>\n" +
				"		<th>Some header 2…</th>\n" +
				"		<td>Some text 2…</td>\n" +
				"		<th>Some other header 2…</th>\n" +
				"		<td>Some other text 2…</td>\n" +
				"	</tr>\n" +
				"</table>"
				;
			const string targetOutput6A = input6;
			const string targetOutput6B = "<table class=\"table\">\n" +
				"	<tr>\n" +
				"		<th>Some header 1…\n" +
				"		<td>Some text 1…\n" +
				"		<th>Some other header 1…\n" +
				"		<td>Some other text 1…\n" +
				"	\n" +
				"	<tr>\n" +
				"		<th>Some header 2…\n" +
				"		<td>Some text 2…\n" +
				"		<th>Some other header 2…\n" +
				"		<td>Some other text 2…\n" +
				"	\n" +
				"</table>"
				;


			// Act
			string output1A = keepingOptionalEndTagsMinifier.Minify(input1).MinifiedContent;
			string output1B = removingOptionalEndTagsMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingOptionalEndTagsMinifier.Minify(input2).MinifiedContent;
			string output2B = removingOptionalEndTagsMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingOptionalEndTagsMinifier.Minify(input3).MinifiedContent;
			string output3B = removingOptionalEndTagsMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingOptionalEndTagsMinifier.Minify(input4).MinifiedContent;
			string output4B = removingOptionalEndTagsMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingOptionalEndTagsMinifier.Minify(input5).MinifiedContent;
			string output5B = removingOptionalEndTagsMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingOptionalEndTagsMinifier.Minify(input6).MinifiedContent;
			string output6B = removingOptionalEndTagsMinifier.Minify(input6).MinifiedContent;

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
		}

		[Fact]
		public void RemovingOptionalEndTagsInSelectsIsCorrect()
		{
			// Arrange
			var keepingOptionalEndTagsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveOptionalEndTags = false });
			var removingOptionalEndTagsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveOptionalEndTags = true });

			const string input1 = "<select name=\"city\">\n" +
				"	<option>Moscow</option>\n" +
				"	<option>St. Petersburg</option>\n" +
				"	<option>Kharkiv</option>\n" +
				"</select>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<select name=\"city\">\n" +
				"	<option>Moscow\n" +
				"	<option>St. Petersburg\n" +
				"	<option>Kharkiv\n" +
				"</select>"
				;


			const string input2 = "<select name=\"preprocessors\">\n" +
				"	<optgroup label=\"Styles\">\n" +
				"		<option>Sass</option>\n" +
				"		<option>LESS</option>\n" +
				"		<option>Stylus</option>\n" +
				"	</optgroup>\n" +
				"	<optgroup label=\"Scripts\">\n" +
				"		<option>CoffeeScript</option>\n" +
				"		<option>TypeScript</option>\n" +
				"		<option>Kaffeine</option>\n" +
				"	</optgroup>\n" +
				"	<option>Dart</option>\n" +
				"</select>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = "<select name=\"preprocessors\">\n" +
				"	<optgroup label=\"Styles\">\n" +
				"		<option>Sass\n" +
				"		<option>LESS\n" +
				"		<option>Stylus\n" +
				"	\n" +
				"	<optgroup label=\"Scripts\">\n" +
				"		<option>CoffeeScript\n" +
				"		<option>TypeScript\n" +
				"		<option>Kaffeine\n" +
				"	</optgroup>\n" +
				"	<option>Dart\n" +
				"</select>"
				;


			const string input3 = "<select name=\"programming_languages\">\n" +
				"	<option>C++</option>\n" +
				"	<option>Delphi</option>\n" +
				"	<option>Java</option>\n" +
				"	<optgroup label=\".NET\">\n" +
				"		<option>C#</option>\n" +
				"		<option>VB.NET</option>\n" +
				"		<option>F#</option>\n" +
				"	</optgroup>\n" +
				"</select>"
				;
			const string targetOutput3A = input3;
			const string targetOutput3B = "<select name=\"programming_languages\">\n" +
				"	<option>C++\n" +
				"	<option>Delphi\n" +
				"	<option>Java\n" +
				"	<optgroup label=\".NET\">\n" +
				"		<option>C#\n" +
				"		<option>VB.NET\n" +
				"		<option>F#\n" +
				"	\n" +
				"</select>"
				;

			// Act
			string output1A = keepingOptionalEndTagsMinifier.Minify(input1).MinifiedContent;
			string output1B = removingOptionalEndTagsMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingOptionalEndTagsMinifier.Minify(input2).MinifiedContent;
			string output2B = removingOptionalEndTagsMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingOptionalEndTagsMinifier.Minify(input3).MinifiedContent;
			string output3B = removingOptionalEndTagsMinifier.Minify(input3).MinifiedContent;

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