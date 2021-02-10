using UnityEngine;
using ZeoFlow.Pickup;
using ZeoFlow.Pickup.Interfaces;

namespace Items
{
	
	public class HelmetManage : MonoBehaviour, IOnAttached
	{
		public Light helmetLight;

		/// <summary>
		/// ONUpdate
		/// </summary>
		/// <param name="playerAttachMenu"></param>
		public void ONUpdate(PlayerAttachSub playerAttachMenu)
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				helmetLight.enabled = !helmetLight.enabled;
			}
		}
	}
}