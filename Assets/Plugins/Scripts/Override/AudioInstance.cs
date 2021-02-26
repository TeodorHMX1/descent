using System;
using UnityEngine;

namespace Override
{
	/// <summary>
	///     <para> AudioInstance </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class AudioInstance : MonoBehaviour
    {
        private static AudioInstance _mInstance;
        private static GameObject _audioInstances;
        private static AudioSource _audioSource;
        private static GameObject _this;

        private void Awake()
        {
            if (_mInstance == null)
            {
                _mInstance = this;
                _this = gameObject;
                if (_audioInstances != null) return;
                _audioInstances = new GameObject {name = "Audio Instances"};
                _audioInstances.transform.SetParent(gameObject.transform);
            }
            else
            {
                Destroy(this);
            }
        }

        public static float GetSoundVolume()
        {
            return PlayerPrefs.GetFloat(Constants.Options.Sound, 1.0f);
        }

        public static void PlaySound(AudioClip audio, SoundVolume volume, string name, float speed, bool oneAtOnce)
        {
            _audioSource = GetAudioSource(name);
            if (_audioSource.isPlaying && oneAtOnce) return;
            _audioSource.pitch = speed;
            switch (volume)
            {
                case SoundVolume.Loud:
                    _audioSource.PlayOneShot(audio, GetSoundVolume() * 1f);
                    break;
                case SoundVolume.Normal:
                    _audioSource.PlayOneShot(audio, GetSoundVolume() * .8f);
                    break;
                case SoundVolume.Weak:
                    _audioSource.PlayOneShot(audio, GetSoundVolume() * .6f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(volume), volume, null);
            }
        }

        private static AudioSource GetAudioSource(string name)
        {
            if (_audioInstances == null)
            {
                _audioInstances = new GameObject {name = "Audio Instances"};
                _audioInstances.transform.SetParent(_this.transform);
            }

            var findGameObj = _audioInstances.transform.Find(name);

            if (findGameObj != null) return findGameObj.GetComponent<AudioSource>();

            var newAudio = new GameObject {name = name};
            newAudio.transform.SetParent(_audioInstances.transform);
            var audioSource = newAudio.AddComponent<AudioSource>();
            return audioSource;
        }
    }
}