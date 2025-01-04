using Unity_Translate.Items;
using Unity_Translate.Translations;

namespace Unity_Translate
{
    public static class LanguageManagerExtensions
    {
        public static void SetLanguageVariable(this LanguageVariable localizedComponent, LanguageTranslationType type)
        {
            LanguageManager.Instance.GetTranslation(localizedComponent, type);
        }
    }
}