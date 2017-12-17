using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MESYP17.Helpers;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace MESYP17.Views
{
    public partial class ScanPage : ContentPage
    {
        public ScanPage()
        {
            InitializeComponent();
            Title = "Scan";
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (CrossConnectivity.Current.IsConnected)
            {
                await GetScannedList();
            }
            else
            {
                await DisplayAlert("Error", "Please Check your internet Connexion", "ok");
            }
        }

        private async void ScanButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CameraScanningPage());
        }


        private async Task GetScannedList()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post,
                        Settings.ApiUrl + "/getParticipantScannees");
                    httpRequestMessage.Headers.Add("Provider", Settings.Provider);
                    httpRequestMessage.Headers.Add("Access-Token", Settings.AccessToken);
                    var response = await httpClient.SendAsync(httpRequestMessage);
                    var json = await response.Content.ReadAsStringAsync();
                    Scannees sc = JsonConvert.DeserializeObject<Scannees>(json);

                    #region Country Switch
                    foreach (var scannee in sc.scannees)
                    {
                        switch (scannee.country)
                        {
                            case "Algeria":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "DZ"); break;
                            case "Bahrain":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "BH"); break;
                            case "Cyprus":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "CY"); break;
                            case "Egypt":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "EG"); break;
                            case "France":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "FR"); break;
                            case "Germany":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "DE"); break;
                            case "Greece":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "GR"); break;
                            case "India":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "IN"); break;
                            case "Jordan":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "JO"); break;
                            case "Kuwait":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "KW"); break;
                            case "Lebanon":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "LB"); break;
                            case "Libya":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "LY"); break;
                            case "Pakistan":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "PK"); break;
                            case "Palestine":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "PS"); break;
                            case "Saudi Arabia":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "SA"); break;
                            case "Spain":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "ES"); break;
                            case "Sudan":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "SD"); break;
                            case "Sudan Subsection":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "SD"); break;
                            case "Tunisia":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "TN"); break;
                            case "UK":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "GB"); break;
                            case "Ukraine":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "UA"); break;
                            case "United Arab Emirates":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "AE"); break;
                            case "USA":
                                scannee.country = String.Format("http://www.geognos.com/api/en/countries/flag/{0}.png", "US"); break;
                        }
                    }
                    #endregion Country Switch


                    List<ScannedParticipants> s = new List<ScannedParticipants>();
                    int x = sc.scannees.Count / 3;
                    for (int i = 0; i < 3 * x; i += 3)
                    {
                        s.Add(new ScannedParticipants()
                        {
                            avatar1 = sc.scannees[i].avatar,
                            avatar2 = sc.scannees[i + 1].avatar,
                            avatar3 = sc.scannees[i + 2].avatar,

                            country1 = sc.scannees[i].country,
                            country2 = sc.scannees[i + 1].country,
                            country3 = sc.scannees[i + 2].country,

                            fullName1 = sc.scannees[i].firstName + " " + sc.scannees[i].lastName,
                            fullName2 = sc.scannees[i + 1].firstName + " " + sc.scannees[i + 1].lastName,
                            fullName3 = sc.scannees[i + 2].firstName + " " + sc.scannees[i + 2].lastName,
                        });
                    }

                    switch (sc.scannees.Count % 3)
                    {
                        case 0:
                            break;
                        case 1:
                            s.Add(new ScannedParticipants()
                            {
                                avatar1 = sc.scannees[x * 3].avatar,
                                country1 = sc.scannees[x * 3].country,
                                fullName1 = sc.scannees[x * 3].firstName + " " + sc.scannees[x * 3].lastName,

                                avatar2 = "",
                                country2 = "",
                                fullName2 = "",

                                avatar3 = "",
                                country3 = "",
                                fullName3 = "",
                            })
                                ;
                            break;
                        case 2:
                            s.Add(new ScannedParticipants()
                            {
                                avatar1 = sc.scannees[x * 3].avatar,
                                country1 = sc.scannees[x * 3].country,
                                fullName1 = sc.scannees[x * 3].firstName + " " + sc.scannees[x * 3].lastName,

                                avatar2 = sc.scannees[x * 3 + 1].avatar,
                                country2 = sc.scannees[x * 3 + 1].country,
                                fullName2 = sc.scannees[x * 3 + 1].firstName + " " + sc.scannees[x * 3 + 1].lastName,

                                avatar3 = "",
                                country3 = "",
                                fullName3 = "",
                            })
                                ;
                            break;
                    }

                    RecentlyScannedList.ItemsSource = s;
                }
            }
        }

    }

    #region test

    public class ScannedParticipants
    {
        public string avatar1 { get; set; }
        public string fullName1 { get; set; }
        public string country1 { get; set; }

        public string avatar2 { get; set; }
        public string fullName2 { get; set; }
        public string country2 { get; set; }

        public string avatar3 { get; set; }
        public string fullName3 { get; set; }
        public string country3 { get; set; }

    }
    #endregion test


    public class Scannee
    {
        public int idParticipant { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string avatar { get; set; }
        public string country { get; set; }
    }

    public class Scannees
    {
        public bool error { get; set; }
        public List<Scannee> scannees { get; set; }
    }
}

