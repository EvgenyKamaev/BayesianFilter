using BayesianFilter.Core.Logger;
using BayesianFilter.Core.Services;
using BayesianFilter.Core.Services.Interfaces;
using BayesianFilter.Web.Services;
using BayesianFilter.Web.Services.Interfaces;
using BayesianFilter.Web.Tools;
using BayesianFilter.Web.Tools.Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BayesianFilter.Web
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
            services.AddTransient<IHostConfig, HostConfig>();
            services.AddTransient<ICoreConfig, HostConfig>();
            services.AddTransient<IHostLogger, HostLogger>();
            services.AddTransient<ICoreLogger, HostLogger>();
            services.AddTransient<IBayesianRepository, BayesianRepository>();
            services.AddTransient<IBayesianClassifier, BayesianClassifier>();
            services.AddSingleton<IBayesianContainer, BayesianContainer>();
            services.AddTransient<IExceptionsRepository, ExceptionsRepository>();
            services.AddTransient<IBayesianService, BayesianService>();
            services.AddControllersWithViews();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BayesianFilter", Version = "v1", Description = "Naive Bayes classifier" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBayesianContainer bayesianContainer, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(Configuration.GetSection("Logging"));
            bayesianContainer.LoadData();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "BayesianFilter v1"); c.RoutePrefix = ""; });

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
            app.UseMiddleware<ExceptionMiddleware>();
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
