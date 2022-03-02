using Bookstore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        //SET UP CONFIGURATION
        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration temp)
        {
            Configuration = temp;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // WE'RE USING THE MVC MODEL :D
            services.AddControllersWithViews();
            // SET UP CONNECTION TO DATABASE
            services.AddDbContext<BookstoreContext>(options =>
            {
                options.UseSqlite(Configuration["ConnectionStrings:BookDBConnection"]);
            });
            services.AddScoped<IBookstoreRepository, EFBookstoreRepository>();

            //enable razor pages
            services.AddRazorPages();

            //Implement session
            services.AddDistributedMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // WE WANT ACCESS TO FILES IN WWWROOT FOLDER
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("categorypage",
                    "{bookCategory}/page{pageNum}",
                    new { Controller = "Home", action = "Index" }
                    );

                endpoints.MapControllerRoute(
                    name: "Paging",
                    pattern: "page{pageNum}", //this sets up the slug we can use
                    defaults: new { Controller = "Home", action = "Index", pageNum = 1 }
                    );

                endpoints.MapControllerRoute("category",
                    "{bookCategory}",
                    new { Controller = "Home", action = "Index", pageNum = 1 }
                    );
                
                // SIMPLE WAY OF SETTING UP DEFAULT ROUTE
                endpoints.MapDefaultControllerRoute();

                // enable razor pages
                endpoints.MapRazorPages();
            });
        }
    }
}
