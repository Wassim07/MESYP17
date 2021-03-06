﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using MESYP17.Helpers;
using MESYP17.Models;
using MESYP17.ViewModels;
using MESYP17.Views;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.LocalNotifications;
using SQLite;
using Xamarin.Forms;
using XamarinForms.SQLite.SQLite;


namespace MESYP17
{
    public partial class App : Application
    {
        private ActivityViewModel activityViewModel;
        ActivityViewModel activities;
        private ParticipantViewModel participantViewModel;
        private SQLiteConnection _sqLiteConnection;


        public App()
        {
            InitializeComponent();
            _sqLiteConnection = DependencyService.Get<ISQLite>().GetConnection();

            activityViewModel = new ActivityViewModel();
            activityViewModel.Activities = new ObservableCollection<Activity>();

            activities = new ActivityViewModel();
            activities.Activities = new ObservableCollection<Activity>();

            participantViewModel = new ParticipantViewModel();
            participantViewModel.Participants = new ObservableCollection<Participant>();

            MainPage = new NavigationPage(new ContentPage()
            {
                BackgroundColor = Color.FromHex("#042536")
            });
        }

        private async void SetMainPage()
        {

            /* First Use of the Application */
            if (String.IsNullOrEmpty(Settings.AccessToken))
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {

                Result requestResult = new Result();

                if (CrossConnectivity.Current.IsConnected)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post,
                            Settings.ApiUrl + "/login");
                        httpRequestMessage.Headers.Add("Provider", Settings.Provider);
                        httpRequestMessage.Headers.Add("Access-Token", Settings.AccessToken);
                        var response = await httpClient.SendAsync(httpRequestMessage);
                        var json = await response.Content.ReadAsStringAsync();
                        requestResult = JsonConvert.DeserializeObject<Result>(json);
                    }
                }

