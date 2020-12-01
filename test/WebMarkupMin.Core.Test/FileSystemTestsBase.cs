using System;
using System.IO;

namespace WebMarkupMin.Core.Test
{
	public abstract class FileSystemTestsBase
	{
		protected string _baseDirectoryPath;


		protected FileSystemTestsBase()
		{
			string appDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
			_baseDirectoryPath = Path.Combine(appDirectoryPath, "../../../");
		}
	}
}