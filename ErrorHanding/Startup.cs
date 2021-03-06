using ErrorHanding.Filter;
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
            services.AddMvc(options =>
            {
                options.Filters.Add(new CustomHandleExceptionFilterAttribute() { ErrorPage="error1"});
            });
        }



        //Request ----------------------[DeveloperExceptonPage]--------------[UseExceptionHandler]--------[UseStatusCode]----[UseDatabaseErrorPage]--> Response

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {

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
