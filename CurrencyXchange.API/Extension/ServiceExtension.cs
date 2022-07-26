using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using CurrencyXchange.Data.Entity;
using CurrencyXChange.Core.Interface;
using CurrencyXChange.Core.Repository;
using CurrencyXChange.Core.Service;
using RestSharp;

namespace CurrencyXchange.API.Extension
{
    public static class ServiceExtension
    {

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyOrigin", builder => builder.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader());
                // .AllowCredentials());

            });
        }

        public static void ConfigureDatabaseContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            //services.AddDbContext<AppDbContext>(options => options
            //                                      // .UseLazyLoadingProxies()
            //                                        .UseSqlServer(connectionString));
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString,
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                });
            });

            services.AddDbContext<AppDbContext>(ServiceLifetime.Transient);
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IXchangeService), typeof(XchangeService));
            services.AddScoped(typeof(IRateService), typeof(RateService));
            services.AddScoped(typeof(ICacheProvider), typeof(CacheProvider));
            services.AddScoped(typeof(IIntegrationService), typeof(IntegrationService));
            services.AddScoped(typeof(IRestClient), typeof(RestClient));
            services.AddScoped<DbContext, AppDbContext>();
        }

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Currency Exchange API",
                    Version = "V1"
                });
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
                //this is the step where we add the operation filter
               // c.OperationFilter<FileUploadOperation>();

            });


            return services;
        }



        public static void UpdateDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<AppDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
    
}
