// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace MESYP17.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        #endregion


        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }
        public static string UserID
        {
            get
            {
                return AppSettings.GetValueOrDefault("UserID", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("UserID", value);
            }
        }

        public static string UserName
        {
            get
            {
                return AppSettings.GetValueOrDefault("UserName", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("UserName", value);
            }
        }

        public static string PictureUrl
        {
            get
            {
                return AppSettings.GetValueOrDefault("PictureUrl", "https://www.ravensbourne.ac.uk/content/img/default-pupil-profile.png");
                //return AppSettings.GetValueOrDefault("PictureUrl", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("PictureUrl", value);
            }
        }

        public static int Score
        {
            get
            {
                return AppSettings.GetValueOrDefault("Score", 0);
            }
            set
            {
                AppSettings.AddOrUpdateValue("Score", value);
            }
        }

        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault("AccessToken", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("AccessToken", value);
            }
        }

        public static string Provider
        {
            get
            {
                //return AppSettings.GetValueOrDefault("Provider", "facebook");
                return AppSettings.GetValueOrDefault("Provider", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("Provider", value);
            }
        }

        public static string ApiUrl
        {
            get
            {
                return AppSettings.GetValueOrDefault("APIUrl", @"insert here your Rest api link");
            }
        }
    }
}
