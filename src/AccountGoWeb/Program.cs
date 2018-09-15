﻿using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace AccountGoWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls()
                .Build();

            host.Run();
        }
    }
}
