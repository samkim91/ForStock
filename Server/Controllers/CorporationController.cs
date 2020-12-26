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

        [HttpGet("info/{stock_code}/{crtfc_key}/{fs_div}")]
        public async Task<JObject> GetCorpInfo(string stock_code, string crtfc_key, string fs_div){
            JObject response = new JObject();
            string corp_code = CorpCodeHelper.GetCorpCode(stock_code);
            
            try{
                // Todo.. GetAsyncStream 가능한지 확인해봐야함.
                HttpResponseMessage httpResponse = await _httpClient.GetAsync("api/company.json?crtfc_key="+crtfc_key+"&corp_code="+corp_code);

                if (httpResponse.IsSuccessStatusCode){
                    string responseString = await httpResponse.Content.ReadAsStringAsync();
                    // CorporationInfo corporationInfo = JsonSerializer.Deserialize<CorporationInfo>(responseString);
                    response.Add(new JProperty("corpInfo", responseString));
                } 
                response.Add(await GetFinancialStatements(corp_code, crtfc_key, fs_div));
            }catch(HttpRequestException ex){
                throw new HttpRequestException(ex.Message);
            }

            return response;
        }

        public async Task<JProperty> GetFinancialStatements(string corp_code, string crtfc_key, string fs_div){
            // request를 보내야하는 year/quarter를 Helper로 만들었으니 가져와서 하나씩 request를 보내고, 그 값을 JArray에 담아서 finacialStatements라는 key로 JObject를 return한다.
            DateHelper dateHelper = new DateHelper();
            JArray jArrayForRequest = dateHelper.GetQuartersForRequest();
            JArray responses = new JArray();

            foreach(JObject jObject in jArrayForRequest){
                HttpResponseMessage httpResponse = await _httpClient
                    .GetAsync("api/fnlttSinglAcntAll.json?crtfc_key="+crtfc_key+"&corp_code="+corp_code+"&bsns_year="+jObject.SelectToken("Year")+
                               "&reprt_code="+jObject.SelectToken("Quarter")+"&fs_div="+fs_div);

                if(httpResponse.IsSuccessStatusCode){
                    string responseString = await httpResponse.Content.ReadAsStringAsync();
                    responses.Add(responseString);
                }
            }
            return new JProperty("financialStatements", responses);
        }
    }
}
