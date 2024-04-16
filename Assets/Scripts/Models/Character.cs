using Scorewarrior.Test.Descriptors;
using Scorewarrior.Test.Views;
using UnityEngine;

namespace Scorewarrior.Test.Models
{
	public class Character
	{
		private enum State
		{
			Idle,
			Aiming,
			Shooting,
			Reloading
		}

		private readonly CharacterPrefab _prefab;
		private readonly Weapon _weapon;
		private readonly Battlefield _battlefield;

		private float _health;
		private float _armor;

		private State _state;
		private Character _currentTarget;
		private float _time;

		public Character(CharacterPrefab prefab, Weapon weapon, Battlefield battlefield)
		{
			_prefab = prefab;
			_weapon = weapon;
			_battlefield = battlefield;
			CharacterDescriptor descriptor = _prefab.GetComponent<CharacterDescriptor>();
			_health = descriptor.MaxHealth;
			_armor = descriptor.MaxArmor;
		}

		public bool IsAlive => _health > 0 || _armor > 0;

		public float Health
		{
			get => _health;
			set => _health = value;
		}

		public float Armor
		{
			get => _armor;
			set => _armor = value;
		}

		public CharacterPrefab Prefab => _prefab;
		public Vector3 Position => _prefab.transform.position;

		public void Update(float deltaTime)
		{
			if (IsAlive)
			{
				switch (_state)
				{
					case State.Idle:
						_prefab.Animator.SetBool("aiming", false);
						_prefab.Animator.SetBool("reloading", false);
						if (_battlefield.TryGetNearestAliveEnemy(this, out Character target))
						{
							_currentTarget = target;
							_state = State.Aiming;
							_time = _prefab.GetComponent<CharacterDescriptor>().AimTime;
							_prefab.transform.LookAt(_currentTarget.Position);
						}
						break;
					case State.Aiming:
						_prefab.Animator.SetBool("aiming", true);
						_prefab.Animator.SetBool("reloading", false);
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
						break;
					case State.Shooting:
						_prefab.Animator.SetBool("aiming", true);
						_prefab.Animator.SetBool("reloading", false);
						if (_currentTarget != null && _currentTarget.IsAlive)
						{
							if (_weapon.HasAmmo)
							{
								if (_weapon.IsReady)
								{
									float random = Random.Range(0.0f, 1.0f);
									bool hit = random <= _prefab.GetComponent<CharacterDescriptor>().Accuracy &&
											random <= _weapon.Prefab.GetComponent<WeaponDescriptor>().Accuracy &&
											random >= _currentTarget.Prefab.GetComponent<CharacterDescriptor>().Dexterity;
									_weapon.Fire(_currentTarget, hit);
									_prefab.Animator.SetTrigger("shoot");
								}
								else
								{
									_weapon.Update(deltaTime);
								}
							}
							else
							{
								_state = State.Reloading;
								_time = _weapon.Prefab.GetComponent<WeaponDescriptor>().ReloadTime;
							}
						}
						else
						{
							_state = State.Idle;
						}
						break;
					case State.Reloading:
						_prefab.Animator.SetBool("aiming", true);
						_prefab.Animator.SetBool("reloading", true);
						_prefab.Animator.SetFloat("reload_time", _weapon.Prefab.GetComponent<WeaponDescriptor>().ReloadTime / 3.3f);
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
						break;
				}
			}
		}
	}
}