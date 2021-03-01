using System.ComponentModel.DataAnnotations;

namespace ForStock.Client.Models
{
    public class IntroModel
    {
        [Key]
        [Required]
        public string crtfc_key { get; set; }
        [Required]
        public string corp_name { get; set; }
        public string stock_code { get; set; }
        public string fs_div { get; set; } = "OFS";
    }
}