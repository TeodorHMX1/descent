using System;
using Destructible;
using Paranoia;
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

		[Header("Flare Data")]
		public GameObject flare;
		public GameObject flareLight;
		public GameObject flareBody;
		
		[Header("Customize")]
		[Tooltip("The seconds after the flare will be disabled")]
		[Range(10, 30)]
		public int seconds = 20;
		[Range(1, 10)]
		public int area = 2;
		public GameObject player;
		public ParanoiaSystem paranoiaSystem;
		
		private bool _isAttached;
		private PickableObject _pickableObject;
		private bool _isPickableObjectNotNull;
		private OutlineObject _outlineObject;
		private bool _isOutlineObjectNotNull;
		private bool _wasDropped;
		private int _time;

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
			if (Time.timeScale == 0) return;
			
			if (_wasDropped)
			{
				_isAttached = false;
				_time++;
				if (_time >= 120)
				{
					GetComponent<Rigidbody>().freezeRotation = true;
				}

				var distance = Vector3.Distance(player.transform.position, transform.position);
				paranoiaSystem.InsideSafeArea = distance <= area * 2;
				if (_time < seconds * 60) return;
				
				flare.RemoveComponent<SphereCollider>();
				flareLight.SetActive(false);
				_wasDropped = false;
				return;
			}

			paranoiaSystem.InsideSafeArea = false;
			if (!_isAttached) return;
			
			if (!InputManager.GetButtonDown("InteractHUD") && !_wasDropped) return;
			
			flareLight.SetActive(true);
			_wasDropped = true;

			if (!_isPickableObjectNotNull) return;

			_pickableObject.OnDrop();
			// flare.AddComponent<SphereCollider>().radius = area;
			flare.RemoveComponent<PickableObject>();
			
			if (_isOutlineObjectNotNull)
			{
				flareBody.RemoveComponent<OutlineObject>();
			}
		}

		/// <summary>
		///     <para> OnTriggerEnter </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="collision"></param>
		private void OnTriggerEnter(Collider collision)
		{
			if (collision.gameObject.name != "Player") return;
		}

		/// <summary>
		///     <para> ONUpdate </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="playerAttachMenu"></param>
		public void ONUpdate(PlayerAttachSub playerAttachMenu)
		{
			flare = _pickableObject.NewFlare;
			_isAttached = true;
		}

		public bool IsAttached => _isAttached;
	}
}