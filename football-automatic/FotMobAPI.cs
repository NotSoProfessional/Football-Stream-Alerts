using RestSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;
using System.CodeDom.Compiler;
using System.Collections;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Net.WebRequestMethods;
using System.Xml;

namespace football_automatic
{
    internal class FotMobAPI
    {
        private RestClientOptions _options;
        private RestClient _client;

        private string base_url = "https://www.fotmob.com/api";

        private string matches_url;
        private string leagues_url;
        private string teams_url;
        private string players_url;
        private string match_details_url;
        private string search_url;

        private RestResponse? cache_countries_leagues = null;

        public enum Entity
        {
            MATCH,
            TEAM,
            LEAGUE,
        }

        public FotMobAPI() {
            _options = new RestClientOptions(base_url);
            _client = new RestClient(_options);

            matches_url = base_url + "/matches?";
            leagues_url = base_url + "/leagues?";
            teams_url = base_url + "/teams?";
            players_url = base_url + "/playerData?";
            match_details_url = base_url + "/matchDetails?";
            search_url = "https://apigw.fotmob.com/searchapi" + "/search?";
        }

        // TODO
        private void CheckDate() { }

        private string FormatDate(DateTime date)
        {
            //return date.Year.ToString() + date.Month.ToString() + date.Day.ToString();
            return date.ToString("yyyyMMdd");
        }

        public async Task<RestResponse> GetMatchesByDate(DateTime date)
        {
            string url = matches_url + "date=" + FormatDate(date);

            var request = new RestRequest(url);

            var response = await _client.GetAsync(request);

            return response;
        }

        public async Task<RestResponse> GetLeague(int id, string tab="overview", string type="league")
        {
            string url = leagues_url + "id=" + id + "&tab=" + tab + "&type=" + type;

            var request = new RestRequest(url);

            var response = await _client.GetAsync(request);

            return response;
        }

        public async Task<RestResponse> GetCountriesLeagues()
        {
            if (cache_countries_leagues == null)
            {
                string url = "https://pub.fotmob.com/prod/pub/leagues/live";

                var request = new RestRequest(url);

                var response = await _client.GetAsync(request);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response.Content);

                string json = JsonConvert.SerializeXmlNode(doc);

                response.Content = json;

                cache_countries_leagues = response;

                return response;
            }
            else
            {
                return cache_countries_leagues;
            }
        }

        public async void GetTeam(int id)
        {
            string url = teams_url + "id=" + id;

            var request = new RestRequest(url);

            var response = await _client.GetAsync(request);
        }

        public async void GetPlayer(int id)
        {
            string url = players_url + "id=" + id;

            var request = new RestRequest(url);

            var response = await _client.GetAsync(request);
        }

        public async Task<RestResponse> GetMatchDetails(int id)
        {
            string url = match_details_url + "matchId=" + id;

            var request = new RestRequest(url);

            var response = await _client.GetAsync(request);

            return response;
        }

        public async Task<RestResponse> GetSearch(string term, Entity entity)
        {
            string url = search_url + "term=" + term + "&entity=" + entity.ToString().ToLower() + "&ver=3";

            var request = new RestRequest(url);

            var response = await _client.GetAsync(request);

            return response;
        }
    }
}
