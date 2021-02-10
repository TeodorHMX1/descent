using System;
using UnityEngine;

namespace ZeoFlow.Pickup
{
	[Serializable]
	public class ThrowingSystemMenu
	{
		public bool enabled = false;

		public KeyCode throwButton = KeyCode.Mouse1;

		public float strength = 100f;

		[NonSerialized] public GameObject obj = null;
		[NonSerialized] public bool throwing = false;
		[NonSerialized] public float throwTime = 1f;
		[NonSerialized] public float defaultThrowTime;
	}
}