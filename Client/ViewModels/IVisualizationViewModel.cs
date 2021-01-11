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
        public List<FinancialStatement> FinancialStatements { get; set; }

        public ChartData dataForRevenue { get; set; }

        public Task Init();
    }
}