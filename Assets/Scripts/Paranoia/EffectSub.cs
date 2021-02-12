using System;
using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Paranoia
{
	/// <summary>
	///     <para> EffectSub </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	[Serializable]
	public class EffectSub
	{
		public bool enabled;
		public bool musicEnabled;
		public HelmetManage helmetObj;
		[Range(0f, 1f)] public float audioClipVolume = 0.1f;
		public List<AudioClip> audioClips = new List<AudioClip>();
		public bool cameraEffectEnabled;
		public GameObject camera;
	}
}