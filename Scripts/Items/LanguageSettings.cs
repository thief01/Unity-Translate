using System.Collections.Generic;
using System.IO;
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
#if UNITY_EDITOR
                        string path = "Assets/Resources/Language Settings.asset";
                        if (!Directory.Exists(path))
                        {
                            var dict = Path.GetDirectoryName(path);
                            Directory.CreateDirectory(dict );
                        }
                        UnityEditor.AssetDatabase.CreateAsset(instance, path);
                        UnityEditor.AssetDatabase.SaveAssets();
                        UnityEditor.AssetDatabase.Refresh();
#endif
                    }
                }
                
                instance.LoadLanguages();

                return instance;
            }
        }
        
        private static LanguageSettings instance;
        
        public SystemLanguage defaultLanguage = SystemLanguage.English;

        public List<Language> languages = new List<Language>();
        
#if UNITY_EDITOR
        
        public void LoadLanguages()
        {
            languages = new List<Language>();
            var guids = UnityEditor.AssetDatabase.FindAssets("t:Language");
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var language = UnityEditor.AssetDatabase.LoadAssetAtPath<Language>(path);
                languages.Add(language);
            }
        }
        
        public void AddLanguage(Language language)
        {
            languages.Add(language);
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
        }
        
        public void RemoveLanguage(Language language)
        {
            languages.Remove(language);
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
        }
        
#endif
    }
}
