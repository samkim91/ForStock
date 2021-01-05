using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System;
using Newtonsoft.Json.Linq;

namespace ForStock.Server.Common
{
    public class DateHelper
    {
        // 보고서 코드인데, 앞에서부터 1분기, 2분기, 3분기, 4분기 순
        public string[] reprt_codes { get; set; } = { "11013", "11012", "11014", "11011" };
        int CurrentYear { get; set; } = Convert.ToInt32(DateTime.Now.Year);
        double CurrentMonth { get; set; } = Convert.ToInt32(DateTime.Now.Month);
        JArray jArray = new JArray();

        JObject jObject = null;

        public JArray GetQuartersForRequest()
        {
            int currentQuarter = Convert.ToInt32(Math.Ceiling(CurrentMonth / 3));
            int startQuarter = 0;
            int startYear = 0;

            if (currentQuarter == 1)
            {
                startQuarter = 4;
                startYear = CurrentYear - 1;
            }
            else
            {
                startQuarter = currentQuarter - 1;
                startYear = CurrentYear;
            }

            // start year
            for (int i = startQuarter - 1; i >= 0; i--)
            {
                jObject = new JObject();
                jObject.Add("Year", startYear);
                jObject.Add("Quarter", reprt_codes[i]);
                jArray.Add(jObject);
            }

            // last 4 years from start year
            for (int j = 4; j > 0; j--)
            {
                startYear--;
                for (int i = 3; i >= 0; i--)
                {
                    jObject = new JObject();
                    jObject.Add("Year", startYear);
                    jObject.Add("Quarter", reprt_codes[i]);
                    jArray.Add(jObject);
                }
            }

            return jArray;
        }
    }
}