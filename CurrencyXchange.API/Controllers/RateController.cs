using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyXChange.Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrencyXchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private ILogger<RateController> _logger;
        private readonly IRateService _rateService;
        public RateController(ILogger<RateController> logger, IRateService rateService)
        {
            _logger = logger;
            _rateService = rateService;
        }

        [HttpGet("List")]
        [ProducesResponseType(typeof(Dictionary<string,decimal>), StatusCodes.Status200OK)]
        public async  Task<IActionResult> List()
        {
            try
            {
                return Ok(await _rateService.FetchRates());
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Fetching Rates, Details {0}", ex.ToString());
                return BadRequest(ex.Message);
            }
        }
    }
}
