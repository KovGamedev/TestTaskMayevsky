using System;
using System.Collections.Generic;
using Scorewarrior.Test.Configs;
using Scorewarrior.Test.Models;
using Scorewarrior.Test.Views;
using UnityEngine;
using UnityEngine.Events;

namespace Scorewarrior.Test
{
	public class Bootstrapper : MonoBehaviour
	{
		[SerializeField] private CharacterPrefab[] _characters;
		[SerializeField] private SpawnPoint[] _spawns;
        [SerializeField] private CharacterModifierConfig[] _charactersModifiers;
        [SerializeField] private WeaponModifierConfig[] _weaponModifiers;
        [SerializeField] private UnityEvent _teamLost;

        private Battlefield _battlefield;

		public void StartBattle()
		{
			var spawnPositionsByTeam = new Dictionary<Faction, List<Vector3>>();
			foreach (SpawnPoint spawn in _spawns)
			{
				var team = spawn.GetTeam();
				if (spawnPositionsByTeam.TryGetValue(team, out List<Vector3> spawnPoints))
				{
					spawnPoints.Add(spawn.transform.position);
				}
				else
				{
					spawnPositionsByTeam.Add(team, new List<Vector3>{ spawn.transform.position });
				}
			}
            var characterModifiersCopy = new CharacterModifierConfig[_charactersModifiers.Length];
            Array.Copy(_charactersModifiers, characterModifiersCopy, _charactersModifiers.Length);
            var weaponModifiersCopy = new WeaponModifierConfig[_weaponModifiers.Length];
            Array.Copy(_weaponModifiers, weaponModifiersCopy, _weaponModifiers.Length);
			_battlefield = new Battlefield(spawnPositionsByTeam, characterModifiersCopy, weaponModifiersCopy);
			_battlefield.Start(_characters, _teamLost);
		}

		public void ClearLevel()
		{
			if(_battlefield != null)
				_battlefield.Destroy();
            _battlefield = null;
        }

        private void Update()
		{
			if (_battlefield == null)
				return;
			_battlefield.Update(Time.deltaTime);
		}
	}
}