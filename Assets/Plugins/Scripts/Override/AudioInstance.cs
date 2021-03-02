using System;
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
        private static AudioSource _audioSource;
        private static GameObject _this;
        private readonly string _characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private bool _isResumed;
        private float _soundVolume;

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

            if (Time.timeScale == 0f)
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

        public static string ID()
        {
            var idGenerated = "";
            for (var i = 0; i < 20; i++)
            {
                idGenerated += _mInstance._characters[Random.Range(0, _mInstance._characters.Length)];
                if (i % 4 == 0 && i != 0 && i != 20) idGenerated += "_";
            }

            return idGenerated;
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
                case SoundVolume.OnBackground:
                    _audioSource.PlayOneShot(audio, GetSoundVolume() * .1f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(volume), volume, null);
            }
        }

        public static void StopSound(string name, bool destroy = false)
        {
            _audioSource = GetAudioSource(name);
            _audioSource.Stop();
            if (destroy) DestroySource(name);
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

        private static AudioSource[] GetAudioSources()
        {
            return _audioInstances == null
                ? new AudioSource[] { }
                : _audioInstances.transform.GetComponentsInChildren<AudioSource>();
        }
    }
}