using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazor.IndexedDB.Framework;
using ForStock.Client.Common;
using ForStock.Client.Models;
using ForStock.Shared.Model;

namespace ForStock.Client.ViewModels
{
    public class VisualizationViewModel
    {
        private IIndexedDbFactory _dbFactory { get; set; }
        private List<FinancialStatement> FinancialStatements { get; set; }
        public VisualizationViewModel() { }
        public VisualizationViewModel(IIndexedDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task Init()
        {
            // indexed DB에서 데이터를 받아온다.
            using (MyIndexedDB db = await this._dbFactory.Create<MyIndexedDB>())
            {
                if (db.FinancialStatement.Any())
                {
                    FinancialStatements = (List<FinancialStatement>)db.FinancialStatement.Select(row => row);
                }
            }
            // 받아온 데이터를 각각의 분류(message field)에 따라서 필요한 데이터를 빼온다.
            MakeUsefulData();
        }

        public void MakeUsefulData()
        {
            // 필요한 데이터를 연간OO, 분기OO 등으로 정리한다.
            FinancialStatement revenue = FinancialStatements.Single(fs => fs.id == "revenue");
            List<ChartDataModel> revenues = (from fi in revenue.list select new ChartDataModel() { Year = fi.bsns_year, Quarter = fi.reprt_code, Amount = fi.thstrm_amount }).ToList();

            ChartData dataForRevenue = new ChartData(revenues);
        }

    }

    
}