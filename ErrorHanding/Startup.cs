using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErrorHanding
{
    public class Startup
    {


        

        public Startup(IConfiguration configuration)
        {


            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }



        //Request ----------------------[DeveloperExceptonPage]--------------[UseExceptionHandler]--------[UseStatusCode]----[UseDatabaseErrorPage]--> Response

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {

                //app.UseDatabaseErrorPage();








                // 1. Yol
                app.UseDeveloperExceptionPage();
                // 2.Yol
                app.UseStatusCodePages("text/plain", "Bir hata var. Durum Kodu: {0}");
                //3.Yol
                app.UseStatusCodePages(async context =>
                {
                    context.HttpContext.Response.ContentType = "text/plain";
                    await context.HttpContext.Response.WriteAsync($"Bir Hata var. Durum kodu:{context.HttpContext.Response.StatusCode}");
                });
            }
            else
            {
                app.UseHsts();
            }
            app.UseExceptionHandler(context =>
            {
                context.Run(async page =>
                {
                    page.Response.StatusCode = 500;
                    page.Response.ContentType = "text/html";
                    await page.Response.WriteAsync($"<html><head></head></h1>Hata var: {page.Response.StatusCode}</h1></html>");
                });
            });





            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
