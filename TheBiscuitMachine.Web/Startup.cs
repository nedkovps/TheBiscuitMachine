using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheBiscuitMachine.Logic;
using TheBiscuitMachine.Logic.Common;
using TheBiscuitMachine.Logic.Configuration;
using TheBiscuitMachine.Logic.Models;
using TheBiscuitMachine.Web.Hubs;

namespace TheBiscuitMachine.Web
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
            services.RegisterDomainDependencies();
            services.Configure<BiscuitMachineOptions>(options => Configuration.Bind("BiscuitMachine", options));
            services.AddSingleton<IMachine, BiscuitMachine>();

            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "BiscuitMachine.UI/build";
            });

            services.AddSignalR();
            services.AddSingleton<BiscuitMachineUIEvents>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BiscuitMachineUIEvents eventEmitter)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapHub<BiscuitMachineHub>("/hub/BiscuitMachine");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "BiscuitMachine.UI";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

            eventEmitter.RegisterEvents();
        }
    }
}
