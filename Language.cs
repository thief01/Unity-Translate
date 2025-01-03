using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace WRA.General.Language
{
    public class Language
    {
        public string ShortLanguageName { get; set; }
        public string LanguageName { get; set; }
        public Dictionary<string, LanguageItem> LanguageItems { get; }
    
        public List<string> Categories { get; set; }

        public Language(string languageData)
        {
            LanguageItems = new Dictionary<string, LanguageItem>();
            ParseLanguageData(languageData);
            CreateCategories();
        }

        public string GetTranslation(string key)
        {
            if (LanguageItems.ContainsKey(key))
            {
                return LanguageItems[key].Translation;
            }

#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                LanguageMissingTranslationsLogger.AddMissingTranslations(key);
                LanguageMissingTranslationsLogger.SaveMissingTranslations();
            }
#endif
            return key;
        }
    
        public Dictionary<string, LanguageItem> GetTranslationsByCategory(string category)
        {
            Dictionary<string, LanguageItem> translations = new Dictionary<string, LanguageItem>();
            foreach (var item in LanguageItems)
            {
                if (item.Value.Category == category)
                {
                    translations.Add(item.Key, item.Value);
                }
            }

            return translations;
        }

#if UNITY_EDITOR
    
        public bool HasTranslation(string key)
        {
            return LanguageItems.ContainsKey(key);
        }
    
        public string GetLanguageAsXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement data = doc.CreateElement("data");
            data.SetAttribute("languageShort", ShortLanguageName);
            data.SetAttribute("languageName", LanguageName);
            doc.AppendChild(data);
            foreach (var item in LanguageItems)
            {
                XmlNode category = doc.SelectSingleNode("data/" + item.Value.Category);
                if (category == null)
                {
                    category = doc.CreateElement("" + item.Value.Category);
                    var atribute = doc.CreateAttribute("category");
                    atribute.Value = item.Value.Category;
                    category.Attributes.Append(atribute);
                    data.AppendChild(category);
                }

                XmlElement element = doc.CreateElement(item.Key);
                element.InnerText = item.Value.Translation;
                category.AppendChild(element);
            }

            return doc.OuterXml;
        }

        public void AddTranslation(string key, LanguageItem languageItem)
        {
            LanguageItems.Add(key, languageItem);
            if (Categories.Contains(languageItem.Category))
                return;
            Categories.Add(languageItem.Category);
        }
    
        public void AddTranslation(LanguageItem languageItem)
        {
            LanguageItems.Add(languageItem.Key, languageItem);
            if (Categories.Contains(languageItem.Category))
                return;
            Categories.Add(languageItem.Category);
        }
    
        public void RemoveTranslation(string key)
        {
            LanguageItems.Remove(key);
        }
    
        public void RemoveCategory(string category)
        {
            var translations = GetTranslationsByCategory(category);
        
            foreach (var translation in translations)
            {
                LanguageItems.Remove(translation.Key);
            }
            Categories.Remove(category);
        }
#endif
        private void ParseLanguageData(string languageData)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(languageData);
            ShortLanguageName = doc.SelectSingleNode("data").Attributes["languageShort"].Value;
            LanguageName = doc.SelectSingleNode("data").Attributes["languageName"].Value;
            ParseLanguageItems(doc);
        }

        private void ParseLanguageItems(XmlDocument document)
        {
            XmlNodeList languageItems = document.SelectNodes("/data/*");
            foreach (XmlNode item in languageItems)
            {
                if (item.Attributes["category"] != null)
                {
                    var category = item.Name;
                    XmlNodeList nodes = document.SelectNodes("/data/" + item.Name + "/*");
                    foreach (XmlNode node in nodes)
                    {
                        AddTranslation(node.Name, category, node.InnerText);
                    }
                }
                else
                {
                    AddTranslation(item.Name, "ND", item.InnerText);
                }
            }
        }
    
        private void CreateCategories()
        {
            Categories = new List<string>();
            foreach (var item in LanguageItems)
            {
                if (Categories.Contains(item.Value.Category))
                    continue;
                Categories.Add(item.Value.Category);
            }
        }
    
        private void AddTranslation(string key, string category, string translation)
        {
            LanguageItem languageItem = new LanguageItem();
            languageItem.Key = key;
            languageItem.Category = category;
            languageItem.Translation = translation;
            LanguageItems.Add(key, languageItem);
        }
    }
}