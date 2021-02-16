using System;
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

        public IntroViewModel() { }

        public IntroViewModel(HttpClient httpClient, IIndexedDbFactory dbFactory)
        {
            _httpClient = httpClient;
            _dbFactory = dbFactory;
        }

        public async Task GetInitInfoFromDb()
        {
            // DB를 확인해서 값이 있으면 초기값을 불러와서 set 해준다.
            using (MyIndexedDB db = await this._dbFactory.Create<MyIndexedDB>())
            {
                if (db.IntroModel.Any())
                {
                    introModel = db.IntroModel.First();
                }

                if (db.CorporationInfo.Any())
                {
                    corporationInfo = db.CorporationInfo.First();
                }
            }
        }

        public async Task GetDataOnclick()
        {

            // Client로부터 입력된 stock_code로 Corporation의 code(Primary Key)를 찾고, 이 corp_code를 이용해서 corp info를 가져온다.
            // parameters : api_key, stock_code, fs_div
            // response : financialstatements
            List<FinancialStatement> financialStatements = await _httpClient.GetFromJsonAsync<List<FinancialStatement>>("corporation/financialstatement/"
                                                                 + introModel.crtfc_key + "/" + introModel.stock_code + "/" + introModel.fs_div);

            // Client로부터 입력된 stock_code로 Corporation의 code(Primary Key)를 찾고, 이 corp_code를 이용해서 corp info를 가져온다.
            // parameters : api_key, stock_code
            // response : corpInfo
            CorporationInfo corporationInfo = await _httpClient.GetFromJsonAsync<CorporationInfo>("corporation/info/" + introModel.crtfc_key + "/" + introModel.stock_code);

            // Http response에 대한 검증을 함.
            if (!financialStatements.Any(fs => fs.message == "정상") || corporationInfo.message != "정상")
            {
                IsRequestSuccessful = false;

                if (!financialStatements.Any(fs => fs.message == "정상"))
                {
                    Message = "Message from Financial statement : " + financialStatements.FirstOrDefault(fs => fs.message != "정상").message;
                }
                else if (corporationInfo.message != "정상")
                {
                    Message = "Message from Corporation info : " + corporationInfo.message;
                }

                return;
            }

            MakeFinancialStatementsToMyData(financialStatements);
            bindCorpInfoToView(corporationInfo);
            saveIntroModelToIndexedDb(introModel);
            saveCorpInfoToIndexedDb(corporationInfo);
        }

        // Dart API로 받아온 financial statements를 필요한 data로 변환해서 정리함.
        private void MakeFinancialStatementsToMyData(List<FinancialStatement> financialStatements)
        {
            // indexed db에 저장할 models를 담는 container
            List<ChartDataModel> chartDataModels = new List<ChartDataModel>();

            // indexed db에 저장하기 위한 models
            ChartDataModel revenue = new ChartDataModel("revenue", "매출액");
            ChartDataModel grossProfit = new ChartDataModel("grossProfit", "매출총이익");
            ChartDataModel operatingIncomeLoss = new ChartDataModel("operatingIncomeLoss", "영업이익");
            ChartDataModel profitLoss = new ChartDataModel("profitLoss", "당기순이익(손실)");
            ChartDataModel costOfSales = new ChartDataModel("costOfSales", "매출원가");
            ChartDataModel sellingAndAdminExpenses = new ChartDataModel("sellingAndAdminExpenses", "판매비와관리비");

            // FinancialStatements에서 visualization을 위해 필요한 data를 뽑는다.
            foreach (FinancialStatement financialStatement in financialStatements)
            {
                revenue.DataSets.AddRange(from list in financialStatement.list
                                          where list != null && list.account_nm == "수익(매출액)" || list.account_nm == "매출액"
                                          select new ChartDataSet(list.bsns_year, list.reprt_code, list.thstrm_amount));
                grossProfit.DataSets.AddRange(from list in financialStatement.list
                                              where list != null && list.account_nm == "매출총이익"
                                              select new ChartDataSet(list.bsns_year, list.reprt_code, list.thstrm_amount));
                operatingIncomeLoss.DataSets.AddRange(from list in financialStatement.list
                                                      where list != null && (list.account_nm == "영업이익" || list.account_nm == "영업이익(손실)")
                                                      select new ChartDataSet(list.bsns_year, list.reprt_code, list.thstrm_amount));
                profitLoss.DataSets.AddRange(from list in financialStatement.list
                                              where list != null && list.account_nm == "당기순이익(손실)" && list.sj_nm == "손익계산서"
                                              select new ChartDataSet(list.bsns_year, list.reprt_code, list.thstrm_amount));
                costOfSales.DataSets.AddRange(from list in financialStatement.list
                                              where list != null && list.account_nm == "매출원가"
                                              select new ChartDataSet(list.bsns_year, list.reprt_code, list.thstrm_amount));
                sellingAndAdminExpenses.DataSets.AddRange(from list in financialStatement.list
                                                          where list != null && list.account_nm == "판매비와관리비"
                                                          select new ChartDataSet(list.bsns_year, list.reprt_code, list.thstrm_amount));
            }
            chartDataModels.Add(revenue);
            chartDataModels.Add(grossProfit);
            chartDataModels.Add(operatingIncomeLoss);
            chartDataModels.Add(profitLoss);
            chartDataModels.Add(costOfSales);
            chartDataModels.Add(sellingAndAdminExpenses);

            makeFourthQuarterData(chartDataModels);
            saveChartDataModelToIndexedDb(chartDataModels);
        }

        private void makeFourthQuarterData(List<ChartDataModel> chartDataModels)
        {
            // 4분기 보고서의 매출액은 연간 매출액이기에, 각 분기의 매출액을 빼서 4분기 매출액을 구한다.
            foreach (ChartDataModel chartDataModel in chartDataModels)
            {
                for (int i = 0; i < chartDataModel.DataSets.Count(); i++)
                {
                    if (chartDataModel.DataSets[i].Quarter == "Q4")
                    {
                        if (chartDataModel.DataSets[i + 1].Amount != null && chartDataModel.DataSets[i + 2].Amount != null && chartDataModel.DataSets[i + 3].Amount != null)
                        {
                            chartDataModel.DataSets[i].Amount = (Convert.ToInt64(chartDataModel.DataSets[i].Amount) - Convert.ToInt64(chartDataModel.DataSets[i + 1].Amount) - Convert.ToInt64(chartDataModel.DataSets[i + 2].Amount) - Convert.ToInt64(chartDataModel.DataSets[i + 3].Amount)).ToString();
                        }
                    }
                }
                // 정리가 끝나면 year > quarter 순으로 바꾼다.
                chartDataModel.DataSets.Reverse();
            }
        }

        // Dart API로 받아온 corporation info를 viewModel에 set함.
        private void bindCorpInfoToView(CorporationInfo corporationInfo)
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

        // 사용자가 입력한 intro data를 Indexed DB에 저장함.
        private async void saveIntroModelToIndexedDb(IntroModel introModel)
        {
            using (MyIndexedDB db = await this._dbFactory.Create<MyIndexedDB>())
            {
                db.IntroModel.Clear();
                db.IntroModel.Add(introModel);
                await db.SaveChanges();
            }
        }

        // Dart API로 받아온 corporation info를 Indexed DB에 저장함.
        private async void saveCorpInfoToIndexedDb(CorporationInfo corporationInfo)
        {
            using (MyIndexedDB db = await this._dbFactory.Create<MyIndexedDB>())
            {
                db.CorporationInfo.Clear();
                db.CorporationInfo.Add(corporationInfo);
                await db.SaveChanges();
            }
        }

        // Dart API로 받고 정리한, Financial statements info를 Indexed DB에 저장함.
        private async void saveChartDataModelToIndexedDb(List<ChartDataModel> chartDataModels)
        {
            using (MyIndexedDB db = await this._dbFactory.Create<MyIndexedDB>())
            {
                db.ChartDataModel.Clear();
                foreach (ChartDataModel chartDataModel in chartDataModels)
                {
                    db.ChartDataModel.Add(chartDataModel);
                }
                await db.SaveChanges();
            }
        }
    }
}
