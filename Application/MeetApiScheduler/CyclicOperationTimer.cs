using DiagBox.DataAccess.Domain.Entities;
using DiagBox.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiagBox.Applications.PCCOMAPI.SchedulerPCComApi
{
   public class CyclicOperationTimer
    {
        private object myLock = new object();
        private Timer _timer;
        SchedulerPCComApiService pScheduler;

        public CyclicOperationTimer( int hour, int minute, int second, int periode, IList<Site> siteList, SchedulerPCComApiService scheduler)
        {
           var periodConverted = new TimeSpan(periode,0,0);

            int periodMilisec = (int)periodConverted.TotalMilliseconds;
            pScheduler = scheduler;

            var dueTime = 0;

            _timer = new  Timer(
                callback: this._timer_Elapsed,
                state: siteList,
                dueTime: dueTime, // décompte avant le début du timer ( 0 = depart immédiat)
                period: periodMilisec
                );

            // Figure how much time until 4:00
            //DateTime fourOClock = DateTime.Today.AddHours(16.0);
            DateTime now = DateTime.Now;
            DateTime hourOClock = DateTime.Today.AddHours(hour);
            hourOClock = hourOClock.AddMinutes(minute);
            hourOClock = hourOClock.AddSeconds(second);

            // If it's already past 4:00, wait until 4:00 tomorrow    
            if (now > hourOClock)
            {
                hourOClock = hourOClock.AddDays(1.0);
            }

            int msUntilFour = (int)((hourOClock - now).TotalMilliseconds);

            // Set the timer to elapse only once, at 4:00.
            _timer.Change(msUntilFour, periodMilisec);
        }
        // TODO Multithreading



        public void Close()
        {
            _timer.Dispose();
        }

        public void _timer_Elapsed(object state)
        {
            IList<Site> listSites = (IList<Site>)state;

            pScheduler.timerSitesValue(listSites);
        }

        public void Stop()
        {
            _timer.Dispose();
        }

    }
}
