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

                // Rysowanie GUI w zakładce ustawień
                guiHandler = (searchContext) =>
                {
                    var settings = LanguageSettings.Instance;

                    EditorGUILayout.LabelField("Example Settings", EditorStyles.boldLabel);
                    var availableLanguages = settings.languages.Where(ctg => ctg != null).Select(ctg => ctg.language.ToString()).ToArray();

                    var lang = (int)settings.defaultLanguage;
                    lang = EditorGUILayout.Popup("Language", lang, availableLanguages);
                    settings.defaultLanguage = (SystemLanguage)lang;
                    SerializedObject serializedSettings = new SerializedObject(settings);
                    SerializedProperty languagesProperty = serializedSettings.FindProperty("languages");

                    serializedSettings.Update(); // Synchronizacja danych
                    EditorGUILayout.PropertyField(languagesProperty, new GUIContent("Languages"));
                    serializedSettings.ApplyModifiedProperties(); // Zapis zmian

                    
                    // settings.exampleString = EditorGUILayout.TextField("Example String", settings.exampleString);
                    // settings.exampleInt = EditorGUILayout.IntField("Example Int", settings.exampleInt);
                    // settings.exampleBool = EditorGUILayout.Toggle("Example Bool", settings.exampleBool);

                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(settings);
                    }
                },

                // Opcjonalnie: klucz wyszukiwania
                keywords = new[] { "language", "unitytranslation", "translation" }
            };

            return provider;
        }
    }
}
