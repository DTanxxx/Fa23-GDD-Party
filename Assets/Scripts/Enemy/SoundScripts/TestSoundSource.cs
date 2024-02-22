using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSoundSource : MonoBehaviour
{
	[SerializeField] private AudioSource source = null;

	private float soundRange = 1000f;

	private void OnMouseDown()
	{
		if (source.isPlaying) { return; }

		source.Play();
		var sound = new Sound(transform.position, soundRange);

		// imitates player footsteps at object on mouse click
		sound.soundType = Sound.SoundType.Footstep;
		Sounds.MakeSound(sound);
	}
}

