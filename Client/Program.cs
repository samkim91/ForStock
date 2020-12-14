using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Blazor.IndexedDB.Framework;
using ForStock.Client.ViewModels;

namespace ForStock.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<IIndexedDbFactory, IndexedDbFactory>();
            builder.Services.AddHttpClient<IIntroViewModel, IntroViewModel>
                ("ServerAPI", Client => Client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddHttpClient("DartAPI", Client => Client.BaseAddress = new Uri("https://opendart.fss.or.kr/api/"));

            
            

            await builder.Build().RunAsync();
        }
    }
}
