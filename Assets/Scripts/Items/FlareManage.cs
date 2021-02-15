using System;
using UnityEngine;
using ZeoFlow;
using ZeoFlow.Pickup;
using ZeoFlow.Pickup.Interfaces;

namespace Items
{
	/// <summary>
	///     <para> FlareManage </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class FlareManage : MonoBehaviour, IOnAttached
	{

		public GameObject flareLight;
		
		private bool _isAttached;

		private void Start()
		{
			flareLight.SetActive(false);
		}

		private void Update()
		{
			if (!_isAttached) return;
			
			if (!InputManager.GetButtonDown("InteractHUD")) return;
		}

		/// <summary>
		///     <para> ONUpdate </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="playerAttachMenu"></param>
		public void ONUpdate(PlayerAttachSub playerAttachMenu)
		{
			_isAttached = true;
		}

		public bool IsAttached => _isAttached;
	}
}