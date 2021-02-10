using UnityEngine;
using ZeoFlow.Pickup.Interfaces;

namespace ZeoFlow.Pickup
{
	public class PickableObject : MonoBehaviour
	{
		public new GameObject gameObject;
		public PhysicsSub physicsMenu = new PhysicsSub();
		public ThrowingSystemMenu throwingSystem = new ThrowingSystemMenu();
		public PlayerAttachSub playerAttachMenu = new PlayerAttachSub();
		public OutlinerSub outlinerMenu = new OutlinerSub();
		private bool _isAttached;

		private void Update()
		{
			if (!_isAttached) return;
			
			if (gameObject.GetComponent<IOnAttached>() != null)
			{
				gameObject.GetComponent<IOnAttached>().ONUpdate(playerAttachMenu);
			}
			
			var position = playerAttachMenu.playerObject.transform.position;
			var positionOffset = playerAttachMenu.position;
			gameObject.transform.position = new Vector3(
				position.x + positionOffset.x,
				position.y + positionOffset.y,
				position.z + positionOffset.z
			);
			var eulerAngles = playerAttachMenu.playerObject.transform.eulerAngles;
			var rotationOffset = playerAttachMenu.rotation;
			gameObject.transform.eulerAngles = new Vector3(
				eulerAngles.x + rotationOffset.x,
				eulerAngles.y + rotationOffset.y,
				eulerAngles.z + rotationOffset.z
			);
		}

		public void OnAttach()
		{
			_isAttached = true;
		}

		public void OnDrop()
		{
			_isAttached = false;
		}
	}
}