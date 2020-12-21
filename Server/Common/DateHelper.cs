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
        JArray jArray = new JArray();
        JObject jObject = null;

        public string GetRequestQuaters()
        {
            int currentYear = Convert.ToInt32(DateTime.Now.Year);
            int twoYearsAgo = Convert.ToInt32(DateTime.Now.AddYears(-2).Year);
            double currentMonth = Convert.ToInt32(DateTime.Now.Month);
            int currentQuarter = 0;
            int startQuarter = 0;
            int leftQuater = 0;

            currentQuarter = Convert.ToInt32(Math.Ceiling(currentMonth / 3));

            if (currentQuarter == 1)
            {
                startQuarter = 4;
            }
            else
            {
                startQuarter = currentQuarter - 1;
            }

            leftQuater = 4 - startQuarter;

            for (int i = startQuarter; i > 0; i--)
            {
                // startQuarter부터 1분기씩 감소하면서 가져옴.
                jObject = new JObject();
                jObject.Add("Year", currentYear);
                jObject.Add("Quarter", reprt_codes[i - 1]);
                jArray.Add(jObject);
                jObject = new JObject();
                jObject.Add("Year", twoYearsAgo);
                jObject.Add("Quarter", reprt_codes[i - 1]);
                jArray.Add(jObject);
            }

            for (int i = leftQuater; i > 0; i--)
            {
                int index = 3;
                // leftQuater부터 1분깄힉 감소하면서 가져옴.
                jObject = new JObject();
                jObject.Add("Year", currentYear - 1);
                jObject.Add("Quarter", reprt_codes[index]);
                jArray.Add(jObject);
                jObject = new JObject();
                jObject.Add("Year", twoYearsAgo - 1);
                jObject.Add("Quarter", reprt_codes[index]);
                jArray.Add(jObject);

                index--;
            }

            return jArray.ToString();
        }
    }
}