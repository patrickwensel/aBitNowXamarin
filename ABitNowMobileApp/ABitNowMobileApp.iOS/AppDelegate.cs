using System;
using System.Collections.Generic;
using System.Linq;
using ABitNowMobileApp.Data;
using Foundation;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfRangeSlider.XForms.iOS;
using Syncfusion.SfRating.XForms.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace ABitNowMobileApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            SfRatingRenderer sfRatingRenderer = new SfRatingRenderer();
            SfRangeSliderRenderer sfRangeSliderRenderer = new SfRangeSliderRenderer();
            SfListViewRenderer.Init();

            WireupDependency();

            App.StatusBarHeight = UIApplication.SharedApplication.StatusBarFrame.Height;
            App.ScreenSize = new Xamarin.Forms.Size(UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);

            var attribute = new UITextAttributes();
            attribute.TextColor = UIColor.Clear;
            UIBarButtonItem.Appearance.SetTitleTextAttributes(attribute, UIControlState.Normal);
            UIBarButtonItem.Appearance.SetTitleTextAttributes(attribute, UIControlState.Highlighted);

            var application = (Xamarin.Forms.Application)ContainerManager.Container.Resolve(typeof(App), typeof(App).ToString());
            LoadApplication(application);

            PrintFontNames();

            var result = base.FinishedLaunching(app, options);
            app.KeyWindow.TintColor = Color.FromHex("#853939").ToUIColor();
            return result;
        }

        private void WireupDependency()
        {
            // Wire up dependencies of core classes
            ContainerManager.Initialize();
            DataBootstrapper.Initialize(ContainerManager.Container);
            FormsBootstrapper.Initialize(ContainerManager.Container);
            iOSBootstrapper.Initialize(ContainerManager.Container);
        }

        private void PrintFontNames()
        {
            foreach (var family in UIFont.FamilyNames)
            {
                System.Diagnostics.Debug.WriteLine($"{family}");

                foreach (var names in UIFont.FontNamesForFamilyName(family))
                {
                    System.Diagnostics.Debug.WriteLine($"{names}");
                }
            }
        }
    }
}
