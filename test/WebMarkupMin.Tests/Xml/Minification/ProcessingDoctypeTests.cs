using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Xml.Minification
{
	public class ProcessingDoctypeTests
	{
		[Fact]
		public void ProcessingDoctypeIsCorrect()
		{
			// Arrange
			var minifier = new XmlMinifier(new XmlMinificationSettings(true));

			const string input1 = "<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.0//EN\"\n" +
				"	\"http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd\">";
			const string targetOutput1 = "<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.0//EN\"" +
				" \"http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd\">";

			const string input2 = "<!DOCTYPE\n" +
				"    svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\"\n" +
				"    \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">"
				;
			const string targetOutput2 = "<!DOCTYPE" +
				" svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\"" +
				" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">"
				;

			const string input3 = "<!DOCTYPE math SYSTEM \r\n" +
				"	\"http://www.w3.org/Math/DTD/mathml1/mathml.dtd\">";
			const string targetOutput3 = "<!DOCTYPE math SYSTEM" +
				" \"http://www.w3.org/Math/DTD/mathml1/mathml.dtd\">";

			const string input4 = "<!DOCTYPE math PUBLIC \"-//W3C//DTD MathML 2.0//EN\"	\n" +
				"    \"http://www.w3.org/Math/DTD/mathml2/mathml2.dtd\">";
			const string targetOutput4 = "<!DOCTYPE math PUBLIC \"-//W3C//DTD MathML 2.0//EN\"" +
				" \"http://www.w3.org/Math/DTD/mathml2/mathml2.dtd\">";

			const string input5 = "<!DOCTYPE recipe>";
			const string targetOutput5 = input5;

			const string input6 = "<!DOCTYPE\r\nrecipe>";
			const string targetOutput6 = "<!DOCTYPE recipe>";

			const string input7 = "<!DOCTYPE\trecipe>";
			const string targetOutput7 = "<!DOCTYPE recipe>";

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;
			string output3 = minifier.Minify(input3).MinifiedContent;
			string output4 = minifier.Minify(input4).MinifiedContent;
			string output5 = minifier.Minify(input5).MinifiedContent;
			string output6 = minifier.Minify(input6).MinifiedContent;
			string output7 = minifier.Minify(input7).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
			Assert.Equal(targetOutput6, output6);
			Assert.Equal(targetOutput7, output7);
		}
	}
}