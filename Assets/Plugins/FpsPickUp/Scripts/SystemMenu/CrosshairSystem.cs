using System;
using UnityEngine;

namespace ZeoFlow.Pickup
{
	[Serializable]
	public class CrosshairSystem
	{
		public bool enabled = true;

		public Texture2D onDefault;
		public Texture2D onAble;
		public Texture2D onGrab;
	}
}