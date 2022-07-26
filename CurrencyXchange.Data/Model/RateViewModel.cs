using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyXchange.Data.Model
{
    public class RateViewModel
    {
        public Dictionary<string,decimal> Rates { get; set; }
        public long TimeStamp { get; set; }
    }

    public class RateResponseViewModel : RateViewModel
    {
        public string Base { get; set; }
        public string Date { get; set; }
        public bool Success { get; set; }
    }
}
