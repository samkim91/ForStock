using System.ComponentModel.DataAnnotations;

namespace ForStock.Shared.Model
{
    public class CorporationInfo
    {
        [Key]
        public string corp_code { get; set; }
        public string corp_name { get; set; }
        public string corp_name_eng { get; set; }
        public string ceo_nm { get; set; }
        public string corp_cls { get; set; }
        public string jurir_no { get; set; }
        public string bizr_no { get; set; }
        public string adres { get; set; }
        public string hm_url { get; set; }
        public string ir_url { get; set; }
        public string phn_no { get; set; }
        public string fax_no { get; set; }
        public string induty_code { get; set; }
        public string est_dt { get; set; }
        public string acc_mt { get; set; }
        public string status { get; set; }
        public string messgae { get; set; }
    }
}