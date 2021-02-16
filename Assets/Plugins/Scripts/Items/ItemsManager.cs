using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZeoFlow;
using ZeoFlow.Pickup;
using Enumerable = System.Linq.Enumerable;

namespace Items
{
	public class ItemsManager : MonoBehaviour
	{
		[Header("Pickaxe")] public PickaxeManage pickaxeManage;
		public GameObject pickaxeObject;
		public PickableObject pickablePickaxe;
		
		// [Header("Flares")] public FlareManage flareManage;
		// public GameObject flaresObject;
		// public PickableObject pickableFlares;
		
		private static int _index;
		private static FlareManage _flareManage;
		private static List<ItemBean> Items = new List<ItemBean>();

		public static void AddItem(ItemBean item)
		{
			Items.Add(item);
		}

		private static ItemsManager _mInstance;

		public static bool CanPickUp(Item item)
		{
			return Items.All(itemS => itemS.ItemType != item);
		}

		public static void Remove(Item item)
		{
			foreach (var itemS in Items.ToList().Where(itemS => itemS.ItemType == item))
			{
				Items.Remove(itemS);
			}
			_index = 0;
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
			if (Time.timeScale == 0) return;
			
			/*
			
			if (pickaxeObject.activeSelf && flaresObject.activeSelf &&
				pickaxeManage.IsAttached && _flareManage.IsAttached)
			{
				if (!global::Items.Contains(Item.Pickaxe))
				{
					_index = global::Items.Count;
					pickaxeObject.SetActive(true);
					flaresObject.SetActive(false);
				}
				else if (!global::Items.Contains(Item.Flare))
				{
					_index = global::Items.Count;
					pickaxeObject.SetActive(false);
					flaresObject.SetActive(true);
				}
			}

			if (pickaxeManage.IsAttached && !global::Items.Contains(Item.Pickaxe)) global::Items.Add(Item.Pickaxe);
			if (_flareManage.IsAttached && !global::Items.Contains(Item.Flare)) global::Items.Add(Item.Flare);

			var switchType = GETSwitchType();
			switch (switchType)
			{
				case SwitchType.None:
					return;
				case SwitchType.Down:
				{
					_index--;
					if (_index < 0) _index = global::Items.Count - 1;
					break;
				}
				case SwitchType.Top:
				{
					_index++;
					if (_index == global::Items.Count) _index = 0;
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
			}

			switch (global::Items[_index])
			{
				case Item.None:
					if (pickaxeManage.IsAttached) pickaxeObject.SetActive(false);
					if (_flareManage.IsAttached) flaresObject.SetActive(false);
					break;
				case Item.Flare:
					if (pickaxeManage.IsAttached) pickaxeObject.SetActive(false);
					if (_flareManage.IsAttached) flaresObject.SetActive(true);
					break;
				case Item.Pickaxe:
					if (pickaxeManage.IsAttached) pickaxeObject.SetActive(true);
					if (_flareManage.IsAttached) flaresObject.SetActive(false);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}*/
		}

		private SwitchType GETSwitchType()
		{
			if (InputManager.GetAxisRaw("SwitchTool") == 0) return SwitchType.None;
			return InputManager.GetAxisRaw("SwitchTool") > 0 ? SwitchType.Top : SwitchType.Down;
		}
	}
}