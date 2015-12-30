using System;
using System.Collections.Generic;
using System.Linq;

namespace FerkopaUtils
{
    public class SummerTime
    {
        public SummerTime()
        {
            
        }

        public SummerTime(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsInTime(DateTime other)
        {
            return StartTime <= other
                && EndTime <= other;
        }
    }
    //todo évenként kell minden országra bővíteni
    public abstract class SumerTimes:List<SummerTime>
    {
     
        public bool InSummerTime(DateTime date)
        {
            // ReSharper disable once SuggestVarOrType_SimpleTypes
            return this.Any(s => s.IsInTime(date));
        }

        public abstract void Init();
    }

    public class SummerTimesHungary:SumerTimes
    {
        public override void Init()
        {  
            //
            // az adatok a vikipediából vannak.
            // http://hu.wikipedia.org/wiki/Ny%C3%A1ri_id%C5%91sz%C3%A1m%C3%ADt%C3%A1s
            // a továábiakban más országokhoz jó a következő:
            // http://www.timeanddate.com/worldclock/
            //
            Add( new SummerTime(new DateTime(1916, 04, 30,23,00,00), new DateTime(1916, 10, 01,1,00,00)) );
            Add(new SummerTime(new DateTime(1917, 04, 16, 23, 00, 00), new DateTime(1917, 6, 17, 2, 00, 00)));
            Add(new SummerTime(new DateTime(1918, 04, 15, 3, 00, 00), new DateTime(1918, 9, 16, 2, 00, 00)));
            Add(new SummerTime(new DateTime(1919, 04, 15, 3, 00, 00), new DateTime(1918, 11, 24, 2, 00, 00)));
            Add(new SummerTime(new DateTime(1941, 04, 7, 23, 00, 00), new DateTime(1942, 11, 2, 2, 00, 00)));
            Add(new SummerTime(new DateTime(1943, 04, 29, 2, 00, 00), new DateTime(1943, 10, 4, 2, 00, 00)));
            Add(new SummerTime(new DateTime(1944, 04, 3, 2, 00, 00), new DateTime(1943, 10, 2, 2, 00, 00)));
            Add(new SummerTime(new DateTime(1945, 05, 3, 23, 00, 00), new DateTime(1945, 11, 1, 0, 00, 00)));
            Add(new SummerTime(new DateTime(1946, 03, 31, 2, 00, 00), new DateTime(1946, 10, 6, 0, 00, 00)));
            Add(new SummerTime(new DateTime(1947, 04, 6, 2, 00, 00), new DateTime(1947, 10, 5, 3, 00, 00)));
            Add(new SummerTime(new DateTime(1948, 04, 4, 2, 00, 00), new DateTime(1948, 10, 3, 3, 00, 00)));
            Add(new SummerTime(new DateTime(1949, 04, 10, 2, 00, 00), new DateTime(1949, 10, 2, 3, 00, 00)));            
            Add(new SummerTime(new DateTime(1954, 05, 23, 0, 00, 00), new DateTime(1954, 10, 2, 2, 00, 00)));
            Add(new SummerTime(new DateTime(1955, 05, 22, 2, 00, 00), new DateTime(1955, 10, 2, 2, 00, 00)));
            Add(new SummerTime(new DateTime(1956, 06, 3, 2, 00, 00), new DateTime(1956, 9, 30, 3, 00, 00)));
            Add(new SummerTime(new DateTime(1957, 06, 2, 2, 00, 00), new DateTime(1957, 9, 29, 3, 00, 00)));
            Add(new SummerTime(new DateTime(1980, 4, 6, 0, 00, 00), new DateTime(1980, 9, 28, 0, 00, 00)));
            Add(new SummerTime(new DateTime(1981, 3, 29, 0, 00, 00), new DateTime(1981, 9, 27, 1, 00, 00)));
            Add(new SummerTime(new DateTime(1982, 3, 28, 0, 00, 00), new DateTime(1982, 9, 26, 1, 00, 00)));
            Add(new SummerTime(new DateTime(1983, 3, 27, 0, 00, 00), new DateTime(1983, 9, 25, 1, 00, 00)));
            Add(new SummerTime(new DateTime(1984, 3, 25, 2, 00, 00), new DateTime(1984, 9, 30, 3, 00, 00)));
            Add(new SummerTime(new DateTime(1985, 3, 31, 2, 00, 00), new DateTime(1985, 9, 29, 3, 00, 00)));
            Add(new SummerTime(new DateTime(1986, 3, 30, 2, 00, 00), new DateTime(1986, 9, 28, 3, 00, 00)));
            Add(new SummerTime(new DateTime(1987, 3, 29, 2, 00, 00), new DateTime(1987, 9, 27, 3, 00, 00)));
            Add(new SummerTime(new DateTime(1988, 3, 27, 2, 00, 00), new DateTime(1988, 9, 25, 3, 00, 00)));
            Add(new SummerTime(new DateTime(1989, 3, 26, 2, 00, 00), new DateTime(1988, 9, 24, 3, 00, 00)));
            Add(new SummerTime(new DateTime(1990, 3, 25, 2, 00, 00), new DateTime(1990, 9, 30, 3, 00, 00)));
            Add(new SummerTime(new DateTime(1991, 3, 31, 2, 00, 00), new DateTime(1991, 9, 29, 3, 00, 00)));
            Add(new SummerTime(new DateTime(1992, 3, 29, 2, 00, 00), new DateTime(1992, 9, 27, 3, 00, 00)));
            Add(new SummerTime(new DateTime(1993, 3, 28, 2, 00, 00), new DateTime(1993, 9, 26, 3, 00, 00)));
            Add(new SummerTime(new DateTime(1994, 3, 27, 2, 00, 00), new DateTime(1994, 9, 25, 3, 00, 00)));
            Add(new SummerTime(new DateTime(1995, 3, 26, 2, 00, 00), new DateTime(1995, 9, 24, 3, 00, 00)));
            Add(new SummerTime(new DateTime(1996, 3, 31, 2, 00, 00), new DateTime(1996, 10, 27, 3, 00, 00)));
            Add(new SummerTime(new DateTime(1997, 3, 30, 2, 00, 00), new DateTime(1997, 10, 26, 3, 00, 00)));
            Add(new SummerTime(new DateTime(1998, 3, 29, 2, 00, 00), new DateTime(1998, 10, 25, 3, 00, 00)));
            Add(new SummerTime(new DateTime(1999, 3, 28, 2, 00, 00), new DateTime(1998, 10, 31, 3, 00, 00)));
            Add(new SummerTime(new DateTime(2000, 3, 26, 2, 00, 00), new DateTime(2000, 10, 29, 3, 00, 00)));
            Add(new SummerTime(new DateTime(2001, 3, 25, 2, 00, 00), new DateTime(2001, 10, 28, 3, 00, 00)));
            Add(new SummerTime(new DateTime(2002, 3, 31, 2, 00, 00), new DateTime(2001, 10, 27, 3, 00, 00)));
            Add(new SummerTime(new DateTime(2003, 3, 30, 2, 00, 00), new DateTime(2003, 10, 26, 3, 00, 00)));
            Add(new SummerTime(new DateTime(2004, 3, 28, 2, 00, 00), new DateTime(2003, 10, 31, 3, 00, 00)));
            Add(new SummerTime(new DateTime(2005, 3, 27, 2, 00, 00), new DateTime(2005, 10, 30, 3, 00, 00)));
            Add(new SummerTime(new DateTime(2006, 3, 26, 2, 00, 00), new DateTime(2006, 10, 29, 3, 00, 00)));
            Add(new SummerTime(new DateTime(2007, 3, 25, 2, 00, 00), new DateTime(2007, 10, 28, 3, 00, 00)));
            Add(new SummerTime(new DateTime(2008, 3, 30, 2, 00, 00), new DateTime(2008, 10, 26, 3, 00, 00)));
            Add(new SummerTime(new DateTime(2009, 3, 29, 2, 00, 00), new DateTime(2008, 10, 25, 3, 00, 00)));
            Add(new SummerTime(new DateTime(2010, 3, 28, 2, 00, 00), new DateTime(2008, 10, 31, 3, 00, 00)));
        }
    }
}
