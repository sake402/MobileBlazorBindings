﻿using WebWindows.Blazor.XamarinForms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamarinTest
{
    public class App : Application
    {
        public App()
        {
            var tabs = new TabbedPage();

            tabs.Children.Add(new BlazorPage { Title = "Blazor" });
            tabs.Children.Add(new LocalHtml { Title = "Local" });
            tabs.Children.Add(new LocalHtmlBaseUrl { Title = "BaseUrl" });
            tabs.Children.Add(new TridentPage { Title = "Trident" });
            tabs.Children.Add(new SpartanPage { Title = "Spartan" });
            tabs.Children.Add(new WebPage { Title = "Web Page" });
            tabs.Children.Add(new WebAppPage { Title = "External" });

            MainPage = tabs;
        }
    }

    public class LocalHtml : ContentPage
    {
        public LocalHtml()
        {
            var browser = new ExtendedWebView();
            browser.OnWebMessageReceived += (sender, message) =>
            {
                browser.SendMessage("Got message: " + message);
            };

            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"<html><body>
                                <h1>Xamarin.Forms</h1>
                                <p>Welcome to WebView.</p>
                                <button onclick=""window.external.sendMessage('blah😋')"">Invoke .NET</button>
                                <script>
                                    window.external.receiveMessage(function (message) {
                                        alert(message);
                                    });
                                </script>
                                </body>
                                </html>";
            browser.Source = htmlSource;
            Content = browser;
        }
    }

    public class TridentPage : ContentPage
    {
        public TridentPage()
        {
            var browser = new WebView();

            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"<html><body>
                                <h1>Trident</h1>
                                <p>This is an IE11 control.</p>
                                <script>document.write(navigator.userAgent);</script>
                                </body>
                                </html>";
            browser.Source = htmlSource;
            Content = browser;
        }
    }

    public class SpartanPage : ContentPage
    {
        public SpartanPage()
        {
            var browser = new SpartanWebView();

            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"<html><body>
                                <h1>Spartan</h1>
                                <p>This is an Edge (pre-Chromium) control.</p>
                                <script>document.write(navigator.userAgent);</script>
                                </body>
                                </html>";
            browser.Source = htmlSource;
            Content = browser;
        }
    }

    public interface IBaseUrl { string Get(); }

    public class LocalHtmlBaseUrl : ContentPage
    {
        public LocalHtmlBaseUrl()
        {
            var browser = new ExtendedWebView();
            var htmlSource = new HtmlWebViewSource();

            htmlSource.Html = @"<html>
                                <head>
                                <link rel=""stylesheet"" href=""default.css"">
                                </head>
                                <body>
                                <h1>Xamarin.Forms</h1>
                                <p>The CSS and image are loaded from local files!</p>
                                <img src='XamarinLogo.png'/>
                                <p><a href=""local.html"">next page</a></p>
                                </body>
                                </html>";

            htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
            browser.Source = htmlSource;
            Content = browser;
        }
    }

    public class WebAppPage : ContentPage
    {
        public WebAppPage()
        {
            var l = new Label
            {
                Text = "These buttons leave the current app and open the built-in web browser app for the platform"
            };

            var openUrl = new Button
            {
                Text = "Open location using built-in Web Browser app"
            };
            openUrl.Clicked += async (sender, e) =>
            {
                await Launcher.OpenAsync("http://xamarin.com/evolve");
            };

            var makeCall = new Button
            {
                Text = "Make call using built-in Phone app"
            };
            makeCall.Clicked += async (sender, e) =>
            {
                await Launcher.OpenAsync("tel:1855XAMARIN");
            };

            Content = new StackLayout
            {
                Padding = new Thickness(5, 20, 5, 0),
                HorizontalOptions = LayoutOptions.Fill,
                Children = {
                    l,
                    openUrl,
                    makeCall
                }
            };
        }
    }

    public class WebPage : ContentPage
    {
        public WebPage()
        {
            var browser = new ExtendedWebView();
            browser.Source = "https://www.whatismybrowser.com/";
            Content = browser;
        }
    }

    public class BlazorPage : ContentPage
    {
        public BlazorPage()
        {
            var child = new BlazorView();
            Content = child;
        }
    }
}
