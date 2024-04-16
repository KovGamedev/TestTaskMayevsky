using Scorewarrior.Test.Descriptors;
using Scorewarrior.Test.Models;
using UnityEngine;

namespace Scorewarrior.Test.Views
{
	public class CharacterPrefab : MonoBehaviour
	{
		public WeaponPrefab Weapon;
		public Animator Animator;

		[SerializeField]
		private Transform _rightPalm;

		public void PlayIdle()
		{
            Animator.SetBool("aiming", false);
            Animator.SetBool("reloading", false);
        }

		public void PlayAiming()
		{
            Animator.SetBool("aiming", true);
            Animator.SetBool("reloading", false);
        }

        public void PlayShot()
        {
            Animator.SetTrigger("shoot");
        }

		public void PlayReloading(float reloadDuration)
		{
            Animator.SetBool("aiming", true);
			Animator.SetBool("reloading", true);
            Animator.SetFloat("reload_time", reloadDuration);
        }

        private void Update()
		{
			if (_rightPalm != null && Weapon != null)
			{
				Weapon.transform.position = _rightPalm.position;
				Weapon.transform.forward = _rightPalm.up;
			}
		}
	}
}
