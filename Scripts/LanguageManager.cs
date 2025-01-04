using System.Collections.Generic;
using System.Linq;
using Unity_Translate.Items;
using Unity_Translate.Translations;
using UnityEngine;
using UnityEngine.Events;

namespace Unity_Translate
{
    public class LanguageManager : MonoBehaviour
    {
        public static UnityEvent LanguageChanged = new UnityEvent();
        public static SystemLanguage CurrentLanguage { get; private set; }
        public static Language CurrentLanguageData { get; private set; }
        
        public static void SetLanguage(SystemLanguage language)
        {
            CurrentLanguage = language;
            PlayerPrefs.SetInt("Language", (int)language);
            PlayerPrefs.Save();
            CurrentLanguageData = LanguageSettings.Instance.languages.FirstOrDefault(x => x.language == language);
            LanguageChanged.Invoke();
        }
        
        public static LanguageItem GetTranslation(LanguageVariable languageVariable, LanguageTranslationType type = LanguageTranslationType.Undefined)
        {
            var langItem = CurrentLanguageData.GetLanguageItem(languageVariable);
            if (langItem.CheckType(type))
            {
                // TODO: log missing translation
                
            }
            return langItem;
        }

        public static LanguageItem GetTranslation(string category, string key, LanguageTranslationType type = LanguageTranslationType.Undefined)
        {
            var langItem = CurrentLanguageData.GetLanguageItem(category, key, type);
            if (langItem.CheckType(type))
            {
                // TODO: log missing translation
            }

            return langItem;
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
    }
}
