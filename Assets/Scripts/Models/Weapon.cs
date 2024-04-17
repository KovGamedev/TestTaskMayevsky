using Scorewarrior.Test.Descriptors;
using Scorewarrior.Test.Views;

namespace Scorewarrior.Test.Models
{
	public class Weapon
	{
		public WeaponPrefab Prefab { get; private set; }
        public bool IsReady { get; private set; }

        private uint _ammo;
        private uint _ammoMax;
		private float _reloadTime;
		private float _fireRate;

        public Weapon(WeaponPrefab prefab)
		{
            Prefab = prefab;
			var config = Prefab.GetConfig();
            _ammo = config.GetClipSize();
			_fireRate = config.GetFireRate();
            _ammoMax = config.GetClipSize();
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
			_reloadTime = 1.0f / _fireRate;
            IsReady = false;
		}

		public void Update(float deltaTime)
		{
			if (IsReady)
				return;

			if (_reloadTime > 0)
			{
				_reloadTime -= deltaTime;
			}
			else
			{
                IsReady = true;
			}
		}
	}
}