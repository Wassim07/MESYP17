using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MESYP17.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace MESYP17.Views
{
    public partial class CameraScanningPage : ContentPage
    {
        bool boolean = true;
        ZXingScannerView zxing;
        ZXingDefaultOverlay overlay;

        public CameraScanningPage()
        {
            Title = "Scanning";

            zxing = new ZXingScannerView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            zxing.OnScanResult += (result) =>
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        if (boolean)
                        {
                            boolean = false;
                            
                            // Stop analysis until we navigate away so we don't keep reading barcodes
                            zxing.IsAnalyzing = false;
                            
                            await Scan(result);

                            //// Navigate away
                            await Navigation.PopAsync();
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "There's no Internet Connexion", "ok");
                    }
                });

            overlay = new ZXingDefaultOverlay
            {
                TopText = "Hold your phone up to the barcode",
                BottomText = "Scanning will happen automatically",
                ShowFlashButton = zxing.HasTorch,
            };
            overlay.FlashButtonClicked += (sender, e) =>
            {
                zxing.IsTorchOn = !zxing.IsTorchOn;
            };

            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            grid.Children.Add(zxing);
            grid.Children.Add(overlay);


            // The root page of your application
            Content = grid;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            zxing.IsScanning = true;
        }

        protected override void OnDisappearing()
        {
            zxing.IsScanning = false;
            base.OnDisappearing();
        }

    private async Task Scan(ZXing.Result result)
        {

            using (HttpClient httpClient = new HttpClient())
            {
                ////  { 
                ////    value: '4164',
                ////    type: 'participant/workshop/competition
                ////  }

                string url ="";
                string parametre="";
                try
                {
                    dynamic qrCode = JObject.Parse(result.Text);
                    string type = qrCode.type;
                    string value = qrCode.value;

                    if (type.Contains("participant"))
                    {
                        url = "/addScannee";
                        parametre = "idScannee";
                    }
                    else if (type.Contains("workshop"))
                    {
                        url = "/addBonusWorkshop";
                        parametre = "idWorkshop";
                    }
                    else if (type.Contains("competition"))
                    {
                        url = "/consumeQRCode";
                        parametre = "code";
                    }


                    if (url!="")
                    {

                        HttpRequestMessage httpRequestMessage1 = new HttpRequestMessage(HttpMethod.Post,
                            Settings.ApiUrl + url);
                        httpRequestMessage1.Headers.Add("Provider", Settings.Provider);
                        httpRequestMessage1.Headers.Add("Access-Token", Settings.AccessToken);

                        var values = new List<KeyValuePair<string, string>>();
                        values.Add(new KeyValuePair<string, string>(parametre, value));
                        httpRequestMessage1.Content = new FormUrlEncodedContent(values);

                        var response1 = await httpClient.SendAsync(httpRequestMessage1);
                        var json1 = await response1.Content.ReadAsStringAsync();
                        Result r=JsonConvert.DeserializeObject<Result>(json1);

                        // Show an alert
                        await DisplayAlert("Scanned Barcode", r.message, "OK");

                        HttpRequestMessage httpRequestMessage2 = new HttpRequestMessage(HttpMethod.Post,
                          Settings.ApiUrl + "/getParticipantScore");
                        httpRequestMessage2.Headers.Add("Provider", Settings.Provider);
                        httpRequestMessage2.Headers.Add("Access-Token", Settings.AccessToken);
                        var response2 = await httpClient.SendAsync(httpRequestMessage2);
                        var json2 = await response2.Content.ReadAsStringAsync();

                        RankingPage.Scores s = JsonConvert.DeserializeObject<RankingPage.Scores>(json2);
                        if (s.scores != null)
                        {
                            Settings.Score = s.scores[0].scoreWorkshop + s.scores[0].scoreBonus;
                        }
                    }

                }
                catch (System.Exception e)
                {
                    await DisplayAlert("Error", "Invalid QrCode", "ok");
                }
            }
        }

    }
}