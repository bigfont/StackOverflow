﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApplication
{
    public class Program
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.ToString().Contains("FileUpload"))
                {
                    await next.Invoke();
                }
                else
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync("<html><body>");

                    await context.Response.WriteAsync(@"
<form action = ""/FileUpload"" method=""post"" enctype=""multipart/form-data"">
<label for=""myfile1"">File</label>
<input type=""file"" name=""myFile1"" />
<label for=""myfile2"">File</label>
<input type=""file"" name=""myFile2"" />
<input type=""submit"" value=""Send"" />
</form>");

                    await context.Response.WriteAsync("</body></html>");
                }
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=FileUpload}/{action=index}/{id?}"
                );

            });
        }

        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                        .UseServer("Microsoft.AspNetCore.Server.Kestrel")
                        .UseStartup<Program>()
                        .Build();

            host.Run();
        }
    }
}