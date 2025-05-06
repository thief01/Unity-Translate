using System.Collections.Generic;
using System.Linq;
using Ultimate_Translation.Translations;
using UnityEngine;

namespace Ultimate_Translation.Items
{
    [CreateAssetMenu(fileName = "New Language", menuName = "thief01/New Language")]
    public class Language : ScriptableObject
    {
        public SystemLanguage language;
        public List<LanguageCategory> languageCategories;
        
        public bool AddCategory(LanguageCategory languageCategory)
        {
            if (languageCategories.Exists(ctg => ctg.categoryName == languageCategory.categoryName))
                return false;
            languageCategories.Add(languageCategory);
            return true;
        }

        public bool AddCategory(string categoryName)
        {
            if (languageCategories.Exists(ctg => ctg.categoryName == categoryName))
                return false;
            languageCategories.Add(new LanguageCategory()
            {
                categoryName = categoryName,
            });

            return true;
        }
        
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

            LanguageMissingLogger.Instance.LogMissingTranslation(category, key, type, language);
            return new LanguageItem() { key = key, translation = key };
        }

        public string[] GetCategories()
        {
            return languageCategories.Select(ctg => ctg.categoryName).ToArray();
        }

        public string[] GetKeys(int category)
        {
            return languageCategories[category].languageItems.Select(ctg => ctg.key).ToArray();
        }
    }
}
