using System.ComponentModel;
using ABitNow.iOS.Dependencies;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Xamarin.Forms.WebView), typeof(CustomWebviewRenderer))]
namespace ABitNow.iOS.Dependencies
{
    public class CustomWebviewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged -= OnElementPropertyChanged;
            }
            if (e.NewElement != null)
            {
                e.NewElement.PropertyChanged += OnElementPropertyChanged;
            }

        }
        private void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //var webView = Control;
            //if (webView != null)
            //{
            //    string webUrl = webView.Url;

            //    if (!string.IsNullOrWhiteSpace(webUrl))
            //    {
            //        if (webUrl.ToLower().Contains("google"))
            //        {
            //            Control.Settings.UserAgentString = "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.0.4) Gecko/20100101 Firefox/4.0";
            //        }
            //    }
            //}
        }
       
    }
}