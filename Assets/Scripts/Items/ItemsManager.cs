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

		private readonly List<Item> _items = new List<Item>
		{
			Item.None
		};

		private void Update()
		{
			if (pickaxeObject.activeSelf && flaresObject.activeSelf &&
				pickaxeManage.IsAttached && flareManage.IsAttached)
			{
				if (!_items.Contains(Item.Pickaxe))
				{
					_index = _items.Count;
					pickaxeObject.SetActive(true);
					flaresObject.SetActive(false);
				}
				else if (!_items.Contains(Item.Flare))
				{
					_index = _items.Count;
					pickaxeObject.SetActive(false);
					flaresObject.SetActive(true);
				}
			}

			if (pickaxeManage.IsAttached && !_items.Contains(Item.Pickaxe)) _items.Add(Item.Pickaxe);
			if (flareManage.IsAttached && !_items.Contains(Item.Flare)) _items.Add(Item.Flare);

			var switchType = GETSwitchType();
			switch (switchType)
			{
				case SwitchType.None:
					return;
				case SwitchType.Down:
				{
					_index--;
					if (_index < 0) _index = _items.Count - 1;
					break;
				}
				case SwitchType.Top:
				{
					_index++;
					if (_index == _items.Count) _index = 0;
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
			}

			switch (_items[_index])
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