using Scorewarrior.Test.Descriptors;
using Scorewarrior.Test.Views;
using UnityEngine;

namespace Scorewarrior.Test.Models
{
    public class Character
    {
        public float Health { get; set; }
        public float Armor { get; set; }
        public CharacterPrefab Prefab { get; private set; }

        private readonly Weapon _weapon;
        private readonly Battlefield _battlefield;

        private CharacterState _state;
        private Character _currentTarget;
        private float _time;

        public Character(CharacterPrefab prefab, Weapon weapon, Battlefield battlefield, Faction faction)
        {
            Prefab = prefab;
            Prefab.ResetState(faction);
            _weapon = weapon;
            _battlefield = battlefield;
            var config = Prefab.GetConfig();
            Health = config.GetMaxHealth();
            Armor = config.GetMaxArmor();
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
            Prefab.HandleDamage(Armor, Health);
            if (!IsAlive)
            {
                _state = CharacterState.Death;
                Prefab.HandleState(_state);
            }
        }

        public bool IsAlive => Health > 0 || Armor > 0;

        public Vector3 Position => Prefab.transform.position;

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

        private void HandleIdleState()
        {
            Prefab.HandleState(_state);
            if (_battlefield.TryGetNearestAliveEnemy(this, out Character target))
            {
                _currentTarget = target;
                _state = CharacterState.Aiming;
                _time = Prefab.GetConfig().GetAimTime();
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
                        bool hit = random <= Prefab.GetConfig().GetAccuracy() &&
                                random <= _weapon.Prefab.GetConfig().GetAccuracy() &&
                                random >= _currentTarget.Prefab.GetConfig().GetDexterity();
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
                    _time = _weapon.Prefab.GetConfig().GetReloadTime();
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
    }
}