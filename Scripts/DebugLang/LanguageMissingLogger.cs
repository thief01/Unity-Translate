using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity_Translate.Items;
using Unity_Translate.Translations;
using UnityEngine;

namespace Unity_Translate.DebugLang
{
    
    public class LanguageMissingLogger
    {
        private const string MISSING_TRANSLATIONS_FILE = "MissingTranslations.data";
        private static readonly string MISSING_TRANSLATIONS_PATH = Application.streamingAssetsPath + "/Data/";
        private static readonly string MISSING_TRANSLATIONS_FILE_PATH =
            MISSING_TRANSLATIONS_PATH + MISSING_TRANSLATIONS_FILE;

        public static LanguageMissingLogger Instance => instance ??= new LanguageMissingLogger();

        private static LanguageMissingLogger instance;

        private List<string> missingKeys;

        private LanguageMissingLogger()
        {
            LoadMissingTranslations();
            Application.quitting += SaveMissingTranslations;
        }
    
        public void AddMissingTranslations(string key)
        {
            if (missingKeys.Contains(key))
                return;
            missingKeys.Add(key);
        }

        public void LogMissingTranslation(LanguageVariable languageVariable, LanguageTranslationType type, SystemLanguage language)
        {
            LogMissingTranslation(languageVariable.Category.ToString(), languageVariable.Key.ToString(), type, language);
        }
        
        public void LogMissingTranslation(string category, string key, LanguageTranslationType type, SystemLanguage language)
        {
            AddMissingTranslations($"{category}.{key}.{type}.{language}");
        }
        
        public void LoadMissingTranslations()
        {
            CheckPath();

            StreamReader sr = new StreamReader(MISSING_TRANSLATIONS_FILE_PATH);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                AddMissingTranslations(line);
            }
            sr.Close();
        }
        
        public void SaveMissingTranslations()
        {
            CheckPath();
            StreamWriter sw = new StreamWriter(MISSING_TRANSLATIONS_FILE_PATH, false, Encoding.Default);
            foreach (var key in missingKeys)
            {
                sw.WriteLine(key);
            }
            sw.Close();
        }
        
        private void CheckPath()
        {
            if (!Directory.Exists(MISSING_TRANSLATIONS_PATH))
            {
                Directory.CreateDirectory(MISSING_TRANSLATIONS_PATH);
            }

            if (!File.Exists(MISSING_TRANSLATIONS_FILE_PATH))
            {
                File.Create(MISSING_TRANSLATIONS_FILE_PATH);
            }
        }
    }
}
