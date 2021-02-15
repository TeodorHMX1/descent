using System.Collections.Generic;
using UnityEngine;
using ZeoFlow;
using ZeoFlow.Pickup;

namespace Items
{
	public class ItemsManager : MonoBehaviour
	{
		[Header("Pickaxe")]
		public PickaxeManage pickaxeManage;
		public GameObject pickaxeObject;
		public PickableObject pickablePickaxe;

		[Header("Flares")]
		public FlareManage flareManage;
		public GameObject flaresObject;
		public PickableObject pickableFlares;

		private List<Item> _items = new List<Item>();

		private void Update()
		{
			
			if (pickaxeObject.activeSelf && flaresObject.activeSelf &&
				pickaxeManage.IsAttached && flareManage.IsAttached)
			{
				if (!_items.Contains(Item.Pickaxe))
				{
					pickaxeObject.SetActive(true);
					flaresObject.SetActive(false);
				} else if (!_items.Contains(Item.Flare))
				{
					pickaxeObject.SetActive(false);
					flaresObject.SetActive(true);
				}
			}
			
			if (pickaxeManage.IsAttached && !_items.Contains(Item.Pickaxe))
			{
				_items.Add(Item.Pickaxe);
			}
			if (flareManage.IsAttached && !_items.Contains(Item.Flare))
			{
				_items.Add(Item.Flare);
			}
			
			if (GETSwitchType() == SwitchType.None) return;
			
			if (!pickaxeManage.IsAttached || !flareManage.IsAttached) return;

			if (pickaxeObject.activeSelf)
			{
				pickaxeObject.SetActive(false);
				flaresObject.SetActive(true);
			}
			else
			{
				pickaxeObject.SetActive(true);
				flaresObject.SetActive(false);
			}

		}

		private SwitchType GETSwitchType()
		{
			if (InputManager.GetAxisRaw("SwitchTool") == 0) return SwitchType.None;
			return InputManager.GetAxisRaw("SwitchTool") > 0 ? SwitchType.Top : SwitchType.Down;
		}
	}
}