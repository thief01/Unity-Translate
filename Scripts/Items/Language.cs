using System.Collections.Generic;
using NUnit.Framework;
using Unity_Translate.DebugLang;
using Unity_Translate.Translations;
using UnityEngine;

namespace Unity_Translate.Items
{
    [CreateAssetMenu(fileName = "New Language", menuName = "thief01/New Language")]
    public class Language : ScriptableObject
    {
        public SystemLanguage language;
        public List<LanguageCategory> languageCategories;
        
        public LanguageItem GetLanguageItem(LanguageVariable languageVariable)
        {
            var category = languageCategories[languageVariable.Category];
            // TODO: Safe code + cache key as the name to make default value
            return category.languageItems[languageVariable.Key];
        }

        public LanguageItem GetLanguageItem(string category, string key, LanguageTranslationType type = LanguageTranslationType.Undefined)
        {
            foreach (var languageCategory in languageCategories)
            {
                if (languageCategory.categoryName == category)
                {
                    foreach (var languageItem in languageCategory.languageItems)
                    {
                        if (languageItem.key == key)
                        {
                            return languageItem;
                        }
                    }
                }
            }

            LanguageMissingsLogger.Instance.LogMissingTranslation(category, key, type, language);
            return new LanguageItem() { key = key, translation = key };
        }

        public List<string> GetCategories()
        {
            List<string> categories = new List<string>();
            foreach (var languageCategory in languageCategories)
            {
                categories.Add(languageCategory.categoryName);
            }

            return categories;
        }
    }
}
