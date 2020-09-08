using System;
using System.Globalization;

namespace ValidConvertToThaiDateTime
{
    class Program
    {
        private static GregorianCalendar GC = new GregorianCalendar();
        private static ThaiBuddhistCalendar TC = new ThaiBuddhistCalendar();

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            //CultureInfo provider = CultureInfo.InvariantCulture;
            CultureInfo provider = CultureInfo.CreateSpecificCulture("th-TH");
            //CultureInfo provider = CultureInfo.CreateSpecificCulture("en-US");


            DateTime result;
            string format = "yyyy-MM-dd HH";




            //var dt1 = "2020-02-29 13";
            //DateTime.TryParseExact(dt1, format, provider, DateTimeStyles.AdjustToUniversal, out result);

            //Convert(result);



            var dt2 = "2563-02-29 23";
            DateTime.TryParseExact(dt2, format, provider, DateTimeStyles.AdjustToUniversal, out result);

            Convert(result);




            Console.WriteLine("end");
            Console.WriteLine("end");

            Console.ReadKey();
        }

        public static DateTime? ToBE(DateTime result)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            var year = 2000;
            if (result.Year < 1800)
            {
                year = result.Year + 543;
            }
            else if (result.Year < 2400)
            {
                year = result.Year;
            }
            else
            {
                year = result.Year - 543;
            }
            DateTime myDT = new DateTime(year, result.Month, result.Day, GC);
            Console.WriteLine("GregorianCalendar" + myDT);
            var beyear = TC.GetYear(myDT);
            Console.WriteLine("beyear" + beyear);
            var beDT = new DateTime(beyear, result.Month, result.Day, TC);

            Console.ResetColor(); Console.ForegroundColor = ConsoleColor.White;
            return beDT;
        }

        public static DateTime? Convert(DateTime dateParse)
        {
            Console.WriteLine("TC.GetYear(dateParse)" + TC.GetYear(dateParse));
            var dateConverted = new DateTime(
                TC.GetYear(dateParse), TC.GetMonth(dateParse), TC.GetDayOfMonth(dateParse)
                , TC.GetHour(dateParse), TC.GetMinute(dateParse), TC.GetSecond(dateParse), TC);
            var date = dateConverted.ToString("dd/MM/yyyy hh:mm:s tt", CultureInfo.CreateSpecificCulture("th-TH"));
            var ge = DateTime.Parse(date);


            Console.ResetColor(); Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("th" + dateConverted);
            Console.WriteLine("ge" + ge);
            Console.ResetColor(); Console.ForegroundColor = ConsoleColor.White;

            return dateConverted;
        }


    }
}
