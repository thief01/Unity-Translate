using Ultimate_Translation.Items;
using UnityEngine;

namespace Ultimate_Translation.Translations
{
    public class LocalizedAudio : LocalizedComponent
    {
        private AudioSource audioSource;
        
        protected override void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            base.Awake();
        }
        
        protected override void UpdateLang()
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
            audioSource.clip = languageItem.audioClip;
        }

        protected override LanguageTranslationType GetTranslationType()
        {
            return LanguageTranslationType.Audio;
        }
    }
}
