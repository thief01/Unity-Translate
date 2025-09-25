#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Ultimate_Translation.Items;
using UnityEditor;
using UnityEngine;

namespace Ultimate_Translation.Editor
{
    public class LanguageEditor : EditorWindow
    {
        private Language UsingLanguage => langs[choicedLang];

        private List<Language> langs = new();
        

        private int choicedLang = 0;
        private int choicedCategory = 0;
        private Vector2 scrollView;

        private string newCategory = "New category";
        private string newKey = "New key";
        private string newTranslation = "New translation";
        private string errorMsg = "";

        [MenuItem("Tools/thief01/Language Editor")]
        private static void OpenWindow()
        {
            LanguageEditor window = (LanguageEditor)GetWindow(typeof(LanguageEditor));
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
        }

        private void OnGUI()
        {
            UpdateLangList();
            LanguageSelection();
            CategorySelection();
            AddingTranslation();

            scrollView = GUILayout.BeginScrollView(scrollView);
            DrawLangView();
            GUILayout.EndScrollView();

            EditorUtility.SetDirty(UsingLanguage);
        }

        private void UpdateLangList()
        {
            LanguageSettings.Instance.LoadLanguages();
            if (langs.Count != LanguageSettings.Instance.languages.Count)
            {
                langs = LanguageSettings.Instance.languages;
                if (choicedLang >= langs.Count)
                {
                    choicedLang = langs.Count - 1;
                }
            }
        }

        private void LanguageSelection()
        {
            GUILayout.BeginHorizontal();
            var langs = LanguageSettings.Instance.languages.Select(ctg => $"{ctg.name} ({ctg.language})").ToArray();
            var tempLang = EditorGUILayout.Popup(choicedLang, langs);
            if (choicedLang != tempLang)
            {
                choicedLang = tempLang;
                choicedCategory = 0;
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
                errorMsg = "";
                if (!UsingLanguage.AddCategory(newCategory))
                {
                    errorMsg = "Category already exists";
                    return;
                }

                choicedCategory = UsingLanguage.languageCategories.Count - 1;
            }

            if (UsingLanguage.languageCategories.Count > 0 && GUILayout.Button("Remove category"))
            {
                errorMsg = "";
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
                errorMsg = "";
                if (!UsingLanguage.languageCategories[choicedCategory].AddLanguageItem(newKey, newTranslation))
                {
                    errorMsg = "Translation already exists";
                }
            }

            GUILayout.EndHorizontal();
        }

        private void DrawLangView()
        {
            if (!string.IsNullOrEmpty(errorMsg))
            {
                EditorGUILayout.HelpBox(errorMsg, MessageType.Error);
            }

            var categories = UsingLanguage.languageCategories;
            if (categories.Count == 0)
            {
                errorMsg = "No categories found";
                return;
            }

            var translations = UsingLanguage.languageCategories[choicedCategory].languageItems;
            translations = translations.OrderBy(ctg => ctg.key).ToList();

            foreach (var translation in translations)
            {
                DrawSingleTranslationLine(translation, "-", RemoveTranslation);
            }
        }

        private void DrawSingleTranslationLine(LanguageItem languageItem, string buttonActionText,
            Action<LanguageItem> onButtonClick)
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(languageItem.key);
            GUILayoutOption[] options = { GUILayout.Width(150), GUILayout.Height(50) };
            languageItem.translation = EditorGUILayout.TextArea(languageItem.translation, options);
            languageItem.audioClip =
                (AudioClip)EditorGUILayout.ObjectField(languageItem.audioClip, typeof(AudioClip), false, options);
            languageItem.sprite =
                (Sprite)EditorGUILayout.ObjectField(languageItem.sprite, typeof(Sprite), false, options);
            if (GUILayout.Button(buttonActionText, GUILayout.Width(50), GUILayout.Height(50)))
            {
                errorMsg = "";
                onButtonClick.Invoke(languageItem);
            }

            GUILayout.EndHorizontal();
        }

        private void RemoveTranslation(LanguageItem languageItem)
        {
            UsingLanguage.languageCategories[choicedCategory].languageItems.Remove(languageItem);
        }
    }
}
#endif