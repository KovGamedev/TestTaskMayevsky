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

        private State _state;
        private Character _currentTarget;
        private float _time;

        private enum State
        {
            Idle,
            Aiming,
            Shooting,
            Reloading,
            Death
        }

        public Character(CharacterPrefab prefab, Weapon weapon, Battlefield battlefield)
        {
            Prefab = prefab;
            _weapon = weapon;
            _battlefield = battlefield;
            CharacterDescriptor descriptor = Prefab.GetComponent<CharacterDescriptor>();
            Health = descriptor.MaxHealth;
            Armor = descriptor.MaxArmor;
        }

        public void GetDamage(float damage)
        {
            if (Armor > 0)
            {
                Armor -= damage;
            }
            else if (Health > 0)
            {
                Health -= damage;
            }
            if (!IsAlive)
            {
                _state = State.Death;
                Prefab.PlayDeath();
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
                case State.Idle:
                    HandleIdleState();
                    break;
                case State.Aiming:
                    HandleAimingState(deltaTime);
                    break;
                case State.Shooting:
                    HandleShootingState(deltaTime);
                    break;
                case State.Reloading:
                    HandleReloadingState(deltaTime);
                    break;
            }
        }

        private void HandleIdleState()
        {
            Prefab.PlayIdle();
            if (_battlefield.TryGetNearestAliveEnemy(this, out Character target))
            {
                _currentTarget = target;
                _state = State.Aiming;
                _time = Prefab.GetComponent<CharacterDescriptor>().AimTime;
                Prefab.transform.LookAt(_currentTarget.Position);
            }
        }

        private void HandleAimingState(float deltaTime)
        {
            Prefab.PlayAiming();
            if (_currentTarget != null && _currentTarget.IsAlive)
            {
                if (_time > 0)
                {
                    _time -= deltaTime;
                }
                else
                {
                    _state = State.Shooting;
                    _time = 0;
                }
            }
            else
            {
                _state = State.Idle;
                _time = 0;
            }
        }

        private void HandleShootingState(float deltaTime)
        {
            Prefab.PlayAiming();
            if (_currentTarget != null && _currentTarget.IsAlive)
            {
                if (_weapon.HasAmmo)
                {
                    if (_weapon.IsReady)
                    {
                        float random = Random.Range(0.0f, 1.0f);
                        bool hit = random <= Prefab.GetComponent<CharacterDescriptor>().Accuracy &&
                                random <= _weapon.Prefab.GetConfig().GetAccuracy() &&
                                random >= _currentTarget.Prefab.GetComponent<CharacterDescriptor>().Dexterity;
                        _weapon.Fire(_currentTarget, hit);
                        Prefab.PlayShot();
                    }
                    else
                    {
                        _weapon.Update(deltaTime);
                    }
                }
                else
                {
                    _state = State.Reloading;
                    _time = _weapon.Prefab.GetConfig().GetReloadTime();
                }
            }
            else
            {
                _state = State.Idle;
            }
        }

        private void HandleReloadingState(float deltaTime)
        {
            Prefab.PlayReloading(_weapon.Prefab.GetConfig().GetReloadTime() / 3.3f);
            if (_time > 0)
            {
                _time -= deltaTime;
            }
            else
            {
                if (_currentTarget != null && _currentTarget.IsAlive)
                {
                    _state = State.Shooting;
                }
                else
                {
                    _state = State.Idle;
                }
                _weapon.Reload();
                _time = 0;
            }
        }
    }
}