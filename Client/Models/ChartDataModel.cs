using System;
using System.Collections.Generic;
using System.Linq;

namespace ForStock.Client.Models
{
    public class ChartData
    {
        public List<string> Years { get; set; } = new List<string>();
        public List<string> Quarters { get; set; } = new List<string>();
        public List<string> YearsAndQuarters { get; set; } = new List<string>();
        public List<string> Amounts { get; set; } = new List<string>();
        List<ChartDataModel> _chartDataModels { get; set; } = new List<ChartDataModel>();
        public ChartData() { }
        public ChartData(List<ChartDataModel> chartDataModels)
        {
            _chartDataModels = chartDataModels;

            MakeChartDataFromModels();
        }

        public void MakeChartDataFromModels()
        {
            Years = (from cdm in _chartDataModels where cdm != null select cdm.Year).ToList();
            Years.Reverse();

            List<string> tempQuarters = (from cdm in _chartDataModels where cdm != null select cdm.Quarter).ToList();
            tempQuarters.Reverse();
            foreach (string temp in tempQuarters)
            {
                if (temp != null)
                {
                    if (temp == "11013")
                    {
                        Quarters.Add("Q1");
                    }
                    else if (temp == "11012")
                    {
                        Quarters.Add("Q2");
                    }
                    else if (temp == "11014")
                    {
                        Quarters.Add("Q3");
                    }
                    else
                    {
                        Quarters.Add("Q4");
                    }
                }
            }

            for (int i = 0; i < Years.Count(); i++)
            {
                YearsAndQuarters.Add(Years[i] + "/" + Quarters[i]);
            }

            Amounts = (from cdm in _chartDataModels where cdm != null select cdm.Amount).ToList();
            Amounts.Reverse();
        }
    }

    public class ChartDataModel
    {
        public string Year { get; set; }
        public string Quarter { get; set; }
        public string Amount { get; set; }

        public ChartDataModel() { }

        public ChartDataModel(string year, string quarter, string amount)
        {
            Year = year;
            Quarter = quarter;
            Amount = amount;
        }
    }
}