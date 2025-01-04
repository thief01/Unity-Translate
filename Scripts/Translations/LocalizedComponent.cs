using System;
using Unity_Translate.Items;
using UnityEngine;

namespace Unity_Translate.Scripts.Translations
{
    public abstract class LocalizedComponent : MonoBehaviour
    {
        [SerializeField] private LanguageVariable languageVariable;
        
        protected LanguageItem languageItem;

        protected virtual void Awake()
        {
            RegisterEvents();
            UpdateLanguageItem();
        }

        private void OnDestroy()
        {
            UnRegisterEvents();
        }
        
        public void SetLanguageVariable(LanguageVariable variable)
        {
            languageVariable = variable;
            UpdateLanguageItem();
        }
        
        protected abstract void UpdateLang();

        private void RegisterEvents()
        {
            LanguageManager.LanguageChanged.AddListener(UpdateLanguageItem); 
        }
        
        private void UnRegisterEvents()
        {
            LanguageManager.LanguageChanged.RemoveListener(UpdateLanguageItem);
        }
        
        private void UpdateLanguageItem()
        {
            languageItem = LanguageManager.GetTranslation(languageVariable);
            UpdateLang();
        }
    }
}
