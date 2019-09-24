#if !NETCOREAPP1_0
using System;
#endif
using System.IO;
#if NETCOREAPP1_0

using Microsoft.Extensions.PlatformAbstractions;
#endif

namespace WebMarkupMin.Core.Test
{
	public abstract class FileSystemTestsBase
	{
		protected string _baseDirectoryPath;


		protected FileSystemTestsBase()
		{
#if NETCOREAPP1_0
			var appEnv = PlatformServices.Default.Application;
			string appDirectoryPath = appEnv.ApplicationBasePath;
#else
			string appDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
#endif
			_baseDirectoryPath = Path.Combine(appDirectoryPath, "../../../");
		}
	}
}