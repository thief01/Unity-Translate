using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace WRA.General.Language
{
    public static class LanguageMissingTranslationsLogger
    {
        private const string MISSING_TRANSLATIONS_FILE = "MissingTranslations.data";
        private static readonly string MISSING_TRANSLATIONS_PATH = Application.streamingAssetsPath + "/Data/";
        private static readonly string MISSING_TRANSLATIONS_FILE_PATH =
            MISSING_TRANSLATIONS_PATH + MISSING_TRANSLATIONS_FILE;
    
        public static List<string> MissingTranslations => missingKeys;
    
        private static List<string> missingKeys = new();
        private static bool isMissingKeysLoaded = false;
    
        public static void AddMissingTranslations(string key)
        {
            if (missingKeys.Contains(key))
                return;
            missingKeys.Add(key);
        }

        public static void SaveMissingTranslations()
        {
            CheckPath();
            LoadMissingTranslations();
            WriteMissingTranslations();
        }
    
        public static void LoadMissingTranslations()
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
    
        private static void CheckPath()
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
    
        private static void WriteMissingTranslations()
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
