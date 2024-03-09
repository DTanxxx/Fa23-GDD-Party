using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Hearing
{
	public class Sound
	{
		public enum SoundType { Footstep, Rock, Bats }

		public Sound(Vector3 _pos, float _range)
		{
			pos = _pos;
			range = _range;
			// escapeDistance: how scary a sound is if it is danger
		}

		public SoundType soundType;

		public readonly Vector3 pos;

		public readonly float range;
	}
}
