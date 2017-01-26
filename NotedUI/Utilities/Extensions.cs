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
    }
}
