using System;
using Destructible;
using Paranoia;
using UnityEngine;
using UnityEngine.Rendering;
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
	public class FlareManage : MonoBehaviour
	{

		[Header("Flare Data")]
		public GameObject flare;
		public GameObject flareLight;
		public GameObject flareBody;
		
		[Header("Customize")]
		[Tooltip("The seconds after the flare will be disabled")]
		[Range(10, 30)]
		public int seconds = 20;
		[Range(1, 20)]
		public int area = 2;
		public GameObject player;
		public ParanoiaSystem paranoiaSystem;
		
		[Header("Mesh Render")]
		public MeshRenderer objMeshRenderer;
		
		private bool _isAttached;
		private bool _wasDropped;
		private int _time;

		private void Start()
		{
			_isAttached = true;
			flareLight.SetActive(false);
			objMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
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

				var flareTransform = transform;
				var playerPosition = player.transform.position;
				var flarePos = new Vector3(flareTransform.position.x, playerPosition.y, flareTransform.position.z);
				var distance = Vector3.Distance(playerPosition, flarePos);
				paranoiaSystem.InsideSafeArea = distance <= area * 2;
				if (_time < seconds * 60) return;
				
				flareLight.SetActive(false);
				_wasDropped = false;
				Destroy(flare, 10);
				return;
			}

			paranoiaSystem.InsideSafeArea = false;
			if (!_isAttached) return;
			
			if (!InputManager.GetButtonDown("InteractHUD") && !_wasDropped) return;
			
			flareLight.SetActive(true);
			_wasDropped = true;
			_isAttached = false;
			ItemsManager.Remove(Item.Flare);
			GetComponent<SyncItem>().OnDestroy();
		}

	}
}