using System;
using System.Configuration;

using WebMarkupMin.AspNet4.WebForms;
using WebMarkupMin.Sample.Logic.Services;

namespace WebMarkupMin.Sample.AspNet45.WebForms
{
	public partial class Default : MinifiedAndCompressedHtmlPage
	{
		public string Body
		{
			get;
			set;
		}


		protected void Page_Load(object sender, EventArgs e)
		{
			var fileContentService = new FileContentService(
				ConfigurationManager.AppSettings["webmarkupmin:Samples:TextContentDirectoryPath"]);
			Body = fileContentService.GetFileContent("index.html");
		}
	}
}