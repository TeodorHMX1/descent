using System;
using System.Collections.Generic;
using Filters;
using Menu;
using Override;
using UnityEngine;

namespace Paranoia
{
    /// <summary>
    ///     <para> HeartbeatSpeed </para>
    ///     <author> @TeodorHMX1 </author>
    /// </summary>
    public enum HeartbeatSpeed
    {
        Normal = 10,
        NormalV2 = 12,
        NormalV3 = 14,
        Increased = 16,
        IncreasedV2 = 18,
        IncreasedV3 = 20,
        MaxSpeed = 22
    }

    /// <summary>
    ///     <para> ParanoiaSystem </para>
    ///     <author> @TeodorHMX1 </author>
    /// </summary>
    public class ParanoiaSystem : MonoBehaviour
    {
        public GameObject player;
        public ParanoiaEntrances numberOfEntrances = ParanoiaEntrances.Two;
        public List<ParanoiaTrigger> paranoiaTriggers = new List<ParanoiaTrigger>();
        public EffectSub effectSub = new EffectSub();

        private AudioSource _audioSource;
        private float _darkAlpha;
        private float _fadeAlpha;
        private FilterIllusions _filterIllusions;
        private FilterParanoia _filterParanoia;
        private FilterParanoiaDark _filterParanoiaDark;
        private Light _playerLight;

        private ParanoiaState _paranoiaBoxState = ParanoiaState.Outside;
        private float _saturationAlpha;
        private bool _isHelmetObjNotNull;
        private bool _isAudioSourceNotNull;
        private bool _insideSafeArea;
        private int _heartbeatTimer;
        private HeartbeatSpeed _heartbeatSpeed = HeartbeatSpeed.Normal;
        private bool _isPlayerLightNotNull;

        /// <summary>
        ///     <para> Start </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.Stop();
            _isAudioSourceNotNull = _audioSource != null;
            _isHelmetObjNotNull = effectSub.helmetObj != null;

            if (effectSub.camera == null) return;

            _filterParanoia = effectSub.camera.GetComponent<FilterParanoia>();
            if (_filterParanoia == null)
            {
                _filterParanoia = effectSub.camera.AddComponent<FilterParanoia>();
                _filterParanoia.brightness = 1.0f;
                _filterParanoia.contrast = 1.0f;
                _filterParanoia.saturation = 1.0f;
            }

            _filterParanoiaDark = effectSub.camera.GetComponent<FilterParanoiaDark>();
            if (_filterParanoiaDark == null)
            {
                _filterParanoiaDark = effectSub.camera.AddComponent<FilterParanoiaDark>();
                _filterParanoiaDark.alpha = 0f;
            }

            _filterIllusions = effectSub.camera.GetComponent<FilterIllusions>();
            if (_filterIllusions == null)
            {
                _filterIllusions = effectSub.camera.AddComponent<FilterIllusions>();
                _filterIllusions.fade = _fadeAlpha;
            }

            if (player == null) return;
            _playerLight = player.transform.Find("PlayerLight").GetComponent<Light>();
            _isPlayerLightNotNull = _playerLight != null;
            if (_isPlayerLightNotNull)
            {
                _playerLight.enabled = false;
            }
        }

