﻿using Scorewarrior.Test.Descriptors;
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
        private BulletPrefabDelegate _lifeEndHandler;

        public void Init(WeaponPrefab weapon, Character target, bool hit, Transform barrelTrandform)
        {
            transform.position = barrelTrandform.position;
            transform.rotation = barrelTrandform.rotation;
            _weapon = weapon;
            _target = target;
            _hit = hit;
            _initialPosition = transform.position;
            Vector3 targetPosition = target.Position + _targetPositionOffset;
            _direction = Vector3.Normalize(targetPosition - transform.position);
            _totalDistance = Vector3.Distance(targetPosition, transform.position);
            _currentDistance = 0;
        }

        public void SetLifeEndHandler(BulletPrefabDelegate hitHandler)
        {
            _lifeEndHandler = hitHandler;
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
                    _target.GetDamage(_weapon.GetConfig().GetDamage());
                }
                _lifeEndHandler(this);
            }
        }
    }
}