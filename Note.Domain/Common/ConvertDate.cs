using System.Globalization;

namespace Note.Domain.Common
{
    public static class ConvertDate
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

        //todo
        //public static string ConvertGregorianToPersian(DateTime dateTime)
        //{
        //    DateTime d = DateTime.Parse(dateTime);
        //    PersianCalendar pc = new PersianCalendar();
        //   var persianDateString = string.Format("{0}/{1}/{2}", pc.GetYear(d), pc.GetMonth(d), pc.GetDayOfMonth(d));
        //    return persianDateString;
        //}
        public static string ToShamsi(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;

            var pc = new PersianCalendar();
            var d = dateTime.Value;

            return $"{pc.GetYear(d):0000}/{pc.GetMonth(d):00}/{pc.GetDayOfMonth(d):00}";
        }
    }
}
