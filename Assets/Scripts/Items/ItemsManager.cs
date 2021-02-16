using System;
using System.Collections.Generic;
using UnityEngine;
using ZeoFlow;
using ZeoFlow.Pickup;

namespace Items
{
	public class ItemsManager : MonoBehaviour
	{
		[Header("Pickaxe")] public PickaxeManage pickaxeManage;
		public GameObject pickaxeObject;
		public PickableObject pickablePickaxe;
		
		[Header("Flares")] public FlareManage flareManage;
		public GameObject flaresObject;
		public PickableObject pickableFlares;
		
		private int _index;
		private static List<Item> Items = new List<Item>
		{
			Item.None
		};

		private static ItemsManager _mInstance;

		public static bool CanPickUp(Item item)
		{
			return !Items.Contains(item);
		}
		
		private void Awake()
		{
			if (_mInstance == null)
			{
				_mInstance = this;
			}
			else
			{
				Debug.LogWarning("You have multiple InputManager instances in the scene!", gameObject);
				Destroy(this);
			}
		}

		private void Update()
		{
			if (pickaxeObject.activeSelf && flaresObject.activeSelf &&
				pickaxeManage.IsAttached && flareManage.IsAttached)
			{
				if (!Items.Contains(Item.Pickaxe))
				{
					_index = Items.Count;
					pickaxeObject.SetActive(true);
					flaresObject.SetActive(false);
				}
				else if (!Items.Contains(Item.Flare))
				{
					_index = Items.Count;
					pickaxeObject.SetActive(false);
					flaresObject.SetActive(true);
				}
			}

			if (pickaxeManage.IsAttached && !Items.Contains(Item.Pickaxe)) Items.Add(Item.Pickaxe);
			if (flareManage.IsAttached && !Items.Contains(Item.Flare)) Items.Add(Item.Flare);

			var switchType = GETSwitchType();
			switch (switchType)
			{
				case SwitchType.None:
					return;
				case SwitchType.Down:
				{
					_index--;
					if (_index < 0) _index = Items.Count - 1;
					break;
				}
				case SwitchType.Top:
				{
					_index++;
					if (_index == Items.Count) _index = 0;
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
			}

			switch (Items[_index])
			{
				case Item.None:
					if (pickaxeManage.IsAttached) pickaxeObject.SetActive(false);
					if (flareManage.IsAttached) flaresObject.SetActive(false);
					break;
				case Item.Flare:
					if (pickaxeManage.IsAttached) pickaxeObject.SetActive(false);
					if (flareManage.IsAttached) flaresObject.SetActive(true);
					break;
				case Item.Pickaxe:
					if (pickaxeManage.IsAttached) pickaxeObject.SetActive(true);
					if (flareManage.IsAttached) flaresObject.SetActive(false);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private SwitchType GETSwitchType()
		{
			if (InputManager.GetAxisRaw("SwitchTool") == 0) return SwitchType.None;
			return InputManager.GetAxisRaw("SwitchTool") > 0 ? SwitchType.Top : SwitchType.Down;
		}
	}
}