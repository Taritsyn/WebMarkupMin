using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.Msie;

namespace WebMarkupMin.Sample.AspNet45.WebForms
{
	public class JsEngineSwitcherConfig
	{
		public static void Configure(IJsEngineSwitcher engineSwitcher)
		{
			engineSwitcher.EngineFactories
				.AddMsie()
				;
		}
	}
}