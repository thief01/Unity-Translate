using System;
using TMPro;
using UnityEngine;

namespace Unity_Translate.Scripts.Translations
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedText : LocalizedComponent
    {
        private TMP_Text tmpText;
        private string[] formatingTexts;

        protected override void Awake()
        {
            tmpText = GetComponent<TMP_Text>();
            base.Awake();
        }
        
        public void SetTextsToFormat(params string[] texts)
        {
            formatingTexts = texts;
            UpdateLang();
        }

        protected override void UpdateLang()
        {
            if(formatingTexts.Length > 0)
            {
                tmpText.text = FormatText();
                return;
            }
            tmpText.text = languageItem.translation;
        }

        private string FormatText()
        {
            var text = "";
            try
            {
                text = string.Format(languageItem.translation, formatingTexts);
                return text;
            }
            catch (Exception e)
            {
                Debug.LogError("Error in formatting text: " + e.Message + " with text: " + languageItem.translation +
                               " and formating texts count: " + formatingTexts.Length);
            }
            
            return languageItem.translation;
        }
    }
}
