using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
#if !NETCOREAPP3_1_OR_GREATER
using Microsoft.AspNetCore.Mvc.Internal;
#endif
using Microsoft.Net.Http.Headers;
#if NETCOREAPP3_1_OR_GREATER

using WebMarkupMin.Sample.AspNetCore.Infrastructure.Internal;
#endif

namespace WebMarkupMin.Sample.AspNetCore.Infrastructure.ActionResults
{
	/// <summary>
	/// An action result which formats the given object as XML
	/// </summary>
	public sealed class XmlResult : ActionResult
	{
		/// <summary>
		/// The default content type of the action result
		/// </summary>
		private static readonly string DefaultContentType = new MediaTypeHeaderValue("text/xml")
		{
			Encoding = Encoding.UTF8
		}.ToString();

		/// <summary>
		/// Gets or sets the <see cref="Net.Http.Headers.MediaTypeHeaderValue"/> representing the Content-Type header of the response
		/// </summary>
		public string ContentType
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the value to be formatted
		/// </summary>
		public IXmlSerializable Value
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs an instance of the <see cref="XmlResult"/>
		/// </summary>
		public XmlResult()
		{ }

		/// <summary>
		/// Constructs an instance of the <see cref="XmlResult"/> with the given <paramref name="value"/>
		/// </summary>
		/// <param name="value">The value to format as XML</param>
		public XmlResult(IXmlSerializable value)
		{
			Value = value;
		}


		public override void ExecuteResult(ActionContext context)
		{
			if (context is null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			HttpResponse response = context.HttpContext.Response;

			ResponseContentTypeHelper.ResolveContentTypeAndEncoding(
				ContentType,
				response.ContentType,
				DefaultContentType,
				out string resolvedContentType,
				out Encoding resolvedContentTypeEncoding
			);

			response.ContentType = resolvedContentType;

			if (Value is not null)
			{
				var settings = new XmlWriterSettings()
				{
					Encoding = resolvedContentTypeEncoding,
					// Add indents in order to test the XML minification
					Indent = true
				};

				using (XmlWriter writer = XmlWriter.Create(response.Body, settings))
				{
					Value.WriteXml(writer);
				}
			}
		}
	}
}