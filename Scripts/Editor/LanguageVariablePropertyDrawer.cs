using Unity_Translate.Items;
using Unity_Translate.Translations;
using UnityEditor;
using UnityEngine;

namespace Unity_Translate.Editor
{
    [CustomPropertyDrawer(typeof(LanguageVariable))]
    public class LanguageVariablePropertyDrawer : PropertyDrawer
    {
        private readonly GUIStyle style = new GUIStyle()
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            normal = new GUIStyleState()
            {
                textColor = Color.red
            }
        };
        
        private string[] categories;
        private string[] keys;
        private double lastUpdateTimeCategories;
        private double lastUpdateTimeKeys;
        private SystemLanguage language;
        private int selectedCategory;
        private int selectedKey;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            
            var languageProperty = property.FindPropertyRelative("PreviewLanguage");
            languageProperty.intValue = (int)(SystemLanguage)EditorGUI.EnumPopup(position, languageProperty.displayName, (SystemLanguage)languageProperty.intValue);
            UpdateCategoriesCache();
            UpdateKeysCache();
            
            var keyProperty = property.FindPropertyRelative("Key");
            var categoryProperty = property.FindPropertyRelative("Category");

            position.y += EditorGUIUtility.singleLineHeight;
            if (categories == null)
            {
                EditorGUI.LabelField(position, "Not found language", style);
                return;
            }
            
            if (categories.Length == 0)
            {
                EditorGUI.LabelField(position, "Not found categories", style);
                return;
            }
            
            GUIContent[] content = new GUIContent[categories.Length];
            for (int i = 0; i < categories.Length; i++)
            {
                content[i] = new GUIContent(categories[i]);
                
            }
            
            categoryProperty.intValue = EditorGUI.Popup(position,categoryProperty.intValue, content);
            position.y += EditorGUIUtility.singleLineHeight;
            
            if (keys == null || keys.Length == 0)
            {
                EditorGUI.LabelField(position, "Not found keys", style);
                return;
            }
            
            GUIContent[] keysContent = new GUIContent[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                keysContent[i] = new GUIContent(keys[i]);
            }
            keyProperty.intValue = EditorGUI.Popup(position, keyProperty.intValue, keysContent);
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (categories == null)
            {
                return EditorGUIUtility.singleLineHeight * 2;
            }
            
            if (categories.Length == 0)
            {
                return EditorGUIUtility.singleLineHeight * 2;
            }
            
            return EditorGUIUtility.singleLineHeight * 3;
        }

        private void UpdateCategoriesCache()
        {
            var shouldUpdate = (EditorApplication.timeSinceStartup - lastUpdateTimeCategories > 5);
            if (categories == null || categories.Length == 0)
            {
                shouldUpdate = true;
            }
            if (!shouldUpdate)
                return;
            categories = LanguageSettings.Instance.GetCategories(language);
            lastUpdateTimeCategories = EditorApplication.timeSinceStartup;
        }

        private void UpdateKeysCache()
        {
            var shouldUpdate = (EditorApplication.timeSinceStartup - lastUpdateTimeKeys > 5);
            if (keys == null || keys.Length == 0)
            {
                shouldUpdate = true;
            }

            if (!shouldUpdate)
                return;
            
            keys = LanguageSettings.Instance.GetKeys(language, selectedCategory);
            
            lastUpdateTimeKeys = EditorApplication.timeSinceStartup;
        }
    }
}
