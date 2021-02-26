using System;
using UnityEngine;

namespace Override
{
    public class AudioBuilder
    {
        private AudioClip _clip;
        private SoundVolume _volume = SoundVolume.Normal;
        private string _name;
        private AudioSource _audioSource;

        public AudioClip Clip
        {
            get => _clip;
            set => _clip = value;
        }

        public SoundVolume Volume
        {
            get => _volume;
            set => _volume = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public AudioSource AudioSource
        {
            get => _audioSource;
            set => _audioSource = value;
        }

        public AudioBuilder WithClip(AudioClip clip)
        {
            Clip = clip;
            return this;
        }

        public AudioBuilder WithVolume(SoundVolume volume)
        {
            Volume = volume;
            return this;
        }

        public AudioBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public AudioBuilder WithAudioSource(AudioSource audioSource)
        {
            AudioSource = audioSource;
            return this;
        }

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
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Volume), Volume, null);
                }
            }
            else
            {
                if (Name == null) return;
                AudioInstance.PlaySound(Clip, Volume, Name, oneAtOnce);
            }
        }
    }
}