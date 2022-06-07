using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Angular1.Minification
{
	public class MinificationOfBindingExpressionsTests
	{
		/// <summary>
		/// Minification of Angular binding expressions in the Mustache-style tags contained in text
		/// </summary>
		[Fact]
		public void MinificationOfBindingExpressionsInMustacheStyleTagsContainedInTextIsCorrect()
		{
			// Arrange
			var keepingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyAngularBindingExpressions = false });
			var minifyingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyAngularBindingExpressions = true });

			const string input1 = "<ul>\n" +
				"	<li data-ng-repeat=\"customer in customers\">{{  customer.name | uppercase  }} - {{  customer.city  }}</li>\n" +
				"</ul>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<ul>\n" +
				"	<li data-ng-repeat=\"customer in customers\">{{customer.name|uppercase}} - {{customer.city}}</li>\n" +
				"</ul>"
				;

			const string input2 = "<ul>\n" +
				"	<li data-ng-repeat=\"customer in customers\">{{  customer.name + \"  -  \" + customer.city  }}</li>\n" +
				"</ul>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = "<ul>\n" +
				"	<li data-ng-repeat=\"customer in customers\">{{customer.name+\"  -  \"+customer.city}}</li>\n" +
				"</ul>"
				;

			const string input3 = "<strong>Price: </strong> {{ 3 * 10 | currency }}";
			const string targetOutput3A = input3;
			const string targetOutput3B = "<strong>Price: </strong> {{3*10|currency}}";

			const string input4 = "<div ng-controller=\"EventController\">\n" +
				"	<button ng-click=\"clickMe($event)\">Event</button>\n" +
				"	<p><code>$event</code>: <pre> {{ $event | json }}</pre></p>\n" +
				"	<p><code>clickEvent</code>: <pre>{{ clickEvent | json }}</pre></p>" +
				"</div>"
				;
			const string targetOutput4A = input4;
			const string targetOutput4B = "<div ng-controller=\"EventController\">\n" +
				"	<button ng-click=\"clickMe($event)\">Event</button>\n" +
				"	<p><code>$event</code>: <pre> {{$event|json}}</pre></p>\n" +
				"	<p><code>clickEvent</code>: <pre>{{clickEvent|json}}</pre></p>" +
				"</div>";

			const string input5 = "<p id=\"one-time-binding-example\">One time binding: {{ :: name }}</p>";
			const string targetOutput5A = input5;
			const string targetOutput5B = "<p id=\"one-time-binding-example\">One time binding: {{::name}}</p>";

			const string input6 = "<div>${{\t  Price  \t}}</div>";
			const string targetOutput6A = input6;
			const string targetOutput6B = "<div>${{Price}}</div>";

			// Act
			string output1A = keepingExpressionsMinifier.Minify(input1).MinifiedContent;
			string output1B = minifyingExpressionsMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingExpressionsMinifier.Minify(input2).MinifiedContent;
			string output2B = minifyingExpressionsMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingExpressionsMinifier.Minify(input3).MinifiedContent;
			string output3B = minifyingExpressionsMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingExpressionsMinifier.Minify(input4).MinifiedContent;
			string output4B = minifyingExpressionsMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingExpressionsMinifier.Minify(input5).MinifiedContent;
			string output5B = minifyingExpressionsMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingExpressionsMinifier.Minify(input6).MinifiedContent;
			string output6B = minifyingExpressionsMinifier.Minify(input6).MinifiedContent;

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

		/// <summary>
		/// Minification of Angular binding expressions in the Mustache-style tags contained in attribute values
		/// </summary>
		[Fact]
		public void MinificationOfBindingExpressionsInMustacheStyleTagsContainedInAttributeValuesIsCorrect()
		{
			// Arrange
			var keepingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyAngularBindingExpressions = false });
			var minifyingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyAngularBindingExpressions = true });

			const string input1 = "<img src=\"/Content/images/icons/{{\t  iconName + '.png'  \t}}\">";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<img src=\"/Content/images/icons/{{iconName+'.png'}}\">";

			const string input2 = "<select size=\"{{\t  listSize  \t}}\"></select>";
			const string targetOutput2A = input2;
			const string targetOutput2B = "<select size=\"{{listSize}}\"></select>";

			const string input3 = "<span class=\"label done-{{\t  todo.done  \t}}\">{{todo.text}}</span>";
			const string targetOutput3A = input3;
			const string targetOutput3B = "<span class=\"label done-{{todo.done}}\">{{todo.text}}</span>";

			const string input4 = "<div style=\"background-color: {{\t  color  \t}}\"></div>";
			const string targetOutput4A = input4;
			const string targetOutput4B = "<div style=\"background-color: {{color}}\"></div>";

			const string input5 = "<button onclick=\"showMessageBox('Error', '{{\t  message  \t}}')\">Show message</button>";
			const string targetOutput5A = input5;
			const string targetOutput5B = input5;

			const string input6 = "<input type=\"text\" value=\"{{\t  text  \t}}\">";
			const string targetOutput6A = input6;
			const string targetOutput6B = "<input type=\"text\" value=\"{{text}}\">";

			const string input7 = "<pre ng-bind-template=\"{{\t  salutation  \t}} {{\t  name  \t}}!\"></pre>";
			const string targetOutput7A = input7;
			const string targetOutput7B = "<pre ng-bind-template=\"{{salutation}} {{name}}!\"></pre>";

			// Act
			string output1A = keepingExpressionsMinifier.Minify(input1).MinifiedContent;
			string output1B = minifyingExpressionsMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingExpressionsMinifier.Minify(input2).MinifiedContent;
			string output2B = minifyingExpressionsMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingExpressionsMinifier.Minify(input3).MinifiedContent;
			string output3B = minifyingExpressionsMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingExpressionsMinifier.Minify(input4).MinifiedContent;
			string output4B = minifyingExpressionsMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingExpressionsMinifier.Minify(input5).MinifiedContent;
			string output5B = minifyingExpressionsMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingExpressionsMinifier.Minify(input6).MinifiedContent;
			string output6B = minifyingExpressionsMinifier.Minify(input6).MinifiedContent;

			string output7A = keepingExpressionsMinifier.Minify(input7).MinifiedContent;
			string output7B = minifyingExpressionsMinifier.Minify(input7).MinifiedContent;

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
		}

		/// <summary>
		/// Minification of Angular binding expressions in element directives
		/// </summary>
		[Fact]
		public void MinificationOfBindingExpressionsInElementDirectivesIsCorrect()
		{
			// Arrange
			var keepingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyAngularBindingExpressions = false });
			var minifyingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyAngularBindingExpressions = true });

			const string input1 = "<ng-pluralize count=\"\t  personCount  \t\"" +
				" when=\"{ '0': 'Nobody is viewing.',\n" +
				"			'one': '1 person is viewing.',\n" +
				"			'other': '{} people are viewing.'}\">\n" +
				"</ng-pluralize>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<ng-pluralize count=\"personCount\"" +
				" when=\"{'0':'Nobody is viewing.'," +
				"'one':'1 person is viewing.'," +
				"'other':'{} people are viewing.'}\">\n" +
				"</ng-pluralize>"
				;

			const string input2 = "<ng:pluralize count=\"\t  personCount  \t\" offset=\"2\"" +
				" when=\"{'0': 'Nobody is viewing.',\n" +
				"	'1': '{{person1}} is viewing.',\n" +
				"	'2': '{{person1}} and {{person2}} are viewing.',\n" +
				"	'one': '{{person1}}, {{person2}} and one other person are viewing.',\n" +
				"	'other': '{{person1}}, {{person2}} and {} other people are viewing.'}\">\n" +
				"</ng:pluralize>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = "<ng:pluralize count=\"personCount\" offset=\"2\"" +
				" when=\"{'0':'Nobody is viewing.'," +
				"'1':'{{person1}} is viewing.'," +
				"'2':'{{person1}} and {{person2}} are viewing.'," +
				"'one':'{{person1}}, {{person2}} and one other person are viewing.'," +
				"'other':'{{person1}}, {{person2}} and {} other people are viewing.'}\">\n" +
				"</ng:pluralize>"
				;

			const string input3 = "<form name=\"myForm\">\n" +
				"	<input type=\"text\" ng-model=\"field\" name=\"myField\" required=\"required\" minlength=\"5\">\n" +
				"	<ng-messages for=\"\t  myForm.myField.$error  \t\">\n" +
				"		<ng-message when=\"required\">You did not enter a field</ng-message>\n" +
				"		<ng-message when=\"minlength\">The value entered is too short</ng-message>\n" +
				"	</ng-messages>\n" +
				"</form>"
				;
			const string targetOutput3A = input3;
			const string targetOutput3B = "<form name=\"myForm\">\n" +
				"	<input type=\"text\" ng-model=\"field\" name=\"myField\" required=\"required\" minlength=\"5\">\n" +
				"	<ng-messages for=\"myForm.myField.$error\">\n" +
				"		<ng-message when=\"required\">You did not enter a field</ng-message>\n" +
				"		<ng-message when=\"minlength\">The value entered is too short</ng-message>\n" +
				"	</ng-messages>\n" +
				"</form>"
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

		/// <summary>
		/// Minification of Angular binding expressions in built-in attribute directives
		/// </summary>
		[Fact]
		public void MinificationOfBindingExpressionsInBuiltinAttributeDirectivesIsCorrect()
		{
			// Arrange
			var keepingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyAngularBindingExpressions = false });
			var minifyingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyAngularBindingExpressions = true });

			const string input1 = "<span ng-bind-html=\"\t  name  \t\"></span>";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<span ng-bind-html=\"name\"></span>";

			const string input2 = "<span ng:bind-html=\"\t  name  \t\"></span>";
			const string targetOutput2A = input2;
			const string targetOutput2B = "<span ng:bind-html=\"name\"></span>";

			const string input3 = "<span ng_bind_html=\"\t  name  \t\"></span>";
			const string targetOutput3A = input3;
			const string targetOutput3B = "<span ng_bind_html=\"name\"></span>";

			const string input4 = "<span x-ng-bind-html=\"\t  name  \t\"></span>";
			const string targetOutput4A = input4;
			const string targetOutput4B = "<span x-ng-bind-html=\"name\"></span>";

			const string input5 = "<span data-ng-bind-html=\"\t  name  \t\"></span>";
			const string targetOutput5A = input5;
			const string targetOutput5B = "<span data-ng-bind-html=\"name\"></span>";

			const string input6 = "<p ng-class=\"{ strike: deleted, bold: important, red: error }\">" +
				"Map Syntax Example</p>";
			const string targetOutput6A = input6;
			const string targetOutput6B = "<p ng-class=\"{strike:deleted,bold:important,red:error}\">" +
				"Map Syntax Example</p>";

			const string input7 = "<p ng-class=\"\t  style  \t\">Using String Syntax</p>";
			const string targetOutput7A = input7;
			const string targetOutput7B = "<p ng-class=\"style\">Using String Syntax</p>";

			const string input8 = "<p ng-class=\"\t  [ style1, style2, style3 ]  \t\">Using Array Syntax</p>";
			const string targetOutput8A = input8;
			const string targetOutput8B = "<p ng-class=\"[style1,style2,style3]\">Using Array Syntax</p>";

			const string input9 = "<div ng-controller=\"\t  SettingsController1  as  settings  \t\"></div>";
			const string targetOutput9A = input9;
			const string targetOutput9B = "<div ng-controller=\"SettingsController1 as settings\"></div>";

			const string input10 = "<button ng-click=\"showMessageBox('Error', message);\">Show message</button>";
			const string targetOutput10A = input10;
			const string targetOutput10B = "<button ng-click=\"showMessageBox('Error',message)\">Show message</button>";

			const string input11 = "<div ng-init=\"  names = ['John', 'Mary', 'Cate', 'Suz']  \"></div>";
			const string targetOutput11A = input11;
			const string targetOutput11B = "<div ng-init=\"names=['John','Mary','Cate','Suz']\"></div>";

			const string input12 = "<input ng-model=\"\t  user.name  \t\"" +
				" ng-model-options=\"{ updateOn: 'default blur', debounce: {'default': 500, 'blur': 0} }\">";
			const string targetOutput12A = input12;
			const string targetOutput12B = "<input ng-model=\"user.name\"" +
				" ng-model-options=\"{updateOn:'default blur',debounce:{'default':500,'blur':0}}\">";

			const string input13 = "<li ng-repeat=\"\t  item  in  items  \t\">{{item}}</li>";
			const string targetOutput13A = input13;
			const string targetOutput13B = "<li ng-repeat=\"item in items\">{{item}}</li>";

			const string input14 = "<li ng-repeat=\"(name, age) in {'adam': 10, 'amalie': 12}\">{{name}}, {{age}}</li>";
			const string targetOutput14A = input14;
			const string targetOutput14B = "<li ng-repeat=\"(name,age)in{'adam':10,'amalie':12}\">{{name}}, {{age}}</li>";

			const string input15 = "<li ng-repeat=\"\t  item  in  items  track  by  $id( item )  \t\">{{item}}</li>";
			const string targetOutput15A = input15;
			const string targetOutput15B = "<li ng-repeat=\"item in items track by $id(item)\">{{item}}</li>";

			const string input16 = "<li data-ng-repeat=\"customer in customers | filter:nameText | orderBy:'name'\">\n" +
				"	{{customer.name}} - {{customer.city}}\n" +
				"</li>"
				;
			const string targetOutput16A = input16;
			const string targetOutput16B = "<li data-ng-repeat=\"customer in customers|filter:nameText|orderBy:'name'\">\n" +
				"	{{customer.name}} - {{customer.city}}\n" +
				"</li>"
				;

			const string input17 = "<li ng-repeat=\"\t  item  in  ::items  \t\">{{item.name}}</li>";
			const string targetOutput17A = input17;
			const string targetOutput17B = "<li ng-repeat=\"item in ::items\">{{item.name}}</li>";

			const string input18 = "<div ng-repeat=\"\t  item  in  itemArray[myIndex]  track  by  item.id  \t\"></div>";
			const string targetOutput18A = input18;
			const string targetOutput18B = "<div ng-repeat=\"item in itemArray[myIndex] track by item.id\"></div>";

			const string input19 = "<header ng-repeat-start=\"\t  item  in  items  \t\">\n" +
				"	Header {{item}}\n" +
				"</header>\n" +
				"<div class=\"body\">\n" +
				"	Body {{item}}\n" +
				"</div>\n" +
				"<footer ng-repeat-end>\n" +
				"	Footer {{item}}\n" +
				"</footer>"
				;
			const string targetOutput19A = input19;
			const string targetOutput19B = "<header ng-repeat-start=\"item in items\">\n" +
				"	Header {{item}}\n" +
				"</header>\n" +
				"<div class=\"body\">\n" +
				"	Body {{item}}\n" +
				"</div>\n" +
				"<footer ng-repeat-end>\n" +
				"	Footer {{item}}\n" +
				"</footer>"
				;

			const string input20 = "<span ng-style=\"{ 'background-color': 'blue' }\">Sample Text</span>";
			const string targetOutput20A = input20;
			const string targetOutput20B = "<span ng-style=\"{'background-color':'blue'}\">Sample Text</span>";

			const string input21 = "<div ng-switch=\"\t  selection  \t\">\n" +
				"	<div ng-switch-when=\"settings\">Settings Div</div>\n" +
				"	<div ng-switch-when=\"home\">Home Span</div>\n" +
				"	<div ng-switch-default>default</div>\n" +
				"</div>"
				;
			const string targetOutput21A = input21;
			const string targetOutput21B = "<div ng-switch=\"selection\">\n" +
				"	<div ng-switch-when=\"settings\">Settings Div</div>\n" +
				"	<div ng-switch-when=\"home\">Home Span</div>\n" +
				"	<div ng-switch-default>default</div>\n" +
				"</div>"
				;

			const string input22 = "<label ng-repeat=\"name in names\" for=\"{{name}}\">\n" +
				"	{{name}}\n" +
				"	<input type=\"radio\" id=\"{{name}}\" name=\"favorite\" " +
				"ng-model=\"my.favorite\" ng-value=\"\t  name  \t\">\n" +
				"</label>"
				;
			const string targetOutput22A = input22;
			const string targetOutput22B = "<label ng-repeat=\"name in names\" for=\"{{name}}\">\n" +
				"	{{name}}\n" +
				"	<input type=\"radio\" id=\"{{name}}\" name=\"favorite\" " +
				"ng-model=\"my.favorite\" ng-value=\"name\">\n" +
				"</label>"
				;

			const string input23 = "<input type=\"checkbox\" ng-model=\"confirm\" " +
				"ng-true-value=\"\t  1 + ':Yes'  \t\" ng-false-value=\"\t  0 + ':No'  \t\">";
			const string targetOutput23A = input23;
			const string targetOutput23B = "<input type=\"checkbox\" ng-model=\"confirm\" " +
				"ng-true-value=\"1+':Yes'\" ng-false-value=\"0+':No'\">";

			const string input24 = "<input type=\"text\" ng-model=\"login\" name=\"login\" " +
				"ng-minlength=\"\t  LOGIN_MIN_LENGTH  \t\" ng-maxlength=\"\t  LOGIN_MAX_LENGTH  \t\">";
			const string targetOutput24A = input24;
			const string targetOutput24B = "<input type=\"text\" ng-model=\"login\" name=\"login\" " +
				"ng-minlength=\"LOGIN_MIN_LENGTH\" ng-maxlength=\"LOGIN_MAX_LENGTH\">";

			const string input25 = "<select ng-model=\"myColor\" " +
				"ng-options=\"\t  color.name  for  color  in  colors  \t\"></select>";
			const string targetOutput25A = input25;
			const string targetOutput25B = "<select ng-model=\"myColor\" " +
				"ng-options=\"color.name for color in colors\"></select>";

			const string input26 = "<select ng-model=\"myColor\" " +
				"ng-options=\"\t  color.name  group  by  color.shade  for  color  in  colors  \t\"></select>";
			const string targetOutput26A = input26;
			const string targetOutput26B = "<select ng-model=\"myColor\" " +
				"ng-options=\"color.name group by color.shade for color in colors\"></select>";

			const string input27 = "<input type=\"text\" ng-model=\"login\" name=\"login\" " +
				"ng-pattern=\"\t  /^[a-z0-9_-]+$/  \t\">";
			const string targetOutput27A = input27;
			const string targetOutput27B = "<input type=\"text\" ng-model=\"login\" name=\"login\" " +
				"ng-pattern=\"/^[a-z0-9_-]+$/\">";

			const string input28 = "<input type=\"text\" ng-model=\"login\" name=\"login\"" +
				" ng-pattern=\"\t  LOGIN_PATTERN  \t\">";
			const string targetOutput28A = input28;
			const string targetOutput28B = "<input type=\"text\" ng-model=\"login\" name=\"login\"" +
				" ng-pattern=\"LOGIN_PATTERN\">";

			const string input29 = "<label for=\"password\">Password:</label>\n" +
				"<input type=\"text\" id=\"password\" ng-model=\"password\">\n" +
				"<br>\n" +
				"<label for=\"confirmPassword\">Confirm password:</label>\n" +
				"<input type=\"text\" id=\"confirmPassword\" ng-model=\"confirmPassword\"" +
				" ng-required=\"password.trim().length > 0\">"
				;
			const string targetOutput29A = input29;
			const string targetOutput29B = "<label for=\"password\">Password:</label>\n" +
				"<input type=\"text\" id=\"password\" ng-model=\"password\">\n" +
				"<br>\n" +
				"<label for=\"confirmPassword\">Confirm password:</label>\n" +
				"<input type=\"text\" id=\"confirmPassword\" ng-model=\"confirmPassword\"" +
				" ng-required=\"password.trim().length>0\">"
				;

			const string input30 = "<form name=\"myForm\">\n" +
				"	<input type=\"text\" ng-model=\"field\" name=\"myField\" required=\"required\" minlength=\"5\">\n" +
				"	<div ng-messages=\"\t  myForm.myField.$error  \t\">\n" +
				"		<div ng-message=\"required\">You did not enter a field</div>\n" +
				"		<div ng-message=\"minlength\">The value entered is too short</div>\n" +
				"	</div>\n" +
				"</form>"
				;
			const string targetOutput30A = input30;
			const string targetOutput30B = "<form name=\"myForm\">\n" +
				"	<input type=\"text\" ng-model=\"field\" name=\"myField\" required=\"required\" minlength=\"5\">\n" +
				"	<div ng-messages=\"myForm.myField.$error\">\n" +
				"		<div ng-message=\"required\">You did not enter a field</div>\n" +
				"		<div ng-message=\"minlength\">The value entered is too short</div>\n" +
				"	</div>\n" +
				"</form>"
				;

			const string input31 = "<div ng-show=\"!showActions\" ng-swipe-left=\"showActions = true\">\n" +
				"	Some list content, like an email in the inbox\n" +
				"</div>"
				;
			const string targetOutput31A = input31;
			const string targetOutput31B = "<div ng-show=\"!showActions\" ng-swipe-left=\"showActions=true\">\n" +
				"	Some list content, like an email in the inbox\n" +
				"</div>"
				;

			const string input32 = "<div ng-show=\"showActions\" ng-swipe-right=\"showActions = false\">\n" +
				"	<button ng-click=\"reply()\">Reply</button>\n" +
				"	<button ng-click=\"delete()\">Delete</button>\n" +
				"</div>"
				;
			const string targetOutput32A = input32;
			const string targetOutput32B = "<div ng-show=\"showActions\" ng-swipe-right=\"showActions=false\">\n" +
				"	<button ng-click=\"reply()\">Reply</button>\n" +
				"	<button ng-click=\"delete()\">Delete</button>\n" +
				"</div>"
				;

			// Act
			string output1A = keepingExpressionsMinifier.Minify(input1).MinifiedContent;
			string output1B = minifyingExpressionsMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingExpressionsMinifier.Minify(input2).MinifiedContent;
			string output2B = minifyingExpressionsMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingExpressionsMinifier.Minify(input3).MinifiedContent;
			string output3B = minifyingExpressionsMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingExpressionsMinifier.Minify(input4).MinifiedContent;
			string output4B = minifyingExpressionsMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingExpressionsMinifier.Minify(input5).MinifiedContent;
			string output5B = minifyingExpressionsMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingExpressionsMinifier.Minify(input6).MinifiedContent;
			string output6B = minifyingExpressionsMinifier.Minify(input6).MinifiedContent;

			string output7A = keepingExpressionsMinifier.Minify(input7).MinifiedContent;
			string output7B = minifyingExpressionsMinifier.Minify(input7).MinifiedContent;

			string output8A = keepingExpressionsMinifier.Minify(input8).MinifiedContent;
			string output8B = minifyingExpressionsMinifier.Minify(input8).MinifiedContent;

			string output9A = keepingExpressionsMinifier.Minify(input9).MinifiedContent;
			string output9B = minifyingExpressionsMinifier.Minify(input9).MinifiedContent;

			string output10A = keepingExpressionsMinifier.Minify(input10).MinifiedContent;
			string output10B = minifyingExpressionsMinifier.Minify(input10).MinifiedContent;

			string output11A = keepingExpressionsMinifier.Minify(input11).MinifiedContent;
			string output11B = minifyingExpressionsMinifier.Minify(input11).MinifiedContent;

			string output12A = keepingExpressionsMinifier.Minify(input12).MinifiedContent;
			string output12B = minifyingExpressionsMinifier.Minify(input12).MinifiedContent;

			string output13A = keepingExpressionsMinifier.Minify(input13).MinifiedContent;
			string output13B = minifyingExpressionsMinifier.Minify(input13).MinifiedContent;

			string output14A = keepingExpressionsMinifier.Minify(input14).MinifiedContent;
			string output14B = minifyingExpressionsMinifier.Minify(input14).MinifiedContent;

			string output15A = keepingExpressionsMinifier.Minify(input15).MinifiedContent;
			string output15B = minifyingExpressionsMinifier.Minify(input15).MinifiedContent;

			string output16A = keepingExpressionsMinifier.Minify(input16).MinifiedContent;
			string output16B = minifyingExpressionsMinifier.Minify(input16).MinifiedContent;

			string output17A = keepingExpressionsMinifier.Minify(input17).MinifiedContent;
			string output17B = minifyingExpressionsMinifier.Minify(input17).MinifiedContent;

			string output18A = keepingExpressionsMinifier.Minify(input18).MinifiedContent;
			string output18B = minifyingExpressionsMinifier.Minify(input18).MinifiedContent;

			string output19A = keepingExpressionsMinifier.Minify(input19).MinifiedContent;
			string output19B = minifyingExpressionsMinifier.Minify(input19).MinifiedContent;

			string output20A = keepingExpressionsMinifier.Minify(input20).MinifiedContent;
			string output20B = minifyingExpressionsMinifier.Minify(input20).MinifiedContent;

			string output21A = keepingExpressionsMinifier.Minify(input21).MinifiedContent;
			string output21B = minifyingExpressionsMinifier.Minify(input21).MinifiedContent;

			string output22A = keepingExpressionsMinifier.Minify(input22).MinifiedContent;
			string output22B = minifyingExpressionsMinifier.Minify(input22).MinifiedContent;

			string output23A = keepingExpressionsMinifier.Minify(input23).MinifiedContent;
			string output23B = minifyingExpressionsMinifier.Minify(input23).MinifiedContent;

			string output24A = keepingExpressionsMinifier.Minify(input24).MinifiedContent;
			string output24B = minifyingExpressionsMinifier.Minify(input24).MinifiedContent;

			string output25A = keepingExpressionsMinifier.Minify(input25).MinifiedContent;
			string output25B = minifyingExpressionsMinifier.Minify(input25).MinifiedContent;

			string output26A = keepingExpressionsMinifier.Minify(input26).MinifiedContent;
			string output26B = minifyingExpressionsMinifier.Minify(input26).MinifiedContent;

			string output27A = keepingExpressionsMinifier.Minify(input27).MinifiedContent;
			string output27B = minifyingExpressionsMinifier.Minify(input27).MinifiedContent;

			string output28A = keepingExpressionsMinifier.Minify(input28).MinifiedContent;
			string output28B = minifyingExpressionsMinifier.Minify(input28).MinifiedContent;

			string output29A = keepingExpressionsMinifier.Minify(input29).MinifiedContent;
			string output29B = minifyingExpressionsMinifier.Minify(input29).MinifiedContent;

			string output30A = keepingExpressionsMinifier.Minify(input30).MinifiedContent;
			string output30B = minifyingExpressionsMinifier.Minify(input30).MinifiedContent;

			string output31A = keepingExpressionsMinifier.Minify(input31).MinifiedContent;
			string output31B = minifyingExpressionsMinifier.Minify(input31).MinifiedContent;

			string output32A = keepingExpressionsMinifier.Minify(input32).MinifiedContent;
			string output32B = minifyingExpressionsMinifier.Minify(input32).MinifiedContent;

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

			Assert.Equal(targetOutput9A, output9A);
			Assert.Equal(targetOutput9B, output9B);

			Assert.Equal(targetOutput10A, output10A);
			Assert.Equal(targetOutput10B, output10B);

			Assert.Equal(targetOutput11A, output11A);
			Assert.Equal(targetOutput11B, output11B);

			Assert.Equal(targetOutput12A, output12A);
			Assert.Equal(targetOutput12B, output12B);

			Assert.Equal(targetOutput13A, output13A);
			Assert.Equal(targetOutput13B, output13B);

			Assert.Equal(targetOutput14A, output14A);
			Assert.Equal(targetOutput14B, output14B);

			Assert.Equal(targetOutput15A, output15A);
			Assert.Equal(targetOutput15B, output15B);

			Assert.Equal(targetOutput16A, output16A);
			Assert.Equal(targetOutput16B, output16B);

			Assert.Equal(targetOutput17A, output17A);
			Assert.Equal(targetOutput17B, output17B);

			Assert.Equal(targetOutput18A, output18A);
			Assert.Equal(targetOutput18B, output18B);

			Assert.Equal(targetOutput19A, output19A);
			Assert.Equal(targetOutput19B, output19B);

			Assert.Equal(targetOutput20A, output20A);
			Assert.Equal(targetOutput20B, output20B);

			Assert.Equal(targetOutput21A, output21A);
			Assert.Equal(targetOutput21B, output21B);

			Assert.Equal(targetOutput22A, output22A);
			Assert.Equal(targetOutput22B, output22B);

			Assert.Equal(targetOutput23A, output23A);
			Assert.Equal(targetOutput23B, output23B);

			Assert.Equal(targetOutput24A, output24A);
			Assert.Equal(targetOutput24B, output24B);

			Assert.Equal(targetOutput25A, output25A);
			Assert.Equal(targetOutput25B, output25B);

			Assert.Equal(targetOutput26A, output26A);
			Assert.Equal(targetOutput26B, output26B);

			Assert.Equal(targetOutput27A, output27A);
			Assert.Equal(targetOutput27B, output27B);

			Assert.Equal(targetOutput28A, output28A);
			Assert.Equal(targetOutput28B, output28B);

			Assert.Equal(targetOutput29A, output29A);
			Assert.Equal(targetOutput29B, output29B);

			Assert.Equal(targetOutput30A, output30A);
			Assert.Equal(targetOutput30B, output30B);

			Assert.Equal(targetOutput31A, output31A);
			Assert.Equal(targetOutput31B, output31B);

			Assert.Equal(targetOutput32A, output32A);
			Assert.Equal(targetOutput32B, output32B);
		}

		/// <summary>
		/// Minification of Angular binding expressions in custom attribute directives
		/// </summary>
		[Fact]
		public void MinificationOfBindingExpressionsInCustomAttributeDirectivesIsCorrect()
		{
			// Arrange
			const string customAngularDirectiveList = "myDirective,myShowModalData";

			var keepingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					MinifyAngularBindingExpressions = false,
					CustomAngularDirectiveList = customAngularDirectiveList
				});
			var minifyingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					MinifyAngularBindingExpressions = true,
					CustomAngularDirectiveList = customAngularDirectiveList
				});

			const string input1 = "<input my-directive=\"1 + 1\">";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<input my-directive=\"1+1\">";

			const string input2 = "<a href=\"#\" my-show-modal-data=\"{ active: true }\">Link</a>";
			const string targetOutput2A = input2;
			const string targetOutput2B = "<a href=\"#\" my-show-modal-data=\"{active:true}\">Link</a>";

			// Act
			string output1A = keepingExpressionsMinifier.Minify(input1).MinifiedContent;
			string output1B = minifyingExpressionsMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingExpressionsMinifier.Minify(input2).MinifiedContent;
			string output2B = minifyingExpressionsMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
		}

		/// <summary>
		/// Minification of Angular binding expressions in class directives
		/// </summary>
		[Fact]
		public void MinificationOfBindingExpressionsInClassDirectivesIsCorrect()
		{
			// Arrange
			var keepingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyAngularBindingExpressions = false });
			var minifyingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyAngularBindingExpressions = true });

			const string input1 = "<span class=\"ng-bind:'Mr. ' + name\"></span>";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<span class=\"ng-bind:'Mr. '+name\"></span>";

			const string input2 = "<span class=\"ng_bind:'Mr. ' + name\"></span>";
			const string targetOutput2A = input2;
			const string targetOutput2B = "<span class=\"ng_bind:'Mr. '+name\"></span>";

			const string input3 = "<span class=\"x-ng-bind:'Mr. ' + name\"></span>";
			const string targetOutput3A = input3;
			const string targetOutput3B = "<span class=\"x-ng-bind:'Mr. '+name\"></span>";

			const string input4 = "<span class=\"data-ng-bind:'Mr. ' + name\"></span>";
			const string targetOutput4A = input4;
			const string targetOutput4B = "<span class=\"data-ng-bind:'Mr. '+name\"></span>";

			const string input5 = "<p class=\"ng-class:{ strike: deleted, bold: important, red: error }\">" +
				"Map Syntax Example</p>";
			const string targetOutput5A = input5;
			const string targetOutput5B = "<p class=\"ng-class:{strike:deleted,bold:important,red:error}\">" +
				"Map Syntax Example</p>";

			const string input6 = "<p class=\"ng-class:style\">Using String Syntax</p>";
			const string targetOutput6A = input6;
			const string targetOutput6B = "<p class=\"ng-class:style\">Using String Syntax</p>";

			const string input7 = "<p class=\"ng-class:[ style1, style2, style3 ]\">Using Array Syntax</p>";
			const string targetOutput7A = input7;
			const string targetOutput7B = "<p class=\"ng-class:[style1,style2,style3]\">Using Array Syntax</p>";

			const string input8 = "<div class=\"ng-init:count = 1\"></div>";
			const string targetOutput8A = input8;
			const string targetOutput8B = "<div class=\"ng-init:count=1\"></div>";

			const string input9 = "<span class=\"ng-style:{ 'background-color': 'blue' }\">Sample Text</span>";
			const string targetOutput9A = input9;
			const string targetOutput9B = "<span class=\"ng-style:{'background-color':'blue'}\">Sample Text</span>";

			// Act
			string output1A = keepingExpressionsMinifier.Minify(input1).MinifiedContent;
			string output1B = minifyingExpressionsMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingExpressionsMinifier.Minify(input2).MinifiedContent;
			string output2B = minifyingExpressionsMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingExpressionsMinifier.Minify(input3).MinifiedContent;
			string output3B = minifyingExpressionsMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingExpressionsMinifier.Minify(input4).MinifiedContent;
			string output4B = minifyingExpressionsMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingExpressionsMinifier.Minify(input5).MinifiedContent;
			string output5B = minifyingExpressionsMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingExpressionsMinifier.Minify(input6).MinifiedContent;
			string output6B = minifyingExpressionsMinifier.Minify(input6).MinifiedContent;

			string output7A = keepingExpressionsMinifier.Minify(input7).MinifiedContent;
			string output7B = minifyingExpressionsMinifier.Minify(input7).MinifiedContent;

			string output8A = keepingExpressionsMinifier.Minify(input8).MinifiedContent;
			string output8B = minifyingExpressionsMinifier.Minify(input8).MinifiedContent;

			string output9A = keepingExpressionsMinifier.Minify(input9).MinifiedContent;
			string output9B = minifyingExpressionsMinifier.Minify(input9).MinifiedContent;

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

			Assert.Equal(targetOutput9A, output9A);
			Assert.Equal(targetOutput9B, output9B);
		}

		/// <summary>
		/// Minification of Angular binding expressions in comment directives
		/// </summary>
		[Fact]
		public void MinificationOfBindingExpressionsInCommentDirectivesIsCorrect()
		{
			// Arrange
			const string customAngularDirectiveList = "myDirective";

			var keepingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					MinifyAngularBindingExpressions = false,
					CustomAngularDirectiveList = customAngularDirectiveList
				});
			var minifyingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					MinifyAngularBindingExpressions = true,
					CustomAngularDirectiveList = customAngularDirectiveList
				});

			const string input = "<!-- directive: my-directive 1 + 1 -->";
			const string targetOutputA = "<!--directive:my-directive 1 + 1-->";
			const string targetOutputB = "<!--directive:my-directive 1+1-->";

			// Act
			string outputA = keepingExpressionsMinifier.Minify(input).MinifiedContent;
			string outputB = minifyingExpressionsMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
		}
	}
}