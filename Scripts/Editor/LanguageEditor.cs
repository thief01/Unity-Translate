#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity_Translate.Items;
using UnityEditor;
using UnityEngine;

namespace Unity_Translate.Editor
{
    public class LanguageEditor : EditorWindow
    {
        private Language UsingLanguage => langs[choicedLang];
        
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
            langs = LanguageSettings.Instance.languages;
            missingTranslations.Clear();
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
            var langs = LanguageSettings.Instance.languages.Select(ctg => ctg.language.ToString()).ToArray();
            var tempLang = EditorGUILayout.Popup(choicedLang, langs);
            if (choicedLang != tempLang)
            {
                choicedLang = tempLang;
                choicedCategory = 0;
            }
            if (GUILayout.Button("Reload languages"))
            {
                InitLangs();
            }
            if (GUILayout.Button("Save language"))
            {
                
                // set dirty
                EditorUtility.SetDirty(UsingLanguage);
                
                // SaveLanguages();
            }
            GUILayout.EndHorizontal();
        }
        
        private void CategorySelection()
        {
            GUILayout.BeginHorizontal();
            var categories = UsingLanguage.languageCategories.Select(ctg => ctg.categoryName).ToArray();
            var tempCategory = EditorGUILayout.Popup(choicedCategory, categories);
            if (choicedCategory != tempCategory)
            {
                choicedCategory = tempCategory;
            }
            newCategory = EditorGUILayout.TextField(newCategory);
            if (GUILayout.Button("Add category"))
            {
                UsingLanguage.languageCategories.Add(new LanguageCategory()
                {
                    categoryName = newCategory
                });
                
                choicedCategory = UsingLanguage.languageCategories.Count - 1;
            }
            if (GUILayout.Button("Remove category"))
            {
                UsingLanguage.languageCategories.Remove(UsingLanguage.languageCategories[choicedCategory]);
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
                
                UsingLanguage.languageCategories[choicedCategory].languageItems.Add(new LanguageItem()
                {
                    key = newKey,
                    translation = newTranslation
                });
            }
            GUILayout.EndHorizontal();
        }

        private void DrawLangView()
        {
            EditorGUILayout.HelpBox("Translations", MessageType.Info);
            
            var categoryItems = UsingLanguage.languageCategories[choicedCategory].languageItems;

            var translations = UsingLanguage.languageCategories[choicedCategory].languageItems;
            translations = translations.OrderBy(ctg => ctg.key).ToList();
            
            foreach (var translation in translations)
            {
                DrawSingleTranslationLine(translation, "-", RemoveTranslation);
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
            languageItem.translation = EditorGUILayout.TextField(languageItem.key, languageItem.translation);
            if (GUILayout.Button(buttonActionText, GUILayout.Width(20)))
            {
                onButtonClick.Invoke(languageItem);
            }
            GUILayout.EndHorizontal();
        }
        
        private void RemoveTranslation(LanguageItem languageItem)
        {
            UsingLanguage.languageCategories[choicedCategory].languageItems.Remove(languageItem);
        }
        
        private void AddMissingTranslation(LanguageItem languageItem)
        {

        }
    }
}
#endif