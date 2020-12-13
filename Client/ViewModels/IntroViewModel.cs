using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ForStock.Shared.Model;

namespace ForStock.Client.ViewModels
{
    public class IntroViewModel : IIntroViewModel
    {
        public string ApiKey { get; set; }
        public string StockCode { get; set; }
        public string CorpCode { get; set; }

        public CorporationInfo corporationInfo = new CorporationInfo();

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
            corporationInfo = await _httpClient.GetFromJsonAsync<CorporationInfo>("corporation/getintro/" + StockCode);
        }
    }
}
