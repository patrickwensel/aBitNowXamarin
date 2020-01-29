
using ABitNowMobileApp.Data;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Plugin.Permissions;
using Xamarin.Forms;

namespace ABitNowMobileApp.Droid
{
    [Activity(Label = "ABitNowMobileApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
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
            UserDialogs.Init(() => this);

            WireupDependency();
            AppContentResolver = this.ContentResolver;
            Instance = this;

            App.ScreenSize = new Xamarin.Forms.Size(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density, Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            App.StatusBarHeight = GetStatusBarHeight();

            var application = (Xamarin.Forms.Application)ContainerManager.Container.Resolve(typeof(App), typeof(App).GetType().ToString());
            LoadApplication(application);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void WireupDependency()
        {
            // Wire up dependencies of core classes
            ContainerManager.Initialize();
            DataBootstrapper.Initialize(ContainerManager.Container);
            FormsBootstrapper.Initialize(ContainerManager.Container);
            AndroidBootstrapper.Initialize(ContainerManager.Container);
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

        private double GetNavigationBarHeight()
        {
            double navBarHeight = -1;
            int resourceId = this.Resources.GetIdentifier("navigation_bar_height", "dimen", "android");

            if (resourceId > 0)
            {
                navBarHeight = this.Resources.GetDimensionPixelSize(resourceId) / Resources.DisplayMetrics.Density;
            }

            return navBarHeight;
        }
    }
}
