using System.ComponentModel;
using System.Threading.Tasks;
using ForStock.Shared.Model;

namespace ForStock.Client.ViewModels
{
    public interface IIntroViewModel
    {
        public CorporationInfo corporationInfo { get; set; }
        public string crtfc_key { get; set; }
        public string stock_code { get; set; }
        public string fs_div { get; set; }

        public string fs_view {get; set;}
        
        public Task UpdateOnclick();

    }
}