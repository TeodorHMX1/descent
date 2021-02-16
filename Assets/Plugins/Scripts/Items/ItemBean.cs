using UnityEngine;

namespace Items
{
	public class ItemBean
	{

		private Item _itemType;
		private GameObject _gameObject;

		public Item ItemType
		{
			get => _itemType;
			set => _itemType = value;
		}

		public GameObject GameObject
		{
			get => _gameObject;
			set => _gameObject = value;
		}

	}
}