using System;
using System.IO;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Moq;
#if NET462
using Environments = Microsoft.AspNetCore.Hosting.EnvironmentName;
using HostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
#elif NETCOREAPP3_1_OR_GREATER
using Environments = Microsoft.Extensions.Hosting.Environments;
using HostingEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;
#else
#error No implementation for this target
#endif

using WebMarkupMin.AspNet.Common;
#if NET462
using WebMarkupMin.AspNetCore1;
#elif NETCOREAPP3_1 || NET5_0
using WebMarkupMin.AspNetCore3;
#elif NET6_0 || NET7_0
using WebMarkupMin.AspNetCore6;
#elif NET8_0 || NET9_0 || NET10_0
using WebMarkupMin.AspNetCoreLatest;
#else
#error No implementation for this target
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
		private HostingEnvironment _environment;


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

			var mockEnvironment = new Mock<HostingEnvironment>();
			mockEnvironment
				.Setup(m => m.EnvironmentName)
				.Returns(Environments.Production)
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