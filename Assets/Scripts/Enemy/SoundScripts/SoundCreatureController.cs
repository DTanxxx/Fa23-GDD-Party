using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Lurkers.Hearing;

namespace Lurkers.Control.Hearing.Character
{
	public class SoundCreatureController : MonoBehaviour, Listen
	{
		private NavMeshAgent agent;
		[SerializeField] private float chaseSpeed;
		[SerializeField] private float soundThreshold = 5f;
		[SerializeField] private float soundExpireDuration = 3.0f;
		// public float listeningRange = 50f;

		// These lists of soundtypes should have no overlap
		[SerializeField] private List<Sound.SoundType> InterestingSounds;
		[SerializeField] private List<Sound.SoundType> DangerousSounds;

		private float curAmplitude;
		private float soundExpiringTimer;

		private void Start()
		{
			agent = GetComponentInParent<NavMeshAgent>();

			/*InterestingSounds = new List<Sound.SoundType>();
			InterestingSounds.Add(Sound.SoundType.Footstep);
			InterestingSounds.Add(Sound.SoundType.Rock);*/

			DangerousSounds = new List<Sound.SoundType>();
			DangerousSounds.Add(Sound.SoundType.Bats);
		}

        private void Update()
        {
            if (soundExpiringTimer > 0f)
            {
				soundExpiringTimer -= Time.deltaTime;
            }
			else
            {
				// expire curSound
				curAmplitude = 0f;
            }
        }

        public void RespondToSound(Sound sound)
		{
			// check if sound's amplitude is detectable
			float distToSound = Vector3.Distance(transform.position, sound.pos);
			float amplitude = sound.amplitude * (1 - distToSound / sound.range);

			if (amplitude >= soundThreshold)
            {
				if (amplitude > curAmplitude)
                {
					// detected!
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
					curAmplitude = amplitude;
					soundExpiringTimer = soundExpireDuration;
				}
			}
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
