using System;
using UnityEngine;
using ZeoFlow.Outline;

namespace Items
{
	/// <summary>
	///     <para> SyncItem </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class SyncItem : MonoBehaviour
	{
		private GameObject _attachTo;
		private GameObject _gameObject;
		private bool _destroy;

		public GameObject AttachTo
		{
			set => _attachTo = value;
		}

		public GameObject GameObject
		{
			set => _gameObject = value;
		}

		public void OnDestroy()
		{
			_destroy = true;
			Destroy(this);
		}

		private void Update()
		{
			if (_destroy) return;
			
			var position = _attachTo.transform.position;
			var eulerAngles = _attachTo.transform.eulerAngles;

			_gameObject.transform.position = position;
			_gameObject.transform.eulerAngles = eulerAngles;
			gameObject.GetComponentInChildren<OutlineObject>().enabled = false;
		}
	}
}