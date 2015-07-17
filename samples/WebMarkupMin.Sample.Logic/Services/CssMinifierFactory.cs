using System;

using WebMarkupMin.Core;
#if !DNXCORE50
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
#if DNXCORE50
			ICssMinifier minifier = new KristensenCssMinifier();
#else
			ICssMinifier minifier;

			switch (minifierName)
			{
				case "KristensenCssMinifier":
					minifier = new KristensenCssMinifier();
					break;
				case "MsAjaxCssMinifier":
					minifier = new MsAjaxCssMinifier();
					break;
				case "YuiCssMinifier":
					minifier = new YuiCssMinifier();
					break;
				default:
					throw new NotSupportedException();
			}
#endif

			return minifier;
		}
	}
}