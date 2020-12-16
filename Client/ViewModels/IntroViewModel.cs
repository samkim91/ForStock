using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ForStock.Shared.Model;

namespace ForStock.Client.ViewModels
{
    public class IntroViewModel : IIntroViewModel
    {
        public string api_key { get; set; } = "082911aaa403b82f86c069cc8034b9ca5cd90a92";
        public string stock_code { get; set; }
        public string corp_code { get; set; }
        public string corp_name { get; set; }
        public string corp_name_eng { get; set; }
        public string ceo_nm { get; set; }
        public string corp_cls { get; set; }
        public string jurir_no { get; set; }
        public string bizr_no { get; set; }
        public string adres { get; set; }
        public string hm_url { get; set; }
        public string ir_url { get; set; }
        public string phn_no { get; set; }
        public string fax_no { get; set; }
        public string induty_code { get; set; }
        public string est_dt { get; set; }
        public string acc_mt { get; set; }

        private HttpClient _httpClient;

        public IntroViewModel()
        {

        }

        public IntroViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task UpdateOnclick()
        {
            CorporationInfo corporationInfo = await _httpClient.GetFromJsonAsync<CorporationInfo>("corporation/info/"+stock_code+"/"+api_key);

            // 기업 정보를 API로 불러오는 코드 필요
            // HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "company.json?crtfc_key="+ApiKey+"&corp_code="+CorpCode);
            // HttpClient httpClient = _httpClientFactory.CreateClient("DartAPI");

            // HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage);
            // string result = await httpClient.GetFromJsonAsync<string>("company.json?crtfc_key="+ApiKey+"&corp_code="+CorpCode);
            // TODO.. CORS 에러가 발생하고 있다.. Services에 AddCors로 해결할 수 있는 것으로 보인다.
            // 결국... 서버를 통해서 정보를 가져오는게 가장 좋다 ... 쑤까불럇
        }
    }
}
