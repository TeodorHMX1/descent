﻿using System;
using UnityEngine;
using ZeoFlow.Pickup.Interfaces;

// ReSharper disable once CheckNamespace
namespace ZeoFlow.Pickup
{
	[Serializable]
	public class PlayerAttachSub
	{
		public bool attachToPlayer;
		public bool createNewObject;
		public GameObject objectToAttach;
		public GameObject playerObject;
		public Vector3 position;
		public Vector3 rotation;
	}
}