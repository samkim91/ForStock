using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace ForStock.Client.Models
{
    public class ChartParameter
    {
        public string Label { get; set; }
        public List<string> Data { get; set; } = new List<string>();

        public ChartParameter()
        {

        }
        public ChartParameter(string message, List<string> amounts)
        {
            Label = message;
            Data = amounts;
        }
    }

    public class Config
    {
        public string type { get; set; }

        public Options options { get; set; } = new Options();

        public Data data { get; set; } = new Data();
    }

    public class Options
    {
        public bool responsive { get; set; } = true;

        public Scales scales { get; set; } = new Scales();

        public class Scales
        {
            public YAxes yAxes { get; set; } = new YAxes();
            public class YAxes
            {
                public Ticks ticks { get; set; } = new Ticks();
                public class Ticks
                {
                    public bool beginAtZero { get; set; } = true;
                }
            }
        }
    }

    public class Data
    {
        public List<string> labels { get; set; } = new List<string>();

        public List<DataSet> dataSets { get; set; } = new List<DataSet>();

        public class DataSet
        {
            public List<string> data { get; set; } = new List<string>();
            public string label { get; set; }
            public List<string> backgroundColor { get; set; } = new List<string>();
            public string type { get; set; }
            public int order { get; set; }

            public DataSet(){

            }

            public DataSet(List<string> data, string label, List<string> backgroundColor, string type, int order){
                this.data = data;
                this.label = label;
                this.backgroundColor = backgroundColor;
                this.type = type;
                this.order = order;
            }
        }
    }
}