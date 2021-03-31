using System;
using UnityEngine;

namespace Override
{
    /// <summary>
    ///     <para> InsideSafeArea </para>
    ///     <author> @TeodorHMX1 </author>
    /// </summary>
    public class AudioBuilder
    {
        private AudioClip _clip;
        private SoundVolume _volume = SoundVolume.Normal;
        private string _name;
        private AudioSource _audioSource;
        private float _speed = 1f;
        private bool destroyOnFinish;

        /// <summary>
        ///     <para> Clip </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        public AudioClip Clip
        {
            get => _clip;
            set => _clip = value;
        }

        /// <summary>
        ///     <para> Volume </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        public SoundVolume Volume
        {
            get => _volume;
            set => _volume = value;
        }

        /// <summary>
        ///     <para> Name </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        /// <summary>
        ///     <para> AudioSource </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        public AudioSource AudioSource
        {
            get => _audioSource;
            set => _audioSource = value;
        }

        /// <summary>
        ///     <para> Speed </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        /// <summary>
        ///     <para> WithClip </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <param name="clip"></param>
        /// <returns></returns>
        public AudioBuilder WithClip(AudioClip clip)
        {
            Clip = clip;
            return this;
        }

        /// <summary>
        ///     <para> WithVolume </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public AudioBuilder WithVolume(SoundVolume volume)
        {
            Volume = volume;
            return this;
        }

        /// <summary>
        ///     <para> WithName </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AudioBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        /// <summary>
        ///     <para> WithAudioSource </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <param name="audioSource"></param>
        /// <returns></returns>
        public AudioBuilder WithAudioSource(AudioSource audioSource)
        {
            AudioSource = audioSource;
            return this;
        }

        /// <summary>
        ///     <para> WithSpeed </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public AudioBuilder WithSpeed(float speed)
        {
            Speed = speed;
            return this;
        }

        /// <summary>
        ///     <para> Stop </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <param name="destroy"></param>
        public void Stop(bool destroy = false)
        {
            if (AudioSource != null)
            {
                AudioSource.Stop();
            }
            else
            {
                if (Name == null) return;
                AudioInstance.StopSound(Name, destroy);
            }
        }

        /// <summary>
        ///     <para> DestroyOnEnd </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <param name="destroyOnFinish"></param>
        /// <returns></returns>
        public AudioBuilder DestroyOnEnd(bool destroyOnFinish = true)
        {
            this.destroyOnFinish = destroyOnFinish;
            return this;
        }

        /// <summary>
        ///     <para> Play </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        /// <param name="oneAtOnce"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Play(bool oneAtOnce = false)
        {
            if (Clip == null)
            {
                Debug.LogWarning("Please set the clip for " + Name + " using the WithClip(AudioClip) on the builder");
                return;
            }

            if (AudioSource != null)
            {
                if (oneAtOnce)
                {
                    if (AudioSource.isPlaying)
                    {
                        return;
                    }
                }

                switch (Volume)
                {
                    case SoundVolume.Loud:
                        AudioSource.PlayOneShot(Clip, AudioInstance.GetSoundVolume() * 1f);
                        break;
                    case SoundVolume.Normal:
                        AudioSource.PlayOneShot(Clip, AudioInstance.GetSoundVolume() * .8f);
                        break;
                    case SoundVolume.Weak:
                        AudioSource.PlayOneShot(Clip, AudioInstance.GetSoundVolume() * .6f);
                        break;
                    case SoundVolume.OnBackground:
                        AudioSource.PlayOneShot(Clip, AudioInstance.GetSoundVolume() * .1f);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Volume), Volume, null);
                }
            }
            else
            {
                if (Name == null) return;
                AudioInstance.PlaySound(Clip, Volume, Name, Speed, oneAtOnce, destroyOnFinish);
            }
        }
    }
}