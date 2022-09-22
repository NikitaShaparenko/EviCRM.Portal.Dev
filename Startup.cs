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

namespace EviCRM.Portal
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins(
                                                            "https://evicrm.ru.com",
                                                            "http://www.evicrm.ru.com",
                                                            "https://www.evicrm.ru.com",
                                                            "https://evicrm.store");
                                  });
            });

            services.AddControllersWithViews();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient();

            //services.AddMvc(options =>
            //{
            //    options.EnableEndpointRouting = false;
            //});

            //services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "ALLOW-FROM https://evicrm.ru.com https://evicrm.space https://evicrm.store https://evicrm.fun");
                context.Response.Headers.Add("Content-Security-Policy", "frame-ancestors https://evicrm.ru.com https://evicrm.space https://evicrm.store https://evicrm.fun");
                await next();
            });

            app.UseCors(builder => builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin());

           // app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.UseCors(MyAllowSpecificOrigins);

            //app.UseMvc(routes =>
            //    {
            //        routes.MapRoute("api", "api/get", new { controller = "Home", action = "API" });
            //        routes.MapRoute("file_uploading","", new { controller = "Home", action = "Upload" });

            //        routes.MapRoute(
            //            name: "default",
            //            template: "{controller=Home}/{action=Index}/{id?}");
            //    });

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapHub<ChatHub>("/file");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
