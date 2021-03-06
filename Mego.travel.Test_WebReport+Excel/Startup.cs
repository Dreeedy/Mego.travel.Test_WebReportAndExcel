using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mego.travel.Test_WebReport_Excel.Models; // ???????????? ???? ???????
using Microsoft.EntityFrameworkCore;

namespace Mego.travel.Test_WebReport_Excel
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // ???????? ?????? ??????????? ? ???????? ??????
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<OrderContext>(options => options.UseSqlServer(connection));
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

            // ???? ??? Read
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // ???? ??? Create
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "create",
                    pattern: "{controller=Home}/{action=Create}");
            });

            // ???? ??? Update
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "update",
                    pattern: "{controller=Home}/{action=Update}/{id}");
            });

            // ???? ??? Delete
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "delete",
                    pattern: "{controller=Home}/{action=Delete}/{id}");
            });

            #region ReportRoute
            // ??????? ?? ???????? ???????? ???????
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Report}/{action=Index}");
            });

            // ???????? ?????
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "report",
                    pattern: "{controller=Report}/{action=Index}/{StartDate}/{EndDate}");
            });
            #endregion
        }
    }
}
