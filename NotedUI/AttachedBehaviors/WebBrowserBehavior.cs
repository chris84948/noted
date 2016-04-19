using mshtml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace NotedUI.AttachedBehaviors
{
    class WebBrowserBehavior
    {
        public static readonly DependencyProperty HTMLProperty =
            DependencyProperty.RegisterAttached("HTML",
                                                typeof(string),
                                                typeof(WebBrowserBehavior),
                                                new PropertyMetadata(null, new PropertyChangedCallback(HTMLChanged)));

        public static string GetHTML(DependencyObject obj)
        {
            return (string)obj.GetValue(HTMLProperty);
        }

        public static void SetHTML(DependencyObject obj, string value)
        {
            obj.SetValue(HTMLProperty, value);
        }

        private static void HTMLChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var browser = d as WebBrowser;
            var newText = e.NewValue as string;

            if (browser != null && !string.IsNullOrWhiteSpace(newText))
            {
                browser.NavigateToString(newText);

            }
            
            if (!browser.IsLoaded)
                browser.LoadCompleted += WebBrowser_LoadCompleted;
        }

        private static void WebBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            var browser = sender as WebBrowser;
            IHTMLDocument2 doc = browser.Document as IHTMLDocument2;
            IHTMLStyleSheet ss = doc.createStyleSheet("", 0);
            ss.cssText = GetCSSStyle(browser);
        }

        public static readonly DependencyProperty CSSStyleProperty =
            DependencyProperty.RegisterAttached("CSSStyle",
                                                typeof(string),
                                                typeof(WebBrowserBehavior),
                                                new PropertyMetadata(null));

        public static string GetCSSStyle(DependencyObject obj)
        {
            return (string)obj.GetValue(CSSStyleProperty);
        }

        public static void SetCSSStyle(DependencyObject obj, string value)
        {
            obj.SetValue(CSSStyleProperty, value);
        }

    }
}
