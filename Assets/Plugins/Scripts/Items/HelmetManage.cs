using System;
using Override;
using UnityEngine;
using ZeoFlow;
using ZeoFlow.Pickup;
using ZeoFlow.Pickup.Interfaces;

namespace Items
{
	/// <summary>
	///     <para> HelmetManage </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class HelmetManage : MonoBehaviour, IOnAttached
	{
		public Light helmetLight;
		public float lightIntensity = 1f;
		public bool attached = false;
		public AudioClip Torch;

		// unity 1 = 1frame
		private readonly FlashPattern[] _lightPattern =
		{
			// initial battery capacity
			new FlashPattern {IsDark = false, Time = 100, Intensity = 1f},

			// light pattern (first light then dark and so on)
			new FlashPattern {IsDark = false, Time = 120, Intensity = 1f},
			new FlashPattern {IsDark = true, Time = 10, Intensity = 0f},
			new FlashPattern {IsDark = false, Time = 120, Intensity = 0.8f},
			new FlashPattern {IsDark = true, Time = 35, Intensity = 0f},
			new FlashPattern {IsDark = false, Time = 60, Intensity = 0.4f},

			// end pattern
			new FlashPattern {IsDark = true, Time = 0}
		};

		private BoxCollider _boxCollider;
		private int _index;
		private bool _isBoxColliderNotNull;
		private bool _outOfBattery;

		private bool _paranoiaTriggered;
		private int _timer;

		/// <summary>
		///     <para> HelmetManage </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public HelmetManage()
		{
			_timer = 0;
			_index = 0;
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Start()
		{
			_boxCollider = GetComponent<BoxCollider>();
			_isBoxColliderNotNull = _boxCollider != null;

			_lightPattern[0].Intensity = lightIntensity;
		}

		private void Update()
		{
			if (!attached) return;

			if (_isBoxColliderNotNull) _boxCollider.enabled = false;
			
			if (InputManager.GetButtonDown("Flashlight"))
			{
				if (!_outOfBattery && !helmetLight.enabled)
				{
					new AudioBuilder()
						.WithClip(Torch)
						.WithName("Torch_Toggle")
						.WithVolume(SoundVolume.Normal)
						.Play();
					_index = 0;
					_timer = 0;
					helmetLight.enabled = true;
					helmetLight.intensity = _lightPattern[0].Intensity;
				}
				else if (helmetLight.enabled)
				{
					new AudioBuilder()
						.WithClip(Torch)
						.WithName("Torch_Toggle")
						.WithVolume(SoundVolume.Normal)
						.Play();
					_index = 0;
					_timer = 0;
					helmetLight.enabled = false;
					if (_paranoiaTriggered)
					{
						_outOfBattery = true;
					}
				}
			}

			if (!_paranoiaTriggered) return;

			if (_outOfBattery) return;
			if (_index < _lightPattern.Length) Flashlight();
		}

		/// <summary>
		///     <para> ONUpdate </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="playerAttachMenu"></param>
		public void ONUpdate(PlayerAttachSub playerAttachMenu)
		{
			attached = true;
		}

		/// <summary>
		///     <para> Flashlight </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Flashlight()
		{
			_timer++;
			if (_timer < _lightPattern[_index].Time) return;
			_timer = 0;

			_index++;
			if (_index < _lightPattern.Length)
			{
				helmetLight.enabled = !_lightPattern[_index].IsDark;
				helmetLight.intensity = _lightPattern[_index].Intensity;
				Debug.Log("here...");
				return;
			}

			_index = 0;
			_outOfBattery = true;
			helmetLight.enabled = false;
			helmetLight.intensity = 0;
		}

		/// <summary>
		///     <para> CanApplyEffect </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <returns param="_outOfBattery"></returns>
		public bool CanApplyEffect()
		{
			return _paranoiaTriggered && (!helmetLight.enabled && !_outOfBattery || _outOfBattery);
		}

		/// <summary>
		///     <para> IsHelmetLightOn </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <returns param="_helmetLight.enabled"></returns>
		public bool IsHelmetLightOn()
		{
			return helmetLight.enabled;
		}

		/// <summary>
		///     <para> SetParanoiaTriggered </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void SetParanoiaTriggered()
		{
			if (_paranoiaTriggered) return;

			_paranoiaTriggered = true;
			_index = 0;
			_timer = 0;
			// _outOfBattery = !helmetLight.enabled;
		}

		/// <summary>
		///     <para> DisableParanoiaTriggered </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void DisableParanoiaTriggered()
		{
			if (!_paranoiaTriggered) return;

			_paranoiaTriggered = false;
			_outOfBattery = false;
		}
	}
}