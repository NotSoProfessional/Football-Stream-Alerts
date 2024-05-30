using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace football_automatic
{
    internal class MatchEvents
    {
        private int _minutes;
        private int _seconds;

        private bool _fulltime = false;
        private bool _halftime = false;
        private bool _started = false;
        private bool _finished = false;

        public MatchEvents(int minutes = 0, int seconds = 0) 
        {
            _minutes = minutes;
            _seconds = seconds;
        }

        public override string ToString()
        {
            return $"{_minutes} : {_seconds}";
        }
    }
}
