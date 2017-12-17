using System;
using MESYP17.Helpers;
using MESYP17.ViewModels;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace MESYP17.Views
{
    public partial class UserDetailsPage : ContentPage
    {
        ActivityViewModel activityViewModel;
        private ZXingBarcodeImageView barcode;
        private bool b;

        public UserDetailsPage(ActivityViewModel a)
        {
            InitializeComponent();
            b = true;
            Title = "Details";
            
            activityViewModel = a;
            BindingContext = activityViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            #region User Picture And Name
            UserPicture.Source = new UriImageSource()
            {
                Uri = new Uri(Settings.PictureUrl),
                CachingEnabled = true,
                CacheValidity = new TimeSpan(240, 0, 0)
            };

            name.Text = Settings.UserName;
            scoreWorkshop.Text = Settings.Score.ToString();

            #endregion User Picture And Name

            if (b)
            {
                GenerateBarcode(); 
            }
        }

        void GenerateBarcode()
        {
            b = false;
            barcode = new ZXingBarcodeImageView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromHex("#042536")
            };
            barcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
            barcode.BarcodeOptions.Width = 300;
            barcode.BarcodeOptions.Height = 300;
            barcode.BarcodeOptions.Margin = 10;
            barcode.HorizontalOptions = LayoutOptions.End;
            barcode.VerticalOptions = LayoutOptions.Start;
            //barcode.BarcodeValue = "{value: 'jhbhjbj', type: 'participant'}";
            barcode.BarcodeValue = "{value: '"+Settings.UserID+"', type: 'participant'}";


            var gestureRecognizer = new TapGestureRecognizer();
            gestureRecognizer.Tapped += async (s, o) =>
            {
                var fullScreenBarcode = new ZXingBarcodeImageView
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.White
                };
                fullScreenBarcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
                fullScreenBarcode.BarcodeValue = barcode.BarcodeValue;


                var page = new ContentPage()
                {
                    Content = fullScreenBarcode
                };

                await Navigation.PushAsync(page);
            };
            gestureRecognizer.NumberOfTapsRequired = 1;

            barcode.GestureRecognizers.Add(gestureRecognizer);


            //Content = new StackLayout()
            //{
            //    Orientation = StackOrientation.Vertical,
            //    Children =
            //    {
            //        barcode,
            //        stackLayout
            //    }
            //};
            stackLayout.Children.Insert(0,barcode);
            
        }
    }

}
