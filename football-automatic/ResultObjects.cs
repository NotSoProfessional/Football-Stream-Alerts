using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace football_automatic
{
    internal class ResultObjects
    {
    }

    interface IResult
    {

    }

    internal class Match : IComparable<Match>, IResult
    {
        public string match_name { get; private set; }
        public int match_id { get; private set; }
        public string home_name { get; private set; }
        public int home_id { get; private set; }
        public string away_name { get; private set; }
        public int away_id { get; private set; }
        public DateTime match_date { get; private set; }

        public Match(int match_id, string home_name, int home_id, string away_name, int away_id, DateTime match_date)
        {
            this.match_id = match_id;
            this.home_name = home_name;
            this.home_id = home_id;
            this.away_name = away_name;
            this.away_id = away_id;
            this.match_date = match_date;

            match_name = home_name + " -VS- " + away_name + "    (" + match_date.ToString() + ")";
        }

        public static bool operator > (Match left, Match right) { return left.match_date > right.match_date; }
        public static bool operator <(Match left, Match right) { return left.match_date < right.match_date; }
        public static bool operator >=(Match left, Match right) { return left.match_date >= right.match_date; }
        public static bool operator <=(Match left, Match right) { return left.match_date <= right.match_date; }

        public override string ToString()
        {
            return match_name;
        }

        public int CompareTo(Match? other)
        {
            if (other != null)
            {
                if (this.match_date < other.match_date) return -1;
                if (this.match_date == other.match_date) return 0;
                if (this.match_date > other.match_date) return 1;
            }
            else
            {
                throw new ArgumentNullException(nameof(other));;
            }

            throw new NotImplementedException();
        }
    }

    internal class League : IResult
    {
        public int league_id { get; private set;}
        public string league_name { get; private set; }
        public string country_name { get; private set; }

        public League(int league_id, string league_name, string country_name)
        {
            this.league_id = league_id;
            this.league_name = league_name;
            this.country_name = country_name;
        }

        public override string ToString()
        {
            return $"{league_name} ({country_name})";
        }
    }
}
