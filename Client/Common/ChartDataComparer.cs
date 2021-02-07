using System.Collections.Generic;
using ForStock.Client.Models;

namespace ForStock.Client.Common
{
    public class ChartDataComparer : IEqualityComparer<ChartDataSet>
    {
        public bool Equals(ChartDataSet x, ChartDataSet y)
        {
            //First check if both object reference are equal then return true
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }
            //If either one of the object refernce is null, return false
            if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
            {
                return false;
            }
            //Comparing all the properties one by one
            return x.Year == y.Year && x.Quarter == y.Quarter;
        }

        public int GetHashCode(ChartDataSet obj)
        {
            //If obj is null then return 0
            if (obj == null)
            {
                return 0;
            }
            //Get the Year hash code value
            //Check for null refernece exception
            int YearHashCode = obj.Year == null ? 0 : obj.Year.GetHashCode();
            //Get the string HashCode Value
            //Check for null refernece exception
            int QuarterHashCode = obj.Quarter == null ? 0 : obj.Quarter.GetHashCode();

            return YearHashCode ^ QuarterHashCode;
        }
    }
}