using System;
using UnityEngine;
using UnityEngine.UI;

namespace ZeoFlow.Pickup
{
	[Serializable]
	public class CrosshairSystem
	{
		public bool enabled = true;

		public Texture2D onDefault;
		public Texture2D onAble;
		public Texture2D onGrab;
		public Texture2D onPuzzle;
		[Tooltip("On Able Show ")]
		public Text guiText;
	}
}