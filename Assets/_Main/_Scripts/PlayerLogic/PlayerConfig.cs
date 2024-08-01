using UnityEngine;

namespace _Main._Scripts.PlayerLogic
{
    [CreateAssetMenu(fileName = "MainPlayerConfig", menuName = "GameConfigs/PlayerConfigs", order = 0)]
    public class PlayerConfig : ScriptableObject
    {
        [Range(0.1f, 10f)] public float speed;

        [Range(2f, 10f)] public float maxLeftRightPosition;
        [Range(1f, 10f)] public float maxXDragDelta;

        [Range(1f, 100f)] public float xSensitivity;
        [Range(1f, 150f)] public float xSensitivityMobile;
        [Range(1f, 50f)] public float xKeySensitivity;
        [Range(0.1f, 10f)] public float xDampingRatio;
        
        [Range(0.1f, 10f) ]public float soldiersMaxPosition;
        [Range(0.1f, 10f) ]public float soldierSpeed;
        
        
        
    }
}