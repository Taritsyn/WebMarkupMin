using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace WebMarkupMin.Sample.AspNet4.Mvc4.Infrastructure.ActionResults
{
	/// <summary>
	/// Represents a class that is used to send XML-formatted content to the response
	/// </summary>
	public sealed class XmlResult : ActionResult
	{
		/// <summary>
		/// Gets or sets a type of the content
		/// </summary>
		public string ContentType
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a content encoding
		/// </summary>
		public Encoding ContentEncoding
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a data
		/// </summary>
		public IXmlSerializable Data
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs an instance of the <see cref="XmlResult"/>
		/// </summary>
		public XmlResult()
		{ }


		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			HttpResponseBase response = context.HttpContext.Response;
			response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "text/xml";
			if (ContentEncoding != null)
			{
				response.ContentEncoding = ContentEncoding;
			}

			if (Data != null)
			{
				var settings = new XmlWriterSettings()
				{
					Encoding = ContentEncoding,
					// Add indents in order to test the XML minification
					Indent = true
				};

				using (XmlWriter writer = XmlWriter.Create(response.Output, settings))
				{
					Data.WriteXml(writer);
				}
			}
		}
	}
}