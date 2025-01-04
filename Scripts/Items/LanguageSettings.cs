using System.Collections.Generic;
using UnityEngine;

namespace Unity_Translate.Items
{
    public class LanguageSettings : ScriptableObject
    {
        public static LanguageSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<LanguageSettings>("Language Settings");
                    if (instance == null)
                    {
                        instance = CreateInstance<LanguageSettings>();
                        
                        // Tworzenie pliku w folderze Resources, jeśli brakuje zasobu
#if UNITY_EDITOR
                        string path = "Assets/Resources/Language Settings.asset";
                        UnityEditor.AssetDatabase.CreateAsset(instance, path);
                        UnityEditor.AssetDatabase.SaveAssets();
                        UnityEditor.AssetDatabase.Refresh();
#endif
                    }
                }

                return instance;
            }
        }
        
        private static LanguageSettings instance;
        
        public SystemLanguage defaultLanguage = SystemLanguage.English;

        public List<Language> languages = new List<Language>();
    }
}
