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
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

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

        [HttpGet("info/{crtfc_key}/{stock_code}")]
        public async Task<string> GetCorpInfo(string crtfc_key, string stock_code)
        {
            string corp_code = CorpCodeHelper.GetCorpCode(stock_code);

            // Todo.. GetAsyncStream 가능한지 확인해봐야함.
            HttpResponseMessage httpResponse = await _httpClient.GetAsync("api/company.json?crtfc_key=" + crtfc_key + "&corp_code=" + corp_code);

            if (httpResponse.IsSuccessStatusCode)
            {
                return await httpResponse.Content.ReadAsStringAsync();
            }

            throw new HttpRequestException();
        }

        [HttpGet("financialstatement/{crtfc_key}/{stock_code}/{bsns_year}/{reprt_code}/{fs_div}")]
        public async Task<string> GetFinancialStatement(string crtfc_key, string stock_code, string bsns_year, string reprt_code, string fs_div)
        {
            string corp_code = CorpCodeHelper.GetCorpCode(stock_code);

            HttpResponseMessage httpResponse = await _httpClient
                    .GetAsync("api/fnlttSinglAcntAll.json?crtfc_key=" + crtfc_key + "&corp_code=" + corp_code + "&bsns_year=" + bsns_year +
                               "&reprt_code=" + reprt_code + "&fs_div=" + fs_div);

            if (httpResponse.IsSuccessStatusCode)
            {
                return await httpResponse.Content.ReadAsStringAsync();
            }
            throw new HttpRequestException();
        }
    }
}
