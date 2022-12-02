using System;

using WebMarkupMin.Core;
#if !NETSTANDARD1_6
using WebMarkupMin.MsAjax;
#endif
using WebMarkupMin.NUglify;
#if !NET40 && !NETSTANDARD1_6
using WebMarkupMin.Yui;
#endif

namespace WebMarkupMin.Sample.Logic.Services
{
	public sealed class CssMinifierFactory
	{
#if NET40
		private static readonly Lazy<CssMinifierFactory> _instance =
			new Lazy<CssMinifierFactory>(() => new CssMinifierFactory());

		public static CssMinifierFactory Instance
		{
			get { return _instance.Value; }
		}


		private CssMinifierFactory()
		{ }


#endif
		public ICssMinifier CreateMinifier(string minifierName)
		{
			ICssMinifier minifier;

			switch (minifierName)
			{
				case "KristensenCssMinifier":
					minifier = new KristensenCssMinifier();
					break;
#if !NETSTANDARD1_6
				case "MsAjaxCssMinifier":
					minifier = new MsAjaxCssMinifier(new MsAjaxCssMinificationSettings { WarningLevel = 2 });
					break;
#endif
#if !NET40 && !NETSTANDARD1_6
				case "YuiCssMinifier":
					minifier = new YuiCssMinifier();
					break;
#endif
				case "NUglifyCssMinifier":
					minifier = new NUglifyCssMinifier(new NUglifyCssMinificationSettings { WarningLevel = 2 });
					break;
				default:
					throw new NotSupportedException();
			}

			return minifier;
		}
	}
}