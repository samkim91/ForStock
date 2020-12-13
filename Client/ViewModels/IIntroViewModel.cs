using System.Threading.Tasks;

namespace ForStock.Client.ViewModels
{
    public interface IIntroViewModel
    {
        public string ApiKey { get; set; }
        public string StockCode { get; set; }
        public string CorpCode { get; set; }

        public Task UpdateOnclick();
    }
}