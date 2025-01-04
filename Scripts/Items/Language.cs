using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Unity_Translate.Items
{
    [CreateAssetMenu(fileName = "New Language", menuName = "thief01/New Language")]
    public class Language : ScriptableObject
    {
        public SystemLanguage language;
        public List<LanguageCategory> languageCategories;
    }
}
