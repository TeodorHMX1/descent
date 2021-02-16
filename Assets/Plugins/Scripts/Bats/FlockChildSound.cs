using UnityEngine;

namespace Bats
{
	
	/// <summary>
	///     <para> FlockChildSound </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class FlockChildSound : MonoBehaviour
	{
		public AudioClip[] idleSounds;
		public float idleSoundRandomChance = .05f;

		public AudioClip[] flightSounds;
		public float flightSoundRandomChance = .05f;

		public AudioClip[] scareSounds;
		public float pitchMin = .85f;
		public float pitchMax = 1.0f;

		public float volumeMin = .6f;
		public float volumeMax = .8f;
		private AudioSource _audio;

		private FlockChild _flockChild;
		private bool _hasLanded;

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void Start()
		{
			_flockChild = GetComponent<FlockChild>();
			_audio = GetComponent<AudioSource>();
			InvokeRepeating(nameof(PlayRandomSound), Random.value + 1, 1.0f);
			if (scareSounds.Length > 0)
				InvokeRepeating(nameof(ScareSound), 1.0f, .01f);
		}

		/// <summary>
		///     <para> PlayRandomSound </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void PlayRandomSound()
		{
			if (!gameObject.activeInHierarchy) return;
			
			switch (_audio.isPlaying)
			{
				case false when flightSounds.Length > 0 && flightSoundRandomChance > Random.value && !_flockChild.landing:
					_audio.clip = flightSounds[Random.Range(0, flightSounds.Length)];
					_audio.pitch = Random.Range(pitchMin, pitchMax);
					_audio.volume = Random.Range(volumeMin, volumeMax);
					_audio.Play();
					break;
				case false when idleSounds.Length > 0 && idleSoundRandomChance > Random.value && _flockChild.landing:
					_audio.clip = idleSounds[Random.Range(0, idleSounds.Length)];
					_audio.pitch = Random.Range(pitchMin, pitchMax);
					_audio.volume = Random.Range(volumeMin, volumeMax);
					_audio.Play();
					_hasLanded = true;
					break;
			}
		}

		/// <summary>
		///     <para> ScareSound </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void ScareSound()
		{
			if (!gameObject.activeInHierarchy) return;
			if (!_hasLanded || _flockChild.landing || !(idleSoundRandomChance * 2 > Random.value)) return;
			
			_audio.clip = scareSounds[Random.Range(0, scareSounds.Length)];
			_audio.volume = Random.Range(volumeMin, volumeMax);
			_audio.PlayDelayed(Random.value * .2f);
			_hasLanded = false;
		}
	}
}