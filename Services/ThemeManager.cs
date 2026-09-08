using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Task2.Services
{
    internal static class ThemeManager
    {
        public static void ApplyTheme(bool isDark)
        {
            var uri = new Uri(isDark
                ? "Themes/Dark.xaml"
                : "Themes/Light.xaml", UriKind.Relative);

            var newDict = new ResourceDictionary { Source = uri };

            var app = Application.Current;
            var existing = app.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null &&
                    (d.Source.OriginalString.Contains("LightTheme") ||
                     d.Source.OriginalString.Contains("DarkTheme")));

            if (existing != null)
                app.Resources.MergedDictionaries.Remove(existing);

            app.Resources.MergedDictionaries.Add(newDict);
        }
    }
}