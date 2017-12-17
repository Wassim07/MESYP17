using System.Net.Http;
using System.Threading.Tasks;
using MESYP17.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MESYP17.Services.LoginServices
{
    public static class GoogleServices
    {
        public static readonly string ClientId = "";
        public static readonly string ClientSecret = "";
        public static readonly string RedirectUri = "";
        public static async Task<string> GetAccessTokenAsync(string code)
        {
            var requestUrl =
                "https://www.googleapis.com/oauth2/v4/token"
                + "?code=" + code
                + "&client_id=" + ClientId
                + "&client_secret=" + ClientSecret
               + "&redirect_uri=" + RedirectUri
            + "&grant_type=authorization_code";

            var httpClient = new HttpClient();

            var response = await httpClient.PostAsync(requestUrl, null);

            var json = await response.Content.ReadAsStringAsync();

            var accessToken = JsonConvert.DeserializeObject<JObject>(json).Value<string>("access_token");

            return accessToken;
        }

        public static async Task<Participant> GetGoogleProfileAsync(string accessToken)
        {
            var requestUrl = "https://www.googleapis.com/plus/v1/people/me?access_token=" + accessToken;

            var httpClient = new HttpClient();

            var userJson = await httpClient.GetStringAsync(requestUrl);

            var googleProfile = JsonConvert.DeserializeObject<GoogleProfile>(userJson);

            Participant profile = new Participant()
            {
                idParticipant= googleProfile.Id,
                firstName= googleProfile.DisplayName,
                avatar = googleProfile.Image.Url
            };

            return profile;
        }
    }

    class GoogleProfile
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public GooglePicture Image { get; set; }
    }

    class GooglePicture
    {
        public string Url { get; set; }
        public bool IsDefault { get; set; }
    }
}

