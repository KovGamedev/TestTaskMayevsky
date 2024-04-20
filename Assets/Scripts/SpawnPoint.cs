using UnityEngine;

namespace Scorewarrior.Test
{
	public class SpawnPoint : MonoBehaviour
	{
		[SerializeField] private Faction _team;

		public Faction GetTeam() => _team;
	}
}