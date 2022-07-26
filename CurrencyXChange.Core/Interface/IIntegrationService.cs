using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CurrencyXchange.Data.Model;

namespace CurrencyXChange.Core.Interface
{
    public interface IIntegrationService
    {
        Task<RateResponseViewModel> FetchRates();
    }
}
