using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MESYP17.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MESYP17.Services.LoginServices
{
    public static class LinkedinServices
    {
        public static readonly string ClientId = "";
        public static readonly string ClientSecret = "";
        public static readonly string RedirectUri = "";

        public static async Task<string> GetAccessTokenAsync(string code)
        {
            var requestUrl =
                "https://www.linkedin.com/oauth/v2/accessToken"
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


        public static async Task<Participant> GetLinkedinProfileAsync(string accessToken)
        {
            var requestUrl = "https://api.linkedin.com/v1/people/~:(id,first-name,last-name,picture-urls::(original))?format=json";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var json = await httpClient.GetStringAsync(requestUrl);
            var linkedinProfile = JsonConvert.DeserializeObject<LinkedinProfile>(json);

            if (linkedinProfile.pictureUrls == null)
            {
                linkedinProfile.pictureUrls = new PictureUrls()
                {
                    values = new List<string> { "https://www.ravensbourne.ac.uk/content/img/default-pupil-profile.png"},
                    _total = 1
                };
            }
            Participant profile = new Participant()
            {
                idParticipant = linkedinProfile.id,
                firstName = linkedinProfile.firstName,
                lastName = linkedinProfile.lastName,
                avatar = linkedinProfile.pictureUrls.values[0]
            };
            return profile;
        }
    }



    class PictureUrls
    {
        public int _total { get; set; }
        public List<string> values { get; set; }
    }

    class LinkedinProfile
    {
        public string firstName { get; set; }
        public string id { get; set; }
        public string lastName { get; set; }
        public PictureUrls pictureUrls { get; set; }
    }
}
