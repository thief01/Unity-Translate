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

                return instance;
            }
        }
        
        private static LanguageSettings instance;
        
        public SystemLanguage defaultLanguage = SystemLanguage.English;

        public List<Language> languages = new List<Language>();
    }
}
