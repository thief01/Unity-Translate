using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Unity_Translate.Items
{
    [System.Serializable]
    public class LanguageCategory
    {
        [FormerlySerializedAs("name")] public string categoryName;
        public List<LanguageItem> languageItems = new List<LanguageItem>();
    }
}
