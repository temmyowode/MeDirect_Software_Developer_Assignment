using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CurrencyXchange.Data.Model;
using PagedList.Core;

namespace CurrencyXChange.Core.Interface
{
    public interface IXchangeService
    {
        Task Trade(TransactionViewModel model);
        IPagedList<TransactionViewModel> FetchHistory(Guid ClientId, string StartDate, string EndDate, int Page = 1, int PageSize = 20);
    }
}
