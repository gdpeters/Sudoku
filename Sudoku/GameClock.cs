using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Sudoku
{
    class GameClock
    {
        private DateTime start, paused;

        public GameClock()
        {

        }

        public void Start()
        {
            start = DateTime.Now;
        }

        public void Pause()
        {
            paused = DateTime.Now;
        }

        public void Resume()
        {
            start = start + (DateTime.Now - paused);
        }

        public String GetTime()
        {
            if (start == null)
            {
                start = DateTime.Now;
            }
            TimeSpan elapsed = DateTime.Now - start;
            int hh = elapsed.Hours;
            int mm = elapsed.Minutes;
            int ss = elapsed.Seconds;
            return String.Format("{0,2:D2}:{1,2:D2}:{2,2:D2}", hh, mm, ss);
        }
    }
}
