using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZeoFlow;

namespace Items
{
	public class ItemsManager : MonoBehaviour
	{
		private static readonly List<ItemBean> Items = new List<ItemBean>();
		private static ItemsManager _mInstance;

		private static int _timeCountdown;

		public static bool CanPickFlare()
		{
			return _timeCountdown <= 0;
		}

		private void Awake()
		{
			if (_mInstance == null)
			{
				_mInstance = this;
			}
			else
			{
				Debug.LogWarning("You have multiple ItemsManager instances in the scene!", gameObject);
				Destroy(this);
			}
		}

		private void Update()
		{
			
			if (Time.timeScale == 0) return;
			if (_timeCountdown > 0) _timeCountdown--;
			if (Items.Count == 0 || Items.Count == 1) return;

			var switchType = GETSwitchType();
			switch (switchType)
			{
				case SwitchType.None:
					return;
				case SwitchType.Down:
				{
					ChangeTool(switchType);
					break;
				}
				case SwitchType.Top:
				{
					ChangeTool(switchType);
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static bool CanPickUp(Item item)
		{
			return Items.All(itemS => itemS.ItemType != item);
		}

		public static void Remove(Item item)
		{
			if (item == Item.Flare) _timeCountdown = 2;

			foreach (var itemS in Items.ToList().Where(itemS => itemS.ItemType == item)) Items.Remove(itemS);
			if (Items.Count == 0) return;
			ChangeTool(SwitchType.Top);
		}

		public static void AddItem(ItemBean item)
		{
			foreach (var itemS in Items) itemS.GameObject.SetActive(false);
			item.GameObject.SetActive(true);
			Items.Add(item);
		}

		private static void ChangeTool(SwitchType switchType)
		{
			var activeIndex = 0;
			var index = 0;
			foreach (var item in Items)
			{
				if (item.GameObject.activeSelf) activeIndex = index;

				item.GameObject.SetActive(false);
				index++;
			}

			switch (switchType)
			{
				case SwitchType.Down:
					Items[activeIndex - 1 < 0 ? Items.Count - 1 : activeIndex - 1].GameObject.SetActive(true);
					break;
				case SwitchType.Top:
					Items[activeIndex + 1 == Items.Count ? 0 : activeIndex + 1].GameObject.SetActive(true);
					break;
				case SwitchType.None:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(switchType), switchType, null);
			}
		}

		private SwitchType GETSwitchType()
		{
			if (InputManager.GetAxisRaw("SwitchTool") == 0) return SwitchType.None;
			return InputManager.GetAxisRaw("SwitchTool") > 0 ? SwitchType.Top : SwitchType.Down;
		}
	}
}