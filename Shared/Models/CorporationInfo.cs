using System.ComponentModel.DataAnnotations;

namespace ForStock.Shared.Model
{
    public class CorporationInfo
    {
        [Key]
        public string CorpCode { get; set; }
        public string CorpName { get; set; }
        public string CorpNameEng { get; set; }
        public string CeoName { get; set; }
        public string CorpClass { get; set; }
        public string CorpNumber { get; set; }
        public string BusinessNumber { get; set; }
        public string Address { get; set; }
        public string Homepage { get; set; }
        public string IrHomepage { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string SectorCode { get; set; }
        public string EstablishDate { get; set; }
        public string AccountMonth { get; set; }
    }
}