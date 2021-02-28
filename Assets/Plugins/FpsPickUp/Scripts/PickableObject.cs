using Items;
using Override;
using UnityEngine;
using ZeoFlow.Pickup.Interfaces;

namespace ZeoFlow.Pickup
{
	public class PickableObject : MonoBehaviour
	{
		public new GameObject gameObject;
		public Item item;
		public ItemFlags itemFlags = ItemFlags.None;
		public string guiText = string.Empty;
		public PhysicsSub physicsMenu = new PhysicsSub();
		public ThrowingSystemMenu throwingSystem = new ThrowingSystemMenu();
		public PlayerAttachSub playerAttachMenu = new PlayerAttachSub();
		public OutlinerSub outlinerMenu = new OutlinerSub();
		public PuzzleSub puzzleSub = new PuzzleSub();
		public AudioClip Itemcollect;

		private bool _isAttached;
		private SyncItem _syncItem;


		private bool toBeDestroyed;

		private void Start()
		{
			itemFlags = ItemFlags.OnGround;
		}

		private void Update()
		{
			if (itemFlags == ItemFlags.OnPlayer || itemFlags == ItemFlags.OnDropped) return;
			if (!_isAttached) return;
			if (!ItemsManager.CanPickUp(item) && item != Item.None) return;
			if (gameObject.GetComponent<SyncItem>() != null) return;

			if (gameObject.GetComponent<IOnAttached>() != null)
				gameObject.GetComponent<IOnAttached>().ONUpdate(playerAttachMenu);

			if (playerAttachMenu.createNewObject)
			{
				var objToAttach = playerAttachMenu.objectToAttach;

				if (item == Item.Flare && !ItemsManager.CanPickFlare())
				{
					toBeDestroyed = true;
					return;
				}

				var newFlare = Instantiate(objToAttach, objToAttach.transform.position,
					Quaternion.Euler(objToAttach.transform.eulerAngles));
				newFlare.name = objToAttach.name;
				newFlare.transform.parent = GameObject.Find("Items").transform;
				_syncItem = newFlare.AddComponent<SyncItem>();
				_syncItem.GameObject = newFlare;
				_syncItem.AttachTo = playerAttachMenu.playerObject;
				ItemsManager.AddItem(new ItemBean {ItemType = item, GameObject = newFlare});
				_isAttached = false;
				if (!toBeDestroyed) return;

				Destroy(newFlare);
				ItemsManager.Remove(Item.Flare);
				toBeDestroyed = false;
			}
			else
			{
				_syncItem = gameObject.AddComponent<SyncItem>();
				_syncItem.GameObject = gameObject;
				_syncItem.AttachTo = playerAttachMenu.playerObject;
				if (item == Item.Flashlight || item == Item.None) return;

				ItemsManager.AddItem(new ItemBean {ItemType = item, GameObject = gameObject});
				itemFlags = ItemFlags.OnPlayer;
			}
		}

		public void OnMovement(bool isRight)
		{
			if (gameObject.GetComponent<IOnPuzzle>() != null) gameObject.GetComponent<IOnPuzzle>().ONMovement(isRight);
		}

		public bool IsPuzzleMoving()
		{
			return gameObject.GetComponent<IOnPuzzle>() != null && gameObject.GetComponent<IOnPuzzle>().ONIsMoving();
		}

		public void OnAttach()
		{
			if (itemFlags == ItemFlags.OnPlayer || itemFlags == ItemFlags.OnDropped) return;
			_isAttached = true;
			new AudioBuilder()
				.WithClip(Itemcollect)
				.WithName("PickableObject_OnAttach")
				.WithVolume(SoundVolume.Weak)
				.Play(true);
		}

		public void OnDrop()
		{
			gameObject.name = gameObject.name.Replace("(Picked)", "(Dropped)");
			itemFlags = ItemFlags.OnDropped;
			_isAttached = false;
			if (_syncItem == null) return;

			_syncItem.OnDestroy();
			ItemsManager.Remove(item);
		}
	}
}