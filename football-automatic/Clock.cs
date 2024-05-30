using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;

namespace football_automatic
{
    internal class ClockIntervalEventArgs: EventArgs
    {
        public string EventMessage { get; }
        public int[] intervals { get; }

        public ClockIntervalEventArgs(string eventMessage, int[] intervals)
        {
            this.EventMessage = eventMessage;
            this.intervals = intervals;
        }
    }

    internal delegate void ClockIntervalEventHandler(object sender, ClockIntervalEventArgs e);

    internal class Clock : INotifyPropertyChanged
    {
        public event ClockIntervalEventHandler ClockIntervalChanged;
        private Timer _timer = new Timer() { Interval = 1000 };
        private DateTime _startTime { get; set; }
        private List<int> _intervals { get; set; } = new List<int>();
        private int _runTime { get; set; } = 0;

        public int RunTime { get { return _runTime; } private set { _runTime = value; } }

        private bool _paused = false;
        private int _minutes { get; set; } = 1;
        public int Minutes
        { 
            get { return _minutes; } 
            private set 
            {
                if (value != _minutes)
                {
                    _minutes = value;
                    NotifyPropertyChanged("DisplayText");
                }
            } 
        }

        private int _seconds 
        {
            get;set;
        }

        public int Seconds
        {
            get { return _seconds; }
            private set
            {
                if (value != _seconds)
                {
                    _seconds = value;
                    NotifyPropertyChanged("DisplayText");
                }
            }
        }

        public Clock() { 
            _startTime = DateTime.Now;
            _timer.Tick += OnTick;
        }

        public void Start() { _timer.Start(); _paused = false; }
        public void Stop() { _timer.Stop();}
        public void Pause() { _paused = true; }

        private void OnTick(object? sender, EventArgs e)
        {
            Increment();
            CallCallbacks();
        }

        public void Increment()
        {
            if (!_paused)
            {
                if (_seconds == 0 & _minutes == 0) _startTime = DateTime.Now.AddSeconds(-1);

                Seconds++;

                if (_seconds >= 60)
                {
                    _minutes++;
                    Seconds = 0;
                }

                List<int> reached_intervals = new List<int>();

                foreach (int interval in _intervals)
                {
                    if (GetTotalSeconds() % interval == 0) reached_intervals.Add(interval);
                }

                if (reached_intervals.Count > 0) RaiseClockIntervalEvent($"{reached_intervals.Count} interval(s) reached", reached_intervals.ToArray());
            }

            _runTime++;
        }

        private void CallCallbacks()
        {
            var callbacks = new List<KeyValuePair<ClockIntervalCallback, int>>(_callbacks);

            foreach (KeyValuePair<ClockIntervalCallback, int> callback in callbacks)
            {
                //if (GetTotalSeconds() % callback.Value == 0) callback.Key();
                if (_runTime % callback.Value == 0) callback.Key();
            }
        }

        public void Set(int minutes = 0, int seconds = 0)
        {
            Minutes = minutes;
            Seconds = seconds;

            _runTime = GetTotalSeconds();
        }

        public void Reset()
        {
            Seconds = 0;
            Minutes = 0;
        }

        public int GetTotalSeconds()
        {
            return (_minutes * 60) + _seconds;
        }

        public delegate void ClockIntervalCallback();
        private Dictionary<ClockIntervalCallback, int> _callbacksold = new Dictionary<ClockIntervalCallback, int>();
        private List<KeyValuePair<ClockIntervalCallback, int>> _callbacks = new List<KeyValuePair<ClockIntervalCallback, int>>();

        public void AddIntervalCallback(ClockIntervalCallback callback, int interval)
        {
            _callbacks.Add(new KeyValuePair<ClockIntervalCallback, int>(callback, interval));
        }

        public void RemoveIntervalCallback(ClockIntervalCallback callback, int interval)
        {
            _callbacks.Remove(new KeyValuePair<ClockIntervalCallback, int>(callback, interval));
        }

        public void AddIntervalEvent(int interval)
        {
            if (!_intervals.Contains(interval))
            {
                _intervals.Add(interval);
            }
        }

        public bool RemoveIntervalEvent(int interval)
        {
            return _intervals.Remove(interval);
        }

        private void RaiseClockIntervalEvent(string message, int[] intervals)
        {
            ClockIntervalEventHandler handler = ClockIntervalChanged; // Create a local copy to avoid thread race conditions

            if (handler != null)
            {
                ClockIntervalEventArgs args = new ClockIntervalEventArgs(message, intervals);
                handler(this, args); // Raise the event by invoking the delegate
            }
        }

        public override string ToString()
        {
            if (_minutes < 100)
            {
                return $"{_minutes.ToString("00")}:{_seconds.ToString("00")}";
            }

            return $"{_minutes}:{_seconds.ToString("00")}";
        }

        public string DisplayText => ToString();

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged(String pName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(pName));
            }
        }
    }
}
