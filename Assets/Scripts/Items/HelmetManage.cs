using System.Collections.Generic;
using UnityEngine;
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
		public int rechargeTime = 400;
		public float lightIntensity = 2.7f;

		private bool _isBoxColliderNotNull;
		private BoxCollider _boxCollider;
		private int _timer;
		private int _index;
		private bool _outOfBattery;
		private int _outTime;

		// unity 1 = 1frame
		private readonly FlashPattern[] _lightPattern =
		{
			// initial battery capacity
			new FlashPattern {IsDark = false, Time = 300, Intensity = 2.7f},

			// light pattern (first light then dark and so on)
			new FlashPattern {IsDark = false, Time = 200, Intensity = 2.7f},
			new FlashPattern {IsDark = true, Time = 10, Intensity = 0f},
			new FlashPattern {IsDark = false, Time = 180, Intensity = 2.5f},
			new FlashPattern {IsDark = true, Time = 10, Intensity = 0f},
			new FlashPattern {IsDark = false, Time = 160, Intensity = 2.3f},
			new FlashPattern {IsDark = true, Time = 10, Intensity = 0f},
			new FlashPattern {IsDark = false, Time = 140, Intensity = 2.2f},
			new FlashPattern {IsDark = true, Time = 10, Intensity = 0f},
			new FlashPattern {IsDark = false, Time = 120, Intensity = 2.0f},
			new FlashPattern {IsDark = true, Time = 10, Intensity = 0f},
			new FlashPattern {IsDark = false, Time = 100, Intensity = 1.8f},
			new FlashPattern {IsDark = true, Time = 20, Intensity = 0f},
			new FlashPattern {IsDark = false, Time = 80, Intensity = 1.6f},
			new FlashPattern {IsDark = true, Time = 35, Intensity = 0f},
			new FlashPattern {IsDark = false, Time = 60, Intensity = 1.4f},

			// end pattern
			new FlashPattern {IsDark = true, Time = 0},
		};

		public HelmetManage()
		{
			_timer = 0;
			_index = 0;
			_outTime = 0;
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

		/// <summary>
		///     <para> ONUpdate </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="playerAttachMenu"></param>
		public void ONUpdate(PlayerAttachSub playerAttachMenu)
		{
			if (_isBoxColliderNotNull) _boxCollider.enabled = false;
			if (Input.GetKeyDown(KeyCode.F))
			{
				if (!_outOfBattery && !helmetLight.enabled)
				{
					_index = 0;
					_timer = 0;
					helmetLight.enabled = true;
					helmetLight.intensity = _lightPattern[0].Intensity;
				} else if (helmetLight.enabled)
				{
					_index = 0;
					_timer = 0;
					helmetLight.enabled = false;
				}
			}

			if (!_outOfBattery)
			{
				if (_index < _lightPattern.Length)
				{
					Flashlight();
				}
			}
			else
			{
				_outTime--;
				if (_outTime <= 0)
				{
					_outOfBattery = false;
				}
			}
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
				return;
			}

			_index = 0;
			_outTime = rechargeTime;
			_outOfBattery = true;
			helmetLight.enabled = false;
			helmetLight.intensity = 0;
		}
	}
}