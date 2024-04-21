using UnityEngine;
using Scorewarrior.Test.Views;
using Scorewarrior.Test.Configs;

namespace Scorewarrior.Test.Models
{
	public class Weapon
	{
		public WeaponPrefab Prefab { get; private set; }
        public bool IsReady { get; private set; }
        public float Accuracy { get; private set; }
        public float ReloadTime { get; private set; }

        private uint _ammo;
        private uint _ammoMax;
		private float _fireRateTime;
		private float _fireRate;
        private WeaponPrefab weaponPrefab;
        private WeaponModifierConfig[] weaponModifiers;

        public Weapon(WeaponPrefab prefab, WeaponModifierConfig[] weaponModifiers)
		{
            Prefab = prefab;
            HandleConfigs(Prefab.GetConfig(), weaponModifiers);
        }

        public bool HasAmmo => _ammo > 0;

		public void Reload()
		{
			_ammo = _ammoMax;
		}

		public void Fire(Character character, bool hit)
		{
			if (!HasAmmo)
				return;

			_ammo--;
            Prefab.Fire(character, hit);
			_fireRateTime = 1.0f / _fireRate;
            IsReady = false;
		}

		public void Update(float deltaTime)
		{
			if (IsReady)
				return;

			if (_fireRateTime > 0)
			{
				_fireRateTime -= deltaTime;
			}
			else
			{
                IsReady = true;
			}
		}

        private void HandleConfigs(WeaponConfig config, WeaponModifierConfig[] weaponModifiers)
        {
            Accuracy = config.GetAccuracy();
            _ammo = config.GetClipSize();
            _fireRate = config.GetFireRate();
            ReloadTime = config.GetReloadTime();

            for (var i = 0; i < config.GetModifiersQuantity(); i++)
            {
                var modifier = weaponModifiers[Random.Range(0, weaponModifiers.Length)];
                Accuracy = Mathf.Clamp(Accuracy + modifier.GetAccuracy(), 0f, Accuracy + modifier.GetAccuracy());
                _ammo += modifier.GetClipSize();
                _fireRate = Mathf.Clamp(_fireRate + modifier.GetFireRate(), 0f, 1f);
                ReloadTime = Mathf.Clamp(ReloadTime + modifier.GetReloadTime(), 0f, ReloadTime + modifier.GetReloadTime());
            }
            _ammoMax = _ammo;
        }
    }
}