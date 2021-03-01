using System;
using Puzzle;
using UnityEngine;

namespace Items
{
	/// <summary>
	///     <para> PickaxeManage </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class PickaxeSpawner : MonoBehaviour, IOnComplete
	{

		public GameObject pickaxe;

		/// <summary>
		///     <para> ONCompleted </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void ONCompleted()
		{
			if(pickaxe.activeSelf) return;
			pickaxe.SetActive(true);
		}
	}
}