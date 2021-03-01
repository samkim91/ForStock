using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazor.IndexedDB.Framework;
using ForStock.Client.Common;
using ForStock.Client.Models;

namespace ForStock.Client.ViewModels
{
    public class VisualizationViewModel : IVisualizationViewModel
    {
        private IIndexedDbFactory _dbFactory { get; set; }
        private readonly List<string> QuartersForXvalues = new List<string>{"Q1", "Q2", "Q3", "Q4"};
        public List<ChartDataModel> ChartDataModels { get; set; } = new List<ChartDataModel>();
        public ChartDataModel Revenue { get; set; } = new ChartDataModel();
        public ChartDataModel GrossProfit { get; set; } = new ChartDataModel();
        public ChartDataModel OperatingIncomeLoss { get; set; } = new ChartDataModel();
        public ChartDataModel ProfitLoss { get; set; } = new ChartDataModel();
        public ChartDataModel CostOfSales { get; set; } = new ChartDataModel();
        public ChartDataModel SellingAndAdminExpenses { get; set; } = new ChartDataModel();
        public ChartParameter YearPerformance {get;set;} = new ChartParameter("YearPerformance", "연간 실적");
        public ChartParameter YearCost {get;set;} = new ChartParameter("YearCost", "연간 비용");
        public ChartParameter QuarterPerformance { get; set; } = new ChartParameter("QuarterPerformance", "분기 실적");
        public ChartParameter QuarterCost { get; set; } = new ChartParameter("QuarterCost", "분기 비용");
        public ChartParameter GroupedQuarterRevenue { get; set; } = new ChartParameter("GroupedQuarterRevenue", "분기별 매출액");
        public ChartParameter GroupedQuarterOperationIncomeLoss { get; set; } = new ChartParameter("GroupedQuarterOperationIncomeLoss", "분기별 영업이익");

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

            // 매출 총이익 grossProfit
            GrossProfit = ChartDataModels.Single(c => c.Id == "grossProfit");

            // 영업이익 operatingIncomeLoss
            OperatingIncomeLoss = ChartDataModels.Single(c => c.Id == "operatingIncomeLoss");

            // 매출 원가 costOfSales
            CostOfSales = ChartDataModels.Single(c => c.Id == "costOfSales");

            // 당기 순이익(손실) profitLoss
            ProfitLoss = ChartDataModels.Single(c => c.Id == "profitLoss");

            // 판매비와 관리비 sellingAndAdminExpenses
            SellingAndAdminExpenses = ChartDataModels.Single(c => c.Id == "sellingAndAdminExpenses");
        }