        /// <summary>
        ///     <para> Update </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void Update()
        {
            if (!_isPlayerLightNotNull) return;
            if (Pause.IsPaused) return;

            switch (numberOfEntrances)
            {
                // 1 entry
                case ParanoiaEntrances.One:
                {
                    var trigger = paranoiaTriggers[0];
                    _paranoiaBoxState = trigger.GETWasCollided() ? ParanoiaState.Inside : ParanoiaState.Outside;
                    break;
                }
                // 2 entries
                case ParanoiaEntrances.Two:
                {
                    var trigger1 = paranoiaTriggers[0];
                    var trigger2 = paranoiaTriggers[1];

                    if (trigger1.GETWasCollided() && !trigger2.GETWasCollided())
                        _paranoiaBoxState = ParanoiaState.Inside;
                    else
                        _paranoiaBoxState = ParanoiaState.Outside;

                    break;
                }
                case ParanoiaEntrances.Three:
                    _paranoiaBoxState = ParanoiaState.Outside;
                    break;
                case ParanoiaEntrances.Four:
                    _paranoiaBoxState = ParanoiaState.Outside;
                    break;
                case ParanoiaEntrances.Five:
                    _paranoiaBoxState = ParanoiaState.Outside;
                    break;
                default:
                    _paranoiaBoxState = ParanoiaState.Outside;
                    break;
            }

            if (_insideSafeArea)
            {
                _paranoiaBoxState = ParanoiaState.SafeArea;
            }

            if (_paranoiaBoxState == ParanoiaState.Outside && _heartbeatSpeed != HeartbeatSpeed.Normal)
            {
                if (_heartbeatTimer >= 10)
                {
                    _heartbeatTimer = 0;
                    _heartbeatSpeed--;
                }
                else
                {
                    _heartbeatTimer++;
                }
            }
            else if(_heartbeatSpeed != HeartbeatSpeed.MaxSpeed)
            {
                if (_heartbeatTimer >= 60)
                {
                    _heartbeatTimer = 0;
                    _heartbeatSpeed++;
                }
                else
                {
                    _heartbeatTimer++;
                }
            }

            switch (_paranoiaBoxState)
            {
                case ParanoiaState.Inside:
                    OnParanoiaEffect();
                    break;
                case ParanoiaState.Outside:
                    OnCancelEffect();

                    if (_isHelmetObjNotNull)
                    {
                        effectSub.helmetObj.DisableParanoiaTriggered();
                    }

                    break;
                case ParanoiaState.SafeArea:
                    OnCancelEffect();

                    if (_isHelmetObjNotNull)
                    {
                        effectSub.helmetObj.DisableParanoiaTriggered();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     <para> OnCancelEffect </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void OnCancelEffect()
        {
            if (_isPlayerLightNotNull)
            {
                _playerLight.enabled = false;
            }
            _audioSource.Stop();
            if (_darkAlpha > 0f)
            {
                if (effectSub.musicEnabled)
                    if (_isAudioSourceNotNull)
                        foreach (var audioClip in effectSub.audioClips)
                        {
                            new AudioBuilder()
                                .WithClip(audioClip)
                                .WithName("ParanoiaEffect_" + audioClip.name)
                                .WithVolume(SoundVolume.Normal)
                                .WithSpeed((int) _heartbeatSpeed / 10f)
                                .Play(true);
                        }

                _darkAlpha -= 0.006f;
            }
            else
                _darkAlpha = 0.0f;

            if (_saturationAlpha > 1.0f)
                _saturationAlpha -= 0.0006f;
            else
                _saturationAlpha = 1.0f;
            if (_fadeAlpha > 0.0f)
            {
                _fadeAlpha -= 0.0003f;
            }
            else
            {
                _fadeAlpha = 0.0f;
            }

            _filterParanoia.brightness = 1.0f;
            _filterParanoia.contrast = 1.0f;
            _filterParanoia.saturation = _saturationAlpha;
            _filterParanoiaDark.alpha = _darkAlpha;
            _filterIllusions.fade = _fadeAlpha;
        }

        /// <summary>
        ///     <para> OnParanoiaEffect </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void OnParanoiaEffect()
        {
            if (_isPlayerLightNotNull)
            {
                _playerLight.enabled = true;
            }
            if (!effectSub.enabled) return;

            if (_isHelmetObjNotNull && effectSub.helmetObj.attached)
            {
                if (effectSub.helmetObj.CanApplyEffect())
                {
                    ApplyParanoiaEffect();
                }
                else
                {
                    if (effectSub.helmetObj.IsHelmetLightOn())
                    {
                        OnCancelEffect();
                    }

                    effectSub.helmetObj.SetParanoiaTriggered();
                }
            }
            else
            {
                ApplyParanoiaEffect();
            }
        }

        /// <summary>
        ///     <para> ApplyParanoiaEffect </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        private void ApplyParanoiaEffect()
        {
            if (effectSub.musicEnabled)
                if (_isAudioSourceNotNull)
                    foreach (var audioClip in effectSub.audioClips)
                    {
                        new AudioBuilder()
                            .WithClip(audioClip)
                            .WithName("ParanoiaEffect_" + audioClip.name)
                            .WithVolume(SoundVolume.Normal)
                            .WithSpeed((int) _heartbeatSpeed / 10f)
                            .Play(true);
                    }

            if (!effectSub.cameraEffectEnabled) return;
            if (_darkAlpha < 1f) _darkAlpha += 0.002f;
            if (_saturationAlpha < 1.2f) _saturationAlpha += 0.0025f;
            if (_fadeAlpha < 0.2f) _fadeAlpha += 0.00001f;
            _filterParanoiaDark.alpha = _darkAlpha;
            _filterParanoia.saturation = _saturationAlpha;
            _filterIllusions.fade = _fadeAlpha;
            _filterIllusions.coloredChange = 2.0f;
        }

        /// <summary>
        ///     <para> InsideSafeArea </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
        public bool InsideSafeArea
        {
            set => _insideSafeArea = value;
        }
    }
}