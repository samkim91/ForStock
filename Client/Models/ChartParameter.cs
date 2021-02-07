using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace ForStock.Client.Models
{
    public class ChartParameter
    {
        public string Label { get; set; }
        public List<string> Data { get; set; } = new List<string>();

        public ChartParameter(){

        }
        public ChartParameter(string message, List<string> amounts){
            Label = message;
            Data = amounts;
        }
    }
}