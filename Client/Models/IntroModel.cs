using System.ComponentModel.DataAnnotations;

namespace ForStock.Client.Models
{
    public class IntroModel
    {
        [Key]
        public string crtfc_key { get; set; }
        public string stock_code { get; set; }
        public string fs_div { get; set; } = "OFS";
    }
}