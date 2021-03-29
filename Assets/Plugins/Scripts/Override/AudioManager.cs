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

		/// <summary>
		///     <para> PlayLoud </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="audio"></param>
		public void PlayLoud(AudioClip audio)
		{
			_audioSource.PlayOneShot(audio, AudioInstance.GetSoundVolume() * 1f);
		}

		/// <summary>
		///     <para> PlayNormal </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="audio"></param>
		public void PlayNormal(AudioClip audio)
		{
			_audioSource.PlayOneShot(audio, AudioInstance.GetSoundVolume() * .8f);
		}
		
		/// <summary>
		///     <para> PlayWeak </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="audio"></param>
		public void PlayWeak(AudioClip audio)
		{
			_audioSource.PlayOneShot(audio, AudioInstance.GetSoundVolume() * .6f);
		}

		/// <summary>
		///     <para> Awake </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Awake()
		{
			if (GetComponent<AudioSource>() == null) gameObject.AddComponent<AudioSource>();
			_audioSource = GetComponent<AudioSource>();
		}
	}
}