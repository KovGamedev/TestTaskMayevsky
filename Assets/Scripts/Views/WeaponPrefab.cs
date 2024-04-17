using Scorewarrior.Test.Models;
using UnityEngine;

namespace Scorewarrior.Test.Views
{
    public class WeaponPrefab : MonoBehaviour
    {
        [SerializeField]
        private Transform _barrelTransform;
        [SerializeField]
        private GameObject _bulletPrefab;

        public void Fire(Character character, bool hit)
        {
            Instantiate(_bulletPrefab, _barrelTransform.position, _barrelTransform.rotation, _barrelTransform)
                .GetComponent<BulletPrefab>()
                .Init(this, character, hit);
        }
    }
}