        public void setChartData(){
            // 연간 실적
            YearPerformance.Xvalues = Revenue.Years;
            YearPerformance.DataSets.Clear();
            YearPerformance.DataSets.Add(new DataSet(Revenue.YearAmounts, Revenue.Message, "bar", 4, "rgba(255, 0, 0, 0.5)", "rgba(0, 0, 0, 0)"));
            YearPerformance.DataSets.Add(new DataSet(GrossProfit.YearAmounts, GrossProfit.Message, "line", 3, "rgba(0, 0, 0, 0)", "rgba(0, 0, 255)"));
            YearPerformance.DataSets.Add(new DataSet(OperatingIncomeLoss.YearAmounts, OperatingIncomeLoss.Message, "line", 2, "rgba(0, 0, 0, 0)", "rgba(0, 75, 0)"));
            YearPerformance.DataSets.Add(new DataSet(ProfitLoss.YearAmounts, ProfitLoss.Message, "line", 1, "rgba(0, 0, 0, 0)", "rgba(255, 255, 0)"));

            // 연간 비용
            YearCost.Xvalues = Revenue.Years;
            YearCost.DataSets.Clear();
            YearCost.DataSets.Add(new DataSet(Revenue.YearAmounts, Revenue.Message, "bar", 3, "rgba(255, 0, 0, 0.5)", "rgba(0, 0, 0, 0)"));
            YearCost.DataSets.Add(new DataSet(CostOfSales.YearAmounts, CostOfSales.Message, "line", 2, "rgba(0, 0, 255, 0)", "rgba(0, 0, 255)"));
            YearCost.DataSets.Add(new DataSet(SellingAndAdminExpenses.YearAmounts, SellingAndAdminExpenses.Message, "line", 1, "rgba(0, 0, 0, 0)", "rgba(0, 100, 0)"));


            // 분기 실적
            QuarterPerformance.Xvalues = Revenue.YearAndQuarters;
            QuarterPerformance.DataSets.Clear();
            QuarterPerformance.DataSets.Add(new DataSet(Revenue.QuarterAmounts, Revenue.Message, "bar", 4, "rgba(255, 0, 0, 0.5)", "rgba(0, 0, 0, 0)"));
            QuarterPerformance.DataSets.Add(new DataSet(GrossProfit.QuarterAmounts, GrossProfit.Message, "line", 3, "rgba(0, 0, 255, 0)", "rgba(0, 0, 255)"));
            QuarterPerformance.DataSets.Add(new DataSet(OperatingIncomeLoss.QuarterAmounts, OperatingIncomeLoss.Message, "line", 2, "rgba(0, 0, 0, 0)", "rgba(0, 100, 0)"));
            QuarterPerformance.DataSets.Add(new DataSet(ProfitLoss.QuarterAmounts, ProfitLoss.Message, "line", 1, "rgba(0, 0, 0, 0)", "rgba(255, 255, 0)"));

            // 분기 비용
            QuarterCost.Xvalues = Revenue.YearAndQuarters;
            QuarterCost.DataSets.Clear();
            QuarterCost.DataSets.Add(new DataSet(Revenue.QuarterAmounts, Revenue.Message, "bar", 3, "rgba(255, 0, 0, 0.5)", "rgba(0, 0, 0, 0)"));
            QuarterCost.DataSets.Add(new DataSet(CostOfSales.QuarterAmounts, CostOfSales.Message, "line", 2, "rgba(0, 0, 0, 0)", "rgba(0, 0, 255)"));
            QuarterCost.DataSets.Add(new DataSet(SellingAndAdminExpenses.QuarterAmounts, SellingAndAdminExpenses.Message, "line", 1, "rgba(0, 0, 0, 0)", "rgba(0, 100, 0)"));

            // 분기그룹 매출액
            GroupedQuarterRevenue.Xvalues = QuartersForXvalues;
            GroupedQuarterRevenue.DataSets.Clear();
            for (int i = 0; i < Revenue.AmountsGroupByYear.Count; i++)
            {
                IGrouping<string, ChartDataSet> GCDS = Revenue.AmountsGroupByYear[i];

                List<string> data = GCDS.Select(c => c.Amount).ToList();
                string label = GCDS.First().Year;
                string backGroundColor = ColorHelper(i);

                GroupedQuarterRevenue.DataSets.Add(new DataSet(data, label, "bar", i, backGroundColor, "rgba(0, 0, 0, 0)"));
            }

            // 분기그룹 영업이익
            GroupedQuarterOperationIncomeLoss.Xvalues = QuartersForXvalues;
            GroupedQuarterOperationIncomeLoss.DataSets.Clear();
            for (int i = 0; i < OperatingIncomeLoss.AmountsGroupByYear.Count; i++)
            {
                IGrouping<string, ChartDataSet> GCDS = OperatingIncomeLoss.AmountsGroupByYear[i];

                List<string> data = GCDS.Select(c => c.Amount).ToList();
                string label = GCDS.First().Year;
                string backGroundColor = ColorHelper(i);

                GroupedQuarterOperationIncomeLoss.DataSets.Add(new DataSet(data, label, "bar", i, backGroundColor, "rgba(0, 0, 0, 0)"));
            }
        }

        public string ColorHelper(int index){
            switch(index){
                case 0:
                return "rgba(255, 0, 0, 0.5)";
                case 1:
                return "rgba(0, 0, 255, 0.5)";
                case 2:
                return "rgba(0, 100, 0, 0.5)";
                case 3:
                return "rgba(255, 255, 0, 0.5)";
                default:
                return "rgba(50, 50, 50, 0.5)";
            }
        }

    }
}