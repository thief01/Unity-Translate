using Unity_Translate.Items;
using UnityEngine;

namespace Unity_Translate.Translations
{
    public abstract class LocalizedComponent : MonoBehaviour
    {
        public LanguageVariable LanguageVariable => languageVariable;
        
        [SerializeField] private LanguageVariable languageVariable;
        
        protected LanguageData languageItem;

        protected virtual void Awake()
        {
            RegisterEvents();
            UpdateLanguageItem();
        }

        private void OnDestroy()
        {
            UnRegisterEvents();
        }

        public void SetLanguageData(LanguageData languageData)
        {
            languageItem = languageData;
            UpdateLang();
        }
        
        public void SetLanguageVariable(LanguageVariable variable)
        {
            languageVariable = variable;
            UpdateLanguageItem();
        }
        
        protected abstract void UpdateLang();
        
        protected abstract LanguageTranslationType GetTranslationType();

        private void RegisterEvents()
        {
            LanguageManager.LanguageChanged.AddListener(UpdateLanguageItem); 
        }
        
        private void UnRegisterEvents()
        {
            LanguageManager.LanguageChanged.RemoveListener(UpdateLanguageItem);
        }
        
        public void UpdateLanguageItem()
        {
            languageItem = LanguageManager.Instance.GetTranslation(languageVariable, GetTranslationType());
            UpdateLang();
        }
    }
}
