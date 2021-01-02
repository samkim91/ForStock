using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazor.IndexedDB.Framework;
using ForStock.Client.Common;
using ForStock.Client.Models;
using ForStock.Shared.Model;
using Microsoft.AspNetCore.Components;

namespace ForStock.Client.ViewModels
{
    public class IntroViewModel : IIntroViewModel
    {
        public CorporationInfo corporationInfo { get; set; } = new CorporationInfo();
        public IntroModel introModel { get; set; } = new IntroModel();
        private HttpClient _httpClient;
        private IIndexedDbFactory _dbFactory;
        public bool IsRequestSuccessful { get; set; } = true;
        public string Message { get; set; }

        public IntroViewModel(){ }

        public IntroViewModel(HttpClient httpClient, IIndexedDbFactory dbFactory)
        {
            _httpClient = httpClient;
            _dbFactory = dbFactory;
        }
        
        // OnInitialized를 viewModel에서 사용할 수 있는 방법을 찾아야함.
        // protected override async void OnInitialized()
        // {   
        //     using (var db = await this._dbFactory.Create<MyIndexedDB>())
        //     {
        //         var corpInfo = db.CorporationInfo.First();
        //         BindCorpInfoToView(corpInfo);
        //     }
        // }
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
            List<FinacialStatement> finacialStatements = await _httpClient.GetFromJsonAsync<List<FinacialStatement>>("corporation/financialstatement/"
                                                                 + introModel.crtfc_key + "/" + introModel.stock_code + "/" + introModel.fs_div);

            // Client로부터 입력된 stock_code로 Corporation의 code(Primary Key)를 찾고, 이 corp_code를 이용해서 corp info를 가져온다.
            // parameters : api_key, stock_code
            // response : corpInfo
            CorporationInfo corporationInfo = await _httpClient.GetFromJsonAsync<CorporationInfo>("corporation/info/" + introModel.crtfc_key + "/" + introModel.stock_code);

            // Http response에 대한 검증을 함.
            if(finacialStatements[0].status != "000" || corporationInfo.status != "000"){
                IsRequestSuccessful = false;

                if(finacialStatements[0].status != "000"){
                    Message = "Message from Financial statement : " + finacialStatements[0].message;
                }else{
                    Message = "Message from Corporation info : " + corporationInfo.message;
                }

                return;
            }
            
            // Dart API로 받아온 financial statements를 필요한 data로 변환해서 정리함.
            MakeFinancialStatementsToMyData(finacialStatements);
            // Dart API로 받아온 corporation info를 viewModel에 set함.
            BindCorpInfoToView(corporationInfo);
            // Data들을 indexed db에 저장.
            SaveIntroModelToIndexedDb(introModel);
            SaveCorpInfoToIndexedDb(corporationInfo);
        }

        private void MakeFinancialStatementsToMyData(List<FinacialStatement> finacialStatements)
        {
            List<FinacialStatement> finacialStatementsForSaving = new List<FinacialStatement>();
            // finacialStatements에서 visualization을 위해 필요한 data를 뽑는다.
            FinacialStatement revenue = new FinacialStatement("revenue", "매출액");
            FinacialStatement grossProfit = new FinacialStatement("grossProfit", "매출총이익");
            FinacialStatement operatingIncomeLoss = new FinacialStatement("operatingIncomeLoss", "영업이익");
            FinacialStatement profitLoss = new FinacialStatement("profitLoss", "당기순이익(손실)");
            FinacialStatement costOfSales = new FinacialStatement("costOfSales", "매출원가");
            FinacialStatement sellingAndAdminExpenses = new FinacialStatement("sellingAndAdminExpenses", "판매비와관리비");

            foreach (FinacialStatement finacialStatement in finacialStatements)
            {
                revenue.list.Add(finacialStatement.list.Find(key => key.account_nm == "수익(매출액)"));
                grossProfit.list.Add(finacialStatement.list.Find(key => key.account_nm == "매출총이익"));
                operatingIncomeLoss.list.Add(finacialStatement.list.Find(key => key.account_nm == "영업이익" || key.account_nm == "영업이익(손실)"));
                profitLoss.list.Add(finacialStatement.list.Find(key => key.account_nm == "당기순이익(손실)"));
                costOfSales.list.Add(finacialStatement.list.Find(key => key.account_nm == "매출원가"));
                sellingAndAdminExpenses.list.Add(finacialStatement.list.Find(key => key.account_nm == "판매비와관리비"));
            }
            
            finacialStatementsForSaving.Add(revenue);
            finacialStatementsForSaving.Add(grossProfit);
            finacialStatementsForSaving.Add(operatingIncomeLoss);
            finacialStatementsForSaving.Add(profitLoss);
            finacialStatementsForSaving.Add(costOfSales);
            finacialStatementsForSaving.Add(sellingAndAdminExpenses);
            
            SaveFinacialStatementToIndexedDb(finacialStatementsForSaving);
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

        private async void SaveFinacialStatementToIndexedDb(List<FinacialStatement> finacialStatements)
        {
            using (MyIndexedDB db = await this._dbFactory.Create<MyIndexedDB>())
            {   
                db.FinacialStatement.Clear();
                foreach(FinacialStatement finacialStatement in finacialStatements){
                    db.FinacialStatement.Add(finacialStatement);
                }
                await db.SaveChanges();
            }
        }
    }
}
