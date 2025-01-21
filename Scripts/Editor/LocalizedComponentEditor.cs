using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity_Translate.Items;
using Unity_Translate.Translations;
using UnityEditor;
using UnityEngine;

namespace Unity_Translate.Editor
{
    [CustomEditor(typeof(LocalizedComponent), true)]
    public class LocalizedComponentEditor : UnityEditor.Editor
    {
        private LocalizedComponent localizedComponent;
        private List<Language> languages;
        private LanguageVariable languageVariable;

        private void OnEnable()
        {
            localizedComponent = (LocalizedComponent) target;
            languages = LanguageSettings.Instance.languages;
            languageVariable = localizedComponent.LanguageVariable;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if(GUILayout.Button("Test lang"))
            {
                var lang = languages.Find(x => x.language == languageVariable.PreviewLanguage);
                if (lang == null)
                {
                    Debug.LogError("Language not found");
                    return;
                }
                var langItem = lang.GetLanguageItem(languageVariable);
                localizedComponent.SetLanguageData(langItem);
            }
        }
    }
}
