using System.Linq;
using Ultimate_Translation.Items;
using UnityEditor;
using UnityEngine;

namespace Ultimate_Translation
{
    public static class LanguageSettingsProvider
    {
        private static SystemLanguage choicedLanguage;
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
                    
                    CreatingLanguage();
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

        public static void CreatingLanguage()
        {
            GUILayout.BeginHorizontal();
            
            choicedLanguage = (SystemLanguage)EditorGUILayout.EnumPopup("Language", choicedLanguage);
            
            if(GUILayout.Button("Create Language"))
            {
                LanguageSettings.Instance.CreateLanguage(choicedLanguage);
            }
            GUILayout.EndHorizontal();
        }
    }
}
