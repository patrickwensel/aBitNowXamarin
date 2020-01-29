using ABitNowMobileApp.Interfaces;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace ABitNowMobileApp.Services
{
    public class PersistanceService : IPersistanceService
    {
        private const string UserEmailKey = "UserEmail";
        private const string PasswordKey = "Password";
        private const string IsLoggedInKey = "IsLoggedIn";

        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public bool IsLoggedIn
        {
            get => AppSettings.GetValueOrDefault(IsLoggedInKey, false);
            set => AppSettings.AddOrUpdateValue(IsLoggedInKey, value);
        }

        public string UserEmail
        {
            get => AppSettings.GetValueOrDefault(UserEmailKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(UserEmailKey, value);
        }

        public string Password
        {
            get => AppSettings.GetValueOrDefault(PasswordKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(PasswordKey, value);
        }
    }
}
