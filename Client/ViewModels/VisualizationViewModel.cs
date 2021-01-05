using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazor.IndexedDB.Framework;
using ForStock.Client.Common;
using ForStock.Shared.Model;

namespace ForStock.Client.ViewModels
{
    public class VisualizationViewModel
    {
        private IIndexedDbFactory _dbFactory { get; set; }
        private List<FinancialStatement> FinancialStatements { get; set; }
        public VisualizationViewModel(){ }
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
                    FinancialStatements = (List<FinancialStatement>) db.FinancialStatement.Select(row => row);
                }
            }
            // 받아온 데이터를 각각의 분류(message field)에 따라서 필요한 데이터를 빼온다.
            MakeUsefulData();
        }
        
        public void MakeUsefulData(){
            // 필요한 데이터를 연간OO, 분기OO 등으로 정리한다.
            

        }

        // 정리된 데이터를 blazor chart.js로 넘긴다.
    }
}