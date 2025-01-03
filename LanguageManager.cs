using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using WRA.Utility.Diagnostics.Logs;
using WRA.Utility.Math;
using Zenject;
using LogType = WRA.Utility.Diagnostics.Logs.LogType;

namespace WRA.General.Language
{
    public class LanguageManager : MonoBehaviour
    {
        public static string LANG_PATH => Application.dataPath + "/Resources/Configs/Langs/";

        public static UnityEvent LanguageChanged = new UnityEvent();
        
        public static string CurrentLanguage { get; private set; }
        public static Language CurrentLanguageData { get; set; }
        public static List<Language> Languages { get; private set; }

        private static Dictionary<SystemLanguage, string> LANGS_MAPPING = new()
        {
            { SystemLanguage.Polish , "PL"},
            { SystemLanguage.English, "EN" }
        };

        [Inject] private static ApplicationProfile ApplicationProfile;

        [Inject]
        private void InitAppProfile(ApplicationProfile applicationProfile)
        {
            ApplicationProfile = applicationProfile;
        }
        
        public static void LoadLanguage()
        {
            Languages = new List<Language>();
            
            ApplicationProfile.Langs.ForEach(ctg =>
            {
                Languages.Add(new Language(ctg.text));
            });


#if UNITY_EDITOR
            CurrentLanguage = GetLangAsString(ApplicationProfile.Language);
#else
            CurrentLanguage = GetLangAsString(Application.systemLanguage);
#endif
            SetLanguage(CurrentLanguage);
        }
        
        public static void SetLanguage(SystemLanguage language)
        {
            var shortLang = "EN";
            if (LANGS_MAPPING.ContainsKey(language))
            {
                shortLang = LANGS_MAPPING[language];
            }
            else
            {
                Diagnostics.Log($"Not found language: {language} in mapping.", LogType.failed);
                Diagnostics.Log($"Using default language: EN", LogType.log);
            }
            SetLanguage(shortLang);
        }
        
        public static void SetLanguage(string language)
        {
            CurrentLanguage = language;
            PlayerPrefs.SetString("Language", language);
            PlayerPrefs.Save();
            CurrentLanguageData = Languages.FirstOrDefault(x =>
                x.ShortLanguageName.ToLower() == CurrentLanguage.ToLower() || x.LanguageName.ToLower() == CurrentLanguage.ToLower());
            LanguageChanged.Invoke();
        }
        
        public static void NextLanguage()
        {
            var index = Languages.IndexOf(CurrentLanguageData);
            index++;
            if (index >= Languages.Count)
            {
                index = 0;
            }
            CurrentLanguageData = Languages[index];
            SetLanguage(CurrentLanguageData.ShortLanguageName);
        }
        
        public static void PreviousLanguage()
        {
            var index = Languages.IndexOf(CurrentLanguageData);
            index--;
            if (index < 0)
            {
                index = Languages.Count - 1;
            }
            CurrentLanguageData = Languages[index];
            SetLanguage(CurrentLanguageData.ShortLanguageName);
        }
        
        // public static string Translate(this string keyWord)
        // {
        //     return GetTranslation(keyWord);
        // }
        
        public static string GetTranslation(string keyWord)
        {
            if(Languages==null || Languages.Count==0)
                LoadLanguage();
            if (string.IsNullOrEmpty(keyWord))
            {
                return ColorHelper.GetTextInColor("KEYWORD IS NULL OR EMPTY", Color.red);
            }
            
            var translation = CurrentLanguageData.GetTranslation(keyWord);
            if (string.IsNullOrEmpty(translation))
            {
                Diagnostics.Log($"Not found key word: {keyWord} in language: {ApplicationProfile.Language}", LogType.failed);
                return ColorHelper.GetTextInColor(keyWord + "_NF", Color.red);
            }

            return translation;
        }

        private static string GetLangAsString(SystemLanguage lang = SystemLanguage.Unknown)
        {
            var shortLang = "EN";
            if (LANGS_MAPPING.ContainsKey(lang))
            {
                shortLang = LANGS_MAPPING[lang];
            }
            shortLang = PlayerPrefs.GetString("Language", shortLang);
            return shortLang;
        }
    }
}
