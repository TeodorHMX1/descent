using UnityEngine;
using ZeoFlow.Pickup;
using ZeoFlow.Pickup.Interfaces;

namespace Items
{
	/// <summary>
	///     <para> PickaxeManage </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class PickaxeManage : MonoBehaviour, IOnAttached
	{
		/// <summary>
		///     <para> ONUpdate </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="playerAttachMenu"></param>
		public void ONUpdate(PlayerAttachSub playerAttachMenu)
		{
			if (GetComponent<BoxCollider>() != null) GetComponent<BoxCollider>().enabled = false;
			Animation anim = GetComponentInChildren<Animation>();
			anim.Play("PickaxeSwinging");
		}
	}
}