using Xunit;

namespace WebMarkupMin.Core.Test.Html.Minification
{
	public class WhitespaceMinificationTests
	{
		[Fact]
		public void WhitespaceMinificationIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.None });
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Safe });
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive });

			const string input1 = " \n \n\t <!-- meta name=\"GENERATOR\" content=\"Microsoft FrontPage 1.0\" --> \n  \n\t\n" +
				"<!DOCTYPE html>\n" +
				"<html>\n" +
				"	<head>\n" +
				"		<meta charset=\"utf-8\">\n" +
				"		<title> \t  Some  title...  \t  </title>\n" +
				"		<base href=\"http://www.example.com/\" target=\"_blank\">\n" +
				"		<link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\">\n" +
				"		<meta name=\"viewport\" content=\"width=device-width\">\n" +
				"		<link href=\"/Bundles/CommonStyles\" rel=\"stylesheet\">\n" +
				"		<style type=\"text/css\">\n" +
				"			.ie table.min-width-content {\n" +
				"				table-layout: auto !important;\n" +
				"			}\n" +
				"		</style>\n" +
				"		<script src=\"/Bundles/Modernizr\"></script>\n" +
				"	</head>\n" +
				"	<body>\n" +
				"		<p>Some text...</p>\n" +
				"		<script src=\"http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js\"></script>\n" +
				"		<script>\n" +
				"			(window.jquery) || document.write('<script src=\"/Bundles/Jquery\"><\\/script>');  \n" +
				"</script>\n" +
				"	    <script src=\"/Bundles/CommonScripts\"></script>\n" +
				"	</body>\n" +
				"</html>\n\n\t \n " +
				"<!-- MEOW -->\t \n\n  \n "
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<!-- meta name=\"GENERATOR\" content=\"Microsoft FrontPage 1.0\" -->" +
				"<!DOCTYPE html>" +
				"<html>" +
				"<head>" +
				"<meta charset=\"utf-8\">" +
				"<title>Some title...</title>" +
				"<base href=\"http://www.example.com/\" target=\"_blank\">" +
				"<link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\">" +
				"<meta name=\"viewport\" content=\"width=device-width\">" +
				"<link href=\"/Bundles/CommonStyles\" rel=\"stylesheet\">" +
				"<style type=\"text/css\">" +
				".ie table.min-width-content {\n" +
				"				table-layout: auto !important;\n" +
				"			}" +
				"</style>" +
				"<script src=\"/Bundles/Modernizr\"></script>" +
				"</head>" +
				"<body>" +
				"<p>Some text...</p>" +
				"<script src=\"http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js\"></script>" +
				"<script>(window.jquery) || document.write('<script src=\"/Bundles/Jquery\"><\\/script>');</script>" +
				"<script src=\"/Bundles/CommonScripts\"></script>" +
				"</body>" +
				"</html>" +
				"<!-- MEOW -->"
				;
			const string targetOutput1C = targetOutput1B;
			const string targetOutput1D = targetOutput1B;


			const string input2 = "<script>alert(\"Hello,     world!\");    </script>";
			const string targetOutput2A = input2;
			const string targetOutput2B = "<script>alert(\"Hello,     world!\");</script>";
			const string targetOutput2C = targetOutput2B;
			const string targetOutput2D = targetOutput2B;


			const string input3 = "<style>cite { quotes: \" «\" \"» \"; }    </style>";
			const string targetOutput3A = input3;
			const string targetOutput3B = "<style>cite { quotes: \" «\" \"» \"; }</style>";
			const string targetOutput3C = targetOutput3B;
			const string targetOutput3D = targetOutput3B;

			const string input4 = "<table class=\"table\">\n" +
				"	<caption>	 Monthly savings \n</caption>\n" +
				"	<colgroup>\n" +
				"		<col style=\"text-align: left\">\n" +
				"		<col style=\"text-align: right\">\n" +
				"	</colgroup>\n" +
				"	<thead>\n" +
				"		<tr>\n" +
				"			<th>	 Month \n</th>\n" +
				"			<th>	 Savings \n</th>\n" +
				"		</tr>\n" +
				"	</thead>\n" +
				"	<tbody>\n" +
				"		<tr>\n" +
				"			<td>	 Jul \n</td>\n" +
				"			<td>	 $2900 \n</td>\n" +
				"		</tr>\n" +
				"		<tr>\n" +
				"			<td>	 Oct \n</td>\n" +
				"			<td>	 $3120 \n</td>\n" +
				"		</tr>\n" +
				"	</tbody>\n" +
				"	<tfoot>\n" +
				"		<tr>\n" +
				"			<td>	 Total \n</td>\n" +
				"			<td>	 $6250 \n</td>\n" +
				"		</tr>\n" +
				"	</tfoot>\n" +
				"</table>"
				;
			const string targetOutput4A = input4;
			const string targetOutput4B = "<table class=\"table\">" +
				"<caption> Monthly savings </caption>" +
				"<colgroup>" +
				"<col style=\"text-align: left\">" +
				"<col style=\"text-align: right\">" +
				"</colgroup>" +
				"<thead>" +
				"<tr>" +
				"<th> Month </th>" +
				"<th> Savings </th>" +
				"</tr>" +
				"</thead>" +
				"<tbody>" +
				"<tr>" +
				"<td> Jul </td>" +
				"<td> $2900 </td>" +
				"</tr>" +
				"<tr>" +
				"<td> Oct </td>" +
				"<td> $3120 </td>" +
				"</tr>" +
				"</tbody>" +
				"<tfoot>" +
				"<tr>" +
				"<td> Total </td>" +
				"<td> $6250 </td>" +
				"</tr>" +
				"</tfoot>" +
				"</table>"
				;
			const string targetOutput4C = "<table class=\"table\">" +
				"<caption>Monthly savings</caption>" +
				"<colgroup>" +
				"<col style=\"text-align: left\">" +
				"<col style=\"text-align: right\">" +
				"</colgroup>" +
				"<thead>" +
				"<tr>" +
				"<th>Month</th>" +
				"<th>Savings</th>" +
				"</tr>" +
				"</thead>" +
				"<tbody>" +
				"<tr>" +
				"<td>Jul</td>" +
				"<td>$2900</td>" +
				"</tr>" +
				"<tr>" +
				"<td>Oct</td>" +
				"<td>$3120</td>" +
				"</tr>" +
				"</tbody>" +
				"<tfoot>" +
				"<tr>" +
				"<td>Total</td>" +
				"<td>$6250</td>" +
				"</tr>" +
				"</tfoot>" +
				"</table>"
				;
			const string targetOutput4D = targetOutput4C;


			const string input5 = "<select name=\"preprocessors\">\n" +
				"	<optgroup label=\"Styles\">\n" +
				"		<option value=\"sass\">	 Sass \n</option>\n" +
				"		<option value=\"less\">	 LESS \n</option>\n" +
				"		<option value=\"stylus\">	 Stylus \n</option>\n" +
				"	</optgroup>\n" +
				"	<optgroup label=\"Scripts\">\n" +
				"		<option value=\"coffeescript\">	 CoffeeScript \n</option>\n" +
				"		<option value=\"typescript\">	 TypeScript \n</option>\n" +
				"		<option value=\"kaffeine\">	 Kaffeine \n</option>\n" +
				"	</optgroup>\n" +
				"	<option value=\"dart\">	 Dart \n</option>\n" +
				"</select>"
				;
			const string targetOutput5A = input5;
			const string targetOutput5B = "<select name=\"preprocessors\">" +
				"<optgroup label=\"Styles\">" +
				"<option value=\"sass\"> Sass </option>" +
				"<option value=\"less\"> LESS </option>" +
				"<option value=\"stylus\"> Stylus </option>" +
				"</optgroup>" +
				"<optgroup label=\"Scripts\">" +
				"<option value=\"coffeescript\"> CoffeeScript </option>" +
				"<option value=\"typescript\"> TypeScript </option>" +
				"<option value=\"kaffeine\"> Kaffeine </option>" +
				"</optgroup>" +
				"<option value=\"dart\"> Dart </option>" +
				"</select>"
				;
			const string targetOutput5C = targetOutput5B;
			const string targetOutput5D = "<select name=\"preprocessors\">" +
				"<optgroup label=\"Styles\">" +
				"<option value=\"sass\">Sass</option>" +
				"<option value=\"less\">LESS</option>" +
				"<option value=\"stylus\">Stylus</option>" +
				"</optgroup>" +
				"<optgroup label=\"Scripts\">" +
				"<option value=\"coffeescript\">CoffeeScript</option>" +
				"<option value=\"typescript\">TypeScript</option>" +
				"<option value=\"kaffeine\">Kaffeine</option>" +
				"</optgroup>" +
				"<option value=\"dart\">Dart</option>" +
				"</select>"
				;


			const string input6 = "<video width=\"320\" height=\"240\" poster=\"video/poster.png\" controls=\"controls\">\n" +
				"	<source src=\"video/ie6.ogv\" type=\"video/ogg\">\n" +
				"	<source src=\"video/ie6.mp4\" type=\"video/mp4\">\n" +
				"	<object type=\"application/x-shockwave-flash\" data=\"player_flv_mini.swf\" " +
				"width=\"320\" height=\"240\">\n" +
				"		<param name=\"movie\" value=\"player_flv_mini.swf\">\n" +
				"		<param name=\"wmode\" value=\"opaque\">\n" +
				"		<param name=\"allowScriptAccess\" value=\"sameDomain\">\n" +
				"		<param name=\"quality\" value=\"high\">\n" +
				"		<param name=\"menu\" value=\"true\">\n" +
				"		<param name=\"autoplay\" value=\"false\">\n" +
				"		<param name=\"autoload\" value=\"false\">\n" +
				"		<param name=\"FlashVars\" value=\"flv=video/ie6.flv&amp;width=320&amp;height=240&amp;buffer=5\">\n" +
				"		<a href=\"video/ie6.flv\">Скачать видео-файл</a>\n" +
				"	</object>\n" +
				"</video>"
				;
			const string targetOutput6A = input6;
			const string targetOutput6B = "<video width=\"320\" height=\"240\" poster=\"video/poster.png\" controls=\"controls\">" +
				"<source src=\"video/ie6.ogv\" type=\"video/ogg\">" +
				"<source src=\"video/ie6.mp4\" type=\"video/mp4\">" +
				"<object type=\"application/x-shockwave-flash\" data=\"player_flv_mini.swf\" " +
				"width=\"320\" height=\"240\">" +
				"<param name=\"movie\" value=\"player_flv_mini.swf\">" +
				"<param name=\"wmode\" value=\"opaque\">" +
				"<param name=\"allowScriptAccess\" value=\"sameDomain\">" +
				"<param name=\"quality\" value=\"high\">" +
				"<param name=\"menu\" value=\"true\">" +
				"<param name=\"autoplay\" value=\"false\">" +
				"<param name=\"autoload\" value=\"false\">" +
				"<param name=\"FlashVars\" value=\"flv=video/ie6.flv&amp;width=320&amp;height=240&amp;buffer=5\">" +
				"<a href=\"video/ie6.flv\">Скачать видео-файл</a> " +
				"</object> " +
				"</video>"
				;
			const string targetOutput6C = targetOutput6B;
			const string targetOutput6D = "<video width=\"320\" height=\"240\" poster=\"video/poster.png\" controls=\"controls\">" +
				"<source src=\"video/ie6.ogv\" type=\"video/ogg\">" +
				"<source src=\"video/ie6.mp4\" type=\"video/mp4\">" +
				"<object type=\"application/x-shockwave-flash\" data=\"player_flv_mini.swf\" " +
				"width=\"320\" height=\"240\">" +
				"<param name=\"movie\" value=\"player_flv_mini.swf\">" +
				"<param name=\"wmode\" value=\"opaque\">" +
				"<param name=\"allowScriptAccess\" value=\"sameDomain\">" +
				"<param name=\"quality\" value=\"high\">" +
				"<param name=\"menu\" value=\"true\">" +
				"<param name=\"autoplay\" value=\"false\">" +
				"<param name=\"autoload\" value=\"false\">" +
				"<param name=\"FlashVars\" value=\"flv=video/ie6.flv&amp;width=320&amp;height=240&amp;buffer=5\">" +
				"<a href=\"video/ie6.flv\">Скачать видео-файл</a>" +
				"</object>" +
				"</video>"
				;


			const string input7 = "<ul>\n" +
				"	<li>	 Item 1 \n</li>\n" +
				"	<li>	 Item 2\n" +
				"		<ul>\n" +
				"			<li>	 Item 21 \n</li>\n" +
				"			<li>	 Item 22 \n</li>\n" +
				"		</ul>\n" +
				"	</li>\n" +
				"	<li>	 Item 3 \n</li>\n" +
				"</ul>"
				;
			const string targetOutput7A = input7;
			const string targetOutput7B = "<ul>" +
				"<li> Item 1 </li> " +
				"<li> Item 2 " +
				"<ul>" +
				"<li> Item 21 </li> " +
				"<li> Item 22 </li>" +
				"</ul> " +
				"</li> " +
				"<li> Item 3 </li>" +
				"</ul>"
				;
			const string targetOutput7C = "<ul>" +
				"<li>Item 1</li>" +
				"<li>Item 2" +
				"<ul>" +
				"<li>Item 21</li>" +
				"<li>Item 22</li>" +
				"</ul>" +
				"</li>" +
				"<li>Item 3</li>" +
				"</ul>"
				;
			const string targetOutput7D = targetOutput7C;


			const string input8 = "<p>	 one  </p>    \n" +
				"<p>  two	 </p>\n\n    \n\t\t  " +
				"<div title=\"Some title...\">  three	 </div>"
				;
			const string targetOutput8A = input8;
			const string targetOutput8B = "<p> one </p> " +
				"<p> two </p> " +
				"<div title=\"Some title...\"> three </div>"
				;
			const string targetOutput8C = "<p>one</p>" +
				"<p>two</p>" +
				"<div title=\"Some title...\">three</div>"
				;
			const string targetOutput8D = targetOutput8C;


			const string input9 = "<p>  New  \n  Release	\n</p>";
			const string targetOutput9A = input9;
			const string targetOutput9B = "<p> New Release </p>";
			const string targetOutput9C = "<p>New Release</p>";
			const string targetOutput9D = targetOutput9C;


			const string input10 = "<p> I'll   tell  you   my  story  with    <span>  5   slides </span> -  " +
				"<img src=\"\"> <span>	!  </span>	</p>";
			const string targetOutput10A = input10;
			const string targetOutput10B = "<p> I'll tell you my story with <span> 5 slides </span> - " +
				"<img src=\"\"> <span> ! </span> </p>";
			const string targetOutput10C = "<p>I'll tell you my story with <span> 5 slides </span> - " +
				"<img src=\"\"> <span> ! </span></p>";
			const string targetOutput10D = "<p>I'll tell you my story with <span>5 slides</span> - " +
				"<img src=\"\"> <span>!</span></p>";


			const string input11 = "<label>		Text:  \n </label> \n\t  " +
				"<textarea> THERE IS NO     KNOWLEDGE \n\n   – \t    THAT IS NOT POWER </textarea> \t\n  ";
			const string targetOutput11A = input11;
			const string targetOutput11B = "<label> Text: </label> " +
				"<textarea> THERE IS NO     KNOWLEDGE \n\n   – \t    THAT IS NOT POWER </textarea>";
			const string targetOutput11C = targetOutput11B;
			const string targetOutput11D = "<label>Text:</label> " +
				"<textarea> THERE IS NO     KNOWLEDGE \n\n   – \t    THAT IS NOT POWER </textarea>";


			const string input12 = "<span>Some text...</span> \n\t  " +
				"<pre title=\"Some title...\">   Some     text </pre> \t\n  " +
				"<span>Some text...</span>"
				;
			const string targetOutput12A = input12;
			const string targetOutput12B = "<span>Some text...</span> " +
				"<pre title=\"Some title...\">   Some     text </pre> " +
				"<span>Some text...</span>"
				;
			const string targetOutput12C = "<span>Some text...</span>" +
				"<pre title=\"Some title...\">   Some     text </pre>" +
				"<span>Some text...</span>"
				;
			const string targetOutput12D = targetOutput12C;


			const string input13 = "<p>Some text...</p> \n\t  " +
				"<pre title=\"Some title...\">	<code>   Some     text </code>\n</pre> \t\n  " +
				"<p>Some text...</p>"
				;
			const string targetOutput13A = input13;
			const string targetOutput13B = "<p>Some text...</p> " +
				"<pre title=\"Some title...\">	<code>   Some     text </code>\n</pre> " +
				"<p>Some text...</p>"
				;
			const string targetOutput13C = "<p>Some text...</p>" +
				"<pre title=\"Some title...\">	<code>   Some     text </code>\n</pre>" +
				"<p>Some text...</p>"
				;
			const string targetOutput13D = targetOutput13C;


			const string input14 = "<p>  An  <del>	 old \n</del>  <ins>	new  \n </ins>  embedded flash animation:  \n " +
				"<embed src=\"helloworld.swf\">	 !  </p>";
			const string targetOutput14A = input14;
			const string targetOutput14B = "<p> An <del> old </del> <ins> new </ins> embedded flash animation: " +
				"<embed src=\"helloworld.swf\"> ! </p>";
			const string targetOutput14C = "<p>An <del> old </del> <ins> new </ins> embedded flash animation: " +
				"<embed src=\"helloworld.swf\"> !</p>";
			const string targetOutput14D = "<p>An <del>old</del> <ins>new</ins> embedded flash animation: " +
				"<embed src=\"helloworld.swf\"> !</p>";


			const string input15 = "<div>\n" +
				"	<svg width=\"150\" height=\"100\" viewBox=\"0 0 3 2\">\n" +
				"		<rect width=\"1\" height=\"2\" x=\"0\" fill=\"#008d46\" />\n" +
				"		<rect width=\"1\" height=\"2\" x=\"1\" fill=\"#ffffff\" />\n" +
				"		<rect width=\"1\" height=\"2\" x=\"2\" fill=\"#d2232c\" />\n" +
				"	</svg>\n" +
				"</div>"
				;
			const string targetOutput15A = input15;
			const string targetOutput15B = "<div> " +
				"<svg width=\"150\" height=\"100\" viewBox=\"0 0 3 2\">" +
				"<rect width=\"1\" height=\"2\" x=\"0\" fill=\"#008d46\" />" +
				"<rect width=\"1\" height=\"2\" x=\"1\" fill=\"#ffffff\" />" +
				"<rect width=\"1\" height=\"2\" x=\"2\" fill=\"#d2232c\" />" +
				"</svg> " +
				"</div>"
				;
			const string targetOutput15C = "<div>" +
				"<svg width=\"150\" height=\"100\" viewBox=\"0 0 3 2\">" +
				"<rect width=\"1\" height=\"2\" x=\"0\" fill=\"#008d46\" />" +
				"<rect width=\"1\" height=\"2\" x=\"1\" fill=\"#ffffff\" />" +
				"<rect width=\"1\" height=\"2\" x=\"2\" fill=\"#d2232c\" />" +
				"</svg>" +
				"</div>"
				;
			const string targetOutput15D = targetOutput15C;


			const string input16 = "<div>\n" +
				"	<svg>\n" +
				"		<text x=\"20\" y=\"40\" " +
				"transform=\"rotate(30, 20,40)\" " +
				"style=\"stroke: none; fill: #000000\">\n" +
				"\t\t  Rotated  SVG  text  \t\n" +
				"		</text>\n" +
				"	</svg>\n" +
				"</div>"
				;
			const string targetOutput16A = input16;
			const string targetOutput16B = "<div> " +
				"<svg>" +
				"<text x=\"20\" y=\"40\" " +
				"transform=\"rotate(30, 20,40)\" " +
				"style=\"stroke: none; fill: #000000\">\n" +
				"\t\t  Rotated  SVG  text  \t\n" +
				"		</text>" +
				"</svg> " +
				"</div>"
				;
			const string targetOutput16C = "<div>" +
				"<svg>" +
				"<text x=\"20\" y=\"40\" " +
				"transform=\"rotate(30, 20,40)\" " +
				"style=\"stroke: none; fill: #000000\">\n" +
				"\t\t  Rotated  SVG  text  \t\n" +
				"		</text>" +
				"</svg>" +
				"</div>"
				;
			const string targetOutput16D = targetOutput16C;


			const string input17 = "<svg width=\"300\" height=\"50\" viewBox=\"0 0 300 50\">\n" +
				"	<rect x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" fill=\"#f6f8fa\" />\n" +
				"	<text x=\"50%\" y=\"50%\" fill=\"#24292e\" font-size=\"16\" text-anchor=\"middle\">\n" +
				"	<![CDATA[\n" +
				"	Vasya Pupkin <vasya-pupkin@mail.ru>\n" +
				"	]]>\n" +
				"	</text>\n" +
				"</svg>"
				;
			const string targetOutput17A = input17;
			const string targetOutput17B = "<svg width=\"300\" height=\"50\" viewBox=\"0 0 300 50\">" +
				"<rect x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" fill=\"#f6f8fa\" />" +
				"<text x=\"50%\" y=\"50%\" fill=\"#24292e\" font-size=\"16\" text-anchor=\"middle\">\n" +
				"	<![CDATA[\n" +
				"	Vasya Pupkin <vasya-pupkin@mail.ru>\n" +
				"	]]>\n" +
				"	</text>" +
				"</svg>"
				;
			const string targetOutput17C = targetOutput17B;
			const string targetOutput17D = targetOutput17C;


			const string input18 = "<div>\n" +
				"	<math>\n" +
				"		<mrow>\n" +
				"			<mrow>\n" +
				"				<msup>\n" +
				"					<mi>a</mi>\n" +
				"					<mn>2</mn>\n" +
				"				</msup>\n" +
				"				<mo>+</mo>\n" +
				"				<msup>\n" +
				"					<mi>b</mi>\n" +
				"					<mn>2</mn>\n" +
				"				</msup>\n" +
				"			</mrow>\n" +
				"			<mo>=</mo>\n" +
				"			<msup>\n" +
				"				<mi>c</mi>\n" +
				"				<mn>2</mn>\n" +
				"			</msup>\n" +
				"		</mrow>\n" +
				"	</math>\n" +
				"</div>"
				;
			const string targetOutput18A = input18;
			const string targetOutput18B = "<div> " +
				"<math>" +
				"<mrow>" +
				"<mrow>" +
				"<msup>" +
				"<mi>a</mi>" +
				"<mn>2</mn>" +
				"</msup>" +
				"<mo>+</mo>" +
				"<msup>" +
				"<mi>b</mi>" +
				"<mn>2</mn>" +
				"</msup>" +
				"</mrow>" +
				"<mo>=</mo>" +
				"<msup>" +
				"<mi>c</mi>" +
				"<mn>2</mn>" +
				"</msup>" +
				"</mrow>" +
				"</math> " +
				"</div>"
				;
			const string targetOutput18C = "<div>" +
				"<math>" +
				"<mrow>" +
				"<mrow>" +
				"<msup>" +
				"<mi>a</mi>" +
				"<mn>2</mn>" +
				"</msup>" +
				"<mo>+</mo>" +
				"<msup>" +
				"<mi>b</mi>" +
				"<mn>2</mn>" +
				"</msup>" +
				"</mrow>" +
				"<mo>=</mo>" +
				"<msup>" +
				"<mi>c</mi>" +
				"<mn>2</mn>" +
				"</msup>" +
				"</mrow>" +
				"</math>" +
				"</div>"
				;
			const string targetOutput18D = targetOutput18C;


			const string input19 = "<dl>\n" +
				"	<dt>  Name:  </dt>\n" +
				"	<dd>  John Doe  \n" +
				"</dd>\n\n" +
				"	<dt>  Gender:  </dt>\n" +
				"	<dd>  Male  \n" +
				"</dd>\n\n" +
				"	<dt>  Day  of  Birth:  </dt>\n" +
				"	<dd>  Unknown  \n" +
				"</dd>\n" +
				"</dl>"
				;
			const string targetOutput19A = input19;
			const string targetOutput19B = "<dl>" +
				"<dt> Name: </dt> " +
				"<dd> John Doe </dd> " +
				"<dt> Gender: </dt> " +
				"<dd> Male </dd> " +
				"<dt> Day of Birth: </dt> " +
				"<dd> Unknown </dd>" +
				"</dl>"
				;
			const string targetOutput19C = "<dl>" +
				"<dt>Name:</dt>" +
				"<dd>John Doe</dd>" +
				"<dt>Gender:</dt>" +
				"<dd>Male</dd>" +
				"<dt>Day of Birth:</dt>" +
				"<dd>Unknown</dd>" +
				"</dl>"
				;
			const string targetOutput19D = targetOutput19C;


			const string input20 = "<menu>\n" +
				"	<menuitem label=\"New\" icon=\"icons/new.png\" onclick=\"new()\">\n" +
				"	</menuitem>\n" +
				"	<menuitem label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\">\n" +
				"	</menuitem>\n" +
				"	<menuitem label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\">\n" +
				"	</menuitem>\n" +
				"</menu>"
				;
			const string targetOutput20A = input20;
			const string targetOutput20B = "<menu>" +
				"<menuitem label=\"New\" icon=\"icons/new.png\" onclick=\"new()\"> </menuitem>" +
				"<menuitem label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\"> </menuitem>" +
				"<menuitem label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\"> </menuitem>" +
				"</menu>"
				;
			const string targetOutput20C = "<menu>" +
				"<menuitem label=\"New\" icon=\"icons/new.png\" onclick=\"new()\"></menuitem>" +
				"<menuitem label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\"></menuitem>" +
				"<menuitem label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\"></menuitem>" +
				"</menu>"
				;
			const string targetOutput20D = targetOutput20C;


			const string input21 = "<menu>\n" +
				"	<command type=\"command\" label=\"New\" icon=\"icons/new.png\" onclick=\"new()\">\n" +
				"	</command>\n" +
				"	<command type=\"command\" label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\">\n" +
				"	</command>\n" +
				"	<command type=\"command\" label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\">\n" +
				"	</command>\n" +
				"</menu>"
				;
			const string targetOutput21A = input21;
			const string targetOutput21B = "<menu>" +
				"<command type=\"command\" label=\"New\" icon=\"icons/new.png\" onclick=\"new()\"> </command>" +
				"<command type=\"command\" label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\"> </command>" +
				"<command type=\"command\" label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\"> </command>" +
				"</menu>"
				;
			const string targetOutput21C = "<menu>" +
				"<command type=\"command\" label=\"New\" icon=\"icons/new.png\" onclick=\"new()\"></command>" +
				"<command type=\"command\" label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\"></command>" +
				"<command type=\"command\" label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\"></command>" +
				"</menu>"
				;
			const string targetOutput21D = targetOutput21C;


			const string input22 = "<figure>\n" +
				"	<img src=\"libsass-logo.png\" alt=\"LibSass logo\" width=\"640\" height=\"320\">\n" +
				"	<figcaption>  Fig 1.  -  LibSass logo. \n" +
				"</figcaption>\n" +
				"</figure>"
				;
			const string targetOutput22A = input22;
			const string targetOutput22B = "<figure>" +
				" <img src=\"libsass-logo.png\" alt=\"LibSass logo\" width=\"640\" height=\"320\"> " +
				"<figcaption> Fig 1. - LibSass logo. </figcaption>" +
				"</figure>"
				;
			const string targetOutput22C = "<figure>" +
				"<img src=\"libsass-logo.png\" alt=\"LibSass logo\" width=\"640\" height=\"320\">" +
				"<figcaption>Fig 1. - LibSass logo.</figcaption>" +
				"</figure>"
				;
			const string targetOutput22D = targetOutput22C;


			const string input23 = "<form>\n" +
				"	<fieldset>\n" +
				"		<legend>  Personal data \n" +
				"</legend>\n" +
				"		Name: <input type=\"text\" size=\"50\"><br>\n" +
				"		Email: <input type=\"text\" size=\"50\"><br>\n" +
				"		Date of birth: <input type=\"text\" size=\"10\">\n" +
				"	</fieldset>\n" +
				"</form>"
				;
			const string targetOutput23A = input23;
			const string targetOutput23B = "<form> " +
				"<fieldset>" +
				"<legend> Personal data </legend>" +
				"Name: <input type=\"text\" size=\"50\"><br> " +
				"Email: <input type=\"text\" size=\"50\"><br> " +
				"Date of birth: <input type=\"text\" size=\"10\"> " +
				"</fieldset> " +
				"</form>"
				;
			const string targetOutput23C = "<form>" +
				"<fieldset>" +
				"<legend>Personal data</legend>" +
				"Name: <input type=\"text\" size=\"50\"><br> " +
				"Email: <input type=\"text\" size=\"50\"><br> " +
				"Date of birth: <input type=\"text\" size=\"10\">" +
				"</fieldset>" +
				"</form>"
				;
			const string targetOutput23D = targetOutput23C;


			const string input24 = "<ruby>\n" +
				"	漢  <rt>  Kan  </rt>\n" +
				"	字  <rt>  ji  </rt>\n" +
				"</ruby>"
				;
			const string targetOutput24A = input24;
			const string targetOutput24B = "<ruby> " +
				"漢 <rt> Kan </rt> " +
				"字 <rt> ji </rt> " +
				"</ruby>"
				;
			const string targetOutput24C = targetOutput24B;
			const string targetOutput24D = "<ruby>" +
				"漢 <rt>Kan</rt> " +
				"字 <rt>ji</rt>" +
				"</ruby>"
				;


			const string input25 = "<ruby>\n" +
				"	漢  <rp>  (</rp>  <rt>  Kan  </rt>  <rp>)  </rp>\n" +
				"	字  <rp>  (</rp>  <rt>  ji  </rt>  <rp>)  </rp>\n" +
				"</ruby>"
				;
			const string targetOutput25A = input25;
			const string targetOutput25B = "<ruby> " +
				"漢 <rp> (</rp> <rt> Kan </rt> <rp>) </rp> " +
				"字 <rp> (</rp> <rt> ji </rt> <rp>) </rp> " +
				"</ruby>"
				;
			const string targetOutput25C = targetOutput25B;
			const string targetOutput25D = "<ruby>" +
				"漢 <rp>(</rp> <rt>Kan</rt> <rp>)</rp> " +
				"字 <rp>(</rp> <rt>ji</rt> <rp>)</rp>" +
				"</ruby>"
				;


			const string input26 = "<ruby>\n" +
				"	東\n" +
				"	<rb>  京  </rb>\n" +
				"	<rt>  とう  </rt>\n" +
				"	<rt>  きょう  </rt>\n" +
				"</ruby>"
				;
			const string targetOutput26A = input26;
			const string targetOutput26B = "<ruby> " +
				"東 " +
				"<rb> 京 </rb> " +
				"<rt> とう </rt> " +
				"<rt> きょう </rt> " +
				"</ruby>";
			const string targetOutput26C = targetOutput26B;
			const string targetOutput26D = "<ruby>" +
				"東 " +
				"<rb>京</rb> " +
				"<rt>とう</rt> " +
				"<rt>きょう</rt>" +
				"</ruby>"
				;


			const string input27 = "<ruby>\n" +
				"	♥\n" +
				"	<rp>:  </rp>\n" +
				"	<rt>  Heart  </rt>\n" +
				"	<rp>,  </rp>\n" +
				"	<rtc>\n" +
				"		<rt lang=\"ru\">  Сердце  </rt>\n" +
				"	</rtc>\n" +
				"	<rp>.  </rp>\n" +
				"	☘\n" +
				"	<rp>:  </rp>\n" +
				"	<rt>  Shamrock  </rt>\n" +
				"	<rp>,  </rp>\n" +
				"	<rtc>\n" +
				"		<rt lang=\"ru\">  Трилистник  </rt>\n" +
				"	</rtc>\n" +
				"	<rp>.  </rp>\n" +
				"	✶\n" +
				"	<rp>:  </rp>\n" +
				"	<rt>  Star  </rt>\n" +
				"	<rp>,  </rp>\n" +
				"	<rtc>\n" +
				"		<rt lang=\"ru\">  Звезда  </rt>\n" +
				"	</rtc>\n" +
				"	<rp>.  </rp>\n" +
				"</ruby>"
				;
			const string targetOutput27A = input27;
			const string targetOutput27B = "<ruby> " +
				"♥ " +
				"<rp>: </rp> " +
				"<rt> Heart </rt> " +
				"<rp>, </rp> " +
				"<rtc> " +
				"<rt lang=\"ru\"> Сердце </rt> " +
				"</rtc> " +
				"<rp>. </rp> " +
				"☘ " +
				"<rp>: </rp> " +
				"<rt> Shamrock </rt> " +
				"<rp>, </rp> " +
				"<rtc> " +
				"<rt lang=\"ru\"> Трилистник </rt> " +
				"</rtc> " +
				"<rp>. </rp> " +
				"✶ " +
				"<rp>: </rp> " +
				"<rt> Star </rt> " +
				"<rp>, </rp> " +
				"<rtc> " +
				"<rt lang=\"ru\"> Звезда </rt> " +
				"</rtc> " +
				"<rp>. </rp> " +
				"</ruby>"
				;
			const string targetOutput27C = targetOutput27B;
			const string targetOutput27D = "<ruby>" +
				"♥ " +
				"<rp>:</rp> " +
				"<rt>Heart</rt> " +
				"<rp>,</rp> " +
				"<rtc>" +
				"<rt lang=\"ru\">Сердце</rt>" +
				"</rtc> " +
				"<rp>.</rp> " +
				"☘ " +
				"<rp>:</rp> " +
				"<rt>Shamrock</rt> " +
				"<rp>,</rp> " +
				"<rtc>" +
				"<rt lang=\"ru\">Трилистник</rt>" +
				"</rtc> " +
				"<rp>.</rp> " +
				"✶ " +
				"<rp>:</rp> " +
				"<rt>Star</rt> " +
				"<rp>,</rp> " +
				"<rtc>" +
				"<rt lang=\"ru\">Звезда</rt>" +
				"</rtc> " +
				"<rp>.</rp>" +
				"</ruby>"
				;


			const string input28 = "<ruby>\n" +
				"	<rb>  旧  </rb>  <rb>  金  </rb>  <rb>  山  </rb>\n" +
				"	<rt>  jiù  </rt>  <rt>  jīn  </rt> <rt>  shān  </rt>\n" +
				"	<rtc>  Сан-Франциско  </rtc>\n" +
				"</ruby>"
				;
			const string targetOutput28A = input28;
			const string targetOutput28B = "<ruby> " +
				"<rb> 旧 </rb> <rb> 金 </rb> <rb> 山 </rb> " +
				"<rt> jiù </rt> <rt> jīn </rt> <rt> shān </rt> " +
				"<rtc> Сан-Франциско </rtc> " +
				"</ruby>"
				;
			const string targetOutput28C = targetOutput28B;
			const string targetOutput28D = "<ruby>" +
				"<rb>旧</rb> <rb>金</rb> <rb>山</rb> " +
				"<rt>jiù</rt> <rt>jīn</rt> <rt>shān</rt> " +
				"<rtc>Сан-Франциско</rtc>" +
				"</ruby>"
				;

			// Act
			string output1A = keepingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1B = safeRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1C = mediumRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1D = aggressiveRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2B = safeRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = mediumRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2D = aggressiveRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3B = safeRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3C = mediumRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3D = aggressiveRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4B = safeRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4C = mediumRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4D = aggressiveRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5B = safeRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5C = mediumRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5D = aggressiveRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingWhitespaceMinifier.Minify(input6).MinifiedContent;
			string output6B = safeRemovingWhitespaceMinifier.Minify(input6).MinifiedContent;
			string output6C = mediumRemovingWhitespaceMinifier.Minify(input6).MinifiedContent;
			string output6D = aggressiveRemovingWhitespaceMinifier.Minify(input6).MinifiedContent;

			string output7A = keepingWhitespaceMinifier.Minify(input7).MinifiedContent;
			string output7B = safeRemovingWhitespaceMinifier.Minify(input7).MinifiedContent;
			string output7C = mediumRemovingWhitespaceMinifier.Minify(input7).MinifiedContent;
			string output7D = aggressiveRemovingWhitespaceMinifier.Minify(input7).MinifiedContent;

			string output8A = keepingWhitespaceMinifier.Minify(input8).MinifiedContent;
			string output8B = safeRemovingWhitespaceMinifier.Minify(input8).MinifiedContent;
			string output8C = mediumRemovingWhitespaceMinifier.Minify(input8).MinifiedContent;
			string output8D = aggressiveRemovingWhitespaceMinifier.Minify(input8).MinifiedContent;

			string output9A = keepingWhitespaceMinifier.Minify(input9).MinifiedContent;
			string output9B = safeRemovingWhitespaceMinifier.Minify(input9).MinifiedContent;
			string output9C = mediumRemovingWhitespaceMinifier.Minify(input9).MinifiedContent;
			string output9D = aggressiveRemovingWhitespaceMinifier.Minify(input9).MinifiedContent;

			string output10A = keepingWhitespaceMinifier.Minify(input10).MinifiedContent;
			string output10B = safeRemovingWhitespaceMinifier.Minify(input10).MinifiedContent;
			string output10C = mediumRemovingWhitespaceMinifier.Minify(input10).MinifiedContent;
			string output10D = aggressiveRemovingWhitespaceMinifier.Minify(input10).MinifiedContent;

			string output11A = keepingWhitespaceMinifier.Minify(input11).MinifiedContent;
			string output11B = safeRemovingWhitespaceMinifier.Minify(input11).MinifiedContent;
			string output11C = mediumRemovingWhitespaceMinifier.Minify(input11).MinifiedContent;
			string output11D = aggressiveRemovingWhitespaceMinifier.Minify(input11).MinifiedContent;

			string output12A = keepingWhitespaceMinifier.Minify(input12).MinifiedContent;
			string output12B = safeRemovingWhitespaceMinifier.Minify(input12).MinifiedContent;
			string output12C = mediumRemovingWhitespaceMinifier.Minify(input12).MinifiedContent;
			string output12D = aggressiveRemovingWhitespaceMinifier.Minify(input12).MinifiedContent;

			string output13A = keepingWhitespaceMinifier.Minify(input13).MinifiedContent;
			string output13B = safeRemovingWhitespaceMinifier.Minify(input13).MinifiedContent;
			string output13C = mediumRemovingWhitespaceMinifier.Minify(input13).MinifiedContent;
			string output13D = aggressiveRemovingWhitespaceMinifier.Minify(input13).MinifiedContent;

			string output14A = keepingWhitespaceMinifier.Minify(input14).MinifiedContent;
			string output14B = safeRemovingWhitespaceMinifier.Minify(input14).MinifiedContent;
			string output14C = mediumRemovingWhitespaceMinifier.Minify(input14).MinifiedContent;
			string output14D = aggressiveRemovingWhitespaceMinifier.Minify(input14).MinifiedContent;

			string output15A = keepingWhitespaceMinifier.Minify(input15).MinifiedContent;
			string output15B = safeRemovingWhitespaceMinifier.Minify(input15).MinifiedContent;
			string output15C = mediumRemovingWhitespaceMinifier.Minify(input15).MinifiedContent;
			string output15D = aggressiveRemovingWhitespaceMinifier.Minify(input15).MinifiedContent;

			string output16A = keepingWhitespaceMinifier.Minify(input16).MinifiedContent;
			string output16B = safeRemovingWhitespaceMinifier.Minify(input16).MinifiedContent;
			string output16C = mediumRemovingWhitespaceMinifier.Minify(input16).MinifiedContent;
			string output16D = aggressiveRemovingWhitespaceMinifier.Minify(input16).MinifiedContent;

			string output17A = keepingWhitespaceMinifier.Minify(input17).MinifiedContent;
			string output17B = safeRemovingWhitespaceMinifier.Minify(input17).MinifiedContent;
			string output17C = mediumRemovingWhitespaceMinifier.Minify(input17).MinifiedContent;
			string output17D = aggressiveRemovingWhitespaceMinifier.Minify(input17).MinifiedContent;

			string output18A = keepingWhitespaceMinifier.Minify(input18).MinifiedContent;
			string output18B = safeRemovingWhitespaceMinifier.Minify(input18).MinifiedContent;
			string output18C = mediumRemovingWhitespaceMinifier.Minify(input18).MinifiedContent;
			string output18D = aggressiveRemovingWhitespaceMinifier.Minify(input18).MinifiedContent;

			string output19A = keepingWhitespaceMinifier.Minify(input19).MinifiedContent;
			string output19B = safeRemovingWhitespaceMinifier.Minify(input19).MinifiedContent;
			string output19C = mediumRemovingWhitespaceMinifier.Minify(input19).MinifiedContent;
			string output19D = aggressiveRemovingWhitespaceMinifier.Minify(input19).MinifiedContent;

			string output20A = keepingWhitespaceMinifier.Minify(input20).MinifiedContent;
			string output20B = safeRemovingWhitespaceMinifier.Minify(input20).MinifiedContent;
			string output20C = mediumRemovingWhitespaceMinifier.Minify(input20).MinifiedContent;
			string output20D = aggressiveRemovingWhitespaceMinifier.Minify(input20).MinifiedContent;

			string output21A = keepingWhitespaceMinifier.Minify(input21).MinifiedContent;
			string output21B = safeRemovingWhitespaceMinifier.Minify(input21).MinifiedContent;
			string output21C = mediumRemovingWhitespaceMinifier.Minify(input21).MinifiedContent;
			string output21D = aggressiveRemovingWhitespaceMinifier.Minify(input21).MinifiedContent;

			string output22A = keepingWhitespaceMinifier.Minify(input22).MinifiedContent;
			string output22B = safeRemovingWhitespaceMinifier.Minify(input22).MinifiedContent;
			string output22C = mediumRemovingWhitespaceMinifier.Minify(input22).MinifiedContent;
			string output22D = aggressiveRemovingWhitespaceMinifier.Minify(input22).MinifiedContent;

			string output23A = keepingWhitespaceMinifier.Minify(input23).MinifiedContent;
			string output23B = safeRemovingWhitespaceMinifier.Minify(input23).MinifiedContent;
			string output23C = mediumRemovingWhitespaceMinifier.Minify(input23).MinifiedContent;
			string output23D = aggressiveRemovingWhitespaceMinifier.Minify(input23).MinifiedContent;

			string output24A = keepingWhitespaceMinifier.Minify(input24).MinifiedContent;
			string output24B = safeRemovingWhitespaceMinifier.Minify(input24).MinifiedContent;
			string output24C = mediumRemovingWhitespaceMinifier.Minify(input24).MinifiedContent;
			string output24D = aggressiveRemovingWhitespaceMinifier.Minify(input24).MinifiedContent;

			string output25A = keepingWhitespaceMinifier.Minify(input25).MinifiedContent;
			string output25B = safeRemovingWhitespaceMinifier.Minify(input25).MinifiedContent;
			string output25C = mediumRemovingWhitespaceMinifier.Minify(input25).MinifiedContent;
			string output25D = aggressiveRemovingWhitespaceMinifier.Minify(input25).MinifiedContent;

			string output26A = keepingWhitespaceMinifier.Minify(input26).MinifiedContent;
			string output26B = safeRemovingWhitespaceMinifier.Minify(input26).MinifiedContent;
			string output26C = mediumRemovingWhitespaceMinifier.Minify(input26).MinifiedContent;
			string output26D = aggressiveRemovingWhitespaceMinifier.Minify(input26).MinifiedContent;

			string output27A = keepingWhitespaceMinifier.Minify(input27).MinifiedContent;
			string output27B = safeRemovingWhitespaceMinifier.Minify(input27).MinifiedContent;
			string output27C = mediumRemovingWhitespaceMinifier.Minify(input27).MinifiedContent;
			string output27D = aggressiveRemovingWhitespaceMinifier.Minify(input27).MinifiedContent;

			string output28A = keepingWhitespaceMinifier.Minify(input28).MinifiedContent;
			string output28B = safeRemovingWhitespaceMinifier.Minify(input28).MinifiedContent;
			string output28C = mediumRemovingWhitespaceMinifier.Minify(input28).MinifiedContent;
			string output28D = aggressiveRemovingWhitespaceMinifier.Minify(input28).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);
			Assert.Equal(targetOutput1D, output1D);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);
			Assert.Equal(targetOutput2D, output2D);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);
			Assert.Equal(targetOutput3D, output3D);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);
			Assert.Equal(targetOutput4D, output4D);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);
			Assert.Equal(targetOutput5D, output5D);

			Assert.Equal(targetOutput6A, output6A);
			Assert.Equal(targetOutput6B, output6B);
			Assert.Equal(targetOutput6C, output6C);
			Assert.Equal(targetOutput6D, output6D);

			Assert.Equal(targetOutput7A, output7A);
			Assert.Equal(targetOutput7B, output7B);
			Assert.Equal(targetOutput7C, output7C);
			Assert.Equal(targetOutput7D, output7D);

			Assert.Equal(targetOutput8A, output8A);
			Assert.Equal(targetOutput8B, output8B);
			Assert.Equal(targetOutput8C, output8C);
			Assert.Equal(targetOutput8D, output8D);

			Assert.Equal(targetOutput9A, output9A);
			Assert.Equal(targetOutput9B, output9B);
			Assert.Equal(targetOutput9C, output9C);
			Assert.Equal(targetOutput9D, output9D);

			Assert.Equal(targetOutput10A, output10A);
			Assert.Equal(targetOutput10B, output10B);
			Assert.Equal(targetOutput10C, output10C);
			Assert.Equal(targetOutput10D, output10D);

			Assert.Equal(targetOutput11A, output11A);
			Assert.Equal(targetOutput11B, output11B);
			Assert.Equal(targetOutput11C, output11C);
			Assert.Equal(targetOutput11D, output11D);

			Assert.Equal(targetOutput12A, output12A);
			Assert.Equal(targetOutput12B, output12B);
			Assert.Equal(targetOutput12C, output12C);
			Assert.Equal(targetOutput12D, output12D);

			Assert.Equal(targetOutput13A, output13A);
			Assert.Equal(targetOutput13B, output13B);
			Assert.Equal(targetOutput13C, output13C);
			Assert.Equal(targetOutput13D, output13D);

			Assert.Equal(targetOutput14A, output14A);
			Assert.Equal(targetOutput14B, output14B);
			Assert.Equal(targetOutput14C, output14C);
			Assert.Equal(targetOutput14D, output14D);

			Assert.Equal(targetOutput15A, output15A);
			Assert.Equal(targetOutput15B, output15B);
			Assert.Equal(targetOutput15C, output15C);
			Assert.Equal(targetOutput15D, output15D);

			Assert.Equal(targetOutput16A, output16A);
			Assert.Equal(targetOutput16B, output16B);
			Assert.Equal(targetOutput16C, output16C);
			Assert.Equal(targetOutput16D, output16D);

			Assert.Equal(targetOutput17A, output17A);
			Assert.Equal(targetOutput17B, output17B);
			Assert.Equal(targetOutput17C, output17C);
			Assert.Equal(targetOutput17D, output17D);

			Assert.Equal(targetOutput18A, output18A);
			Assert.Equal(targetOutput18B, output18B);
			Assert.Equal(targetOutput18C, output18C);
			Assert.Equal(targetOutput18D, output18D);

			Assert.Equal(targetOutput19A, output19A);
			Assert.Equal(targetOutput19B, output19B);
			Assert.Equal(targetOutput19C, output19C);
			Assert.Equal(targetOutput19D, output19D);

			Assert.Equal(targetOutput20A, output20A);
			Assert.Equal(targetOutput20B, output20B);
			Assert.Equal(targetOutput20C, output20C);
			Assert.Equal(targetOutput20D, output20D);

			Assert.Equal(targetOutput21A, output21A);
			Assert.Equal(targetOutput21B, output21B);
			Assert.Equal(targetOutput21C, output21C);
			Assert.Equal(targetOutput21D, output21D);

			Assert.Equal(targetOutput22A, output22A);
			Assert.Equal(targetOutput22B, output22B);
			Assert.Equal(targetOutput22C, output22C);
			Assert.Equal(targetOutput22D, output22D);

			Assert.Equal(targetOutput23A, output23A);
			Assert.Equal(targetOutput23B, output23B);
			Assert.Equal(targetOutput23C, output23C);
			Assert.Equal(targetOutput23D, output23D);

			Assert.Equal(targetOutput24A, output24A);
			Assert.Equal(targetOutput24B, output24B);
			Assert.Equal(targetOutput24C, output24C);
			Assert.Equal(targetOutput24D, output24D);

			Assert.Equal(targetOutput25A, output25A);
			Assert.Equal(targetOutput25B, output25B);
			Assert.Equal(targetOutput25C, output25C);
			Assert.Equal(targetOutput25D, output25D);

			Assert.Equal(targetOutput26A, output26A);
			Assert.Equal(targetOutput26B, output26B);
			Assert.Equal(targetOutput26C, output26C);
			Assert.Equal(targetOutput26D, output26D);

			Assert.Equal(targetOutput27A, output27A);
			Assert.Equal(targetOutput27B, output27B);
			Assert.Equal(targetOutput27C, output27C);
			Assert.Equal(targetOutput27D, output27D);

			Assert.Equal(targetOutput28A, output28A);
			Assert.Equal(targetOutput28B, output28B);
			Assert.Equal(targetOutput28C, output28C);
			Assert.Equal(targetOutput28D, output28D);
		}
	}
}