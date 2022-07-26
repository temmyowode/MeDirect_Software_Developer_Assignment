using Microsoft.Extensions.Configuration;
using Respawn;
using System.Net.Http;
using Xunit;

namespace CurrencyXchange.Test.Fixtures
{
    [Trait("Category", "Integration")]
    public abstract class IntegrationTest : IClassFixture<ApiWebApplicationFactory>
    {
        protected readonly ApiWebApplicationFactory _factory;
        protected readonly HttpClient _client;

        public IntegrationTest(ApiWebApplicationFactory fixture)
        {
            _factory = fixture;
            _client = _factory.CreateClient();
        }
    }
}
