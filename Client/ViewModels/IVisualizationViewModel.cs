using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazor.IndexedDB.Framework;
using ForStock.Client.Common;
using ForStock.Client.Models;
using ForStock.Shared.Model;

namespace ForStock.Client.ViewModels
{
    public interface IVisualizationViewModel
    {
        public List<ChartDataModel> ChartDataModels { get; set; }
        public ChartDataModel Revenue { get; set; }
        public ChartDataModel GrossProfit { get; set; }
        public ChartDataModel OperatingIncomeLoss { get; set; }
        public ChartDataModel ProfitLoss { get; set; }
        public ChartDataModel CostOfSales { get; set; }
        public ChartDataModel SellingAndAdminExpenses { get; set; }
        public Task Init();

        // public void onLoadClick();
    }
}