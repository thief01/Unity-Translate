using System.Collections.Generic;
using System.Linq;
using Unity_Translate.DebugLang;
using Unity_Translate.Items;
using Unity_Translate.Translations;
using UnityEngine;
using UnityEngine.Events;

namespace Unity_Translate
{
    public class LanguageManager
    {
        public static UnityEvent LanguageChanged = new UnityEvent();
        public SystemLanguage CurrentLanguage { get; private set; }
        public Language CurrentLanguageData { get; private set; }
        
        public LanguageManager Instance => instance ??= new LanguageManager();
        
        private LanguageManager instance;
        
        public void SetLanguage(SystemLanguage language)
        {
            CurrentLanguage = language;
            PlayerPrefs.SetInt("Language", (int)language);
            PlayerPrefs.Save();
            CurrentLanguageData = LanguageSettings.Instance.languages.FirstOrDefault(x => x.language == language);
            LanguageChanged.Invoke();
        }
        
        public LanguageItem GetTranslation(LanguageVariable languageVariable, LanguageTranslationType type = LanguageTranslationType.Undefined)
        {
            var langItem = CurrentLanguageData.GetLanguageItem(languageVariable);
            if (langItem.CheckType(type))
            {
                // TODO: log missing translation
                LanguageMissingLogger.Instance.LogMissingTranslation(languageVariable, type, CurrentLanguage);
                
            }
            return langItem;
        }

        public LanguageItem GetTranslation(string category, string key, LanguageTranslationType type = LanguageTranslationType.Undefined)
        {
            var langItem = CurrentLanguageData.GetLanguageItem(category, key, type);
            if (langItem.CheckType(type))
            {
                // TODO: log missing translation
                LanguageMissingLogger.Instance.LogMissingTranslation(category, key, type, CurrentLanguage);
            }

            return langItem;
        }
        
        public List<string> GetCategories()
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
