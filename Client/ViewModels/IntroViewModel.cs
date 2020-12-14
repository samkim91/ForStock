using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ForStock.Shared.Model;

namespace ForStock.Client.ViewModels
{
    public class IntroViewModel : IIntroViewModel
    {
        public string ApiKey { get; set; } = "082911aaa403b82f86c069cc8034b9ca5cd90a92";
        public string StockCode { get; set; }
        public string CorpCode { get; set; }
        public string CorpName { get; set; }
        public string CorpNameEng { get; set; }
        public string CeoName { get; set; }
        public string CorpClass { get; set; }
        public string CorpNumber { get; set; }
        public string BusinessNumber { get; set; }
        public string Address { get; set; }
        public string Homepage { get; set; }
        public string IrHomepage { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string SectorCode { get; set; }
        public string EstablishDate { get; set; }
        public string AccountMonth { get; set; }

        private HttpClient _httpClient;

        private IHttpClientFactory _httpClientFactory;

        public IntroViewModel()
        {

        }

        public IntroViewModel(HttpClient httpClient, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;
        }

        public async Task UpdateOnclick()
        {
            CorpCode = await _httpClient.GetFromJsonAsync<string>("corporation/info/" + StockCode);

            // 기업 정보를 API로 불러오는 코드 필요
            // HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "company.json?crtfc_key="+ApiKey+"&corp_code="+CorpCode);
            HttpClient httpClient = _httpClientFactory.CreateClient("DartAPI");

            // HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage);
            string result = await httpClient.GetFromJsonAsync<string>("company.json?crtfc_key="+ApiKey+"&corp_code="+CorpCode);
            // TODO.. CORS 에러가 발생하고 있다.. Services에 AddCors로 해결할 수 있는 것으로 보인다.
            
        }
    }
}
