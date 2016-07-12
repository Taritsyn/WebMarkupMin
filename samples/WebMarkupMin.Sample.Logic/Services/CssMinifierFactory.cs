using System;

using WebMarkupMin.Core;
using WebMarkupMin.NUglify;
#if !NETSTANDARD1_6
using WebMarkupMin.MsAjax;
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
					minifier = new MsAjaxCssMinifier();
					break;
				case "YuiCssMinifier":
					minifier = new YuiCssMinifier();
					break;
#endif
				case "NUglifyCssMinifier":
					minifier = new NUglifyCssMinifier();
					break;
				default:
					throw new NotSupportedException();
			}

			return minifier;
		}
	}
}