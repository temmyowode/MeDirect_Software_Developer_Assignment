using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CurrencyXchange.API.Controllers;
using CurrencyXchange.Test.Fixtures;
using CurrencyXchange.Test.Utils;
using CurrencyXChange.Core.Interface;
using CurrencyXChange.Core.Service;
using FluentAssertions;
using Xunit;

namespace CurrencyXchange.Test
{
    public class RateControllerTest : IntegrationTest
    {
        public RateControllerTest(ApiWebApplicationFactory fixture)
            : base(fixture) { }

        [Fact]
        public async Task GET_retrieves_rates()
        {
            var response = await _client.GetAndDeserialize<Dictionary<string,decimal>>("api/Rate/List");
            response.Should().ContainKeys("USD","GBP");
        }
    }
}
