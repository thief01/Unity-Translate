using UnityEngine;

namespace Unity_Translate.Scripts.Translations
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
            audioSource.clip = languageItem.audioClip;
        }
    }
}
