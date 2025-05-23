using UnityEngine;

namespace LocalizationSystem.Data
{
    [CreateAssetMenu(fileName = "LocalizationData_", menuName = "Localization/New localization data", order = 0)]
    public class LocalizationData : ScriptableObject
    {
        [field:SerializeField] public string Yandexi18nLang { get; private set; }
        [field:SerializeField] public TextAsset LocalizationJsonFile { get; private set; }
        [field:SerializeField] public FontHolder OverrideFontAsset { get; private set; }
    }
}