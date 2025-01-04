using UnityEngine;
using UnityEngine.UI;

namespace Unity_Translate.Scripts.Translations
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
    }
}
