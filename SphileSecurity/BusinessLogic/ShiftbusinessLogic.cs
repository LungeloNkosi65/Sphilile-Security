using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SphileSecurity.BusinessLogic
{
    public class ShiftbusinessLogic
    {
        public static int  CalcNoOfHours(DateTime timeFrom, DateTime timetTo)
        {
            var result = timetTo.Subtract(timeFrom);
            return result.Hours*(-1);
        }
    }
}