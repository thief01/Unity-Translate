using System.Linq;
using Ultimate_Translation.DebugLang;
using Ultimate_Translation.Items;
using Ultimate_Translation.Translations;
using UnityEngine;
using UnityEngine.Events;

namespace Ultimate_Translation
{
    public class LanguageManager
    {
        public static UnityEvent LanguageChanged = new UnityEvent();
        public SystemLanguage CurrentLanguage { get; private set; }
        public Language CurrentLanguageData { get; private set; }
        
        public static LanguageManager Instance => instance ??= new LanguageManager();
        
        private static LanguageManager instance;
        
        private LanguageManager()
        {
            CurrentLanguage = (SystemLanguage) PlayerPrefs.GetInt("Language", (int) SystemLanguage.English);
            CurrentLanguageData = LanguageSettings.Instance.languages.FirstOrDefault(x => x.language == CurrentLanguage);
        }
        
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
                LanguageMissingLogger.Instance.LogMissingTranslation(languageVariable, type, CurrentLanguage);
                
            }
            return langItem;
        }

        public LanguageItem GetTranslation(string category, string key, LanguageTranslationType type = LanguageTranslationType.Undefined)
        {
            var langItem = CurrentLanguageData.GetLanguageItem(category, key, type);
            if (langItem.CheckType(type))
            {
                LanguageMissingLogger.Instance.LogMissingTranslation(category, key, type, CurrentLanguage);
            }

            return langItem;
        }
    }
}
