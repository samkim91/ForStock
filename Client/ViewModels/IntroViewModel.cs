using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ForStock.Shared.Model;

namespace ForStock.Client.ViewModels
{
    public class IntroViewModel : IIntroViewModel
    {
        public CorporationInfo corporationInfo { get; set; } = new CorporationInfo();
        public string crtfc_key { get; set; } = "082911aaa403b82f86c069cc8034b9ca5cd90a92";
        public string stock_code { get; set; } = "005930";
        public string fs_div { get; set; } = "OFS";
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
            // Client로부터 입력된 stock_code로 Corporation의 code(Primary Key)를 찾고, 이 corp_code를 이용해서 corp info와 재무제표를 가져온다.
            // parameters : stock_code, api_key, fs_div
            corporationInfo = await _httpClient.GetFromJsonAsync<CorporationInfo>("corporation/info/" + stock_code + "/" + crtfc_key + "/" + fs_div);

            // Dart API로 받아온 corporation info를 viewModel에 set 해줌
            BindCorpInfoToView(corporationInfo);
            
            // corporation info 와 를 indexedDB에 저장함.




            // 기업 정보를 API로 불러오는 코드 필요
            // HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "company.json?crtfc_key="+ApiKey+"&corp_code="+CorpCode);
            // HttpClient httpClient = _httpClientFactory.CreateClient("DartAPI");

            // HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage);
            // string result = await httpClient.GetFromJsonAsync<string>("company.json?crtfc_key="+ApiKey+"&corp_code="+CorpCode);
            // TODO.. CORS 에러가 발생하고 있다.. Services에 AddCors로 해결할 수 있는 것으로 보인다.
            // 결국... 서버를 통해서 정보를 가져오는게 가장 좋다 ... 쑤까불럇
        }

        private void BindCorpInfoToView(CorporationInfo corporationInfo)
        {
            this.corporationInfo.corp_code = corporationInfo.corp_code;
            this.corporationInfo.corp_code = corporationInfo.corp_code;
            this.corporationInfo.corp_name = corporationInfo.corp_name;
            this.corporationInfo.corp_name_eng = corporationInfo.corp_name_eng;
            this.corporationInfo.ceo_nm = corporationInfo.ceo_nm;
            this.corporationInfo.corp_cls = corporationInfo.corp_cls;
            this.corporationInfo.jurir_no = corporationInfo.jurir_no;
            this.corporationInfo.bizr_no = corporationInfo.bizr_no;
            this.corporationInfo.adres = corporationInfo.adres;
            this.corporationInfo.hm_url = corporationInfo.hm_url;
            this.corporationInfo.ir_url = corporationInfo.ir_url;
            this.corporationInfo.phn_no = corporationInfo.phn_no;
            this.corporationInfo.fax_no = corporationInfo.fax_no;
            this.corporationInfo.induty_code = corporationInfo.induty_code;
            this.corporationInfo.est_dt = corporationInfo.est_dt;
            this.corporationInfo.acc_mt = corporationInfo.acc_mt;
        }
    }
}
