using System.Collections.Generic;
using UnityEngine;

namespace Override
{
	/// <summary>
	///     <para> AudioManager </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class AudioManager : MonoBehaviour
	{

		private static AudioManager _mInstance;
		private static GameObject _gameObject;
		private static List<AudioSource> _audioSources = new List<AudioSource>();

		public static void PlayOneShot(AudioClip audio, SoundVolume volume = SoundVolume.Normal)
		{
			var audioSource = _gameObject.AddComponent<AudioSource>();
			audioSource.PlayOneShot(audio, 1f);
			_audioSources.Add(audioSource);
		}

		public static float GetSoundVolume()
		{
			return PlayerPrefs.GetFloat(Constants.Options.Sound, 1.0f);
		}
		
		private void Awake()
		{
			if (_mInstance == null)
			{
				_mInstance = this;
				_gameObject = gameObject;
			}
			else
			{
				Destroy(this);
			}
		}
	}
}