using Unity_Translate.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Unity_Translate.Translations
{
    [RequireComponent(typeof(Image))]
    public class LocalizedImage : LocalizedComponent
    {
        private Image image;

        protected override void Awake()
        {
            image = GetComponent<Image>();
            base.Awake();
        }

        protected override void UpdateLang()
        {
            image.sprite = languageItem.sprite;
        }
        
        protected override LanguageTranslationType GetTranslationType()
        {
            return LanguageTranslationType.Image;
        }
    }
}
