using Ultimate_Translation.Items;
using Ultimate_Translation.Translations;

namespace Ultimate_Translation
{
    public static class LanguageManagerExtensions
    {
        public static void SetLanguageVariable(this LanguageVariable localizedComponent, LanguageTranslationType type)
        {
            LanguageManager.Instance.GetTranslation(localizedComponent, type);
        }
    }
}