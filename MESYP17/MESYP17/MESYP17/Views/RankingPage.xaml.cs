using System;
using System.Collections.Generic;
using MESYP17.Helpers;
using Xamarin.Forms;
using MESYP17.ViewModels;

namespace MESYP17.Views
{
    public partial class RankingPage : ContentPage
    {
        ParticipantViewModel participantViewModel;
        public RankingPage(ParticipantViewModel p)
        {
            InitializeComponent();

            participantViewModel = p;
            BindingContext = participantViewModel;

            
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            #region User informations
            UserPicture.Source = new UriImageSource()
            {
                Uri = new Uri(Settings.PictureUrl),
                CachingEnabled = true,
                CacheValidity = new TimeSpan(240, 0, 0)
            };

            UserName.Text = Settings.UserName;
            UserScore.Text = Settings.Score.ToString();
            #endregion User informations
        }
        
        public class Score
        {
            public int idParticipant { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public int scoreBonus { get; set; }
            public int scoreWorkshop { get; set; }
        }
        public class Scores
        {
            public bool error { get; set; }
            public List<Score> scores { get; set; }
        }
    }
}
