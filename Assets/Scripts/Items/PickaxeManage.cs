using UnityEngine;
using ZeoFlow.Pickup;
using ZeoFlow.Pickup.Interfaces;

namespace Items
{
	public class PickaxeManage : MonoBehaviour, IOnAttached
	{

		/// <summary>
		/// ONUpdate
		/// </summary>
		/// <param name="playerAttachMenu"></param>
		public void ONUpdate(PlayerAttachSub playerAttachMenu)
		{
			if (GetComponent<BoxCollider>() != null)
			{
				GetComponent<BoxCollider>().enabled = false;
			}
		}
	}
}