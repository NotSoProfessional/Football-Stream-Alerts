using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebSocketSharp;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace football_automatic
{
    internal class Parsers
    {
        public static List<Match> SearchMatch(RestResponse response)
        {
            JObject results = JObject.Parse(response.Content);

            IList<JToken> raw_matches = results["aggregations"]["types"]["buckets"][0]["top_hits"]["hits"]["hits"].Children().ToList();


            List<Match> match_list = new List<Match>();

            foreach (JToken token in raw_matches)
            {
                int match_id = Int32.Parse(token["_id"].ToString());
                string home_name = token["_source"]["homeName"].ToString();
                int home_id = Int32.Parse(token["_source"]["homeId"].ToString());
                string away_name = token["_source"]["awayName"].ToString();
                int away_id = Int32.Parse(token["_source"]["awayId"].ToString());
                DateTime match_date = (DateTime)token["_source"]["matchDate"];
                match_date = match_date.AddHours(1);

                Match match = new Match(match_id, home_name, home_id, away_name, away_id, match_date);

                match_list.Add(match);
            }

            return match_list;
        }

        public static List<Match> DateMatch(RestResponse response)
        {
            JObject results = JObject.Parse(response.Content);

            IList<JToken> raw_leagues = results["leagues"].Children().ToList();


            List<Match> match_list = new List<Match>();

            foreach (JToken token in raw_leagues)
            {
                IList<JToken> raw_matches = token["matches"].Children().ToList();

                foreach (JToken j_token in raw_matches)
                {
                    int match_id = Int32.Parse(j_token["id"].ToString());
                    string home_name = j_token["home"]["name"].ToString();
                    int home_id = Int32.Parse(j_token["home"]["id"].ToString());
                    string away_name = j_token["away"]["name"].ToString();
                    int away_id = Int32.Parse(j_token["away"]["id"].ToString());
                    DateTime match_date = (DateTime)j_token["status"]["utcTime"];
                    match_date = match_date.AddHours(1);

                    Match match = new Match(match_id, home_name, home_id, away_name, away_id, match_date);

                    match_list.Add(match);
                }
            }

            //match_list.Sort();

            return match_list;
        }

        public static Match MatchDetails(RestResponse response)
        {
            JObject details = JObject.Parse(response.Content);

            int match_id = Int32.Parse(details["general"]["matchId"].ToString());
            string home_name = details["general"]["homeTeam"]["name"].ToString();
            int home_id = Int32.Parse(details["general"]["homeTeam"]["id"].ToString());
            string away_name = details["general"]["awayTeam"]["name"].ToString();
            int away_id = Int32.Parse(details["general"]["awayTeam"]["id"].ToString());
            DateTime match_date = (DateTime)details["general"]["matchTimeUTCDate"];
            match_date = match_date.AddHours(1);

            return new Match(match_id, home_name, home_id, away_name, away_id, match_date);
        }

        public static Events MatchEvents(RestResponse response)
        {
            JObject details = JObject.Parse(response.Content);

            JToken status = details["header"]["status"];

            Events events = new Events();

            bool finished = (bool)status["finished"];
            bool started = (bool)status["started"];

            int minutes = 0;
            int seconds = 0;

            if (!finished & started)
            {
                string[] live_time = status["liveTime"]["long"].ToString().Split(":");

                if (live_time.Length == 2)
                {
                    minutes = Int32.Parse(live_time[0]);
                    seconds = Int32.Parse(live_time[1]);

                    //Event base_event = new Event((int)EventId.START_STOP, "", minutes, 0, minutes, seconds);
                    //StopStart ss_event = new StopStart(StopStart.Type.SECOND_HALF, base_event);
                    //events.events.Add(ss_event);
                }
                else
                {
                    Event base_event = new Event((int)EventId.START_STOP, "", minutes, 0, minutes, seconds);

                    switch (status["liveTime"]["longKey"].ToString())
                    {
                        case "pause_match":
                            minutes = 45;

                            break;

                        case "finished":
                            minutes = 90;
   
                            break;

                        case "":
                            //StopStart ss_event = new StopStart(StopStart.Type.SECOND_HALF, base_event);
                            //events.events.Add(ss_event);

                            break;

                        default:


                            break;
                            /*case "FT":
                                minutes = 90;
                                break;*/
                    }
                }
            }
            else if (finished & started)
            {
                throw new NotImplementedException();
            }

            int home_score = (int)details["header"]["teams"][0]["score"];
            int away_score = (int)details["header"]["teams"][0]["score"];

            IList<JToken> raw_events = details["content"]["matchFacts"]["events"]["events"].Children().ToList();

            foreach (JToken match_event in raw_events)
            {
                string type = match_event["type"].ToString();

                int? eventId = match_event["eventId"] == null ? null : (int)match_event["eventId"];
                string timestr = match_event["timeStr"].ToString();
                int time = (int)match_event["time"];
                int overload_time = match_event["overloadTime"].ToString().IsNullOrEmpty() ? 0 : (int)match_event["overloadTime"];

                if (Math.Abs(time - minutes) > 2) { minutes = time; seconds = 0; }

                Event base_event;

                if (eventId != null)
                {
                    base_event = new Event((int)eventId, timestr, time, overload_time, minutes, seconds);
                }
                else
                {
                    base_event = new Event(-1, timestr, time, overload_time, minutes, seconds);
                }


                switch (type)
                {
                    case "Goal":
                        Player scorer = new Player((int)match_event["playerId"]);
                        Player? assist = match_event["assistPlayerId"] == null ? null : new Player((int)match_event["assistPlayerId"]);

                        int new_home_score = (int)match_event["newScore"][0];
                        int new_away_score = (int)match_event["newScore"][1];

                        Goal goal = new Goal(scorer, assist, new_home_score, new_away_score, base_event);

                        events.goals.Add(goal);
                        events.events.Add(goal);

                        break;

                    case "Substitution":
                        Player off = new Player((int)match_event["swap"][1]["id"]);
                        Player on = new Player((int)match_event["swap"][0]["id"]);

                        Substitution sub = new Substitution(off, on, base_event);

                        events.subs.Add(sub);
                        events.events.Add(sub);

                        break;

                    case "Card":
                        Player offender = new Player((int)match_event["player"]["id"]);
                        string card_type = match_event["card"].ToString();

                        Card card = new Card(offender, card_type, base_event);

                        events.cards.Add(card);
                        events.events.Add(card);

                        break;

                    case "Half":
                        string half_type = match_event["halfStrShort"].ToString();

                        minutes = Program.clock.Minutes + (Program.clock.Minutes - 45);
                        seconds = Program.clock.Seconds;

                        base_event = new Event((int)EventId.HALF, timestr, time, overload_time, minutes, seconds);

                        Half half = new Half(half_type, base_event);

                        events.halfs.Add(half);
                        events.events.Add(half);

                        break;

                    case "AddedTime":
                        int added = (int)match_event["minutesAddedInput"];

                        base_event = new Event((int)EventId.ADDED_TIME, timestr, time, overload_time, minutes, seconds);

                        AddedTime ad = new AddedTime(added, base_event);

                        events.added.Add(ad);
                        events.events.Add(ad);

                        break;

                    case "VAR":
                        Player involved = new Player((int)match_event["player"]["id"]);

                        bool pending = (bool)match_event["VAR"]["pendingDecision"];
                        string outcome = match_event["VAR"]["decision"]["key"][0].ToString();
                        string reason = match_event["VAR"]["decision"]["key"][1].ToString();

                        VAR var = new VAR(involved, pending, outcome, reason, base_event);

                        events.vars.Add(var);
                        events.events.Add(var);

                        break;

                    default:
                        throw new NotImplementedException(type);
                }
            }

            if (events.halfs.Count > 0 & status["liveTime"]["longKey"].ToString() == "")
            {
                Event base_event = new Event((int)EventId.START_STOP, "", 45, 0, 45, 0);
                StopStart ss_event = new StopStart(StopStart.Type.SECOND_HALF, base_event);
                events.events.Add(ss_event);
            }

            events.minutes = minutes;
            events.seconds = seconds;
            events.home_score = home_score;
            events.away_score = away_score;

            return events;
        }

        public static List<League> SearchLeague (RestResponse response)
        {
            JObject results = JObject.Parse(response.Content);

            IList<JToken> raw_leagues = results["aggregations"]["types"]["buckets"][0]["top_hits"]["hits"]["hits"].Children().ToList();


            List<League> league_list = new List<League>();

            foreach (JToken token in raw_leagues)
            {
                int league_id = Int32.Parse(token["_id"].ToString());
                string country_name = token["_source"]["leagueCountryName"].ToString();
                string league_name = token["_source"]["name"].ToString();


                League league = new League(league_id, league_name, country_name);

                league_list.Add(league);
            }

            return league_list;
        }

        public static List<Match> LeagueMatch(RestResponse response, League league)
        {
            return LeagueMatch(response, league.league_id);
        }

        public static List<Match> LeagueMatch(RestResponse response, int league_id)
        {
            JObject results = JObject.Parse(response.Content);

            IList<JToken> raw_matches = results["matches"]["allMatches"].Children().ToList();

            List<Match> match_list = new List<Match>();

            foreach (JToken token in raw_matches)
            {
                int match_id = Int32.Parse(token["id"].ToString());
                string home_name = token["home"]["name"].ToString();
                int home_id = Int32.Parse(token["home"]["id"].ToString());
                string away_name = token["away"]["name"].ToString();
                int away_id = Int32.Parse(token["away"]["id"].ToString());
                DateTime match_date = (DateTime)token["status"]["utcTime"];

                Match match = new Match(match_id, home_name, home_id, away_name, away_id, match_date);

                match_list.Add(match);
            }

            match_list.Reverse();

            return match_list;
        }

        public static List<String> Countries(RestResponse response)
        {
            JObject results = JObject.Parse(response.Content);

            IList<JToken> raw_countries = results["leagues"]["group"].Children().ToList();

            List<String> countries = new List<String>();

            foreach (JToken country in raw_countries)
            {
                countries.Add(country["@cname"].ToString());
            }

            return countries;
        }

        public static List<League> Leagues(RestResponse response, int country_id) {
            JObject results = JObject.Parse(response.Content);

            List<League> leagues = new List<League>();

            IList<JToken> raw_leagues = results["leagues"]["group"][country_id]["league"].Children().ToList();

            int league_id;
            string league_name;
            string country_name = results["leagues"]["group"][country_id]["@cname"].ToString();

            if (raw_leagues[1].Children().First().ToString() == "0")
            {
                league_id = Int32.Parse(raw_leagues[0].Children().First().ToString());
                league_name = raw_leagues[3].Children().First().ToString();
                
                League league = new League(league_id, league_name, country_name);

                leagues.Add(league);

            } else
            {
                foreach (JToken i_league in raw_leagues)
                {
                    league_id = Int32.Parse(i_league["@id"].ToString());
                    league_name = i_league["@name"].ToString();

                    League league = new League(league_id, league_name, country_name);

                    leagues.Add(league);
                }
            }

            return leagues;
        }
    }
}