                if (requestResult.error)
                {
                    MainPage = new NavigationPage(new LoginPage());
                }
                else
                {
                    GetData();


                    #region nextPage Definition
                    var nextPage = new TabbedPage()
                    {
                        Title = "MESYP17",
                        Children =
                            {
                                new ActivitiesPage(activities),
                                new RankingPage(participantViewModel),
                                new ScanPage(),
                                new UserDetailsPage(activityViewModel)
                            }
                    };

                    #region Toolbar

                    /* Item 1 */
                    var contact = new ToolbarItem()
                    {
                        Text = "Contact",
                        Order = ToolbarItemOrder.Secondary
                    };
                    contact.Clicked += async (s, e2) =>
                    {
                        await nextPage.Navigation.PushAsync(new ContactUsPage());
                    };
                    nextPage.ToolbarItems.Add(contact);

                    /* Item 2 */
                    var sponsors = new ToolbarItem()
                    {
                        Text = "Sponsors",
                        Order = ToolbarItemOrder.Secondary
                    };
                    sponsors.Clicked += (s, e2) =>
                    {
                        Device.OpenUri(
                            new Uri("http://me-syp.ieee.tn/sponsors.html#sponsorsList"));
                    };

                    nextPage.ToolbarItems.Add(sponsors);

                    /* Item 3 */
                    var about = new ToolbarItem()
                    {
                        Text = "About",
                        Order = ToolbarItemOrder.Secondary
                    };
                    about.Clicked += async (s, e2) =>
                    {
                        await
                            nextPage.DisplayAlert("About: ",
                                "The MESYP 17 application is a mobile application designed and created by the MESYP development team exclusively for the congress participants.\n\nThis application is an essential tool that will facilitate and help the participants in the follow - up of the congress, in the organization and the follow - up of a specific planning to each,in making good plans in Tunisia and more!\n\nMESYP'17 is multiplatform available in IOS, Android and Windows Phone.\n\nThe functionalities of this application are:\n\n -Get Forecast: Thanks to the geolocalization, Participants will get the possibility to consult the temperature of various places in Tunisia at every moment including that of Gammarth, Tunis, where the congress will be held.\n\n  - The Notification of any activity during the congress: A detailed reminder notice will be sent to you before the start of each activity throughout the congress.Place, time, name of the activity, name of workshop, speaker name etc.will be included in each notification.\n\n  - Possibility to consult the various places near the hotel by Vynd Tool.\n\n  - Appnext: Possibility to consult the specific schedule for each participant during the congress or per day.\n\n  - Gain bonus points: Several challenges will allow the participants to collect points by scanning QR codes that will be available at the end of each challenge or by scanning other participants QR Codes, encouraging  networking with other participants, creating connection between them and involving them in exchanging and discovering new persons.Collect points will increase participants’ chances of winning a  prizes during the awards ceremony.\n\n  - possibility to vote in the third phase of the DOA competition.\n\n - Infoline: A list containing the contacts of all the members of the committee will be available in case of need.",
                                "ok");
                    };

                    nextPage.ToolbarItems.Add(about);

                    #endregion Toolbar

                    if (DateTime.Now.Year.Equals(2017) && DateTime.Now.Month.Equals(8) && DateTime.Now.Day.Equals(3) && DateTime.Now.Hour >= 21 && DateTime.Now.Minute > 30)
                    {
                        #region Toolbar When Vote is Open

                        /* Item 4 */
                        var vote = new ToolbarItem()
                        {
                            Text = "Vote",
                            Order = ToolbarItemOrder.Secondary
                        };
                        vote.Clicked += async (s, e2) =>
                        {
                            await nextPage.Navigation.PushAsync(new VotePage());
                        };
                        nextPage.ToolbarItems.Add(vote);
                        #endregion Toolbar When Vote is Open
                    }

                    #endregion nextPage Definition
                    MainPage = new NavigationPage(nextPage);
                }

            }


        }

        protected override void OnStart()
        {
            SetMainPage();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }


        async void GetData()
        {
            if (String.IsNullOrEmpty(Settings.AccessToken))
            {
            }

            else
            {
                /* GetActivitiesData */
                if (CrossConnectivity.Current.IsConnected)
                {
                    _sqLiteConnection.DropTable<Activity>();
                    _sqLiteConnection.CreateTable<Activity>();


                    activityViewModel.Activities = await activityViewModel.GetAllActivitiesList();
                    #region ActivityPage Activities List

                    try
                    {
                        foreach (var activity in activityViewModel.Activities)
                        {
                            string d = activity.date;
                            string date = d.Split()[0];
                            DateTime d1 = DateTime.Parse(date);

                            if (d1.Day.Equals(DateTime.Now.Day))
                            {
                                activities.Activities.Add(activity);
                            }
                        }
                    }
                    catch (Exception)
                    { }
                    #endregion ActivityPage Activities List

                    #region ActivityPage Notification
                    
                    try
                    {

                        foreach (var acti in activities.Activities)
                        {
                            DateTime date1 = DateTime.Parse(acti.date);


                            
                            if (date1.Minute < 15)
                            {
                                DateTime date2 = new DateTime(2017, 7, date1.Day, date1.Hour - 1, date1.Minute - 15 + 60, 0);

                                DateTime showDate = new DateTime(2017, 8, date2.Day, date2.Hour, date2.Minute, 00);
                            
                                if (DateTime.Now < showDate)
                                {
                                    CrossLocalNotifications.Current.Show(acti.name, acti.description, acti.idActivity, showDate);
                                }
                            }

                            else
                            {
                                DateTime show = new DateTime(date1.Year, date1.Month, date1.Day, date1.Hour, date1.Minute - 15, 00);
                            
                                if (DateTime.Now < show)
                                {
                                    CrossLocalNotifications.Current.Show(acti.name, acti.description, acti.idActivity, show);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    #endregion ActivityPage Notification


                    _sqLiteConnection.InsertAll(activityViewModel.Activities);
                }

                else
                {
                    activityViewModel.Activities.Clear();
                    for (int i = 0; i < _sqLiteConnection.Table<Activity>().Count(); i++)
                    {
                        activityViewModel.Activities.Add(_sqLiteConnection.Table<Activity>().ElementAt(i));
                    }
                }


                // GetRankingData

                if (CrossConnectivity.Current.IsConnected)
                {
                    _sqLiteConnection.DropTable<Participant>();
                    _sqLiteConnection.CreateTable<Participant>();

                    participantViewModel.Participants = await participantViewModel.GetRanking();
                    _sqLiteConnection.InsertAll(participantViewModel.Participants);
                }

                else
                {
                    for (int i = 0; i < _sqLiteConnection.Table<Participant>().Count(); i++)
                    {
                        participantViewModel.Participants.Add(_sqLiteConnection.Table<Participant>().ElementAt(i));
                    }
                }



                // GetUserInfos
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                            Settings.ApiUrl + "/getParticipantInfo");
                        httpRequestMessage.Headers.Add("Provider", Settings.Provider);
                        httpRequestMessage.Headers.Add("Access-Token", Settings.AccessToken);
                        var response = await httpClient.SendAsync(httpRequestMessage);
                        var json = await response.Content.ReadAsStringAsync();

                        ParticipantViewModel p = JsonConvert.DeserializeObject<ParticipantViewModel>(json);

                        if (p != null)
                        {
                            if (Settings.UserID.Equals(""))
                            {
                                Settings.UserID = p.Participants[0].idParticipant;
                            }
                            if (Settings.PictureUrl.Equals("https://www.ravensbourne.ac.uk/content/img/default-pupil-profile.png"))
                            {
                                Settings.PictureUrl = p.Participants[0].avatar; 
                            }
                            if (Settings.UserName.Equals(""))
                            {
                                Settings.UserName = p.Participants[0].firstName + " " + p.Participants[0].lastName; 
                            }
                            if (Settings.Score==0)
                            {
                                Settings.Score = p.Participants[0].scoreWorkshop + p.Participants[0].scoreBonus; 
                            }
                        }
                    }
                }



                // GetUserScore
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {

                        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post,
                            Settings.ApiUrl + "/getParticipantScore");
                        httpRequestMessage.Headers.Add("Provider", Settings.Provider);
                        httpRequestMessage.Headers.Add("Access-Token", Settings.AccessToken);
                        var response = await httpClient.SendAsync(httpRequestMessage);
                        var json = await response.Content.ReadAsStringAsync();

                        RankingPage.Scores s = JsonConvert.DeserializeObject<RankingPage.Scores>(json);
                        if (s.scores != null)
                        {
                            Settings.Score = s.scores[0].scoreWorkshop + s.scores[0].scoreBonus;
                        }
                    }
                }
            }
            _sqLiteConnection.Close();
        }
    }
}
