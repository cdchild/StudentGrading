using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentGrading.Models
{
    //helper class
    public class Helpers
    {
        //helper to take the date from first datetime and time from second datetime handling empty datetime objects also
        public static DateTime CombineDate1Time2(DateTime? date1, DateTime? time2)
        {
            DateTime dateTimeWDate =
                (!date1.HasValue) ? dateTimeWDate = new DateTime() : dateTimeWDate = (DateTime)time2;
            DateTime dateTimeWTime =
                (!time2.HasValue) ? dateTimeWTime = new DateTime() : dateTimeWTime = (DateTime)time2;
            return new DateTime(
                    dateTimeWDate.Year,
                    dateTimeWDate.Month,
                    dateTimeWDate.Day,
                    dateTimeWTime.Hour,
                    dateTimeWTime.Minute,
                    dateTimeWTime.Second
                    );
        }
    }
}