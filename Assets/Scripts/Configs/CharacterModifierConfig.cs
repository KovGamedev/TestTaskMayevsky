using UnityEngine;

namespace Scorewarrior.Test.Configs
{
    [CreateAssetMenu(fileName = "CharacterModifierConfig", menuName = "ScriptableObjects/CharacterModifierConfig")]
    public class CharacterModifierConfig : ScriptableObject
    {
		[SerializeField] private float _accuracy;
		[SerializeField] private float _dexterity;
		[SerializeField] private float _health;
		[SerializeField] private float _armor;
        [SerializeField] private float _aimTime;

        public float GetAccuracy() => _accuracy;

        public float GetDexterity() => _dexterity;

        public float GetHealth() => _health;

        public float GetArmor() => _armor;

        public float GetAimTime() => _aimTime;
    }
}