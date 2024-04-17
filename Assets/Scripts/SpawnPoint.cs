using UnityEngine;

namespace Scorewarrior.Test
{
	public class SpawnPoint : MonoBehaviour
	{
		[SerializeField] private uint _team;

		public uint GetTeam() => _team;
	}
}