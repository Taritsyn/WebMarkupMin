using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.Msie;

namespace WebMarkupMin.Sample.AspNet45.WebForms
{
	public class JsEngineSwitcherConfig
	{
		public static void Configure(JsEngineSwitcher engineSwitcher)
		{
			engineSwitcher.EngineFactories
				.AddMsie()
				;
		}
	}
}