using CurrencyXchange.Data.Model;
using CurrencyXChange.Core.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyXChange.Core.Service
{
    public class IntegrationService : IIntegrationService
    {
        private ILogger<IntegrationService> _logger;
        private readonly IRestClient _client;
        private ExchangeSetting exchangeSetting;

        public IntegrationService(ILogger<IntegrationService> logger, IOptions<ExchangeSetting> options, IRestClient client)
        {
            _logger = logger;
            exchangeSetting = options.Value;
            _client = client;
        }

        public async Task<RateResponseViewModel> FetchRates()
        {
            try
            {
                var resp = new RateResponseViewModel();
                var url = exchangeSetting.Url;
                url = url.Replace("{baseCurrency}", exchangeSetting.BaseCurrency);
                _client.BaseUrl = new Uri(url);
                RestRequest request = new RestRequest(Method.GET);
                request.AddHeader("apikey", exchangeSetting.APIKey);

                IRestResponse response = await _client.ExecuteAsync(request);
                if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
                {
                    resp = JsonConvert.DeserializeObject<RateResponseViewModel>(response.Content);

                    return resp;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occurred Getting Rates. Details: {0}", ex.ToString());
                throw;
            }
        }

    }
}
