using System.ComponentModel;
using ABitNow.Droid.Dependencies;
using Android.Content;
using Android.Webkit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.WebView), typeof(CustomWebviewRenderer))]
namespace ABitNow.Droid.Dependencies
{
    public class CustomWebviewRenderer : WebViewRenderer
    {
        private Context context;

        public CustomWebviewRenderer(Context context) : base(context)
        {
            this.context = context;
        }

       
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var webView = (global::Android.Webkit.WebView)Control;
            if (webView != null)
            {
                string webUrl = webView.Url;

                if (!string.IsNullOrWhiteSpace(webUrl))
                {
                    if (webUrl.ToLower().Contains("google"))
                    {
                        Control.Settings.UserAgentString = "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.0.4) Gecko/20100101 Firefox/4.0";
                    }
                }
            }
        }
       
    }
}