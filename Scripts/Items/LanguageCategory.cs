using System.Collections.Generic;

namespace Ultimate_Translation.Items
{
    [System.Serializable]
    public class LanguageCategory
    {
        public string categoryName;
        public List<LanguageItem> languageItems = new List<LanguageItem>();

        public bool AddLanguageItem(LanguageItem item)
        {
            if (languageItems.Exists(ctg => ctg.key == item.key))
                return false;

            languageItems.Add(item);
            return true;
        }

        public bool AddLanguageItem(string key, string value)
        {
            if (languageItems.Exists(ctg => ctg.key == key))
                return false;

            languageItems.Add(new LanguageItem() { key = key, translation = value });
            return true;
        }

        public bool RemoveLanguageItem(string key)
        {
            return languageItems.RemoveAll(ctg => ctg.key == key) > 0;
        }

        public bool RemoveLanguageItem(LanguageItem item)
        {
            return languageItems.Remove(item);
        }

        public void RemoveLanguageItem(int id)
        {
            languageItems.RemoveAt(id);
        }
    }
}