using System;
using ABitNowMobileApp.Interfaces;
using ABitNowMobileApp.Messages;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace ABitNowMobileApp
{
    public partial class App : Xamarin.Forms.Application
    {
        private readonly INavigationService _navigationService;
        private readonly IMessagingService _messagingService;

        public static double SafeAreaTopMargin = 0;
        public static double SafeAreaBottomMargin = 0;
        public static Size ScreenSize;
        public static double StatusBarHeight;
        public static double NavigationBarHeight = 44;

        public App(INavigationService navigationService, IMessagingService messagingService)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTUwMDk4QDMxMzcyZTMzMmUzMFdHVENJbzdhN1NDTkZJRk03UlpSNDhtZ1EvMWQ5Yno4cGRIbjZDdDBLeE09");
            InitializeComponent();
            _navigationService = navigationService;
            _messagingService = messagingService;

            PageAppearing += new EventHandler<Xamarin.Forms.Page>(OnPageAppearing);

            _navigationService.NavigateToFirstScreenAsync();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            _messagingService.Send<AppLifecycleChangedMessage>(new AppLifecycleChangedMessage(Enums.AppLifecycleState.Sleep), this);
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            _messagingService.Send<AppLifecycleChangedMessage>(new AppLifecycleChangedMessage(Enums.AppLifecycleState.Resume), this);
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
