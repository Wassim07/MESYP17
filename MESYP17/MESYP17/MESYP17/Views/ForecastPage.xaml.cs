using MESYP17.Services.WeatherServices;
using Xamarin.Forms;

namespace MESYP17.Views
{
    public partial class ForecastPage : ContentPage
    {

        public ForecastPage(WeatherForecastRoot f)
        {
            InitializeComponent();
            ListViewWeather.ItemsSource = f.Items;
        }
        public ForecastPage()
        {
            InitializeComponent();
        }
    }
}
