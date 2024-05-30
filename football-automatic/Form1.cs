using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Globalization;

namespace football_automatic
{
    public partial class Form1 : Form
    {
        TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;

        FotMobAPI api = Program.api;

        List<Match> matches = new List<Match>();
        List<League> leagues = new List<League>();
        List<League> searched_leagues = new List<League>();

        FotMobAPI.Entity select_entity = FotMobAPI.Entity.MATCH;

        //private readonly extern Clock clock = new Clock();
        Clock clock = Program.clock;

        MatchManager mm;

        public Form1()
        {
            InitializeComponent();
            label_clock.DataBindings.Add(new Binding("Text", clock, "DisplayText"));
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            comboBox_searchEntity.Items.AddRange(Enum.GetNames(typeof(FotMobAPI.Entity))
                .Select(name => textInfo.ToTitleCase(name.ToLower())).ToArray());

            comboBox_searchEntity.SelectedIndex = 0;

            var results = await api.GetCountriesLeagues();
            var countries = Parsers.Countries(results);

            comboBox_country.Items.AddRange(countries.ToArray());
            //comboBox_country.SelectedIndex = 0;

            int index = comboBox_country.Items.IndexOf("England");

            comboBox_country.SelectedIndex = (index > -1) ? index : 0;

            /*clock.ClockIntervalChanged += eventTest;

            clock.AddIntervalEvent(5);
            clock.AddIntervalEvent(5);

            clock.AddIntervalCallback(testCallback, 2);
            clock.AddIntervalCallback(testCallback, 2);*/



            Server server = new Server();
            server.Start();

            //webView21.EnsureCoreWebView2Async().Wait();
            //webView21.NavigateToString("https://google.co.uk/");
        }

        private void testCallback()
        {
            Console.WriteLine("Callback occured!");
            clock.RemoveIntervalCallback(testCallback, 2);

        }

        private void eventTest(object sender, ClockIntervalEventArgs e)
        {
            foreach (int interval in e.intervals) Console.WriteLine($"{interval} was reached!");
            clock.RemoveIntervalEvent(5);
        }

        private async void button_search_Click(object sender, EventArgs e)
        {
            var entity = (FotMobAPI.Entity)Enum.ToObject(typeof(FotMobAPI.Entity), comboBox_searchEntity.SelectedIndex);

            UpdateSelectButtonText(entity);

            listBox1.Items.Clear();

            var response = await api.GetSearch(textBox_searchQuery.Text, entity);

            List<IResult> results = new List<IResult>();

            switch (entity)
            {
                case FotMobAPI.Entity.MATCH:
                    matches = Parsers.SearchMatch(response);

                    results = matches.Cast<IResult>().ToList();

                    break;
                case FotMobAPI.Entity.TEAM:

                    break;
                case FotMobAPI.Entity.LEAGUE:
                    searched_leagues = Parsers.SearchLeague(response);

                    results = searched_leagues.Cast<IResult>().ToList();

                    break;
                default:
                    throw new NotImplementedException("Search Entity not yet implemented.");
            }

            listBox1.Items.AddRange(results.ToArray());
        }

        private async void button_gamesToday_Click(object sender, EventArgs e)
        {
            UpdateSelectButtonText(FotMobAPI.Entity.MATCH);

            listBox1.Items.Clear();

            var results = await api.GetMatchesByDate(DateTime.Today);

            matches = Parsers.DateMatch(results);

            foreach (Match match in matches)
            {
                listBox1.Items.Add(match.match_name);
            }
        }

        private async void button_selectMatch(object sender, EventArgs e)
        {
            switch (select_entity)
            {
                case FotMobAPI.Entity.MATCH:
                    Program.selectedMatch = matches[listBox1.SelectedIndex];

                    label_selectedMatch.Text = Program.selectedMatch.ToString();

                    mm = new MatchManager(Program.selectedMatch);
                    break;
                case FotMobAPI.Entity.TEAM:

                    break;
                case FotMobAPI.Entity.LEAGUE:
                    var results = await api.GetLeague(searched_leagues[listBox1.SelectedIndex].league_id);
                    matches = Parsers.LeagueMatch(results, listBox1.SelectedIndex);

                    listBox1.Items.Clear();
                    listBox1.Items.AddRange(matches.ToArray());

                    UpdateSelectButtonText(FotMobAPI.Entity.MATCH);
                    break;
                default:
                    throw new NotImplementedException("Search Entity not yet implemented.");
            }
        }

        private async void comboBox_country_SelectedIndexChanged(object sender, EventArgs e)
        {
            var results = await api.GetCountriesLeagues();
            leagues = Parsers.Leagues(results, comboBox_country.SelectedIndex);

            comboBox_league.Items.Clear();

            comboBox_league.Items.AddRange(leagues.Select(league => league.league_name).ToArray());

            comboBox_league.SelectedIndex = 0;
        }

        private async void comboBox_league_SelectedIndexChanged(object sender, EventArgs e)
        {
            var results = await api.GetLeague(leagues[comboBox_league.SelectedIndex].league_id);
            matches = Parsers.LeagueMatch(results, comboBox_league.SelectedIndex);

            listBox1.Items.Clear();
            listBox1.Items.AddRange(matches.ToArray());

            UpdateSelectButtonText(FotMobAPI.Entity.MATCH);
        }

        private void button_startClock_Click(object sender, EventArgs e)
        {
            clock.Start();
        }

        private void comboBox_searchEntity_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void UpdateSelectButtonText(FotMobAPI.Entity entity)
        {
            select_entity = entity;

            string prefix = "Select";
            string entity_name = Enum.GetName(typeof(FotMobAPI.Entity), entity);

            entity_name = textInfo.ToTitleCase(entity_name.ToLower());

            button1.Text = $"{prefix} {entity_name}";
        }

        private void button_resetClock_Click(object sender, EventArgs e)
        {
            clock.Reset();
        }

        private void button_pauseClock_Click(object sender, EventArgs e)
        {
            clock.Stop();
        }

        private void button_setClock_Click(object sender, EventArgs e)
        {
            Program.clock.Set(Int32.Parse(textBox_setMinutes.Text), Int32.Parse(textBox_setSeconds.Text));
        }

        private void label_clock_Click(object sender, EventArgs e)
        {

        }
    }
}