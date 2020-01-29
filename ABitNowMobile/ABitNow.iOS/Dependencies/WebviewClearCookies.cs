using ABitNow.Contracts.Services.General;
using ABitNow.iOS.Dependencies;
using Foundation;
using Xamarin.Forms;


[assembly: Dependency(typeof(WebviewClearCookies))]
namespace ABitNow.iOS.Dependencies
{
    public class WebviewClearCookies : IWebviewClearCookies
    {
        public void ClearAllCookies()
        {
            NSHttpCookieStorage CookieStorage = NSHttpCookieStorage.SharedStorage;
            foreach (var cookie in CookieStorage.Cookies)
                CookieStorage.DeleteCookie(cookie);
        }
    }
}