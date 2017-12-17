using System;
using System.Collections.ObjectModel;
using MESYP17.Models;
using MESYP17.Services.WeatherServices;
using MESYP17.ViewModels;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace MESYP17.Views
{
    public partial class ActivitiesPage : ContentPage
    {
        ActivityViewModel activityViewModel;
        bool boolean;
        public WeatherForecastRoot Forecast;
        public WeatherRoot weatherRoot;

        public ActivitiesPage(ActivityViewModel a)
        {
            InitializeComponent();
            boolean = true;
            activityViewModel = a;

            BindingContext = activityViewModel;


            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
            {
            }
            else
            {
                GetWeather();
            }
        }

        private async void GetWeather()
        {
            try
            {
                //if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                //{
                //    var gps = await CrossGeolocator.Current.GetPositionAsync(10000);
                //    weatherRoot = await new WeatherService().GetWeather(gps.Latitude, gps.Longitude);
                //    Temp.Text = $"Temp: {weatherRoot?.MainWeather?.Temperature ?? 0}°{"C"}";
                //    im.Source = weatherRoot.DisplayIcon;
                //    Condition.Text = $"{weatherRoot.Name}: {weatherRoot?.Weather?[0]?.Description ?? string.Empty}";
                //}
                //else
                //{
                if (CrossGeolocator.IsSupported &&
                    CrossGeolocator.Current.IsGeolocationAvailable &&
                    CrossGeolocator.Current.IsGeolocationEnabled &&
                    CrossConnectivity.IsSupported &&
                    CrossConnectivity.Current.IsConnected
                  )
                {
                    var gps = await CrossGeolocator.Current.GetPositionAsync(new TimeSpan(10000));
                    weatherRoot = await new WeatherService().GetWeather(gps.Latitude, gps.Longitude);
                    Temp.Text = $"Temp: {weatherRoot?.MainWeather?.Temperature ?? 0}°{"C"}";
                    im.Source = weatherRoot.DisplayIcon;
                    Condition.Text = $"{weatherRoot.Name}: {weatherRoot?.Weather?[0]?.Description ?? string.Empty}";
                }
                else Temp.Text = "Unable to get Weather";
                //}
            }
            catch (Exception ex)
            {
                Temp.Text = "Unable to get Weather";
            }
        }
        private async void GetForecast(object sender, EventArgs e)
        {
            if (weatherRoot == null)
            {
                await DisplayAlert("Notification", "In order to get informations about the weather you need to activate your GPS and Restart the application", "ok");
            }
            else
            {
                //Get forecast based on cityId
                Forecast = await new WeatherService().GetForecast(weatherRoot.CityId);
                await Navigation.PushAsync(new ForecastPage(Forecast));
            }

            boolean = false;
        }
    }

}
