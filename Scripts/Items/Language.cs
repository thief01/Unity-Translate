using System.Collections.Generic;
using NUnit.Framework;
using Unity_Translate.Scripts.Items;
using UnityEngine;

namespace Unity_Translate.Items
{
    [CreateAssetMenu(fileName = "New Language", menuName = "thief01/New Language")]
    public class Language : ScriptableObject
    {
        public SystemLanguage language;
        public List<LanguageCategory> languageCategories;

        public LanguageItem GetTranslation(string category, string key)
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

            // TODO: log missing translation
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
