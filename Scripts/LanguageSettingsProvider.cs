using System.Linq;
using Unity_Translate.Items;
using UnityEditor;
using UnityEngine;

namespace Unity_Translate
{
    public static class LanguageSettingsProvider 
    {
        [SettingsProvider]
        public static SettingsProvider CreateCustomSettingsProvider()
        {
            var provider = new SettingsProvider("Project/thief01/Language Settings", SettingsScope.Project)
            {
                label = "Language Settings",

                guiHandler = (searchContext) =>
                {
                    var settings = LanguageSettings.Instance;
                    
                    var availableLanguages = settings.languages.Where(ctg => ctg != null).Select(ctg => ctg.language.ToString()).ToArray();

                    var lang = (int)settings.defaultLanguage;
                    lang = EditorGUILayout.Popup("Language", lang, availableLanguages);
                    settings.defaultLanguage = (SystemLanguage)lang;
                    SerializedObject serializedSettings = new SerializedObject(settings);
                    SerializedProperty languagesProperty = serializedSettings.FindProperty("languages");

                    serializedSettings.Update();
                    EditorGUILayout.PropertyField(languagesProperty, new GUIContent("Languages"));
                    serializedSettings.ApplyModifiedProperties();
                    
                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(settings);
                    }
                },
                
                keywords = new[] { "language", "unitytranslation", "translation" }
            };

            return provider;
        }
    }
}
