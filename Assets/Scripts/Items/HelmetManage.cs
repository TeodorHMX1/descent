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
		
		private bool _isBoxColliderNotNull;
		private BoxCollider _boxCollider;
		
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
			if (Input.GetKeyDown(KeyCode.F)) helmetLight.enabled = !helmetLight.enabled;
		}
	}
}