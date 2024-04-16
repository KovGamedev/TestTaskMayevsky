using Scorewarrior.Test.Descriptors;
using Scorewarrior.Test.Models;
using UnityEngine;

namespace Scorewarrior.Test.Views
{
	public class BulletPrefab : MonoBehaviour
	{
		private Character _target;
		private WeaponPrefab _weapon;
		private bool _hit;

		private Vector3 _position;
		private Vector3 _direction;
		private float _totalDistance;
		private float _currentDistance;

		public void Init(WeaponPrefab weapon, Character target, bool hit)
		{
			_weapon = weapon;
			_target = target;
			_hit = hit;
			_position = transform.position;
			Vector3 targetPosition = target.Position + Vector3.up * 2.0f;
			_direction = Vector3.Normalize(targetPosition - transform.position);
			_totalDistance = Vector3.Distance(targetPosition, transform.position);
			_currentDistance = 0;
		}

		public void Update()
		{
			_currentDistance += Time.deltaTime * 30;
			if (_currentDistance < _totalDistance)
			{
				transform.position = _position + _currentDistance * _direction;
			}
			else
			{
				if (_hit)
				{
					WeaponDescriptor weaponDescriptor = _weapon.GetComponent<WeaponDescriptor>();
					CharacterDescriptor targetDescriptor = _target.Prefab.GetComponent<CharacterDescriptor>();
					float damage = weaponDescriptor.Damage;
					if (_target.Armor > 0)
					{
						_target.Armor -= damage;
					}
					else if (_target.Health > 0)
					{
						_target.Health -= damage;
					}
					if (_target.Armor <= 0 && _target.Health <= 0)
					{
						_target.Prefab.Animator.SetTrigger("die");
					}
				}
				Destroy(gameObject);
			}
		}


	}
}