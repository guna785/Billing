using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Helper
{
    public static class GetFInancialYear
    {
        public static string ToFinancialYear(this DateTime dateTime)
        {
            return (dateTime.Month >= 4 ? dateTime.ToString("yyyy") + dateTime.AddYears(1).ToString("yy") : dateTime.AddYears(-1).ToString("yyyy") + dateTime.ToString("yy"));
        }
        public static string ToFinancialYearShort(this DateTime dateTime, string sid)
        {

            var Id = Convert.ToInt32(sid.Substring(6));
            var lfy = sid.Substring(0, 6);
            var fy = (dateTime.Month >= 4 ? dateTime.ToString("yyyy") + dateTime.AddYears(1).ToString("yy") : dateTime.AddYears(-1).ToString("yyyy") + dateTime.ToString("yy"));
            if (lfy.Equals(fy))
            {
                var s = Convert.ToInt32(sid);
                s++;
                return s.ToString();
            }
            else
            {
                return fy+"0001";
            }
        }

    }
}
