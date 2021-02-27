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
        public ChartParameter YearPerformance { get; set; }
        public ChartParameter YearCost { get; set; }
        public ChartParameter QuarterPerformance { get; set; }
        public ChartParameter QuarterCost { get; set; }
        public ChartParameter GroupedQuarterRevenue { get; set; }
        public ChartParameter GroupedQuarterOperationIncomeLoss { get; set; }
        public Task Init();

        // public void onLoadClick();
    }
}