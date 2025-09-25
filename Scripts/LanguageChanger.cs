using System;
using Ultimate_Translation;
using UnityEngine;

namespace Unity_Translate.Scripts
{
    public class LanguageChanger : MonoBehaviour
    {
        [SerializeField] private SystemLanguage language;
        private LanguageManager languageManager;
        private void Awake()
        {
            languageManager = LanguageManager.Instance;
        }

        public void UseLanguage()
        {
            languageManager.SetLanguage(language);
        }
    }
}
