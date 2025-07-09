using UnityEngine;

namespace Ultimate_Translation.Items
{
    public class LanguageData
    {
        public string translation;
        public Sprite sprite;
        public AudioClip audioClip;

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