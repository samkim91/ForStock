using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazor.IndexedDB.Framework;
using ForStock.Client.Common;
using ForStock.Client.Models;
using ForStock.Shared.Model;

namespace ForStock.Client.ViewModels
{
    public class IntroViewModel : IIntroViewModel
    {
        public CorporationInfo corporationInfo { get; set; } = new CorporationInfo();
        public IntroModel introModel { get; set; } = new IntroModel();
        private HttpClient _httpClient { get; set; }
        private IIndexedDbFactory _dbFactory { get; set; }
        public bool IsRequestSuccessful { get; set; } = true;
        public string Message { get; set; }

        public IntroViewModel(){ }

        public IntroViewModel(HttpClient httpClient, IIndexedDbFactory dbFactory)
        {
            _httpClient = httpClient;
            _dbFactory = dbFactory;
        }
        
        public async Task GetInitInfoFromDb(){
            // DB를 확인해서 값이 있으면 초기값을 불러와서 set 해준다.
            using (MyIndexedDB db = await this._dbFactory.Create<MyIndexedDB>())
            {   
                if(db.IntroModel.Any()){
                    introModel = db.IntroModel.First();
                }

                if(db.CorporationInfo.Any()){
                    corporationInfo = db.CorporationInfo.First();
                }
            }
        }

        public async Task GetDataOnclick()
        {

            // Client로부터 입력된 stock_code로 Corporation의 code(Primary Key)를 찾고, 이 corp_code를 이용해서 corp info를 가져온다.
            // parameters : api_key, stock_code, fs_div
            // response : financialstatements
            List<FinancialStatement> FinancialStatements = await _httpClient.GetFromJsonAsync<List<FinancialStatement>>("corporation/financialstatement/"
                                                                 + introModel.crtfc_key + "/" + introModel.stock_code + "/" + introModel.fs_div);

            // Client로부터 입력된 stock_code로 Corporation의 code(Primary Key)를 찾고, 이 corp_code를 이용해서 corp info를 가져온다.
            // parameters : api_key, stock_code
            // response : corpInfo
            CorporationInfo corporationInfo = await _httpClient.GetFromJsonAsync<CorporationInfo>("corporation/info/" + introModel.crtfc_key + "/" + introModel.stock_code);

            // Http response에 대한 검증을 함.
            if (!FinancialStatements.Any(fs => fs.message == "정상") || corporationInfo.message != "정상")
            {
                IsRequestSuccessful = false;

                if (!FinancialStatements.Any(fs => fs.message == "정상"))
                {
                    Message = "Message from Financial statement : " + FinancialStatements.FirstOrDefault(fs => fs.message != "정상").message;
                }
                else if(corporationInfo.message != "정상")
                {
                    Message = "Message from Corporation info : " + corporationInfo.message;
                }

                return;
            }
            
            // Dart API로 받아온 financial statements를 필요한 data로 변환해서 정리함.
            MakeFinancialStatementsToMyData(FinancialStatements);
            // Dart API로 받아온 corporation info를 viewModel에 set함.
            BindCorpInfoToView(corporationInfo);
            // Data들을 indexed db에 저장.
            SaveIntroModelToIndexedDb(introModel);
            SaveCorpInfoToIndexedDb(corporationInfo);
        }

        private void MakeFinancialStatementsToMyData(List<FinancialStatement> FinancialStatements)
        {
            List<FinancialStatement> FinancialStatementsForSaving = new List<FinancialStatement>();
            // FinancialStatements에서 visualization을 위해 필요한 data를 뽑는다.
            FinancialStatement revenue = new FinancialStatement("revenue", "매출액");
            FinancialStatement grossProfit = new FinancialStatement("grossProfit", "매출총이익");
            FinancialStatement operatingIncomeLoss = new FinancialStatement("operatingIncomeLoss", "영업이익");
            FinancialStatement profitLoss = new FinancialStatement("profitLoss", "당기순이익(손실)");
            FinancialStatement costOfSales = new FinancialStatement("costOfSales", "매출원가");
            FinancialStatement sellingAndAdminExpenses = new FinancialStatement("sellingAndAdminExpenses", "판매비와관리비");

            foreach (FinancialStatement FinancialStatement in FinancialStatements)
            {
                revenue.list.Add(FinancialStatement.list.Find(key => key.account_nm == "수익(매출액)"));
                grossProfit.list.Add(FinancialStatement.list.Find(key => key.account_nm == "매출총이익"));
                operatingIncomeLoss.list.Add(FinancialStatement.list.Find(key => key.account_nm == "영업이익" || key.account_nm == "영업이익(손실)"));
                profitLoss.list.Add(FinancialStatement.list.Find(key => key.account_nm == "당기순이익(손실)"));
                costOfSales.list.Add(FinancialStatement.list.Find(key => key.account_nm == "매출원가"));
                sellingAndAdminExpenses.list.Add(FinancialStatement.list.Find(key => key.account_nm == "판매비와관리비"));
            }
            
            FinancialStatementsForSaving.Add(revenue);
            FinancialStatementsForSaving.Add(grossProfit);
            FinancialStatementsForSaving.Add(operatingIncomeLoss);
            FinancialStatementsForSaving.Add(profitLoss);
            FinancialStatementsForSaving.Add(costOfSales);
            FinancialStatementsForSaving.Add(sellingAndAdminExpenses);
            
            SaveFinancialStatementToIndexedDb(FinancialStatementsForSaving);
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

        private async void SaveIntroModelToIndexedDb(IntroModel introModel)
        {
            using (MyIndexedDB db = await this._dbFactory.Create<MyIndexedDB>())
            {
                db.IntroModel.Clear();
                db.IntroModel.Add(introModel);
                await db.SaveChanges();
            }
        }

        private async void SaveCorpInfoToIndexedDb(CorporationInfo corporationInfo)
        {
            using (MyIndexedDB db = await this._dbFactory.Create<MyIndexedDB>())
            {   
                db.CorporationInfo.Clear();
                db.CorporationInfo.Add(corporationInfo);
                await db.SaveChanges();
            }
        }

        private async void SaveFinancialStatementToIndexedDb(List<FinancialStatement> FinancialStatements)
        {
            using (MyIndexedDB db = await this._dbFactory.Create<MyIndexedDB>())
            {   
                db.FinancialStatement.Clear();
                foreach(FinancialStatement FinancialStatement in FinancialStatements){
                    db.FinancialStatement.Add(FinancialStatement);
                }
                await db.SaveChanges();
            }
        }
    }
}
