using System;
using System.Collections.Generic;
using UnityEngine;

namespace Paranoia
{
	[Serializable]
	public class EffectSub
	{
		public bool enabled;
		public bool musicEnabled;
		[Range(0f, 1f)] public float audioClipVolume = 0.1f;
		public List<AudioClip> audioClips = new List<AudioClip>();
		public bool cameraEffectEnabled;
		public GameObject camera;
	}
}