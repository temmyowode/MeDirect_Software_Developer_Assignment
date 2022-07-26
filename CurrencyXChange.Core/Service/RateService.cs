using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CurrencyXchange.Data.Model;
using CurrencyXchange.Data.Utils;
using CurrencyXChange.Core.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CurrencyXChange.Core.Service
{
    public class RateService : IRateService
    {
        private ILogger<RateService> _logger;
        private readonly ICacheProvider _cacheProvider;
        private readonly IIntegrationService _integrationService;
        private readonly CacheSetting cacheSetting;
        public RateService(ILogger<RateService> logger, ICacheProvider cacheProvider, IIntegrationService integrationService, IOptions<CacheSetting> options)
        {
            _logger = logger;
            _cacheProvider = cacheProvider;
            _integrationService = integrationService;
            cacheSetting = options.Value;
        }

        public async Task<Dictionary<string,decimal>> FetchRates()
        {
            try
            {
                var rates = new Dictionary<string, decimal>();
                var xChangeRate= _cacheProvider.GetFromCache<RateViewModel>(CacheKeys.Rate);
                if (xChangeRate == null)
                {
                    xChangeRate = await FetchExchangeRates();
                    if (xChangeRate != null)
                    {
                        _cacheProvider.SetCache<RateViewModel>(CacheKeys.Rate, xChangeRate, DateTimeOffset.Now.AddMinutes(cacheSetting.Duration));
                    }
                }

                rates = xChangeRate.Rates;
                return rates;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occurred Getting Rates. Details: {0}", ex.ToString());
                throw;
            }
        }

        public async Task<RateViewModel> FetchExchangeRates()
        {
            try
            {
                RateViewModel xChangeRate = new RateViewModel();
                var rateResp = await _integrationService.FetchRates();
                if (rateResp.Success)
                {
                    xChangeRate.Rates = rateResp.Rates;
                    xChangeRate.TimeStamp = rateResp.TimeStamp;
                }

                return xChangeRate;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occurred Getting Rates from the Exchange Service. Details: {0}", ex.ToString());
                throw;
            }
        }
    }
}
