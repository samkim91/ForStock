using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ForStock.Server.Common;
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
        
        public string fs_view {get; set;}

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
            string response = await _httpClient.GetStringAsync("corporation/info/" + crtfc_key + "/" + stock_code);

            JObject jObject = JObject.Parse(response);
            CorporationInfo corporationInfo = jObject.ToObject<CorporationInfo>();

            // Dart API로 받아온 corporation info를 viewModel에 set 해줌
            BindCorpInfoToView(corporationInfo);

            GetFinancialStatements();
            // corporation info 와 를 indexedDB에 저장함.
        }

        private async void GetFinancialStatements(){
            DateHelper dateHelper = new DateHelper();
            JArray jArrayForRequest = dateHelper.GetQuartersForRequest();
            List<FinacialStatement> finacialStatements = new List<FinacialStatement>();

            foreach(JObject jObject in jArrayForRequest){
                string response = await _httpClient.GetStringAsync("corporation/financialstatement/" + crtfc_key + "/" + stock_code + "/" + jObject.SelectToken("Year") +
                                         "/" + jObject.SelectToken("Quarter") + "/" + fs_div);

                JObject jObject1 = JObject.Parse(response);
                FinacialStatement finacialStatement = jObject1.ToObject<FinacialStatement>();
                finacialStatement.id = jObject.SelectToken("Year") + "/" + jObject.SelectToken("Quarter");
                finacialStatement.year = jObject.SelectToken("Year").ToString();
                finacialStatement.quarter = jObject.SelectToken("Quarter").ToString();
                finacialStatements.Add(finacialStatement);
            }

            MakeFinancialStatementsToMyData(finacialStatements);
        }

        private void MakeFinancialStatementsToMyData(List<FinacialStatement> finacialStatements){
            // finacialStatements에서 매출액만 뽑는다.
            FinacialStatement revenue = new FinacialStatement("Revenue", "매출액");
            FinacialStatement grossProfit = new FinacialStatement("grossProfit", "매출총이익");
            FinacialStatement operatingIncomeLoss = new FinacialStatement("operatingIncomeLoss", "영업이익");
            FinacialStatement profitLoss = new FinacialStatement("profitLoss", "당기순이익(손실)");
            
            foreach(FinacialStatement finacialStatement in finacialStatements){
                revenue.list.Add(finacialStatement.list.Find(key => key.account_nm == "수익(매출액)"));
                grossProfit.list.Add(finacialStatement.list.Find(key => key.account_nm == "매출총이익"));
                // 영업이익 부분이 제대로 불러와지지 않고 있다. 이부분은 확인해봐야한다.
                operatingIncomeLoss.list.Add(finacialStatement.list.Find(key => key.account_nm == "영업이익" || key.account_nm == "영업이익(손실)"));
                profitLoss.list.Add(finacialStatement.list.Find(key => key.account_nm == "당기순이익(손실)"));
            }
            string stop = null;
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
