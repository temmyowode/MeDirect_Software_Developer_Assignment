using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyXchange.Data.Model
{
    public class ExchangeSetting
    {
        public string Url { get; set; }
        //public string EndPoint { get; set; }
        public string APIKey { get; set; }
        public string BaseCurrency { get; set; }
    }
}
