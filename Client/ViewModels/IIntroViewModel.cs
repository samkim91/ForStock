using System.ComponentModel;
using System.Threading.Tasks;
using ForStock.Client.Models;
using ForStock.Shared.Model;

namespace ForStock.Client.ViewModels
{
    public interface IIntroViewModel
    {
        public CorporationInfo corporationInfo { get; set; }
        public IntroModel introModel { get; set; }
        public Task GetInitInfoFromDb();
        public Task GetDataOnclick();
    }
}