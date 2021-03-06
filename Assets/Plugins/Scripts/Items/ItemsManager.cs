﻿using System;
using System.Collections.Generic;
using System.Linq;
using Menu;
using UnityEngine;
using ZeoFlow;

// ReSharper disable once CheckNamespace
namespace Items
{
    public class ItemsManager : MonoBehaviour
    {

        private static readonly List<Item> ItemsUnlocked = new List<Item>();
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
            if (Pause.IsPaused) return;
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

        public static bool Unlocked(Item item)
        {
            return ItemsUnlocked.Contains(item);
        }

        public static void Unlock(Item item)
        {
            if (ItemsUnlocked.Contains(item)) return;
            ItemsUnlocked.Add(item);
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
            var itemsToRemove = new List<ItemBean>();
            foreach (var itemS in Items)
            {
                // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
                if (itemS.GameObject == null)
                {
                    itemsToRemove.Add(itemS);
                }
                else
                {
                    itemS.GameObject.SetActive(false);
                }
            }
            foreach (var itemS in itemsToRemove)
            {
                Items.Remove(itemS);
            }
            item.GameObject.SetActive(true);
            Items.Add(item);
        }

        private static void ChangeTool(SwitchType switchType)
        {
            var activeIndex = 0;
            var index = 0;
            foreach (var itemS in Items.ToList().Where(itemS => itemS.GameObject == null))
            {
                Items.Remove(itemS);
            }
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

        public static void Clean()
        {
            Items.Clear();
            ItemsUnlocked.Clear();
        }

        private SwitchType GETSwitchType()
        {
            if (InputManager.GetAxisRaw("SwitchTool") == 0) return SwitchType.None;
            return InputManager.GetAxisRaw("SwitchTool") > 0 ? SwitchType.Top : SwitchType.Down;
        }
    }
}