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
    }
}
