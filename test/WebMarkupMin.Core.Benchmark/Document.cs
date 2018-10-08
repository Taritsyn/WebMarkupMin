namespace WebMarkupMin.Core.Benchmark
{
	internal sealed class Document
	{
		public string Url
		{
			get;
			set;
		}

		public string Path
		{
			get;
			set;
		}

		public string Content
		{
			get;
			set;
		}


		public Document()
			: this(string.Empty)
		{ }

		public Document(string url)
		{
			Url = url;
		}
	}
}