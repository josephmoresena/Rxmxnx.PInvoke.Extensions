using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Rxmxnx.PInvoke.Json;

namespace WebAssemblyAppTest
{
	public class Program
	{
		public static JsonSerializerOptions JsonOptions = new()
		{
			PropertyNameCaseInsensitive = true,
			Converters = { new CStringJsonConverter(), new CStringSequenceJsonConverter(), },
		};

		public static async Task Main(String[] args)
		{
			WebAssemblyHostBuilder? builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("app");

			builder.Services.AddTransient(_ => new HttpClient
			{
				BaseAddress = new(builder.HostEnvironment.BaseAddress),
			});

			await builder.Build().RunAsync();
		}
	}
}