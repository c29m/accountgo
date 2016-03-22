﻿using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebAngular
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            //// Set up configuration sources.
            //var builder = new ConfigurationBuilder()
            //    //.SetBasePath(appEnv.ApplicationBasePath)
            //    .AddJsonFile("appsettings.json")
            //    .AddEnvironmentVariables();
            //Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");

            //    // 404 routingfor SPA
            //    routes.MapRoute("spa-fallback", "{*anything}", new { controller = "Home", action = "Index" });

            //});
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
