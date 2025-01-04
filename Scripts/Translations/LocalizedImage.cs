using UnityEngine;
using UnityEngine.UI;

namespace Unity_Translate.Scripts.Translations
{
    [RequireComponent(typeof(Image))]
    public class LocalizedImage : LocalizedComponent
    {
        public Image Image
        {
            get
            {
                if (image == null)
                {
                    image = GetComponent<Image>();
                }

                return image;
            }
        }
        
        private Image image;
        
        protected override void UpdateLang()
        {
            
        }
    }
}
