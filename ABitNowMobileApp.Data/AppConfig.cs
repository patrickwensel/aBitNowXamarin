using System;
namespace ABitNowMobileApp.Data
{
    public class AppConfig
    {
        // Production url
        public static string BaseUrl = "https://someurl.com/api/v1";

        // Development url
        //public static string BaseUrl = "https://stagging-someurl.com/api/v1";

        public static TimeSpan HttpRequestTimeout = TimeSpan.FromSeconds(90);
    }
}
