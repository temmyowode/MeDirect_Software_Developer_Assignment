using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CurrencyXchange.Data.Model;
using CurrencyXchange.Test.Fixtures;
using CurrencyXchange.Test.Utils;
using FluentAssertions;
using Newtonsoft.Json;
using PagedList.Core;
using Xunit;

namespace CurrencyXchange.Test
{
    public class CurrencyXchangeTest : IntegrationTest
    {
        public CurrencyXchangeTest(ApiWebApplicationFactory fixture)
            : base(fixture) { }

        [Fact]
        public async Task Post_Trades()
        {
            var rates = await _client.GetAndDeserialize<Dictionary<string, decimal>>("api/Rate/List");
            rates.Should().NotBeNull();


            var model = new TransactionViewModel()
            {
                Amount = 200,
                ClientId = Guid.NewGuid(),
                CurrencyFrom = "EUR",
                CurrencyTo = "NGN",
            };

            var json = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Currency/Exchange/Trade", stringContent);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task History()
        {

            var response = await _client.GetAndDeserialize<IEnumerable<TransactionViewModel>>("api/Currency/Exchange/History");
            //Assert.Equal(1, response.PageNumber);
            //Assert.Equal(20, response.PageSize);
            response.Should().NotBeEmpty();
        }
    }
}
