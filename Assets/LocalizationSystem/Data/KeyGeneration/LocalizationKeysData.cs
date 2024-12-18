using System.Collections.Generic;
using LocalizationSystem.Data.Extensions;
using UnityEngine;

namespace LocalizationSystem.Data.KeyGeneration
{
    [CreateAssetMenu(fileName = "LocalizationKeysData", menuName = "Localization/New LocalizationKeysData", order = 0)]
    public class LocalizationKeysData : ScriptableObject
    {
        [field: SerializeField] public EnumHolder[] Keys { get; private set; } =
        {
            new()
            {
                Name = "None"
            }
        };

#if UNITY_EDITOR
        [ContextMenu("GenerateKeys")]
        public void Generate()
        {
            
            KeyGenerator.SetEnums(Keys);
            KeyGenerator.GenerateEnumKeys("LocalizationKey");
            KeyGenerator.GenerateDictionaryKeys("LocalizationKeys");
        }
#endif

        private void OnValidate()
        {
            foreach (var key in Keys)
            {
                key.Name = key.Name.ToPascalCase();
            }
        }
    }
}