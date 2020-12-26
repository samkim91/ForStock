using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ForStock.Shared.Model;
using Newtonsoft.Json.Linq;

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
            // response : corpInfo, financialStatements
            string response = await _httpClient.GetStringAsync("corporation/info/" + stock_code + "/" + crtfc_key + "/" + fs_div);
            
            JObject jObject = new JObject(response);

            JToken corpInfoObject = jObject["corpInfo"];
            JToken financialStatementsObject = jObject["financialStatements"];

            // corporation info 와 를 indexedDB에 저장함.

            // Dart API로 받아온 corporation info를 viewModel에 set 해줌
            CorporationInfo corporationInfo = corpInfoObject.ToObject<CorporationInfo>();
            BindCorpInfoToView(corporationInfo);

            // financialStatements에서 필요한 값들만 모아놓는 List<FinancialStatement>를 만들어서 이를 저장하고 사용하자.
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
