using System;
using UnityEngine;

namespace ZeoFlow.Pickup
{
	[Serializable]
	public class PhysicsSub
	{
		public enum objectRotation
		{
			FaceForward,
			TurnOnY,
			None
		};

		public objectRotation objectDirection;

		public bool placeObjectBack = false;

		public float placeDistance = 3f;

		public bool keepRotation = false;
		[NonSerialized] public Quaternion intRotation;
		[NonSerialized] public bool objectRotated = false;
		[NonSerialized] public Quaternion objectRot;
		[NonSerialized] public Vector3 objectPos;
		[NonSerialized] public bool canPlaceBack = false;
	}
}