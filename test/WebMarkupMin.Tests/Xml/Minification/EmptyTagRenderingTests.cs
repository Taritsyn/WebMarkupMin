using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Xml.Minification
{
	public class EmptyTagRenderingTests
	{
		[Fact]
		public void EmptyTagRenderingIsCorrect()
		{
			// Arrange
			var emptyTagWithSlashMinifier = new XmlMinifier(
				new XmlMinificationSettings(true) { RenderEmptyTagsWithSpace = false });
			var emptyTagWithSpaceAndSlashMinifier = new XmlMinifier(
				new XmlMinificationSettings(true) { RenderEmptyTagsWithSpace = true });

			const string input1 = "<?xml version=\"1.0\" standalone=\"no\"?>" +
				"<!DOCTYPE svg PUBLIC \" -//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">" +
				"<svg xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\" width=\"220\" height=\"220\">" +
				"<circle cx=\"110\" cy=\"110\" r=\"100\" fill=\"#fafaa2\" stroke=\"#000\" />" +
				"<rect x=\"10\" y=\"10\" width=\"200\" height=\"200\" fill=\"#fafaa2\" stroke=\"#000\"/>" +
				"</svg>"
				;
			const string targetOutput1A = "<?xml version=\"1.0\" standalone=\"no\"?>" +
				"<!DOCTYPE svg PUBLIC \" -//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">" +
				"<svg xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\" width=\"220\" height=\"220\">" +
				"<circle cx=\"110\" cy=\"110\" r=\"100\" fill=\"#fafaa2\" stroke=\"#000\"/>" +
				"<rect x=\"10\" y=\"10\" width=\"200\" height=\"200\" fill=\"#fafaa2\" stroke=\"#000\"/>" +
				"</svg>"
				;
			const string targetOutput1B = "<?xml version=\"1.0\" standalone=\"no\"?>" +
				"<!DOCTYPE svg PUBLIC \" -//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">" +
				"<svg xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\" width=\"220\" height=\"220\">" +
				"<circle cx=\"110\" cy=\"110\" r=\"100\" fill=\"#fafaa2\" stroke=\"#000\" />" +
				"<rect x=\"10\" y=\"10\" width=\"200\" height=\"200\" fill=\"#fafaa2\" stroke=\"#000\" />" +
				"</svg>"
				;

			// Act
			string output1A = emptyTagWithSlashMinifier.Minify(input1).MinifiedContent;
			string output1B = emptyTagWithSpaceAndSlashMinifier.Minify(input1).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
		}
	}
}