using UnityEngine;

namespace Scorewarrior.Test.Descriptors
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "ScriptableObjects/CharacterConfig")]
    public class CharacterConfig : ScriptableObject
    {
		[SerializeField] private float _accuracy;
		[SerializeField] private float _dexterity;
		[SerializeField] private float _maxHealth;
		[SerializeField] private float _maxArmor;
        [SerializeField] private float _aimTime;

        public float GetAccuracy() => _accuracy;

        public float GetDexterity() => _dexterity;

        public float GetMaxHealth() => _maxHealth;

        public float GetMaxArmor() => _maxArmor;

        public float GetAimTime() => _aimTime;
    }
}