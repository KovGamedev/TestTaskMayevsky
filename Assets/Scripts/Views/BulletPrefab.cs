using Scorewarrior.Test.Descriptors;
using Scorewarrior.Test.Models;
using UnityEngine;

namespace Scorewarrior.Test.Views
{
    public class BulletPrefab : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Vector3 _targetPositionOffset;

        private Character _target;
        private WeaponPrefab _weapon;
        private bool _hit;
        private Vector3 _initialPosition;
        private Vector3 _direction;
        private float _totalDistance;
        private float _currentDistance;

        public void Init(WeaponPrefab weapon, Character target, bool hit)
        {
            _weapon = weapon;
            _target = target;
            _hit = hit;
            _initialPosition = transform.position;
            Vector3 targetPosition = target.Position + _targetPositionOffset;
            _direction = Vector3.Normalize(targetPosition - transform.position);
            _totalDistance = Vector3.Distance(targetPosition, transform.position);
            _currentDistance = 0;
        }

        private void Update()
        {
            _currentDistance += Time.deltaTime * _speed;
            if (_currentDistance < _totalDistance)
            {
                transform.position = _initialPosition + _currentDistance * _direction;
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