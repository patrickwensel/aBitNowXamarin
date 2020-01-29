using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace ABitNowMobileApp.Droid
{
    [Activity(Label = "McClain Wines", NoHistory = true, Theme = "@style/Theme.Splash", ConfigurationChanges = ConfigChanges.ScreenSize, ScreenOrientation = ScreenOrientation.Portrait, ClearTaskOnLaunch = true, LaunchMode = LaunchMode.SingleTop)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.NewTask);
            intent.AddFlags(ActivityFlags.ClearTop);
            StartActivity(intent);
            Finish();
        }
    }
}
