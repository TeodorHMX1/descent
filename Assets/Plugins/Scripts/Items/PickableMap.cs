using UnityEngine;
using ZeoFlow.Pickup.Interfaces;

namespace Items
{
	/// <summary>
	///     <para> PickableMap </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class PickableMap : MonoBehaviour, IOnPicked
	{

		/// <summary>
		///     <para> ONPicked </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void ONPicked()
		{
			Destroy(gameObject);
			ItemsManager.Unlock(Item.Map);
		}
	}
}