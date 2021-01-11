using System;
using System.Collections.Generic;
using System.Linq;

namespace ForStock.Client.Models
{
    public class ChartData
    {
        private List<string> years;
        private List<string> quarters;
        private List<string> yearsAndQuarters;
        private List<string> amounts { get; set; }
        List<ChartDataModel> _chartDataModels { get; set; } = new List<ChartDataModel>();
        public ChartData() { }
        public ChartData(List<ChartDataModel> chartDataModels)
        {
            _chartDataModels = chartDataModels;
        }
        public List<string> Years
        {
            get
            {
                return years;
            }
            set
            {
                years = (from cdm in _chartDataModels select cdm.Year).ToList();
            }
        }
        public List<string> Quarters
        {
            get
            {
                return quarters;
            }
            set
            {
                List<string> tempQuarters = (from cdm in _chartDataModels select cdm.Quarter).ToList();
                foreach (string temp in tempQuarters)
                {
                    if (temp == "11013")
                    {
                        quarters.Add("Q1");
                    }
                    else if (temp == "11012")
                    {
                        quarters.Add("Q2");
                    }
                    else if (temp == "11014")
                    {
                        quarters.Add("Q3");
                    }
                    else
                    {
                        quarters.Add("Q4");
                    }
                }
            }
        }

        public List<string> YearsAndQuarters
        {
            get
            {
                return yearsAndQuarters;
            }
            set
            {
                for (int i = 0; i < years.Count(); i++)
                {
                    yearsAndQuarters.Add(years[i] + "/" + quarters[i]);
                }
            }
        }
        public List<string> Amounts
        {
            get
            {
                return amounts;
            }
            set
            {
                amounts = (from cdm in _chartDataModels select cdm.Amount).ToList();
            }
        }
    }

    public class ChartDataModel
    {
        public string Year { get; set; }
        public string Quarter { get; set; }
        public string Amount { get; set; }
    }
}