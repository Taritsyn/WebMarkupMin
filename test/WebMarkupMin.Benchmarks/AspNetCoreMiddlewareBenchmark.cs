using System;
using System.IO;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Moq;

using WebMarkupMin.AspNet.Common;
#if NET461
using WebMarkupMin.AspNetCore1;
#endif
#if NETCOREAPP2_0
using WebMarkupMin.AspNetCore2;
#endif
using WebMarkupMin.Core;
using WebMarkupMin.Core.Loggers;

namespace WebMarkupMin.Benchmarks
{
	[MemoryDiagnoser]
	public class AspNetCoreMiddlewareBenchmark
	{
		private static readonly string s_content = null;

		private IServiceProvider _services;
		private IHostingEnvironment _environment;


		static AspNetCoreMiddlewareBenchmark()
		{
			string absoluteDirectoryPath = Utils.GetAbsoluteDirectoryPath("../../../Files/html");
			string absoluteFilePath = Path.Combine(absoluteDirectoryPath, "josh-ducks-periodic-table.html");

			s_content = File.ReadAllText(absoluteFilePath);
		}


		[GlobalSetup]
		public void GlobalSetup()
		{
			var mockServices = new Mock<IServiceProvider>();
			mockServices
				.Setup(s => s.GetService(typeof(IHtmlMinificationManager)))
				.Returns(new HtmlMinificationManager(new NullLogger(),
					Options.Create(new HtmlMinificationOptions
					{
						CssMinifierFactory = new KristensenCssMinifierFactory(),
						JsMinifierFactory = new CrockfordJsMinifierFactory()
					})
				))
				;
			mockServices
				.Setup(s => s.GetService(typeof(IHttpCompressionManager)))
				.Returns(new HttpCompressionManager(Options.Create(new HttpCompressionOptions())))
				;

			var mockEnvironment = new Mock<IHostingEnvironment>();
			mockEnvironment
				.Setup(m => m.EnvironmentName)
				.Returns(EnvironmentName.Production)
				;

			_services = mockServices.Object;
			_environment = mockEnvironment.Object;
		}

		[Benchmark(Baseline = true)]
		public async Task None()
		{
			await Process(true, true);
		}

		[Benchmark()]
		public async Task Minification()
		{
			await Process(false, true);
		}

		[Benchmark()]
		public async Task Compression()
		{
			await Process(true, false);
		}

		[Benchmark()]
		public async Task MinificationAndCompression()
		{
			await Process(false, false);
		}

		private async Task Process(bool disableMinification, bool disableCompression)
		{
			HttpContext context = CreateHttpContext();
			IOptions<WebMarkupMinOptions> options = Options.Create(new WebMarkupMinOptions
			{
				DisableMinification = disableMinification,
				DisableCompression = disableCompression,
				HostingEnvironment = _environment
			});

			var middleware = new WebMarkupMinMiddleware(NextOperation, options, _services);
			await middleware.Invoke(context);
		}

		private static Task NextOperation(HttpContext context)
		{
			HttpResponse response = context.Response;
			response.StatusCode = 200;
			response.ContentType = "text/html; charset=utf-8";

			return response.WriteAsync(s_content);
		}

		private static HttpContext CreateHttpContext()
		{
			HttpContext context = new DefaultHttpContext();

			HttpRequest request = context.Request;
			request.Method = "GET";
			request.Path = new PathString("/");
			request.QueryString = QueryString.Empty;
			request.Headers[HeaderNames.AcceptEncoding] = "gzip, deflate, br";

			context.Response.Body = new MemoryStream();

			return context;
		}

		[GlobalCleanup]
		public void GlobalCleanup()
		{
			_services = null;
			_environment = null;
		}
	}
}