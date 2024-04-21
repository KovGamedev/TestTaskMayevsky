using UnityEngine;

namespace Scorewarrior.Test.Configs
{
    [CreateAssetMenu(fileName = "WeaponModifierConfig", menuName = "ScriptableObjects/WeaponModifierConfig")]
    public class WeaponModifierConfig : ScriptableObject
    {
        [SerializeField] private float _accuracy;
        [SerializeField] private float _fireRate;
        [SerializeField] private uint _clipSize;
        [SerializeField] private float _reloadTime;

        public float GetAccuracy() => _accuracy;

        public float GetFireRate() => _fireRate;

        public uint GetClipSize() => _clipSize;

        public float GetReloadTime() => _reloadTime;
    }
}