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

        public List<string> QuarterAmounts
        {
            get
            {
                return (from ds in DataSets select ds.Amount).ToList();
            }
            set
            {

            }
        }
        public List<string> YearAmounts
        {
            get
            {
                // long의 형식으로 나오기 떄문에 string 으로 바꿔준다.
                var tempResult = (from ds in DataSets
                        group ds by ds.Year into EveryYear
                        select EveryYear.Sum(ds => long.Parse(ds.Amount))).ToList();

                return tempResult.ConvertAll<string>(x => x.ToString());
            }
        }
        public List<IGrouping<string, ChartDataSet>> AmountsGroupByQuarter
        {
            get
            {
                // long의 형식으로 나오기 때문에 string 으로 바꿔준다.
                var tempResult = (from ds in DataSets
                                    group ds by ds.Quarter into g
                                    orderby g.Key
                                    select g).ToList();

                return tempResult;
            }
        }
        public List<string> Years
        {
            get
            {
                return (from ds in DataSets select ds.Year).Distinct().ToList();
            }
        }
        public List<string> YearAndQuarters
        {
            get
            {
                return (from ds in DataSets select ds.YearAndQuarter).ToList();
            }
            set
            {

            }
        }


        public List<ChartDataSet> DataSets { get; set; } = new List<ChartDataSet>();

        public ChartDataModel()
        {
        }

        public ChartDataModel(string id, string message)
        {
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