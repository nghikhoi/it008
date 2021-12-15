﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using UI.ViewModels;

namespace UI.Services {
	
	public class Language {
		
		public static readonly string defaultLanguage = "vi-VN";
		public static readonly List<string> availableLanguage = new List<string>() {
			"en-US",
			"vi-VN"
		};
		
		public static void ApplyLanguage(string cultureName = null) {
			if (cultureName != null)
				Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);

			ResourceDictionary dict = new ResourceDictionary();
			String langKey = Thread.CurrentThread.CurrentCulture.ToString();
			if (!availableLanguage.Contains(langKey))
				langKey = defaultLanguage;
			dict.Source = new Uri("..\\Resources\\Lang\\" + langKey + ".xaml", UriKind.Relative);

			App.Instance.Resources.MergedDictionaries.Add(dict);
            if (LanguageViewModel.Instance != null)
                LanguageViewModel.Instance.CurrentLanguage = ""; //Notify
        }

		public static string getCurrentLanguage() {
			return Thread.CurrentThread.CurrentCulture.ToString();
		}

		public static string getText(string key) {
			return App.Instance.Resources[key].ToString();
		}

    }

    public class LanguageViewModel : ViewModelBase
    {

        public static LanguageViewModel Instance;
        public LanguageViewModel() {
            Instance = this;
        }

        public string CurrentLanguage
        {
            get => Language.getCurrentLanguage();
            set => OnPropertyChanged(nameof(CurrentLanguage));
        }
	}
	
}