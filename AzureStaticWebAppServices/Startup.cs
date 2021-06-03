using AzureStaticWebAppServices.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureStaticWebAppServices
{
    public class Startup
    {

        public const string MYPOLICY = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }



        public static string [] GetClients ()
        {

            using (AzureStaticWebAppServicesContext ctx = new AzureStaticWebAppServicesContext())
            {

                return ctx.Clients.Select ( x => x.ClientUrl ).ToArray () ;
   

            }




        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<AzureStaticWebAppServicesContext>();
    //        services.AddDbContext<ApplicationDbContext>(options =>
    //options.UseSqlServer(
    //    Configuration.GetConnectionString("DefaultConnection")));



            //string[] origins = new string[] {                                       "file:///S:/w/AzureStaticWeb/JYHome/index.html",
            //                              "https://thankful-glacier-05748170f.azurestaticapps.net",
            //                              "http://localhost"
            //         };



            services.AddCors(options =>
            {
                options.AddPolicy(name: MYPOLICY,
                                  builder =>
                                  {
                                      //builder.all
                                      builder.WithOrigins(

                                          GetClients () 

                                          );
                                  });
            });

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
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();


            //app.UseCors(MYPOLICY);
            app.UseCors();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
