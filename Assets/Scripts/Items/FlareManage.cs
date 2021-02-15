using System;
using Destructible;
using UnityEngine;
using ZeoFlow;
using ZeoFlow.Outline;
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

		public GameObject flare;
		public GameObject flareLight;
		public GameObject flareBody;
		
		private bool _isAttached;
		private PickableObject _pickableObject;
		private bool _isPickableObjectNotNull;
		private OutlineObject _outlineObject;
		private bool _isOutlineObjectNotNull;

		private void Start()
		{
			flareLight.SetActive(false);
			_pickableObject = GetComponent<PickableObject>();
			_isPickableObjectNotNull = _pickableObject != null;
			_outlineObject = flareBody.GetComponent<OutlineObject>();
			_isOutlineObjectNotNull = _outlineObject != null;
		}

		private void Update()
		{
			if (!_isAttached) return;
			
			if (!InputManager.GetButtonDown("InteractHUD")) return;

			if (!_isPickableObjectNotNull) return;
			
			_pickableObject.OnDrop();
			flare.RemoveComponent<PickableObject>();
			
			if (_isOutlineObjectNotNull)
			{
				flareBody.RemoveComponent<OutlineObject>();
			}
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