using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using MESYP17.Helpers;
using MESYP17.Models;
using MESYP17.Views;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace MESYP17.ViewModels
{
    public class ParticipantViewModel : INotifyPropertyChanged
    {
        public ParticipantViewModel()
        {
            _refreshCommand = new Command(RefreshList);
        }

        public bool Error { get; set; }

        ObservableCollection<Participant> _participants;
        public ObservableCollection<Participant> Participants
        {
            get { return _participants; }
            set
            {
                _participants = value;
                OnPropertyChanged(nameof(Participants));
            }
        }



        Command _refreshCommand;
        public Command RefreshCommand
        {
            get
            {
                return _refreshCommand;
            }
        }


        bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }


        public async void RefreshList()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                Participants = await GetRanking();
            }
        }



        public async Task<ObservableCollection<Participant>> GetRanking()
        {
            IsBusy = true;

            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage httpRequestMessage1 = new HttpRequestMessage(HttpMethod.Post,
                    Settings.ApiUrl + "/getParticipantsScores");

                var response1 = await httpClient.SendAsync(httpRequestMessage1);
                var json1 = await response1.Content.ReadAsStringAsync();
                ParticipantViewModel p = JsonConvert.DeserializeObject<ParticipantViewModel>(json1);

                foreach (var participant in p.Participants)
                {
                    participant.fullName = participant.firstName + " " + participant.lastName;
                    participant.score = participant.scoreBonus + participant.scoreWorkshop;

                    #region Country Switch
                    

                    switch (participant.country)
                    {
                        case "Algeria":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "DZ"); break;
                        case "Bahrain":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "BH"); break;
                        case "Cyprus":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "CY"); break;
                        case "Egypt":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "EG"); break;
                        case "France":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "FR"); break;
                        case "Germany":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "DE"); break;
                        case "Greece":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "GR"); break;
                        case "India":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "IN"); break;
                        case "Jordan":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "JO"); break;
                        case "Kuwait":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "KW"); break;
                        case "Lebanon":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "LB"); break;
                        case "Libya":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "LY"); break;
                        case "Pakistan":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "PK"); break;
                        case "Palestine":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "PS"); break;
                        case "Saudi Arabia":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "SA"); break;
                        case "Spain":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "ES"); break;
                        case "Sudan":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "SD"); break;
                        case "Sudan Subsection":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "SD"); break;
                        case "Tunisia":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "TN"); break;
                        case "UK":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "GB"); break;
                        case "Ukraine":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "UA"); break;
                        case "United Arab Emirates":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "AE"); break;
                        case "USA":
                            participant.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "US"); break;
                    }
                    #endregion Country Switch
                }


                HttpRequestMessage httpRequestMessage2 = new HttpRequestMessage(HttpMethod.Post,
                           Settings.ApiUrl + "/getParticipantScore");
                httpRequestMessage2.Headers.Add("Provider", Settings.Provider);
                httpRequestMessage2.Headers.Add("Access-Token", Settings.AccessToken);
                var response2 = await httpClient.SendAsync(httpRequestMessage2);
                var json2 = await response2.Content.ReadAsStringAsync();

                RankingPage.Scores s = JsonConvert.DeserializeObject<RankingPage.Scores>(json2);
                if (s.scores != null)
                {
                    Settings.Score = s.scores[0].scoreWorkshop + s.scores[0].scoreBonus;
                }


                _participants = p.Participants;
            }

            IsBusy = false;
            return _participants;
        }


        #region INotifyPropertyChanged implementation

        //To let the UI know that something changed on the View Model
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
