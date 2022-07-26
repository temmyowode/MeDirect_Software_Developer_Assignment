using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CurrencyXchange.Data.Enitity;
using CurrencyXchange.Data.Entity;
using CurrencyXchange.Data.Model;
using CurrencyXChange.Core.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PagedList.Core;

namespace CurrencyXChange.Core.Service
{
    public class XchangeService : IXchangeService
    {
        private ILogger<XchangeService> _logger;
        private readonly IRepository<TransactionHistory> _transHistoryRepo;
        private readonly TransactionSetting transactionSetting;
        private readonly IRateService _rateService;
        private readonly IMapper _mapper;
        private readonly string _connStr;
        public XchangeService(ILogger<XchangeService> logger, IRepository<TransactionHistory> transHistoryRepo, IOptions<TransactionSetting> options, IMapper mapper, IRateService rateService, IConfiguration config)
        {
            _logger = logger;
            _transHistoryRepo = transHistoryRepo;
            transactionSetting = options.Value;
            _mapper = mapper;
            _rateService = rateService;
            _connStr = config.GetConnectionString("DefaultConnection");
        }

        public DbContextOptionsBuilder<AppDbContext> DbOptions
        {
            get
            {
                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                optionsBuilder.UseSqlServer(_connStr);

                return optionsBuilder;
            }
        }
        public async Task Trade(TransactionViewModel model)
        {
            try
            {
                var startDate = DateTime.Now.AddMinutes(-1 * transactionSetting.Duration);

                var transactions = _transHistoryRepo.Filter(x => x.ClientId == model.ClientId && (x.TransDate >= startDate && x.TransDate <= DateTime.Now));
                if (transactions.Count() < transactionSetting.Limit)
                {
                    var transHistory = _mapper.Map<TransactionHistory>(model);

                    var rates = await _rateService.FetchRates();
                    var currRate = rates.Where(x => x.Key == model.CurrencyTo).ToList();
                    if (currRate.Count() == 0)
                    {
                        throw new Exception("Invalid Currency");
                    }

                    transHistory.Rate = currRate[0].Value;
                    transHistory.Value = currRate[0].Value * model.Amount;
                    transHistory.DateCreated = DateTime.Now;
                    transHistory.TransDate = DateTime.Now;

                    //_transHistoryRepo.Create(transHistory);
                    using (var _context = new AppDbContext(DbOptions.Options))
                    {
                        await _context.AddAsync(transHistory);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    throw new Exception("Transaction Limit Exceeded");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred during trading. Details {0}", ex.ToString());
                throw;
            }
        }

        public IPagedList<TransactionViewModel> FetchHistory(Guid ClientId, string StartDate, string EndDate, int Page=1, int PageSize=20)
        {
            try
            {
                var sDate = DateTime.Today;
                var eDate = DateTime.Now;

                var dateFormat = new string[] { "yyyy-MM-dd", "yyyy/MM/dd", "dd-MM-yyyy", "dd/MM/yyyy" };

                if (!string.IsNullOrEmpty(StartDate))
                {
                    DateTime.TryParseExact(StartDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out sDate);
                }

                if (!string.IsNullOrEmpty(EndDate))
                {
                    DateTime.TryParseExact(EndDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out eDate);
                }

                var query = _transHistoryRepo.Filter(x => x.TransDate >= sDate && x.TransDate <= eDate);

                if (ClientId != Guid.Empty)
                {
                    query = query.Where(x => x.ClientId == ClientId);
                }

                var tranHistory = query.Select(x=> _mapper.Map<TransactionViewModel>(x)).ToPagedList(Page, PageSize);

                return tranHistory;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred during trading. Details {0}", ex.ToString());
                throw;
            }
        }
    }
}
