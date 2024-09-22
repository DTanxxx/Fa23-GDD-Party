using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Hearing
{
	// Implements things sounds can do (like Unity Physics)
	public static class Sounds
	{
		public static void MakeSound(Sound sound)
		{
			Collider[] col = Physics.OverlapSphere(sound.pos, sound.range, 
				(1 << LayerMask.NameToLayer("Cthulhu")) | (1 << LayerMask.NameToLayer("MothMan")));

			for (int i = 0; i < col.Length; i++)
			{
				if (col[i].transform.parent.TryGetComponent(out Listen listener))
				{
					listener.RespondToSound(sound);
				}
			}
		}
	}
}

