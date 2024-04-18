using System.Collections.Generic;
using Scorewarrior.Test.Views;
using UnityEngine;

namespace Scorewarrior.Test.Models
{
	public class Battlefield
	{
		private readonly Dictionary<uint, List<Vector3>> _spawnPositionsByTeam;
		private readonly Dictionary<uint, List<Character>> _charactersByTeam;

		private bool _paused;

		public Battlefield(Dictionary<uint, List<Vector3>> spawnPositionsByTeam)
		{
			_spawnPositionsByTeam = spawnPositionsByTeam;
			_charactersByTeam = new Dictionary<uint, List<Character>>();
		}

		public void Start(CharacterPrefab[] prefabs)
		{
			_paused = false;
			_charactersByTeam.Clear();

			List<CharacterPrefab> availablePrefabs = new List<CharacterPrefab>(prefabs);
			foreach (var positionsPair in _spawnPositionsByTeam)
			{
				List<Vector3> positions = positionsPair.Value;
				List<Character> characters = new List<Character>();
				_charactersByTeam.Add(positionsPair.Key, characters);
				int i = 0;
				while (i < positions.Count && availablePrefabs.Count > 0)
				{
					int index = Random.Range(0, availablePrefabs.Count);
					characters.Add(CreateCharacterAt(availablePrefabs[index], this, positions[i]));
					availablePrefabs.RemoveAt(index);
					i++;
				}
			}
		}

		public bool TryGetNearestAliveEnemy(Character character, out Character target)
		{
			if (TryGetTeam(character, out uint team))
			{
				Character nearestEnemy = null;
				float nearestDistance = float.MaxValue;
				List<Character> enemies = team == 1 ? _charactersByTeam[2] : _charactersByTeam[1];
				foreach (Character enemy in enemies)
				{
					if (enemy.IsAlive)
					{
						float distance = Vector3.Distance(character.Position, enemy.Position);
						if (distance < nearestDistance)
						{
							nearestDistance = distance;
							nearestEnemy = enemy;
						}
					}
				}
				target = nearestEnemy;
				return target != null;
			}
			target = default;
			return false;
		}

		public bool TryGetTeam(Character target, out uint team)
		{
			foreach (var charactersPair in _charactersByTeam)
			{
				List<Character> characters = charactersPair.Value;
				foreach (Character character in characters)
				{
					if (character == target)
					{
						team = charactersPair.Key;
						return true;
					}
				}
			}
			team = default;
			return false;
		}

		public void Update(float deltaTime)
		{
			if (!_paused)
			{
				foreach (var charactersPair in _charactersByTeam)
				{
					List<Character> characters = charactersPair.Value;
					foreach (Character character in characters)
					{
						character.Update(deltaTime);
					}
				}
			}
		}

		private static Character CreateCharacterAt(CharacterPrefab prefab, Battlefield battlefield, Vector3 position)
		{
			CharacterPrefab character = Object.Instantiate(prefab);
			character.transform.position = position;
			return new Character(character, new Weapon(character.GetWeapon()), battlefield);
		}
	}
}