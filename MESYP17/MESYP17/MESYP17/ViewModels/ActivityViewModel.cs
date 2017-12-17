using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using MESYP17.Helpers;
using MESYP17.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace MESYP17.ViewModels
{

    public class ActivityViewModel : INotifyPropertyChanged
    {
        public ActivityViewModel()
        {
        }

        public bool Error { get; set; }

        ObservableCollection<Activity> _activities;
        public ObservableCollection<Activity> Activities
        {
            get { return _activities; }
            set
            {
                _activities = value;
                OnPropertyChanged(nameof(Activities));
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
        
        public async Task<ObservableCollection<Activity>> GetAllActivitiesList()
        {
            IsBusy = true;
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post,
                    Settings.ApiUrl + "/getParticipantActivities");
                httpRequestMessage.Headers.Add("Provider", Settings.Provider);
                httpRequestMessage.Headers.Add("Access-Token", Settings.AccessToken);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var json = await response.Content.ReadAsStringAsync();

                ActivityViewModel a = JsonConvert.DeserializeObject<ActivityViewModel>(json);
                _activities = a.Activities;
                Debug.WriteLine(json);


            }

            IsBusy = false;
            return _activities;
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
