using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Blazor.IndexedDB.Framework;
using ForStock.Client.Common;
using ForStock.Client.Models;
using ForStock.Shared.Model;

namespace ForStock.Client.ViewModels
{
    public class VisualizationViewModel : IVisualizationViewModel
    {
        private IIndexedDbFactory _dbFactory { get; set; }
        public List<ChartDataModel> ChartDataModels { get; set; } = new List<ChartDataModel>();
        public ChartDataModel Revenue { get; set; } = new ChartDataModel();
        public ChartDataModel GrossProfit { get; set; } = new ChartDataModel();
        public ChartDataModel OperatingIncomeLoss { get; set; } = new ChartDataModel();
        public ChartDataModel ProfitLoss { get; set; } = new ChartDataModel();
        public ChartDataModel CostOfSales { get; set; } = new ChartDataModel();
        public ChartDataModel SellingAndAdminExpenses { get; set; } = new ChartDataModel();
        public ChartParameter QuarterPerformance { get; set; } = new ChartParameter("QuarterPerformance", "분기 실적");

        public VisualizationViewModel() { }
        public VisualizationViewModel(IIndexedDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task Init()
        {
            // indexed DB에서 데이터를 받아온다.
            await this._dbFactory.Create<MyIndexedDB>();

            using (MyIndexedDB db = await this._dbFactory.Create<MyIndexedDB>())
            {
                if (!db.ChartDataModel.Any())
                {
                    return;
                }
                ChartDataModels = db.ChartDataModel.Select(row => row).ToList<ChartDataModel>();
            }
            // 받아온 데이터를 각각의 분류(message field)에 따라서 필요한 데이터를 빼온다.
            getEachData();
            setChartData();
        }

        public void getEachData()
        {
            // 매출액 revenue
            Revenue = ChartDataModels.Single(c => c.Id == "revenue");
            Revenue.DataSets.Reverse();

            // 매출 총이익 grossProfit
            GrossProfit = ChartDataModels.Single(c => c.Id == "grossProfit");
            GrossProfit.DataSets.Reverse();

            // 영업이익 operatingIncomeLoss
            OperatingIncomeLoss = ChartDataModels.Single(c => c.Id == "operatingIncomeLoss");
            OperatingIncomeLoss.DataSets.Reverse();

            // 매출 원가 costOfSales
            CostOfSales = ChartDataModels.Single(c => c.Id == "costOfSales");
            CostOfSales.DataSets.Reverse();

            // 당기 순이익(손실) profitLoss
            ProfitLoss = ChartDataModels.Single(c => c.Id == "profitLoss");
            ProfitLoss.DataSets.Reverse();

            // 판매비와 관리비 sellingAndAdminExpenses
            SellingAndAdminExpenses = ChartDataModels.Single(c => c.Id == "sellingAndAdminExpenses");
            SellingAndAdminExpenses.DataSets.Reverse();
        }

        public void setChartData(){
            QuarterPerformance.Xvalues = Revenue.YearAndQuarters;
            QuarterPerformance.DataSets.Clear();
            QuarterPerformance.DataSets.Add(new DataSet(Revenue.Amounts, Revenue.Message, "bar", 3, "rgba(255, 0, 0, 0.5)"));
            QuarterPerformance.DataSets.Add(new DataSet(GrossProfit.Amounts, GrossProfit.Message, "line", 2, "rgba(0, 0, 255, 0.5)"));
            QuarterPerformance.DataSets.Add(new DataSet(OperatingIncomeLoss.Amounts, OperatingIncomeLoss.Message, "line", 1, "rgba(0, 75, 0, 0.5)"));
        }

    }
}