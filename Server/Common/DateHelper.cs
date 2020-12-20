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

        JObject jObject = new JObject();

        public string GetRequestQuaters()
        {
            int currentYear = Convert.ToInt32(DateTime.Now.Year);
            double currentMonth = Convert.ToInt32(DateTime.Now.Month);
            int currentQuarter = 0;
            int startQuarter = 0;

            currentQuarter = Convert.ToInt32(Math.Ceiling(currentMonth / 3));

            if (currentQuarter == 1)
            {
                startQuarter = 4;
            }
            else
            {
                startQuarter = currentQuarter - 1;
            }

            for (int i = 0; i < 4; i++)
            {
                
            }

            return null;
        }
    }
}