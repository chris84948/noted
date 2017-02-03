using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace NotedUI
{
    public static class Extensions
    {
        public static void Reset(this Timer timer)
        {
            if (timer == null)
                return; 

            timer.Stop();
            timer.Start();
        }

        public static bool DateMatches(this DateTime dt1, DateTime dt2)
        {
            return Math.Abs((dt1 - dt2).TotalSeconds) <= 1;
        }
    }
}
