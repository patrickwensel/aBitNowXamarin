using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ABitNow.Behaviors
{
   public sealed class NavigatedCommandWebView
    {
        public static readonly BindableProperty NavigatedCommandProperty =
            BindableProperty.CreateAttached(
                "NavigatedCommand",
                typeof(ICommand),
                typeof(NavigatedCommandWebView),
                default(ICommand),
                BindingMode.TwoWay,null,
                
                PropertyChanged);

        private static void PropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WebView webview)
            {
                webview.Navigated -= WebviewOnNavigated;
                webview.Navigated += WebviewOnNavigated;
            }
        }

        private static void WebviewOnNavigated(object sender, WebNavigatedEventArgs e)
        {
            var webview = sender as WebView;

            if (webview != null && webview.IsEnabled)
            {
                var command = GetWebviewNavigatedCommand(webview);
                if (command != null && command.CanExecute(e.Url))
                {
                    command.Execute(e.Url);
                }
            }
        }


        public static ICommand GetWebviewNavigatedCommand(BindableObject bindableObject)
        {
            return (ICommand)bindableObject.GetValue(NavigatedCommandProperty);
        }

        public static void SetWebviewNavigatedCommand(BindableObject bindableObject, object value)
        {
            bindableObject.SetValue(NavigatedCommandProperty, value);
        }
    }
}
