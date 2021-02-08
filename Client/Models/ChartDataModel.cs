using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ForStock.Client.Models
{
    public class ChartDataModel
    {   
        [Key]
        public string Id { get; set; }
        public string Message { get; set; }
        public List<string> Amounts {
            get{
                return (from ds in DataSets select ds.Amount).ToList();
            }
            set{

            }
        }
        public List<string> YearAndQuarters{
            get{
                return (from ds in DataSets select ds.YearAndQuarter).ToList();
            }
            set{

            }
        }

        public List<ChartDataSet> DataSets { get; set; } = new List<ChartDataSet>();
        
        public ChartDataModel()
        {
        }
        
        public ChartDataModel(string id, string message){
            Id = id;
            Message = message;
        }
    }

    public class ChartDataSet
    {
        public string Year { get; set; }
        public string Quarter { get; set; }
        public string Amount { get; set; }
        public string YearAndQuarter { get; set; }

        public ChartDataSet() { }

        public ChartDataSet(string year, string businessCode, string amount)
        {
            Year = year;
            Quarter = getQuarterFromBusinessCode(businessCode);
            Amount = amount;
            YearAndQuarter = Year + "/" + Quarter;
        }

        public string getQuarterFromBusinessCode(string businessCode)
        {
            switch (businessCode)
            {
                case "11013":
                    return "Q1";
                case "11012":
                    return "Q2";
                case "11014":
                    return "Q3";
                case "11011":
                    return "Q4";
                default:
                    return "error";
            }
        }
    }
}