#if NET452 || NETCOREAPP1_0
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
#if NET452 || NETCOREAPP1_0
			var appEnv = PlatformServices.Default.Application;
			_baseDirectoryPath = Path.Combine(appEnv.ApplicationBasePath, "../../../");
#elif NET40
			_baseDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../");
#else
#error No implementation for this target
#endif
		}
	}
}