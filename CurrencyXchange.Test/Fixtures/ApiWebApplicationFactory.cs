using CurrencyXchange.API;
using CurrencyXChange.Core.Interface;
using CurrencyXChange.Core.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyXchange.Test.Fixtures
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Program>
    {
        public IConfiguration Configuration { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                Configuration = new ConfigurationBuilder()
                  .AddJsonFile("integrationsettings.json")
                  .Build();

                config.AddConfiguration(Configuration);
            });

            builder.ConfigureTestServices(services =>
            {
                services.AddTransient<IRateService, RateService>();
                services.AddTransient<IXchangeService, XchangeService>();
            });
        }
    }
}
