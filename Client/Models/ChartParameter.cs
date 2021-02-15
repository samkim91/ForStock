using System.Collections.Generic;

namespace ForStock.Client.Models
{
    public class ChartParameter
    {
        // Chart의 독립성을 가리기 위한 ID
        public string Id { get; set; }
        // Chart의 기본 type을 나타냄. 
        public string Subject { get; set; }
        public List<string> Xvalues { get; set; }
        public List<DataSet> DataSets { get; set; } = new List<DataSet>();

        public ChartParameter(){

        }
        public ChartParameter(string id, string subject)
        {
            this.Id = id;
            this.Subject = subject;
        }
    }
    
    public class DataSet
    {
        public List<string> Data { get; set; } = new List<string>();
        public string Label { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }
        public string LineTension { get; set; } = "0"; // line chart에서 완만도를 나타내는데 0이면 직선.
        public string BackgroundColor { get; set; }
        public string BorderColor { get; set; }
        public DataSet()
        {

        }

        public DataSet(List<string> data, string label, string type, int order, string backgroundColor, string borderColor)
        {
            this.Data = data;
            this.Label = label;
            this.Type = type;
            this.Order = order;
            this.BackgroundColor = backgroundColor;
            this.BorderColor = borderColor;
        }
    }
}