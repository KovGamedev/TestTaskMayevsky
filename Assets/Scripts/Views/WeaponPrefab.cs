using Scorewarrior.Test.Models;
using UnityEngine;

namespace Scorewarrior.Test.Views
{
	public class WeaponPrefab : MonoBehaviour
	{
		public Transform BarrelTransform;

		[SerializeField]
		private GameObject _bulletPrefab;

		public void Fire(Character character, bool hit)
		{
			GameObject bulletObject = Instantiate(_bulletPrefab, BarrelTransform);
			BulletPrefab bullet = bulletObject.GetComponent<BulletPrefab>();
			bullet.transform.position = BarrelTransform.position;
			bullet.Init(this, character, hit);
		}
	}
}