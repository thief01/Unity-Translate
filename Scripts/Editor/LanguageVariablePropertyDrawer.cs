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
            var keyProperty = property.FindPropertyRelative("Key");
            var categoryProperty = property.FindPropertyRelative("Category");
            var categories = LanguageManager.GetCategories();

            GUIStyle style = new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState()
                {
                    textColor = Color.red
                }
            };
            if (categories == null)
            {
                EditorGUI.LabelField(position, "Not found language", style);
                return;
            }

            if (categories.Count == 0)
            {
                EditorGUI.LabelField(position, "Not found categories", style);
                return;
            }

            categoryProperty.intValue = EditorGUI.Popup(position, categoryProperty.displayName,categoryProperty.intValue, categories.ToArray());
            position.y += EditorGUIUtility.singleLineHeight;

            var keys = LanguageManager.GetKeys(categories[categoryProperty.intValue]).ToArray();
            
            if (keys.Length == 0)
            {
                EditorGUI.LabelField(position, "Not found keys", style);
                return;
            }
            keyProperty.intValue = EditorGUI.Popup(position, keyProperty.displayName, keyProperty.intValue, keys);
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var categories = LanguageManager.GetCategories();
            if (categories == null)
            {
                return EditorGUIUtility.singleLineHeight;
            }
            
            if (categories.Count == 0)
            {
                return EditorGUIUtility.singleLineHeight;
            }
            
            var selectedCategory = property.FindPropertyRelative("Category").intValue;
            if (LanguageManager.GetKeys(LanguageManager.GetCategories()[selectedCategory]).Count == 0)
            {
                return EditorGUIUtility.singleLineHeight * 2;
            }
            return EditorGUIUtility.singleLineHeight * 2;
        }
    }
}
