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
        public async Task<CorporationInfo> GetCorpInfo(string crtfc_key, string stock_code)
        {   
            // stock code를 corp_code(고유번호)로 바꿈
            string corp_code = CorpCodeHelper.GetCorpCode(stock_code);

            // Corporation의 infomation을 API로 요청.
            // parameters : api_key, corp_code
            // response : corperation infomation
            CorporationInfo corporationInfo = await _httpClient.GetFromJsonAsync<CorporationInfo>("api/company.json?crtfc_key=" + crtfc_key + "&corp_code=" + corp_code);

            return corporationInfo;
        }

        [HttpGet("financialstatement/{crtfc_key}/{stock_code}/{fs_div}")]
        public async Task<List<FinacialStatement>> GetFinancialStatement(string crtfc_key, string stock_code, string fs_div)
        {
            // stock code를 corp_code(고유번호)로 바꿈
            string corp_code = CorpCodeHelper.GetCorpCode(stock_code);
            
            // 검색해야할 사업연도와 보고서코드를 Helper class에서 가져옴.
            DateHelper dateHelper = new DateHelper();
            JArray jArrayForRequest = dateHelper.GetQuartersForRequest();
            List<FinacialStatement> finacialStatements = new List<FinacialStatement>();

            // Corporation의 financial statements를 API로 요청.
            // parameters : api_key, corp_code, bsns_year, reprt_code, fs_div
            // response : financial statement
            foreach(JObject jObject in jArrayForRequest){
                FinacialStatement finacialStatement = await _httpClient.GetFromJsonAsync<FinacialStatement>("api/fnlttSinglAcntAll.json?crtfc_key=" + crtfc_key + "&corp_code=" + corp_code + "&bsns_year=" + jObject.SelectToken("Year") +
                                         "&reprt_code=" + jObject.SelectToken("Quarter") + "&fs_div=" + fs_div);
                finacialStatement.id = jObject.SelectToken("Year") + "/" + jObject.SelectToken("Quarter");
                finacialStatement.year = jObject.SelectToken("Year").ToString();
                finacialStatement.quarter = jObject.SelectToken("Quarter").ToString();
                finacialStatements.Add(finacialStatement);
            }

            return finacialStatements;
        }
    }
}
