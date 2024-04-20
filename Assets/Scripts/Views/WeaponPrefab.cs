using Scorewarrior.Test.Models;
using UnityEngine;

namespace Scorewarrior.Test.Views
{
    public class WeaponPrefab : MonoBehaviour
    {
        [SerializeField] private Transform _barrelTransform;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private WeaponConfig _config;

        private Pool<BulletPrefab> _bulletsPool;

        public void Fire(Character character, bool hit)
        {
            _bulletsPool
                .Acquire()
                .Init(this, character, hit, _barrelTransform);
        }

        public WeaponConfig GetConfig()
        {
            return _config;
        }

        private void Start()
        {
            _bulletsPool = new Pool<BulletPrefab>(
                () => {
                    var bullet = Instantiate(_bulletPrefab).GetComponent<BulletPrefab>();
                    bullet.SetLifeEndHandler((BulletPrefab bullet) => _bulletsPool.Release(bullet));
                    return bullet;
                },
                (BulletPrefab bullet) => bullet.gameObject.SetActive(true),
                (BulletPrefab bullet) => bullet.gameObject.SetActive(false)
            );
        }
    }
}