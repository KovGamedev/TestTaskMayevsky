using System.Collections.Generic;
using Scorewarrior.Test.Models;
using Scorewarrior.Test.Views;
using UnityEngine;

namespace Scorewarrior.Test
{
	public class Bootstrapper : MonoBehaviour
	{
		[SerializeField]
		private CharacterPrefab[] _characters;
		[SerializeField]
		private SpawnPoint[] _spawns;

		private Battlefield _battlefield;

		public void Start()
		{
			Dictionary<uint, List<Vector3>> spawnPositionsByTeam = new Dictionary<uint, List<Vector3>>();
			foreach (SpawnPoint spawn in _spawns)
			{
				uint team = spawn.Team;
				if (spawnPositionsByTeam.TryGetValue(team, out List<Vector3> spawnPoints))
				{
					spawnPoints.Add(spawn.transform.position);
				}
				else
				{
					spawnPositionsByTeam.Add(team, new List<Vector3>{ spawn.transform.position });
				}
				Destroy(spawn.gameObject);
			}
			_battlefield = new Battlefield(spawnPositionsByTeam);
			_battlefield.Start(_characters);
		}

		public void Update()
		{
			_battlefield.Update(Time.deltaTime);
		}


	}
}