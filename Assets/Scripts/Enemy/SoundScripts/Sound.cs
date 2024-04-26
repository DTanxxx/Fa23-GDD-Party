using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Hearing
{
	public class Sound
	{
		public enum SoundType { Footstep, Rock, Bats, Gravel, Water }

		public Sound(Vector3 _pos, float _range, float _amp)
		{
			pos = _pos;
			range = _range;
			amplitude = _amp;
			// escapeDistance: how scary a sound is if it is danger
		}

		public SoundType soundType;

		public readonly Vector3 pos;

		public readonly float range;

		public readonly float amplitude;
	}
}
