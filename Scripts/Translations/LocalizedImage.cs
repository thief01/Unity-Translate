using Ultimate_Translation.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Ultimate_Translation.Translations
{
    [RequireComponent(typeof(Image))]
    public class LocalizedImage : LocalizedComponent
    {
        [SerializeField] private bool setNativeSize = true;
        private Image image;

        protected override void Awake()
        {
            image = GetComponent<Image>();
            base.Awake();
        }

        protected override void UpdateLang()
        {
            if (image == null)
                image = GetComponent<Image>();
            image.sprite = languageItem.sprite;
            if (setNativeSize)
                image.SetNativeSize();
        }
        
        protected override LanguageTranslationType GetTranslationType()
        {
            return LanguageTranslationType.Image;
        }
    }
}
