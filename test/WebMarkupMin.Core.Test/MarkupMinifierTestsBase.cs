#if DNXCORE50 || DNX451
using Microsoft.Extensions.PlatformAbstractions;
#elif NET40
using System;
using System.IO;
#else
#error No implementation for this target
#endif

namespace WebMarkupMin.Core.Test
{
	public abstract class MarkupMinifierTestsBase
	{
		protected string _baseDirectoryPath;


		protected MarkupMinifierTestsBase()
		{
#if DNXCORE50 || DNX451
			var appEnv = (IApplicationEnvironment)CallContextServiceLocator.Locator.ServiceProvider
				.GetService(typeof(IApplicationEnvironment));
			_baseDirectoryPath = appEnv.ApplicationBasePath;
#elif NET40
			_baseDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../");
#else
#error No implementation for this target
#endif
		}
	}
}