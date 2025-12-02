using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note.Model.Common
{
    public class ConvertDate
    {
        public static DateTime? ConvertPersianToGregorian(string? persianDate)
        {
            if (string.IsNullOrWhiteSpace(persianDate) || persianDate.Length != 8)
                return null;

            try
            {
                var year = int.Parse(persianDate.Substring(0, 4));
                var month = int.Parse(persianDate.Substring(4, 2));
                var day = int.Parse(persianDate.Substring(6, 2));

                var persianCalendar = new PersianCalendar();
                return persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
            }
            catch
            {
                return null;
            }
        }

        public static string ConvertGregorianToPersian(DateTime dateTime)
        {
            DateTime d = DateTime.Parse(dateTime);
            PersianCalendar pc = new PersianCalendar();
           var persianDateString = string.Format("{0}/{1}/{2}", pc.GetYear(d), pc.GetMonth(d), pc.GetDayOfMonth(d));
            return persianDateString;
        }
    }
}
