using System;
using UnityEngine;

namespace ZeoFlow.Pickup
{
	[Serializable]
	public class AudioSoundsSub
	{
		public bool enabled = true;

		public AudioClip pickedUpAudio;

		public AudioClip objectHeldAudio;

		public AudioClip throwAudio;

		[NonSerialized] public bool letGoFired = false;
	}
}