using Scorewarrior.Test.Configs;
using Scorewarrior.Test.Views;
using UnityEngine;
using UnityEngine.Events;

namespace Scorewarrior.Test.Models
{
    public class Character
    {
        public float Health { get; set; }
        public float Armor { get; set; }
        public CharacterPrefab Prefab { get; private set; }

        private readonly Weapon _weapon;
        private readonly Battlefield _battlefield;

        private UnityEvent _deathEvent = new();
        private CharacterState _state;
        private Character _currentTarget;
        private float _time;
        private float _maxHealth;
        private float _maxArmor;
        private Faction _faction;
        private float _accuracy;
        private float _dexterity;
        private float _aimTime;

        public Character(
            CharacterPrefab prefab,
            Weapon weapon,
            Battlefield battlefield,
            Faction faction,
            CharacterModifierConfig[] characterModifiers
        )
        {
            Prefab = prefab;
            _faction = faction;
            Prefab.ResetState(_faction);
            _weapon = weapon;
            _battlefield = battlefield;
            HandleConfigs(Prefab.GetConfig(), characterModifiers);
        }

        public void GetDamage(float damage)
        {
            if (Armor > 0)
            {
                Armor = Mathf.Clamp(Armor - damage, 0, Armor);
            }
            else if (Health > 0)
            {
                Health = Mathf.Clamp(Health - damage, 0, Health);
            }
            Prefab.HandleDamage(Armor / _maxArmor, Health / _maxHealth);
            if (!IsAlive)
            {
                _state = CharacterState.Death;
                Prefab.HandleState(_state);
                _deathEvent.Invoke();
                _deathEvent.RemoveAllListeners();
            }
        }

        public UnityEvent GetDeathEvent() => _deathEvent;

        public bool IsAlive => Health > 0 || Armor > 0;

        public Vector3 Position => Prefab.transform.position;

        public Faction GetFaction() => _faction;

        public void Update(float deltaTime)
        {
            if (!IsAlive)
                return;

            switch (_state)
            {
                case CharacterState.Idle:
                    HandleIdleState();
                    break;
                case CharacterState.Aiming:
                    HandleAimingState(deltaTime);
                    break;
                case CharacterState.Shooting:
                    HandleShootingState(deltaTime);
                    break;
                case CharacterState.Reloading:
                    HandleReloadingState(deltaTime);
                    break;
            }
        }

        private void HandleConfigs(CharacterConfig config, CharacterModifierConfig[] characterModifiers)
        {
            Health = config.GetMaxHealth();
            Armor = config.GetMaxArmor();
            _accuracy = config.GetAccuracy();
            _dexterity = config.GetDexterity();
            _aimTime = config.GetAimTime();

            for (var i = 0; i < config.GetModifiersQuantity(); i++)
            {
                var modifier = characterModifiers[Random.Range(0, characterModifiers.Length)];
                Health = Mathf.Clamp(Health + modifier.GetHealth(), 0f, Health + modifier.GetHealth());
                Armor = Mathf.Clamp(Armor + modifier.GetArmor(), 0f, Armor + modifier.GetArmor());
                _accuracy = Mathf.Clamp(_accuracy + modifier.GetAccuracy(), 0f, 1f);
                _dexterity = Mathf.Clamp(_dexterity + modifier.GetDexterity(), 0f, 1f);
                _aimTime = Mathf.Clamp(_aimTime + modifier.GetAimTime(), 0f, _aimTime + modifier.GetAimTime());
            }
            _maxHealth = Health;
            _maxArmor = Armor;
        }

        private void HandleIdleState()
        {
            Prefab.HandleState(_state);
            if ((_currentTarget == null || !_currentTarget.IsAlive) && _battlefield.TryGetNearestAliveEnemy(this, out Character target))
            {
                _currentTarget = target;
            }
            if (_currentTarget != null && _currentTarget.IsAlive)
            {
                _state = CharacterState.Aiming;
                _time = _aimTime;
                Prefab.HandleNewTarget(_currentTarget.Prefab.transform);
            }
        }

        private void HandleAimingState(float deltaTime)
        {
            Prefab.HandleState(_state);
            if (_currentTarget != null && _currentTarget.IsAlive)
            {
                if (_time > 0)
                {
                    _time -= deltaTime;
                }
                else
                {
                    _state = CharacterState.Shooting;
                    _time = 0;
                }
            }
            else
            {
                _state = CharacterState.Idle;
                _time = 0;
            }
        }

        private void HandleShootingState(float deltaTime)
        {
            Prefab.HandleState(CharacterState.Aiming);
            if (_currentTarget != null && _currentTarget.IsAlive)
            {
                if (_weapon.HasAmmo)
                {
                    if (_weapon.IsReady)
                    {
                        float random = Random.value;
                        bool hit = random <= _accuracy && random <= _weapon.Accuracy && random >= _dexterity;
                        _weapon.Fire(_currentTarget, hit);
                        Prefab.HandleState(_state);
                    }
                    else
                    {
                        _weapon.Update(deltaTime);
                    }
                }
                else
                {
                    _state = CharacterState.Reloading;
                    _time = _weapon.ReloadTime;
                }
            }
            else
            {
                _state = CharacterState.Idle;
            }
        }

        private void HandleReloadingState(float deltaTime)
        {
            Prefab.HandleState(_state);
            if (_time > 0)
            {
                _time -= deltaTime;
            }
            else
            {
                if (_currentTarget != null && _currentTarget.IsAlive)
                {
                    _state = CharacterState.Shooting;
                }
                else
                {
                    _state = CharacterState.Idle;
                }
                _weapon.Reload();
                _time = 0;
            }
        }

        ~Character()
        {
            _deathEvent.RemoveAllListeners();
        }
    }
}