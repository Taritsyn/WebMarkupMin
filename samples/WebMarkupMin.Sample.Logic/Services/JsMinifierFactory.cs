using System;

using WebMarkupMin.Core;
#if !NETSTANDARD1_5
using WebMarkupMin.MsAjax;
using WebMarkupMin.Yui;
#endif

namespace WebMarkupMin.Sample.Logic.Services
{
	public sealed class JsMinifierFactory
	{
#if NET40
		private static readonly Lazy<JsMinifierFactory> _instance =
			new Lazy<JsMinifierFactory>(() => new JsMinifierFactory());

		public static JsMinifierFactory Instance
		{
			get { return _instance.Value; }
		}


		private JsMinifierFactory()
		{ }


#endif
		public IJsMinifier CreateMinifier(string minifierName)
		{
#if NETSTANDARD1_5
			IJsMinifier minifier = new CrockfordJsMinifier();
#else
			IJsMinifier minifier;

			switch (minifierName)
			{
				case "CrockfordJsMinifier":
					minifier = new CrockfordJsMinifier();
					break;
				case "MsAjaxJsMinifier":
					minifier = new MsAjaxJsMinifier();
					break;
				case "YuiJsMinifier":
					minifier = new YuiJsMinifier();
					break;
				default:
					throw new NotSupportedException();
			}
#endif

			return minifier;
		}
	}
}