using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForStock.Shared.Model;
using ForStock.Server.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.IO;
using System.Text.Json;

namespace ForStock.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CorporationController : ControllerBase
    {
        private readonly ILogger<CorporationController> logger;

        private readonly HttpClient _httpClient;

        public CorporationController(ILogger<CorporationController> logger, IHttpClientFactory clientFactory)
        {
            this.logger = logger;
            this._httpClient = clientFactory.CreateClient("DartAPI");

        }

        [HttpGet("info/{stock_code}/{api_key}")]
        public async Task<CorporationInfo> GetProfile(string stock_code, string api_key){            
            string corp_code = CorpCodeHelper.GetCorpCode(stock_code);
            
            HttpResponseMessage response = await _httpClient.GetAsync("api/company.json?crtfc_key="+api_key+"&corp_code="+corp_code);

            if (response.IsSuccessStatusCode){
                string responseString = await response.Content.ReadAsStringAsync();
                CorporationInfo corporationInfo = JsonSerializer.Deserialize<CorporationInfo>(responseString);
                return corporationInfo;
            }
            throw new HttpRequestException("Can't request corporation's info: "+response.StatusCode);
        }
    }
}
