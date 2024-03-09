using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Lurkers.Hearing;

namespace Lurkers.Character
{
	public class SoundCreatureController : MonoBehaviour, Listen
	{
		private NavMeshAgent agent;
		[SerializeField] private float chaseSpeed;
		// public float listeningRange = 50f;

		// These lists of soundtypes should have no overlap
		[SerializeField] private List<Sound.SoundType> InterestingSounds;
		[SerializeField] private List<Sound.SoundType> DangerousSounds;

		private void Start()
		{
			agent = GetComponent<NavMeshAgent>();

			InterestingSounds = new List<Sound.SoundType>();
			InterestingSounds.Add(Sound.SoundType.Footstep);
			InterestingSounds.Add(Sound.SoundType.Rock);

			DangerousSounds = new List<Sound.SoundType>();
			DangerousSounds.Add(Sound.SoundType.Bats);
		}

		public void RespondToSound(Sound sound)
		{
			if (InterestingSounds.Contains(sound.soundType))
			{
				MoveTo(sound.pos);
			}
			else if (DangerousSounds.Contains(sound.soundType))
			{
				Vector3 dir = sound.pos - transform.position;

				MoveTo(sound.pos - (dir * 10f));
			}
			Debug.Log(name + " responding to sound at " + sound.pos);
		}

		private void MoveTo(Vector3 pos)
		{
			// might want to check if the player is in LOS or if the distance the agent needs to // travel is within a certain range
			agent.SetDestination(pos);
			agent.isStopped = false;
			agent.speed = chaseSpeed;
		}

	}
}
