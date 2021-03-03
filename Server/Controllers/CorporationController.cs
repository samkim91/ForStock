using System.Collections.Generic;
using System.Threading.Tasks;
using ForStock.Shared.Models;
using ForStock.Server.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using ForStock.Shared.Common;
using System;

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

        [HttpGet("info/{crtfc_key}/{key_word}")]
        public async Task<CorporationInfo> GetCorpInfo(string crtfc_key, string key_word)
        {   
            CorporationInfo corporationInfo = new CorporationInfo();

            // stock code를 corp_code(고유번호)로 바꿈
            string corp_code = CorpCodeHelper.GetCorpCode(key_word);

            if(String.IsNullOrWhiteSpace(corp_code)){
                corporationInfo.message = "해당 회사를 찾을 수 없습니다.";
                return corporationInfo;
            }

            // Corporation의 infomation을 API로 요청.
            // parameters : api_key, corp_code
            // response : corperation infomation
            corporationInfo = await _httpClient.GetFromJsonAsync<CorporationInfo>("api/company.json?crtfc_key=" + crtfc_key + "&corp_code=" + corp_code);

            return corporationInfo;
        }

        [HttpGet("financialstatement/{crtfc_key}/{key_word}/{fs_div}")]
        public async Task<List<FinancialStatement>> GetFinancialStatement(string crtfc_key, string key_word, string fs_div)
        {
            // stock code를 corp_code(고유번호)로 바꿈
            string corp_code = CorpCodeHelper.GetCorpCode(key_word);
            
            // 검색해야할 사업연도와 보고서코드를 Helper class에서 가져옴.
            DateHelper dateHelper = new DateHelper();
            JArray jArrayForRequest = dateHelper.GetQuartersForRequest();
            List<FinancialStatement> FinancialStatements = new List<FinancialStatement>();

            // Corporation의 financial statements를 API로 요청.
            // parameters : api_key, corp_code, bsns_year, reprt_code, fs_div
            // response : financial statement
            foreach(JObject jObject in jArrayForRequest){
                FinancialStatement FinancialStatement = await _httpClient.GetFromJsonAsync<FinancialStatement>("api/fnlttSinglAcntAll.json?crtfc_key=" + crtfc_key + "&corp_code=" + corp_code + "&bsns_year=" + jObject.SelectToken("Year") +
                                         "&reprt_code=" + jObject.SelectToken("Quarter") + "&fs_div=" + fs_div);
                FinancialStatement.id = jObject.SelectToken("Year") + "/" + jObject.SelectToken("Quarter");
                FinancialStatement.year = jObject.SelectToken("Year").ToString();
                FinancialStatement.quarter = jObject.SelectToken("Quarter").ToString();
                FinancialStatements.Add(FinancialStatement);
            }

            return FinancialStatements;
        }
    }
}
