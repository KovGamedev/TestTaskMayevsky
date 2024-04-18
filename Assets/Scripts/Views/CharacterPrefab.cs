using Scorewarrior.Test.Descriptors;
using UnityEngine;

namespace Scorewarrior.Test.Views
{
	public class CharacterPrefab : MonoBehaviour
	{
        [SerializeField] private WeaponPrefab _weapon;
		[SerializeField] private Animator _animator;
		[SerializeField] private Transform _rightPalm;
        [SerializeField] private float _reloadingDurationMultiplier;
        [SerializeField] private CharacterConfig _config;

        public WeaponPrefab GetWeapon() => _weapon;

        public CharacterConfig GetConfig() => _config;

        public void HandleState(CharacterState state)
        {
            switch (state)
            {
                case CharacterState.Idle:
                    PlayIdle();
                    break;
                case CharacterState.Aiming:
                    PlayAiming();
                    break;
                case CharacterState.Shooting:
                    PlayShot();
                    break;
                case CharacterState.Reloading:
                    PlayReloading();
                    break;
                case CharacterState.Death:
                    PlayDeath();
                    break;
            }
        }

		private void PlayIdle()
		{
            _animator.SetBool("aiming", false);
            _animator.SetBool("reloading", false);
        }

        private void PlayAiming()
		{
            _animator.SetBool("aiming", true);
            _animator.SetBool("reloading", false);
        }

        private void PlayShot()
        {
            _animator.SetTrigger("shoot");
        }

        private void PlayReloading()
		{
            _animator.SetBool("aiming", true);
			_animator.SetBool("reloading", true);
            _animator.SetFloat("reload_time", _reloadingDurationMultiplier * _weapon.GetConfig().GetReloadTime());
        }

        private void PlayDeath()
		{
            _animator.SetTrigger("die");
        }
    }
}
