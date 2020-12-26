using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Microsoft.Net.Http.Headers;
using System;

namespace ForStock.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddControllers().AddNewtonsoftJson();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                    policy.WithOrigins("http://localhost:5000", "https://localhost:5001")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddHttpClient("DartAPI", Client =>
            {
                Client.BaseAddress = new Uri("https://opendart.fss.or.kr/");
                // 하루 종일 삽질한 결과... request를 보낼 때 header 정보가 엄청 중요하다는 것을 알았다. Open Dart를 사용하는 것에는 User-Agent가 없으면 response가 안 온다..
                Client.DefaultRequestHeaders.Add("Accept", "*/*");
                Client.DefaultRequestHeaders.Add("Accept-Language", new string[] { "ko-KR", "ko" });
                Client.DefaultRequestHeaders.Add("Accept-Encoding", new string[] { "kgzip", "deflate", "br" });
                Client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                Client.DefaultRequestHeaders.Add("User-Agent", "localhost");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
