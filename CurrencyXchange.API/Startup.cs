using AutoMapper;
using CurrencyXchange.API.Extension;
using CurrencyXchange.Data.AutoMapper;
using CurrencyXchange.Data.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyXchange.API
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(env.ContentRootPath)
                   .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                   .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                   .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton((sp) => Configuration);
            services.ConfigureCors();
            services.ConfigureDatabaseContext(Configuration);
            services.ConfigureRepositoryWrapper();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MyMappingProfiles());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.ConfigureSwagger();
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

            services.AddOptions();
            services.Configure<CacheSetting>(Configuration.GetSection("CacheSetting"));
            services.Configure<ExchangeSetting>(Configuration.GetSection("ExchangeSetting"));
            services.Configure<TransactionSetting>(Configuration.GetSection("TransactionSetting"));

            services.AddMemoryCache();
            services.AddMvc();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowMyOrigin");
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "My API V1");
                c.DocumentTitle = "Currency Exchange APIs";
            });
        }
    }
}
