using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Menu;
using UnityEngine;
using Random = UnityEngine.Random;

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
        private static GameObject _this;
        private const string CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private bool _isResumed;
        private float _soundVolume;

        /// <summary>
        ///     <para> Awake </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void Awake()
        {
            if (_mInstance == null)
            {
                _mInstance = this;
                _this = gameObject;
                _soundVolume = GetSoundVolume();
                if (_audioInstances != null) return;
                _audioInstances = new GameObject {name = "Audio Instances"};
                _audioInstances.transform.SetParent(gameObject.transform);
            }
            else
            {
                Destroy(this);
            }
        }

        /// <summary>
        ///     <para> Update </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void Update()
        {
            if (Math.Abs(_soundVolume - GetSoundVolume()) > 0)
            {
                _soundVolume = GetSoundVolume();
                var audioSources = GetAudioSources();
                foreach (var audioSource in audioSources)
                {
                    audioSource.volume = _soundVolume;
                }
            }

            if (Pause.IsPaused)
            {
                var audioSources = GetComponentsInChildren<AudioSource>();
                foreach (var audioSource in audioSources) audioSource.Pause();
                _isResumed = false;
            }
            else if (!_isResumed)
            {
                var audioSources = GetComponentsInChildren<AudioSource>();
                foreach (var audioSource in audioSources)
                    if (!audioSource.isPlaying)
                        audioSource.Play();
                _isResumed = true;
            }
        }

        /// <summary>
        ///     <para> ID </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <returns>unique ID</returns>
        public static string ID()
        {
            var idGenerated = "";
            for (var i = 0; i < 20; i++)
            {
                idGenerated += CHARACTERS[Random.Range(0, CHARACTERS.Length)];
                if (i % 4 == 0 && i != 0 && i != 20) idGenerated += "_";
            }

            return idGenerated;
        }

        /// <summary>
        ///     <para> ID </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <returns>sound volume</returns>
        public static float GetSoundVolume()
        {
            return PlayerPrefs.GetFloat(Constants.Options.Sound, 1.0f);
        }

        /// <summary>
        ///     <para> PlaySound </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <param name="audio"></param>
        /// <param name="volume"></param>
        /// <param name="name"></param>
        /// <param name="speed"></param>
        /// <param name="oneAtOnce"></param>
        /// <param name="destroyOnFinish"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static async void PlaySound(AudioClip audio, SoundVolume volume, string name, float speed,
            bool oneAtOnce, bool destroyOnFinish)
        {
            var audioSource = GetAudioSource(name);
            if (audioSource.isPlaying && oneAtOnce) return;
            audioSource.pitch = speed;
            switch (volume)
            {
                case SoundVolume.Loud:
                    audioSource.PlayOneShot(audio, GetSoundVolume() * 1f);
                    break;
                case SoundVolume.Normal:
                    audioSource.PlayOneShot(audio, GetSoundVolume() * .8f);
                    break;
                case SoundVolume.Weak:
                    audioSource.PlayOneShot(audio, GetSoundVolume() * .6f);
                    break;
                case SoundVolume.OnBackground:
                    audioSource.PlayOneShot(audio, GetSoundVolume() * .1f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(volume), volume, null);
            }

            if (destroyOnFinish)
            {
                await Task.Delay(TimeSpan.FromMilliseconds((int) audio.length * 1000));
                DestroyAudioSource(name);
            }
        }

        /// <summary>
        ///     <para> StopSound </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="destroy"></param>
        public static void StopSound(string name, bool destroy = false)
        {
            var audioSource = GetAudioSource(name);
            audioSource.Stop();
            if (destroy) DestroySource(name);
        }

        /// <summary>
        ///     <para> GetAudioSource </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <param name="name"></param>
        /// <returns>audio source</returns>
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

        /// <summary>
        ///     <para> DestroyAudioSource </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <param name="name"></param>
        private static void DestroyAudioSource(string name)
        {
            if (_audioInstances == null)
            {
                return;
            }

            var findGameObj = _audioInstances.transform.Find(name);
            if (findGameObj != null) Destroy(findGameObj.gameObject);
        }

        /// <summary>
        ///     <para> DestroySource </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <param name="name"></param>
        private static void DestroySource(string name)
        {
            if (_audioInstances == null)
            {
                _audioInstances = new GameObject {name = "Audio Instances"};
                _audioInstances.transform.SetParent(_this.transform);
            }

            var findGameObj = _audioInstances.transform.Find(name);

            if (findGameObj != null) Destroy(findGameObj.gameObject);
        }

        /// <summary>
        ///     <para> DestroySource </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <returns>audio sources list</returns>
        private static IEnumerable<AudioSource> GetAudioSources()
        {
            return _audioInstances == null
                ? new AudioSource[] { }
                : _audioInstances.transform.GetComponentsInChildren<AudioSource>();
        }
    }
    
    
}