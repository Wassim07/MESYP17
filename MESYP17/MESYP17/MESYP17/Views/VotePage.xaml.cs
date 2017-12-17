using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using MESYP17.Helpers;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MESYP17.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VotePage : ContentPage
    {
        private Teams teams;


        public VotePage()
        {
            InitializeComponent();
            Team1Button.IsVisible = false;
            Team2Button.IsVisible = false;
            teams = new Teams();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (CrossConnectivity.Current.IsConnected)
            {
                GetTeams();
            }
        }

        private async void VoteForTeam1(object sender, EventArgs e)
        {
            if (teams.status)
            {

                if (CrossConnectivity.Current.IsConnected)
                {
                    using (HttpClient httpClient = new HttpClient())

                    {
                        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post,
                            Settings.ApiUrl + "/vote");
                        httpRequestMessage.Headers.Add("Provider", Settings.Provider);
                        httpRequestMessage.Headers.Add("Access-Token", Settings.AccessToken);

                        var values = new List<KeyValuePair<string, string>>();
                        values.Add(new KeyValuePair<string, string>("idTeam", teams.team1.idTeam));
                        httpRequestMessage.Content = new FormUrlEncodedContent(values);

                        var response = await httpClient.SendAsync(httpRequestMessage);
                        var json = await response.Content.ReadAsStringAsync();


                        Result r = JsonConvert.DeserializeObject<Result>(json);
                        await DisplayAlert("Message", r.message, "ok");
                    }
                }
            }
        }

        private async void VoteForTeam2(object sender, EventArgs e)
        {
            if (teams.status)
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post,
                            Settings.ApiUrl + "/vote");
                        httpRequestMessage.Headers.Add("Provider", Settings.Provider);
                        httpRequestMessage.Headers.Add("Access-Token", Settings.AccessToken);

                        var values = new List<KeyValuePair<string, string>>();
                        values.Add(new KeyValuePair<string, string>("idTeam", teams.team2.idTeam));
                        httpRequestMessage.Content = new FormUrlEncodedContent(values);

                        var response = await httpClient.SendAsync(httpRequestMessage);
                        var json = await response.Content.ReadAsStringAsync();


                        Result r = JsonConvert.DeserializeObject<Result>(json);
                        
                        await DisplayAlert("Message", r.message, "ok");
                    }
                }
            }
        }


        public async void GetTeams()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                        Settings.ApiUrl + "/getTeams");

                    var response = await httpClient.SendAsync(httpRequestMessage);
                    var json = await response.Content.ReadAsStringAsync();


                    Vote v = JsonConvert.DeserializeObject<Vote>(json);

                    try
                    {

                        if (v != null)
                        {
                            teams = v.results;
                            Team1Name.Text = teams.team1.teamName;
                            Team2Name.Text = teams.team2.teamName;

                            Team1Button.IsVisible = teams.status;
                            Team2Button.IsVisible = teams.status;

                            Team1Button.IsEnabled = teams.status;
                            Team2Button.IsEnabled = teams.status;
                            
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
    }

    class Team1
    {
        public string teamName { get; set; }
        public string idTeam { get; set; }
    }

    class Team2
    {
        public string teamName { get; set; }
        public string idTeam { get; set; }
    }

    class Teams
    {
        public bool status { get; set; }
        public Team1 team1 { get; set; }
        public Team2 team2 { get; set; }
    }

    class Vote
    {
        public Teams results { get; set; }
        public bool error { get; set; }
    }


}