using Foundation;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfRating.XForms.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
namespace ABitNow.iOS
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
            SfListViewRenderer.Init();


            App.StatusBarHeight = UIApplication.SharedApplication.StatusBarFrame.Height;
            App.ScreenSize = new Xamarin.Forms.Size(UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);

            var attribute = new UITextAttributes();
            attribute.TextColor = UIColor.Clear;
            UIBarButtonItem.Appearance.SetTitleTextAttributes(attribute, UIControlState.Normal);
            UIBarButtonItem.Appearance.SetTitleTextAttributes(attribute, UIControlState.Highlighted);

            LoadApplication(new App());

            PrintFontNames();

            var result = base.FinishedLaunching(app, options);
            app.KeyWindow.TintColor = Color.FromHex("#853939").ToUIColor();
            return result;

            //return base.FinishedLaunching(app, options);
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
