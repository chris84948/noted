using mshtml;
using System;
using System.Windows;
using System.Windows.Controls;

namespace NotedUI.AttachedBehaviors
{
    class WebBrowserBehavior
    {
        public static readonly DependencyProperty SetCSSOnLoadProperty =
            DependencyProperty.RegisterAttached("SetCSSOnLoad",
                                                typeof(bool),
                                                typeof(WebBrowserBehavior),
                                                new PropertyMetadata(false, new PropertyChangedCallback(SetCSSOnLoadChanged)));

        public static bool GetSetCSSOnLoad(DependencyObject obj)
        {
            return (bool)obj.GetValue(SetCSSOnLoadProperty);
        }

        public static void SetSetCSSOnLoad(DependencyObject obj, bool value)
        {
            obj.SetValue(SetCSSOnLoadProperty, value);
        }

        private static void SetCSSOnLoadChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WebBrowser browser = (WebBrowser)d;
            browser.LoadCompleted += (s, args) => SetupWebBrowserCSS(browser);
        }


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
            var newText = "<div id='content'>" + e.NewValue as string + "</div>";

            if (browser != null && !string.IsNullOrWhiteSpace(newText))
            {
                UpdateContent2(browser, newText);
            }
        }

        private static void UpdateContent(WebBrowser browser, string markdown)
        {
            var document = browser.Document as IHTMLDocument3;
            var element = document?.getElementById("content");

            if (element == null)
                browser.NavigateToString(markdown);
            else
                element.innerHTML = markdown;
        }

        private static void UpdateContent2(WebBrowser browser, string markdown)
        {
            HTMLDocument document = (HTMLDocument)browser.Document;
            IHTMLElement textArea = document?.getElementById("content");

            if (textArea == null)
                browser.NavigateToString(markdown);
            else
                textArea.innerHTML = markdown;
        }

        private static void SetupWebBrowserCSS(WebBrowser browser)
        {
            IHTMLDocument2 doc = (IHTMLDocument2)browser.Document;

            if (doc == null)
                return;

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
