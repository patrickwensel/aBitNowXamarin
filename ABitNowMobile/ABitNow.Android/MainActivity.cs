using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;
using Android.Content;
using Xamarin.Forms;
using Plugin.Permissions;

namespace ABitNow.Droid
{
    [Activity(Label = "ABitNow", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static ContentResolver AppContentResolver;
        public static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            UserDialogs.Init(()=>this);

            AppContentResolver = this.ContentResolver;
            Instance = this;

            App.ScreenSize = new Xamarin.Forms.Size(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density, Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            App.StatusBarHeight = GetStatusBarHeight();

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private double GetStatusBarHeight()
        {
            double statusBarHeight = -1;
            int resourceId = this.Resources.GetIdentifier("status_bar_height", "dimen", "android");

            if (resourceId > 0)
            {
                statusBarHeight = this.Resources.GetDimensionPixelSize(resourceId) / Resources.DisplayMetrics.Density;
            }

            return statusBarHeight;
        }

        //private double GetNavigationBarHeight()
        //{
        //    double navBarHeight = -1;
        //    int resourceId = this.Resources.GetIdentifier("navigation_bar_height", "dimen", "android");

        //    if (resourceId > 0)
        //    {
        //        navBarHeight = this.Resources.GetDimensionPixelSize(resourceId) / Resources.DisplayMetrics.Density;
        //    }

        //    return navBarHeight;
        //}
    }
}