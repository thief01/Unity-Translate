#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WRA.General.Language.Editor
{
    public class LanguageEditor : EditorWindow
    {
        private List<Language> langs = new List<Language>();

        private List<LanguageItemEditor> missingTranslations = new();

        private int choicedLang = 0;
        private int choicedCategory = 0;
        private Vector2 scrollView;
        
        private string newCategory = "New category";
        private string newKey = "New key";
        private string newTranslation = "New translation";
        
        [MenuItem("thief01/Tools/Language Editor")]
        private static void OpenWindow()
        {
            LanguageEditor window = (LanguageEditor)EditorWindow.GetWindow(typeof(LanguageEditor));
            window.Show();
        }

        private void OnValidate()
        {
            InitLangs();
        }

        private void OnEnable()
        {
            InitLangs();
        }
        
        private void InitLangs()
        {
            AssetDatabase.Refresh();
            LanguageManager.LoadLanguage();
            langs = LanguageManager.Languages;
            missingTranslations.Clear();
            LanguageMissingTranslationsLogger.LoadMissingTranslations();
            LanguageMissingTranslationsLogger.MissingTranslations.ForEach(ctg =>
            {
                missingTranslations.Add(new LanguageItemEditor()
                {
                    Key = ctg,
                    Category = "Missing",
                    Translation = ctg,
                    IsMissing = true
                });
            });
            RefreshStateOfMissingTranslations();
        }

        private void OnGUI()
        {
            LanguageSelection();
            CategorySelection();
            AddingTranslation();
            
            scrollView = GUILayout.BeginScrollView(scrollView);
            DrawLangView();
            DrawMissingTranslations();
            GUILayout.EndScrollView();
        }

        private void LanguageSelection()
        {
            GUILayout.BeginHorizontal();
            var tempLang = EditorGUILayout.Popup(choicedLang, langs.Select(ctg => ctg.ShortLanguageName).ToArray());
            if (choicedLang != tempLang)
            {
                choicedLang = tempLang;
                choicedCategory = 0;
                RefreshStateOfMissingTranslations();
            }
            if (GUILayout.Button("Reload languages"))
            {
                InitLangs();
            }
            if (GUILayout.Button("Save language"))
            {
                SaveLanguages();
            }
            GUILayout.EndHorizontal();
        }
        
        private void RefreshStateOfMissingTranslations()
        {
            missingTranslations.ForEach(ctg =>
            {
                ctg.IsMissing = !langs[choicedLang].HasTranslation(ctg.Key);
            });
        }
        
        private void CategorySelection()
        {
            GUILayout.BeginHorizontal();
            var tempCategory = EditorGUILayout.Popup(choicedCategory, langs[choicedLang].Categories.ToArray());
            if (choicedCategory != tempCategory)
            {
                choicedCategory = tempCategory;
            }
            newCategory = EditorGUILayout.TextField(newCategory);
            if (GUILayout.Button("Add category"))
            {
                langs[choicedLang].Categories.Add(newCategory);
                choicedCategory = langs[choicedLang].Categories.Count - 1;
            }
            if (GUILayout.Button("Remove category"))
            {
                langs[choicedLang].RemoveCategory(langs[choicedLang].Categories[choicedCategory]);
                choicedCategory = 0;
                return;
            }
            GUILayout.EndHorizontal();
        }
        
        private void AddingTranslation()
        {
            GUILayout.BeginHorizontal();
            
            newKey = EditorGUILayout.TextField(newKey);
            newTranslation = EditorGUILayout.TextField(newTranslation);
            if (GUILayout.Button("Add translation"))
            {
                langs[choicedLang].AddTranslation(newKey, new LanguageItem()
                {
                    Key = newKey,
                    Category = langs[choicedLang].Categories[choicedCategory],
                    Translation = newTranslation
                });
            }
            GUILayout.EndHorizontal();
        }

        private void DrawLangView()
        {
            EditorGUILayout.HelpBox("Translations", MessageType.Info);

            var translations = langs[choicedLang].GetTranslationsByCategory(langs[choicedLang].Categories[choicedCategory]);
            translations = translations.OrderBy(ctg => ctg.Key).ToDictionary(ctg => ctg.Key, ctg => ctg.Value);
            
            foreach (var translation in translations)
            {
                DrawSingleTranslationLine(translation.Value, "-", RemoveTranslation);
            }
        }
        
        private void DrawMissingTranslations()
        {
            EditorGUILayout.HelpBox("Missing translations", MessageType.Info);
            foreach (var translation in missingTranslations)
            {
                if (translation.IsMissing)
                {
                    DrawSingleTranslationLine(translation, "+", AddMissingTranslation);
                }
            }
        }

        private void DrawSingleTranslationLine(LanguageItem languageItem, string buttonActionText,
            Action<LanguageItem> onButtonClick)
        {
            GUILayout.BeginHorizontal();
            languageItem.Translation = EditorGUILayout.TextField(languageItem.Key, languageItem.Translation);
            if (GUILayout.Button(buttonActionText, GUILayout.Width(20)))
            {
                onButtonClick.Invoke(languageItem);
            }
            GUILayout.EndHorizontal();
        }
        
        private void RemoveTranslation(LanguageItem languageItem)
        {
            langs[choicedLang].RemoveTranslation(languageItem.Key);
            RefreshStateOfMissingTranslations();
        }
        
        private void AddMissingTranslation(LanguageItem languageItem)
        {
            languageItem.Category = langs[choicedLang].Categories[choicedCategory];
            langs[choicedLang].AddTranslation(languageItem);
            RefreshStateOfMissingTranslations();
        }
        
        private void SaveLanguages()
        {
            foreach (var lang in langs)
            {
                var path = LanguageManager.LANG_PATH + lang.ShortLanguageName + ".xml";
                var xml = lang.GetLanguageAsXml();
                
                StreamWriter sw = new StreamWriter(path, false);
                sw.Write(xml);
                sw.Close();
            }
        }
    }
}
#endif