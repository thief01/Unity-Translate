using System.Collections.Generic;
using Unity_Translate.Items;
using UnityEngine.Serialization;

namespace Unity_Translate.Scripts.Items
{
    [System.Serializable]
    public class LanguageCategory
    {
        [FormerlySerializedAs("name")] public string categoryName;
        public List<LanguageItem> languageItems;
    }
}
