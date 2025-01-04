using System.Collections.Generic;
using System.Linq;
using Unity_Translate.Items;
using UnityEngine;
using UnityEngine.Events;

namespace Unity_Translate
{
    public class LanguageManager : MonoBehaviour
    {
        // public static string LANG_PATH => Application.dataPath + "/Resources/Configs/Langs/";

        public static UnityEvent LanguageChanged = new UnityEvent();
        
        public SystemLanguage CurrentLanguage { get; private set; }
        
        public Language CurrentLanguageData { get; private set; }

        
        public void SetLanguage(SystemLanguage language)
        {
            CurrentLanguage = language;
            PlayerPrefs.SetInt("Language", (int)language);
            PlayerPrefs.Save();
            CurrentLanguageData = LanguageSettings.Instance.languages.FirstOrDefault(x => x.language == language);
            LanguageChanged.Invoke();
        }

        public LanguageItem GetTranslation(string category, string key)
        {
            return CurrentLanguageData.GetTranslation(category, key);
        }
        
        public static List<string> GetCategories()
        {
            if (LanguageSettings.Instance == null || LanguageSettings.Instance.languages == null)
            {
                return null;
            }
            var langs = LanguageSettings.Instance.languages;
            var categories = langs.SelectMany(x => x.languageCategories.Select(y => y.categoryName)).ToList();
            return categories;
        }
        
        public static List<string> GetKeys(string category)
        {
            var langs = LanguageSettings.Instance.languages;
            var keys = langs.SelectMany(lang => lang.languageCategories.Where(cat => cat.categoryName == category)
                .SelectMany(x => x.languageItems.Select(y => y.key))).ToList();
            return keys;
        }

        // public static Language CurrentLanguageData { get; set; }
        // public static List<Language> Languages { get; private set; }


//         public static void LoadLanguage()
//         {
//             Languages = new List<Language>();
//             
//             ApplicationProfile.Langs.ForEach(ctg =>
//             {
//                 Languages.Add(new Language(ctg.text));
//             });
//
//
// #if UNITY_EDITOR
//             CurrentLanguage = GetLangAsString(ApplicationProfile.Language);
// #else
//             CurrentLanguage = GetLangAsString(Application.systemLanguage);
// #endif
//             SetLanguage(CurrentLanguage);
//         }
//         
//         public static void SetLanguage(SystemLanguage language)
//         {
//             var shortLang = "EN";
//             if (LANGS_MAPPING.ContainsKey(language))
//             {
//                 shortLang = LANGS_MAPPING[language];
//             }
//             else
//             {
//                 Diagnostics.Log($"Not found language: {language} in mapping.", LogType.failed);
//                 Diagnostics.Log($"Using default language: EN", LogType.log);
//             }
//             SetLanguage(shortLang);
//         }
//         
//         public static void SetLanguage(string language)
//         {
//             CurrentLanguage = language;
//             PlayerPrefs.SetString("Language", language);
//             PlayerPrefs.Save();
//             CurrentLanguageData = Languages.FirstOrDefault(x =>
//                 x.ShortLanguageName.ToLower() == CurrentLanguage.ToLower() || x.LanguageName.ToLower() == CurrentLanguage.ToLower());
//             LanguageChanged.Invoke();
//         }
//         
//         public static void NextLanguage()
//         {
//             var index = Languages.IndexOf(CurrentLanguageData);
//             index++;
//             if (index >= Languages.Count)
//             {
//                 index = 0;
//             }
//             CurrentLanguageData = Languages[index];
//             SetLanguage(CurrentLanguageData.ShortLanguageName);
//         }
//         
//         public static void PreviousLanguage()
//         {
//             var index = Languages.IndexOf(CurrentLanguageData);
//             index--;
//             if (index < 0)
//             {
//                 index = Languages.Count - 1;
//             }
//             CurrentLanguageData = Languages[index];
//             SetLanguage(CurrentLanguageData.ShortLanguageName);
//         }
//         
//         // public static string Translate(this string keyWord)
//         // {
//         //     return GetTranslation(keyWord);
//         // }
//         
//         public static string GetTranslation(string keyWord)
//         {
//             if(Languages==null || Languages.Count==0)
//                 LoadLanguage();
//             if (string.IsNullOrEmpty(keyWord))
//             {
//                 return ColorHelper.GetTextInColor("KEYWORD IS NULL OR EMPTY", Color.red);
//             }
//             
//             var translation = CurrentLanguageData.GetTranslation(keyWord);
//             if (string.IsNullOrEmpty(translation))
//             {
//                 Diagnostics.Log($"Not found key word: {keyWord} in language: {ApplicationProfile.Language}", LogType.failed);
//                 return ColorHelper.GetTextInColor(keyWord + "_NF", Color.red);
//             }
//
//             return translation;
//         }
//
//         private static string GetLangAsString(SystemLanguage lang = SystemLanguage.Unknown)
//         {
//             var shortLang = "EN";
//             if (LANGS_MAPPING.ContainsKey(lang))
//             {
//                 shortLang = LANGS_MAPPING[lang];
//             }
//             shortLang = PlayerPrefs.GetString("Language", shortLang);
//             return shortLang;
//         }
    }
}
