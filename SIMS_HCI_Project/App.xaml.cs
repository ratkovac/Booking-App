using BookingApp.Localization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp
{
    public partial class App : System.Windows.Application
    {
        public static bool IsDark = false;
        public static string CurrentLanguage = "sr-LATN";

        public void ChangeTheme(Uri uri)
        {
            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = uri });
        }

        public void ChangeLanguage(string currLang)
        {
            TranslationSource.Instance.CurrentCulture = new System.Globalization.CultureInfo(currLang);
        }
    }
}
