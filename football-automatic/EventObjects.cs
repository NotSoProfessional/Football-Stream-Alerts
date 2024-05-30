using Accessibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace football_automatic
{
    /*struct Status
    {
        public bool finished;
        public bool ongoing;

    }*/

    struct Events
    {
        public int minutes = 0;
        public int seconds = 0;
        public int home_score = 0;
        public int away_score = 0;

        public List<Substitution> subs = new List<Substitution>();
        public List<Goal> goals = new List<Goal>();
        public List<Card> cards = new List<Card>();
        public List<VAR> vars = new List<VAR>();
        public List<Half> halfs = new List<Half>();
        public List<AddedTime> added = new List<AddedTime>();

        public List<Event> events = new List<Event>();

        public Events() { }

        public int GetTotalTime() { return (minutes * 60) + seconds; }
    }

    internal class Event : IEquatable<Event>
    {
        public readonly string _timestr;
        public readonly int _time;
        public readonly int _overload_time;
        public readonly int _eventId;

        public readonly int _minutes;
        public readonly int _seconds;

        public Event(int eventId, string timestr, int time, int? overload_time = 0, int minutes = 0, int seconds = 0)
        {
            _timestr = timestr;
            _time = time;
            _overload_time = overload_time == null ? 0 : (int)overload_time;
            _eventId = eventId;
            _minutes = minutes;
            _seconds = seconds;
        }

        public Event(Event match_event)
        {
            _timestr = match_event._timestr; _time = match_event._time;
            _overload_time = match_event._overload_time; _eventId = match_event._eventId;
            _minutes = match_event._minutes; _seconds = match_event._seconds;
        }

        public int GetTotalTime() { return (_minutes * 60) + _seconds; }

        public bool Equals(Event? other)
        {
            if (other == null) return false;

            if (_eventId == (int)EventId.HALF & other._eventId == (int)EventId.HALF)
            {
                Half left = (Half)this;
                Half right = (Half)other;

                return left._type == right._type;
            }

            if (_eventId == (int)EventId.ADDED_TIME & other._eventId == (int)EventId.HALF)
            {
                return this._time == other._time;
            }

            if (_eventId == (int)EventId.START_STOP & other._eventId == (int)EventId.START_STOP)
            {
                StopStart left = (StopStart)this;
                StopStart right = (StopStart)other;

                return (left._type == right._type);
            }

                if (_eventId == -1 & other._eventId == -1)
            {
                Substitution left = (Substitution)this;
                Substitution right = (Substitution)other;

                return (left._off._id == right._off._id & left._on._id == right._on._id);
            }

            return _eventId == other._eventId;
        }

        public override string ToString()
        {
            return $"({_eventId}) {this.GetType()} in {_timestr} ({_minutes}:{_seconds})";
        }
    }

    internal class Substitution : Event
    {
        public readonly Player _off;
        public readonly Player _on;

        public Substitution(Player off, Player on, Event match_event)
            : base(match_event)
        {
            _off = off;
            _on = on;
        }
    }

    internal class Goal : Event
    {
        Player scorer;
        Player? assist;
        int home_score;
        int away_score;
        bool og;

        public Goal(Player scorer, Player? assist, int home_score, int away_score, Event match_event)
            : base(match_event)
        {
            this.scorer = scorer;
            this.assist = assist;
            this.home_score = home_score;
            this.away_score = away_score;
        }
    }

    internal class Card : Event
    {
        Player _offender;
        string _card;

        public Card(Player offender, string card, Event match_event)
            : base(match_event)
        {
            _offender = offender;
            _card = card;
        }
    }

    internal class AddedTime : Event 
    {
        int _added;

        public AddedTime(int added, Event match_event)
            : base(match_event)
        {
            _added = added;
        }
    }

    internal class VAR : Event
    {
        Player _involved;
        bool _pending;
        string _outcome;
        string _reason;

        public VAR(Player involved, bool pending, string outcome, string reason, Event match_event)
            : base(match_event)
        {
            _involved = involved;
            _pending = pending;
            _outcome = outcome;
            _reason = reason;
        }
    }

    internal class Half : Event
    {
        public readonly string _type;

        public Half(string type, Event match_event)
            : base(match_event)
        {
            _type = type;
        }
    }

    internal class StopStart : Event
    {
        public enum Type
        {
            KICKOFF,
            SECOND_HALF,
            START_SECOND_HALF
        }

        public readonly Type _type;

        public StopStart(Type type, Event match_event)
            : base(match_event)
        {
            _type = type;
        }
    }

    public enum EventId
    {
        HALF,
        ADDED_TIME,
        START_STOP
    }
}
