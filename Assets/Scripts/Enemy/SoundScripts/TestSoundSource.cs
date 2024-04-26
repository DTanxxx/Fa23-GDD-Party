using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Hearing
{
	public class TestSoundSource : MonoBehaviour
	{
		[SerializeField] private AudioSource source = null;
		[SerializeField] private float soundAmplitude = 100f;

		private float soundRange = 1000f;

		private void OnMouseDown()
		{
			if (source.isPlaying) { return; }

			source.Play();
			var sound = new Sound(transform.position, soundRange, soundAmplitude);

			// imitates player footsteps at object on mouse click
			sound.soundType = Sound.SoundType.Footstep;
			Sounds.MakeSound(sound);
		}
	}
}

