using UnityEngine;

namespace Unity_Translate.Scripts.Items
{
    [System.Serializable]
    public class LanguageItem
    {
        public string key;
        public string translation;
        public AudioClip audioClip;
        public Sprite sprite;
        
        public bool CheckType(LanguageTranslationType type)
        {
            switch (type)
            {
                case LanguageTranslationType.Text:
                    return !string.IsNullOrEmpty(translation);
                case LanguageTranslationType.Image:
                    return sprite != null;
                case LanguageTranslationType.Audio:
                    return audioClip != null;
                default:
                    return false;
            }
        }
    }
}
