using Microsoft.AspNetCore.Mvc;
using PalaCrypto.Model;
using PalaCrypto.Service;

namespace PalaCrypto.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PalaCryptoController : ControllerBase
    {

        private readonly ILogger<PalaCryptoController> _logger;
        private readonly LogService _serviceLog;

        public PalaCryptoController(ILogger<PalaCryptoController> logger, LogService serviceLog)
        {
            _logger = logger;
            _serviceLog = serviceLog;
        }

        [HttpGet("{item}/{letter}/{time}")]
        public async Task<ActionResult<Dictionary<string,LogDifferenceAdmin>>> GetPriceForChart(char letter, int time, string item)
        {
           return Ok(_serviceLog.GetPriceForChart(letter, time, item));
        }

        [HttpGet("{item}")]
        public async Task<ActionResult<Dictionary<string, LogDifferenceAdmin>>> GetAllPriceItemPositive(string item)
        {
            return Ok(_serviceLog.GetAllPriceItemPositive(item));
        }

        [HttpGet("{item}")]
        public async Task<ActionResult<Dictionary<string, LogDifferenceAdmin>>> GetAllPriceItemNegative(string item)
        {
            return Ok(_serviceLog.GetAllPriceItemNegative(item));
        }

        [HttpGet("{item}")]
        public async Task<ActionResult<Dictionary<string, LogDifferenceAdmin>>> GetAllPriceItem(string item)
        {
            return Ok(_serviceLog.GetAllPriceItem(item));
        }

        [HttpGet()]
        public async Task<ActionResult<Dictionary<string, LogDifferenceAdmin>>> GetAllLastDifference()
        {
            return Ok(_serviceLog.GetAllLastDifference());
        }


    }
}
