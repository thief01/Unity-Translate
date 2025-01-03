using TMPro;
using UnityEngine;

namespace WRA.General.Language
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextTranslator : MonoBehaviour
    {
        public TMP_Text TMPText
        {
            get
            {
                if (tmpText == null)
                {
                    tmpText = GetComponent<TMP_Text>();
                }

                return tmpText;
            }
        }
        
        [SerializeField] private string textKey;
        
        private TMP_Text tmpText;
        private string[] formatingTexts;

        private void Awake()
        {
            tmpText = GetComponent<TMP_Text>();
            RegisterEvents();
            UpdateLang();
        }

        private void OnEnable()
        {
            UpdateLang();
        }

        private void OnDestroy()
        {
            UnRegisterEvents();
        }
        
        private void RegisterEvents()
        {
            LanguageManager.LanguageChanged.AddListener(UpdateLang); 
        }

        private void UnRegisterEvents()
        {
            LanguageManager.LanguageChanged.RemoveListener(UpdateLang);
        }

        public void SetTextsToFormat(params string[] texts)
        {
            formatingTexts = texts;
            UpdateLang();
        }

        public void SetTextKey(string textKey)
        {
            this.textKey = textKey;
            UpdateLang();
        }

        protected virtual void UpdateLang()
        {
            var text= LanguageManager.GetTranslation(textKey);
            if (formatingTexts != null)
            {
                TMPText.text = string.Format(text, formatingTexts);
                return;
            }
            TMPText.text = text;
        }
    }
}
