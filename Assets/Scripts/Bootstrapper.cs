using System.Collections.Generic;
using Scorewarrior.Test.Models;
using Scorewarrior.Test.Views;
using UnityEngine;

namespace Scorewarrior.Test
{
	public class Bootstrapper : MonoBehaviour
	{
		[SerializeField] private CharacterPrefab[] _characters;
		[SerializeField] private SpawnPoint[] _spawns;

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
			_battlefield = new Battlefield(spawnPositionsByTeam);
			_battlefield.Start(_characters);
		}

        private void Update()
		{
			if (_battlefield == null)
				return;
			_battlefield.Update(Time.deltaTime);
		}
	}
}