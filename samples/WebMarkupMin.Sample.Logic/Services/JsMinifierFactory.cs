using System;

using WebMarkupMin.Core;
#if !NETSTANDARD1_6
using WebMarkupMin.MsAjax;
#if !NET40
using WebMarkupMin.Yui;
#endif
using WebMarkupMin.NUglify;
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
			IJsMinifier minifier;

			switch (minifierName)
			{
				case "CrockfordJsMinifier":
					minifier = new CrockfordJsMinifier();
					break;
#if !NETSTANDARD1_6
				case "MsAjaxJsMinifier":
					minifier = new MsAjaxJsMinifier(new MsAjaxJsMinificationSettings { WarningLevel = 2 });
					break;
#if !NET40
				case "YuiJsMinifier":
					minifier = new YuiJsMinifier(new YuiJsMinificationSettings { WarningLevel = 1 });
					break;
#endif
				case "NUglifyJsMinifier":
					minifier = new NUglifyJsMinifier(new NUglifyJsMinificationSettings { WarningLevel = 2 });
					break;
#endif
				default:
					throw new NotSupportedException();
			}

			return minifier;
		}
	}
}