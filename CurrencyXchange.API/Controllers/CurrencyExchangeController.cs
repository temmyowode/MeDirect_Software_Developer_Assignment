using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyXchange.Data.Model;
using CurrencyXChange.Core.Interface;
using CurrencyXChange.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PagedList.Core;

namespace CurrencyXchange.API.Controllers
{
    [Route("api/Currency/Exchange")]
    [ApiController]
    public class CurrencyExchangeController : ControllerBase
    {
        private ILogger<CurrencyExchangeController> _logger;
        private readonly IXchangeService _xChangeService;
        public CurrencyExchangeController(ILogger<CurrencyExchangeController> logger, IXchangeService xChangeService)
        {
            _logger = logger;
            _xChangeService = xChangeService;
        }

        [HttpGet("History")]
        [ProducesResponseType(typeof(PagedList<TransactionViewModel>),StatusCodes.Status200OK)]
        public IActionResult FetchHistory(Guid ClientId, string StartDate, string EndDate, int Page=1, int PageSize=20)
        {
            try
            {
                return Ok(_xChangeService.FetchHistory(ClientId,StartDate,EndDate,Page,PageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Fetching History. Details {0}", ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Trade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Trade(TransactionViewModel Model)
        {
            try
            {
                _xChangeService.Trade(Model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Trading Currency. Details {0}", ex.ToString());
                return BadRequest(ex.Message);
            }
        }
    }
}
