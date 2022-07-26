using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyXchange.Data.Enitity
{
    public class TransactionHistory
    {
        public long Id { get; set; }
        public Guid ClientId { get; set; }
        public string CurrencyFrom { get; set; }
        public string CurrencyTo { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
        public decimal Value { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
