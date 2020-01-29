using ABitNow.Contracts.Services.General;
using ABitNow.Droid.Dependencies;
using Android.Webkit;
using Xamarin.Forms;


[assembly: Dependency(typeof(WebviewClearCookies))]
namespace ABitNow.Droid.Dependencies
{
    public class WebviewClearCookies : IWebviewClearCookies
    {
        public void ClearAllCookies()
        {
            var cookieManager = CookieManager.Instance;
            cookieManager.RemoveAllCookie();
        }
    }
}