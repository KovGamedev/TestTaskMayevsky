using UnityEngine;

namespace Scorewarrior.Test.Configs
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "ScriptableObjects/CharacterConfig")]
    public class CharacterConfig : ScriptableObject
    {
		[SerializeField, Range(0f, 1f)] private float _accuracy;
		[SerializeField, Range(0f, 1f)] private float _dexterity;
		[SerializeField, Min(0f)] private float _maxHealth;
		[SerializeField, Min(0f)] private float _maxArmor;
        [SerializeField, Min(0f)] private float _aimTime;
        [SerializeField, Min(0f)] private int _modifiersQuantity;

        public float GetAccuracy() => _accuracy;

        public float GetDexterity() => _dexterity;

        public float GetMaxHealth() => _maxHealth;

        public float GetMaxArmor() => _maxArmor;

        public float GetAimTime() => _aimTime;

        public int GetModifiersQuantity() => _modifiersQuantity;
    }
}