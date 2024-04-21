using UnityEngine;

namespace Scorewarrior.Test.Configs
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "ScriptableObjects/WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField, Min(0f)] private float _damage;
        [SerializeField, Range(0f, 1f)] private float _accuracy;
        [SerializeField, Min(0f)] private float _fireRate;
        [SerializeField, Min(0f)] private uint _clipSize;
        [SerializeField, Min(0f)] private float _reloadTime;
        [SerializeField, Min(0f)] private int _modifiersQuantity;

        public float GetDamage() => _damage;

        public float GetAccuracy() => _accuracy;

        public float GetFireRate() => _fireRate;

        public uint GetClipSize() => _clipSize;

        public float GetReloadTime() => _reloadTime;

        public int GetModifiersQuantity() => _modifiersQuantity;
    }
}
