using System.Net.Http;
using System.Threading.Tasks;
using MESYP17.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MESYP17.Services.LoginServices
{
    public static class FacebookServices
    {
        //Get these credentials after creating your app on this link        
        public static readonly string ClientId = "";
        public static readonly string SecretClientId = "";
        public static readonly string RedirectUri = "";
        public static async Task<string> GetAccessTokenAsync(string url)
        {
            if (url.Contains("access_token") && url.Contains("&expires_in="))
            {
                string at="";

                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                {
                    at = url.Replace(RedirectUri + "?#access_token=", "");
                }
                else
                {
                    at = url.Replace(RedirectUri + "?#access_token=", "");
                }

                var accessToken = at.Remove(at.IndexOf("&expires_in="));

                return accessToken;
            }

            return string.Empty;
        }


        public static async Task<Participant> GetFacebookProfileAsync(string accessToken)
        {
            var requestUrl =
                "https://graph.facebook.com/v2.7/me/?fields=first_name,last_name,picture.width(500).height(500),work&access_token="
                + accessToken;

            var httpClient = new HttpClient();

            var userJson = await httpClient.GetStringAsync(requestUrl);

            var facebookProfile = JsonConvert.DeserializeObject<FacebookProfile>(userJson);

            if (facebookProfile.Picture== null)
            {
                facebookProfile.Picture= new FacebookPicture()
                {
                    Data = new Data()
                    {
                        Url = "https://www.ravensbourne.ac.uk/content/img/default-pupil-profile.png"
                    }
                };
            }


            Participant profile = new Participant()
            {
                idParticipant = facebookProfile.Id,
                firstName = facebookProfile.First_Name,
                lastName = facebookProfile.Last_Name,
                avatar = facebookProfile.Picture.Data.Url
            };
            return profile;
        }
    }

    class FacebookProfile
    {
        public string Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public FacebookPicture Picture { get; set; }
    }

    class FacebookPicture
    {
        public Data Data { get; set; }
    }

    class Data
    {
        public bool IsSilhouette { get; set; }
        public string Url { get; set; }
    }

}