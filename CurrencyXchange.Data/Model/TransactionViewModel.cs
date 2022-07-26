using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyXchange.Data.Model
{
    public class TransactionViewModel
    {
        public Guid ClientId { get; set; }
        public string CurrencyFrom { get; set; }
        public string CurrencyTo { get; set; }
        public decimal Amount { get; set; }
        public decimal Value { get; set; }
        public DateTime TransDate { get; set; }
        public decimal Rate { get; set; }
        public string TransactionDate 
        {
            get
            {
                return this.TransDate.ToString("yyyy-MM-dd");
            }
        }
    }
}
