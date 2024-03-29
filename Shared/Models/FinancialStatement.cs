using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForStock.Shared.Models
{

    public class FinancialStatement
    {
        [Key]
        public string id { get; set; }
        public string year { get; set; }
        public string quarter { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public List<FinancialInfo> list { get; set; } = new List<FinancialInfo>();

        public FinancialStatement(){}
        public FinancialStatement(string id, string message)
        {
            this.id = id;
            this.message = message;
        }
    }

    public class FinancialInfo
    {
        public string corp_code { get; set; }
        public string rcept_no { get; set; }
        public string reprt_code { get; set; }
        public string bsns_year { get; set; }
        public string sj_div { get; set; }
        public string sj_nm { get; set; }
        public string account_id { get; set; }
        public string account_nm { get; set; }
        public string account_detail { get; set; }
        public string thstrm_nm { get; set; }
        public string thstrm_amount { get; set; }
        public string thstrm_add_amount { get; set; }
        public string frmtrm_nm { get; set; }
        public string frmtrm_amount { get; set; }
        public string frmtrm_q_nm { get; set; }
        public string frmtrm_q_amount { get; set; }
        public string frmtrm_add_amount { get; set; }
        public string bfefrmtrm_nm { get; set; }
        public string bfefrmtrm_amount { get; set; }
        public string ord { get; set; }
    }
}