using UnityEngine;
using ZeoFlow;

namespace Items
{
	public class ItemsManager : MonoBehaviour
	{
		[Header("Pickaxe")]
		public PickaxeManage pickaxeManage;
		public GameObject pickaxeObject;

		[Header("Flares")]
		public FlareManage flareManage;
		public GameObject flaresObject;

		private void Update()
		{
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