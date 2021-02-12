using System.Collections.Generic;
using Filters;
using UnityEngine;

namespace Paranoia
{
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

		private ParanoiaState _paranoiaBoxState = ParanoiaState.Outside;
		private float _saturationAlpha;
		private bool _isHelmetObjNotNull;
		private bool _isAudioSourceNotNull;

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Start()
		{
			_audioSource = GetComponent<AudioSource>();
			_isAudioSourceNotNull = _audioSource != null;
			_isHelmetObjNotNull = effectSub.helmetObj != null;
			
			if (effectSub.camera == null) return;

			_filterParanoia = effectSub.camera.AddComponent<FilterParanoia>();
			_filterParanoia.brightness = 1.0f;
			_filterParanoia.contrast = 1.0f;
			_filterParanoia.saturation = 1.0f;

			_filterParanoiaDark = effectSub.camera.AddComponent<FilterParanoiaDark>();
			_filterParanoiaDark.alpha = 0f;

			_filterIllusions = effectSub.camera.AddComponent<FilterIllusions>();
			_filterIllusions.fade = _fadeAlpha;
			_filterIllusions.enabled = false;
		}

		/// <summary>
		///     <para> Update </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Update()
		{
			if (player == null) return;

			switch (numberOfEntrances)
			{
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

			switch (_paranoiaBoxState)
			{
				case ParanoiaState.Inside:
					OnParanoiaEffect();
					break;
				case ParanoiaState.Outside:
					OnCancelEffect();
					break;
				default:
					OnCancelEffect();
					break;
			}
		}

		/// <summary>
		///     <para> OnCancelEffect </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void OnCancelEffect()
		{
			if (_darkAlpha > 0f)
				_darkAlpha -= 0.006f;
			else
				_darkAlpha = 0.0f;
			if (_saturationAlpha > 1.0f)
				_saturationAlpha -= 0.006f;
			else
				_saturationAlpha = 1.0f;
			if (_fadeAlpha > 0.0f)
			{
				_fadeAlpha -= 0.006f;
			}
			else
			{
				_fadeAlpha = 0.0f;
				_filterIllusions.enabled = false;
			}

			_filterParanoia.brightness = 1.0f;
			_filterParanoia.contrast = 1.0f;
			_filterParanoia.saturation = _saturationAlpha;
			_filterParanoiaDark.alpha = _darkAlpha;
			_filterIllusions.fade = _fadeAlpha;
			
			if (_isHelmetObjNotNull)
			{
				effectSub.helmetObj.DisableParanoiaTriggered();
			}
		}

		/// <summary>
		///     <para> OnParanoiaEffect </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void OnParanoiaEffect()
		{
			if (!effectSub.enabled) return;

			if (_isHelmetObjNotNull)
			{
				if (effectSub.helmetObj.IsOutOfBattery())
				{
					ApplyParanoiaEffect();
				}
				else
				{
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
						_audioSource.PlayOneShot(audioClip, effectSub.audioClipVolume);

			if (effectSub.cameraEffectEnabled)
			{
				if (_darkAlpha < 1.75f) _darkAlpha += 0.002f;
				if (_saturationAlpha < 1.2f) _saturationAlpha += 0.0025f;
				if (_fadeAlpha < 0.2f) _fadeAlpha += 0.001f;
				_filterParanoiaDark.alpha = _darkAlpha;
				_filterParanoia.saturation = _saturationAlpha;
				_filterIllusions.fade = _fadeAlpha;
				_filterIllusions.enabled = true;
			}
		}
	}
}