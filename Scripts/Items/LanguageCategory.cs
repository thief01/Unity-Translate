using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Unity_Translate.Items
{
    [System.Serializable]
    public class LanguageCategory
    {
        public string name;
        public List<LanguageItem> languageItems;
    }
}
