using Unity_Translate.Items;
using Unity_Translate.Translations;
using UnityEditor;
using UnityEngine;

namespace Unity_Translate.Editor
{
    [CustomPropertyDrawer(typeof(LanguageVariable))]
    public class LanguageVariablePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            
            var languageProperty = property.FindPropertyRelative("PreviewLanguage");
            languageProperty.intValue = (int)(SystemLanguage)EditorGUI.EnumPopup(position, languageProperty.displayName, (SystemLanguage)languageProperty.intValue);

            
            var keyProperty = property.FindPropertyRelative("Key");
            var categoryProperty = property.FindPropertyRelative("Category");
            var categories = LanguageSettings.Instance.GetCategories((SystemLanguage)languageProperty.intValue);
            
            GUIStyle style = new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState()
                {
                    textColor = Color.red
                }
            };
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
            
            var keys = LanguageSettings.Instance.GetKeys((SystemLanguage)languageProperty.intValue, categoryProperty.intValue);
            
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
            var languageProperty = property.FindPropertyRelative("PreviewLanguage");
            var categories = LanguageSettings.Instance.GetCategories((SystemLanguage)languageProperty.intValue);
            if (categories == null)
            {
                return EditorGUIUtility.singleLineHeight * 2;
            }
            
            if (categories.Length == 0)
            {
                return EditorGUIUtility.singleLineHeight * 2;
            }
            
            var selectedCategory = property.FindPropertyRelative("Category").intValue;
            var keys = LanguageSettings.Instance.GetKeys((SystemLanguage)languageProperty.intValue, selectedCategory);
            if (keys == null || keys.Length == 0)
            {
                return EditorGUIUtility.singleLineHeight * 3;
            }
            return EditorGUIUtility.singleLineHeight * 3;
        }
    }
}
