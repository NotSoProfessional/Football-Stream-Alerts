using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace football_automatic
{
    enum Status
    {
        NOT_STARTED,
        FIRST_HALF,
        SECOND_HALF,
        FINISHED
    }

    internal class MatchManager
    {
        private Match _match;
        private Match? _updatedMatch;
  
        private List<Event> _eventQueue = new List<Event>();
        private List<Event> _notifiedEvents = new List<Event>();

        private Status _status = Status.NOT_STARTED;
        private int _halfTimeStamp = 0;

        public MatchManager(Match match) 
        {
            _match = match;
            Program.clock.AddIntervalCallback(OnUpdate, 5);
        }

        private async void OnUpdate()
        {
            if (_updatedMatch != null)
            {
                _match = _updatedMatch;
                _updatedMatch = null;
            }

            var results = await Program.api.GetMatchDetails(_match.match_id);

            _match = Parsers.MatchDetails(results);

            updateEvents(Parsers.MatchEvents(results));

            List<Event> tmp_queue = new List<Event>(_eventQueue);

            foreach (Event match_event in tmp_queue)
            {
                if ((match_event.GetTotalTime()-20 <= Program.clock.GetTotalSeconds()) | ((match_event._time + match_event._overload_time) <= Program.clock.Minutes & match_event._minutes - (match_event._time + match_event._overload_time) > 1) | (match_event._eventId == 2 & match_event.GetTotalTime() - 20 <= Program.clock.RunTime))
                {
                    switch (match_event._eventId)
                    {
                        case (int)EventId.HALF:
                            Program.clock.Pause();

                            if (((Half)match_event)._type == "HT")
                            {
                                _halfTimeStamp = Program.clock.RunTime;
                            }

                            Console.WriteLine($"Notify {match_event}");
                            _notifiedEvents.Add(match_event);
                            _eventQueue.Remove(match_event);

                            break;

                        case (int)EventId.START_STOP:
                            StopStart ss_event = (StopStart)match_event;

                            StopStart.Type type = ss_event._type;

                            switch (type)
                            {
                                case StopStart.Type.KICKOFF:
                                    Program.clock.Set(0);
                                    Program.clock.Start();

                                    _notifiedEvents.Add(match_event);
                                    _eventQueue.Remove(match_event);
                                    Console.WriteLine($"Notify {match_event}");

                                    break;

                                case StopStart.Type.SECOND_HALF:
                                    int total_diff = _halfTimeStamp - Program.clock.RunTime;
                                    int minutes = (int)Math.Floor(total_diff / (float)60);
                                    int seconds = total_diff - (minutes * 60);
                                    
                                    Event base_event = new Event((int)EventId.START_STOP, "", minutes, 0, minutes+45, seconds);
                                    StopStart stopStart = new StopStart(StopStart.Type.START_SECOND_HALF, base_event);

                                    _eventQueue.Add(stopStart);

                                    _notifiedEvents.Add(match_event);
                                    _eventQueue.Remove(match_event);
                                    Console.WriteLine($"Notify {match_event}");


                                    break;
                                case StopStart.Type.START_SECOND_HALF:
                                    Console.WriteLine($"Notify {match_event}");
                                    Program.clock.Set(45);
                                    Program.clock.Start();

                                    _notifiedEvents.Add(match_event);
                                    _eventQueue.Remove(match_event);
                                    Console.WriteLine($"Notify {match_event}");

                                    break;
                            }

                            break;

                        default:
                            Console.WriteLine($"Notify {match_event}");
                            _notifiedEvents.Add(match_event);
                            _eventQueue.Remove(match_event);

                            break;
                    }
                }
            }
        }

        public void updateEvents(Events events)
        {
            List<Event> list = new List<Event>();

            foreach (Event match_event in events.events)
            {
                if (!_eventQueue.Contains(match_event) & !_notifiedEvents.Contains(match_event))
                {
                    _eventQueue.Add(match_event);
                    Console.WriteLine($"Incoming new event! {match_event}");
                }
            }

            Console.WriteLine($"Retreived Events at {Program.clock}");
        }
    }
}
