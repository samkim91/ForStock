using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForStock.Shared.Model;
using ForStock.Server.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;



namespace ForStock.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CorporationController : ControllerBase
    {
        private readonly ILogger<CorporationController> logger;

        public CorporationController(ILogger<CorporationController> logger)
        {
            this.logger = logger;
        }

        [HttpGet("info/{stockcode}")]
        public async Task<string> GetProfile(string stockcode){
            string corpCode = CorpCodeHelper.GetCorpCode(stockcode);
            
            return await Task.FromResult("\""+corpCode+"\"");
        }
    }
}
