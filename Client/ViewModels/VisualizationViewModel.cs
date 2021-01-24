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
        public List<FinancialStatement> FinancialStatements { get; set; } = new List<FinancialStatement>();
        public ChartData dataForRevenue { get; set; } = new ChartData();
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
                if (!db.FinancialStatement.Any())
                {
                    return;
                }
                FinancialStatements = db.FinancialStatement.Select(row => row).ToList<FinancialStatement>();
            }
            // 받아온 데이터를 각각의 분류(message field)에 따라서 필요한 데이터를 빼온다.
            MakeUsefulData();
        }

        public void MakeUsefulData()
        {
            // 필요한 데이터를 연간OO, 분기OO 등으로 정리한다.
            FinancialStatement revenue = FinancialStatements.Single(fs => fs.id == "revenue");

            List<ChartDataModel> revenues =
                (from fi in revenue.list
                 where fi != null
                 select new ChartDataModel() { Year = fi.bsns_year, Quarter = fi.reprt_code, Amount = fi.thstrm_amount }).ToList();

            // 4분기 보고서의 매출액은 연간 매출액이기에, 각 분기의 매출액을 빼서 4분기 매출액을 구한다.
            for (int i = 0; i < revenues.Count(); i++)
            {
                if (revenues[i].Quarter == "11011")
                {
                    if(revenues[i+1].Amount != null && revenues[i+2].Amount  != null && revenues[i+3].Amount != null){
                        revenues[i].Amount = (Convert.ToInt64(revenues[i].Amount) - Convert.ToInt64(revenues[i + 1].Amount) - Convert.ToInt64(revenues[i + 2].Amount) - Convert.ToInt64(revenues[i + 3].Amount)).ToString();
                    }
                }
            }
            this.dataForRevenue = new ChartData(revenues);
        }
    }
}