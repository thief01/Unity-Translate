using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity_Translate.Items;
using Unity_Translate.Translations;
using UnityEngine;

namespace Unity_Translate.DebugLang
{
    
    public class LanguageMissingsLogger
    {
        private const string MISSING_TRANSLATIONS_FILE = "MissingTranslations.data";
        private static readonly string MISSING_TRANSLATIONS_PATH = Application.streamingAssetsPath + "/Data/";
        private static readonly string MISSING_TRANSLATIONS_FILE_PATH =
            MISSING_TRANSLATIONS_PATH + MISSING_TRANSLATIONS_FILE;

        public static LanguageMissingsLogger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LanguageMissingsLogger();
                }
                return instance;
            }
        }

        private static LanguageMissingsLogger instance;
    
        public List<string> MissingTranslations => missingKeys;
    
        private List<string> missingKeys = new();
        private bool isMissingKeysLoaded = false;
    
        public void AddMissingTranslations(string key)
        {
            if (missingKeys.Contains(key))
                return;
            missingKeys.Add(key);
        }

        public void LogMissingTranslation(string category, string key, LanguageTranslationType type, SystemLanguage language)
        {
            
        }
        
        public void LogMissingTranslation(LanguageVariable languageVariable, LanguageTranslationType type, SystemLanguage language)
        {
            
        }

        public void SaveMissingTranslations()
        {
            CheckPath();
            LoadMissingTranslations();
            WriteMissingTranslations();
        }
    
        public void LoadMissingTranslations()
        {
            if (isMissingKeysLoaded)
                return;
            if (!Directory.Exists(MISSING_TRANSLATIONS_PATH))
            {
                Directory.CreateDirectory(MISSING_TRANSLATIONS_PATH);
            }

            StreamReader sr = new StreamReader(MISSING_TRANSLATIONS_FILE_PATH);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                AddMissingTranslations(line);
            }
            sr.Close();
        }
    
        private void CheckPath()
        {
            if (!Directory.Exists(MISSING_TRANSLATIONS_PATH))
            {
                Directory.CreateDirectory(MISSING_TRANSLATIONS_PATH);
                isMissingKeysLoaded = true;
            }

            if (!File.Exists(MISSING_TRANSLATIONS_FILE_PATH))
            {
                File.Create(MISSING_TRANSLATIONS_FILE_PATH);
                isMissingKeysLoaded = true;
            }
        }
    
        private void WriteMissingTranslations()
        {
            return;
            StreamWriter sw = new StreamWriter(MISSING_TRANSLATIONS_FILE_PATH, false, Encoding.Default);
            foreach (var key in missingKeys)
            {
                sw.WriteLine(key);
            }
            sw.Close();
        }
    }
}
