using ABitNow.Bootstrap;
using ABitNow.Contracts.Services.General;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace ABitNow
{
    public partial class App : Xamarin.Forms.Application
    {
        public static double SafeAreaTopMargin = 0;
        public static double SafeAreaBottomMargin = 0;
        public static Size ScreenSize;
        public static double StatusBarHeight;
        public static double NavigationBarHeight = 44;

        public App()
        {
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTUwMDk4QDMxMzcyZTMzMmUzMFdHVENJbzdhN1NDTkZJRk03UlpSNDhtZ1EvMWQ5Yno4cGRIbjZDdDBLeE09");

            InitializeComponent();

            InitializeApp();

            PageAppearing += new EventHandler<Xamarin.Forms.Page>(OnPageAppearing);

            InitializeNavigation();

        }

        private async Task InitializeNavigation()
        {
            var navigationService = AppContainer.Resolve<INavigationService>();
            await navigationService.InitializeAsync();
        }

        private void InitializeApp()
        {
            AppContainer.RegisterDependencies();

            //var shoppingCartViewModel = AppContainer.Resolve<ShoppingCartViewModel>();
           //shoppingCartViewModel.InitializeMessenger();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private void OnPageAppearing(object sender, Xamarin.Forms.Page e)
        {
            if (Device.RuntimePlatform != Xamarin.Forms.Device.iOS)
                return;

            ContentPage page = e as ContentPage;
            if (page == null)
                return;

            // set safe insets on content that you want to be within the safe area
            var safeInsets = page.On<iOS>().SafeAreaInsets();
            //View firstChild = page.Content;
            //Thickness pageMargin = firstChild.Margin;
            SafeAreaTopMargin = safeInsets.Top >= SafeAreaTopMargin ? safeInsets.Top : SafeAreaTopMargin;
            SafeAreaBottomMargin = safeInsets.Bottom >= SafeAreaBottomMargin ? safeInsets.Bottom : SafeAreaBottomMargin;
        }
    }
}
