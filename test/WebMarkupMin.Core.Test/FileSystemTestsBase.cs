#if NETCOREAPP1_0 || NET452
using Microsoft.Extensions.PlatformAbstractions;
#elif NET40
using System;
#else
#error No implementation for this target
#endif
using System.IO;

namespace WebMarkupMin.Core.Test
{
	public abstract class FileSystemTestsBase
	{
		protected string _baseDirectoryPath;


		protected FileSystemTestsBase()
		{
#if NETCOREAPP1_0 || NET452
			var appEnv = PlatformServices.Default.Application;
			_baseDirectoryPath = Path.Combine(appEnv.ApplicationBasePath,
	#if NETCOREAPP1_0
				"../../../"
	#else
				"../../../../"
	#endif
			);
#elif NET40
			_baseDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../");
#else
#error No implementation for this target
#endif
		}
	}
}