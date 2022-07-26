using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyXChange.Core.Interface
{
    public interface IRateService
    {
        Task<Dictionary<string, decimal>> FetchRates();
    }
}
