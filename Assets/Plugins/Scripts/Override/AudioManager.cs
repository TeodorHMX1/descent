using UnityEngine;

namespace Override
{
	/// <summary>
	///     <para> AudioManager </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class AudioManager : MonoBehaviour
	{

		private static AudioSource _audioSource;

		public void PlayLoud(AudioClip audio)
		{
			_audioSource.PlayOneShot(audio, AudioInstance.GetSoundVolume() * 1f);
		}

		public void PlayNormal(AudioClip audio)
		{
			_audioSource.PlayOneShot(audio, AudioInstance.GetSoundVolume() * .8f);
		}

		public void PlayWeak(AudioClip audio)
		{
			_audioSource.PlayOneShot(audio, AudioInstance.GetSoundVolume() * .6f);
		}

		private void Awake()
		{
			if (GetComponent<AudioSource>() == null) gameObject.AddComponent<AudioSource>();
			_audioSource = GetComponent<AudioSource>();
		}
	}
}