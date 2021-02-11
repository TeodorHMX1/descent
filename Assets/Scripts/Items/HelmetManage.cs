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
			new FlashPattern {IsDark = false, Time = 300},

			// light pattern (first light then dark and so on)
			new FlashPattern {IsDark = false, Time = 200},
			new FlashPattern {IsDark = true, Time = 10},
			new FlashPattern {IsDark = false, Time = 180},
			new FlashPattern {IsDark = true, Time = 10},
			new FlashPattern {IsDark = false, Time = 160},
			new FlashPattern {IsDark = true, Time = 10},
			new FlashPattern {IsDark = false, Time = 140},
			new FlashPattern {IsDark = true, Time = 10},
			new FlashPattern {IsDark = false, Time = 120},
			new FlashPattern {IsDark = true, Time = 10},
			new FlashPattern {IsDark = false, Time = 100},
			new FlashPattern {IsDark = true, Time = 20},
			new FlashPattern {IsDark = false, Time = 80},
			new FlashPattern {IsDark = true, Time = 35},
			new FlashPattern {IsDark = false, Time = 60},

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
				if (!_outOfBattery)
				{
					_index = 0;
					_timer = 0;
					helmetLight.enabled = !helmetLight.enabled;
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
				return;
			}

			_index = 0;
			_outTime = rechargeTime;
			_outOfBattery = true;
			helmetLight.enabled = false;
		}
	}
}