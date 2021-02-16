using Items;
using UnityEngine;
using ZeoFlow.Pickup.Interfaces;

namespace ZeoFlow.Pickup
{
	public class PickableObject : MonoBehaviour
	{
		public new GameObject gameObject;
		public Item item;
		public ItemFlags ItemFlags = ItemFlags.None;
		public PhysicsSub physicsMenu = new PhysicsSub();
		public ThrowingSystemMenu throwingSystem = new ThrowingSystemMenu();
		public PlayerAttachSub playerAttachMenu = new PlayerAttachSub();
		public OutlinerSub outlinerMenu = new OutlinerSub();
		public PuzzleSub puzzleSub = new PuzzleSub();

		private bool _isAttached;
		private SyncItem _syncItem;

		public bool IsAttached
		{
			set => _isAttached = value;
		}

		private void Start()
		{
			ItemFlags = ItemFlags.OnGround;
		}

		private void Update()
		{
			if (ItemFlags == ItemFlags.OnPlayer || ItemFlags == ItemFlags.OnDropped) return;
			if (!_isAttached) return;
			if (!ItemsManager.CanPickUp(item) && item != Item.None) return;
			if (gameObject.GetComponent<SyncItem>() != null) return;

			if (gameObject.GetComponent<IOnAttached>() != null)
				gameObject.GetComponent<IOnAttached>().ONUpdate(playerAttachMenu);

			if (playerAttachMenu.createNewObject)
			{
				if (item == Item.Flare && !ItemsManager.CanPickFlare())
				{
					return;
				}
				var newFlare = Instantiate(gameObject, gameObject.transform.position,
					Quaternion.Euler(gameObject.transform.eulerAngles));
				newFlare.name = gameObject.name;
				newFlare.GetComponent<PickableObject>().IsAttached = false;
				newFlare.GetComponent<PickableObject>().gameObject = newFlare;
				newFlare.transform.parent = GameObject.Find("Items").transform;

				gameObject.name += "(Picked)";
				_syncItem = gameObject.AddComponent<SyncItem>();
				_syncItem.GameObject = gameObject;
				_syncItem.AttachTo = playerAttachMenu.playerObject;
				ItemsManager.AddItem(new ItemBean {ItemType = item, GameObject = gameObject});
				Destroy(gameObject.GetComponent<PickableObject>());
			}
			else
			{
				_syncItem = gameObject.AddComponent<SyncItem>();
				_syncItem.GameObject = gameObject;
				_syncItem.AttachTo = playerAttachMenu.playerObject;
				ItemsManager.AddItem(new ItemBean {ItemType = item, GameObject = gameObject});
			}

			ItemFlags = ItemFlags.OnPlayer;
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
			if (ItemFlags == ItemFlags.OnPlayer || ItemFlags == ItemFlags.OnDropped) return;
			_isAttached = true;
		}

		public void OnDrop()
		{
			gameObject.name = gameObject.name.Replace("(Picked)", "(Dropped)");
			ItemFlags = ItemFlags.OnDropped;
			_isAttached = false;
			if (_syncItem == null) return;

			_syncItem.OnDestroy();
			ItemsManager.Remove(item);
		}
	}
}