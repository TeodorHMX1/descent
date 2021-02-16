using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bats
{
	/// <summary>
	///     <para> Start </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class FlockController : MonoBehaviour
	{
		public FlockChild childPrefab;
		public int childAmount = 250;
		public bool slowSpawn;
		public float spawnSphere = 3.0f;
		public float spawnSphereHeight = 3.0f;
		public float spawnSphereDepth = -1.0f;
		public float minSpeed = 6.0f;
		public float maxSpeed = 10.0f;
		public float minScale = .7f;
		public float maxScale = 1.0f;
		public float soarFrequency;
		public string soarAnimation = "BatSoar";
		public string flapAnimation = "BatFlap";
		public string idleAnimation = "BatIdle";
		public float diveValue = 7.0f;
		public float diveFrequency = 0.5f;
		public float minDamping = 1.0f;
		public float maxDamping = 2.0f;
		public float waypointDistance = 1.0f;
		public float minAnimationSpeed = 2.0f;
		public float maxAnimationSpeed = 4.0f;
		public float randomPositionTimer = 10.0f;
		public float positionSphere = 25.0f;
		public float positionSphereHeight = 25.0f;
		public float positionSphereDepth = -1.0f;
		public bool childTriggerPos;
		public bool forceChildWaypoints;
		public float forcedRandomDelay = 1.5f;
		public bool flatFly;
		public bool flatSoar;
		public bool birdAvoid;
		public int birdAvoidHorizontalForce = 1000;
		public bool birdAvoidDown;
		public bool birdAvoidUp;
		public int birdAvoidVerticalForce = 300;
		public float birdAvoidDistanceMax = 4.5f;
		public float birdAvoidDistanceMin = 5.0f;
		public float soarMaxTime;
		public LayerMask avoidanceMask = -1;
		public List<FlockChild> roamers;
		public Vector3 posBuffer;
		public int updateDivisor = 1;
		public float newDelta;
		public int updateCounter;
		public float activeChildren;
		public bool groupChildToNewTransform;
		public Transform groupTransform;
		public string groupName = "";
		public bool groupChildToFlock;
		public Vector3 startPosOffset;
		public Transform thisT;

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void Start()
		{
			thisT = transform;

			if (Math.Abs(positionSphereDepth - -1) < 0) positionSphereDepth = positionSphere;
			if (Math.Abs(spawnSphereDepth - -1) < 0) spawnSphereDepth = spawnSphere;

			posBuffer = thisT.position + startPosOffset;
			if (!slowSpawn) AddChild(childAmount);
			InvokeRepeating(nameof(SetFlockRandomPosition), randomPositionTimer, randomPositionTimer);
		}

		/// <summary>
		///     <para> Update </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void Update()
		{
			if (activeChildren > 0)
			{
				if (updateDivisor > 1)
				{
					updateCounter++;
					updateCounter = updateCounter % updateDivisor;
					newDelta = Time.deltaTime * updateDivisor;
				}
				else
				{
					newDelta = Time.deltaTime;
				}
			}

			UpdateChildAmount();
		}

		/// <summary>
		///     <para> OnDrawGizmos </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void OnDrawGizmos()
		{
			if (thisT == null) thisT = transform;
			if (!Application.isPlaying && posBuffer != thisT.position + startPosOffset)
				posBuffer = thisT.position + startPosOffset;
			if (Math.Abs(positionSphereDepth - -1) < 0) positionSphereDepth = positionSphere;
			if (Math.Abs(spawnSphereDepth - -1) < 0) spawnSphereDepth = spawnSphere;
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(posBuffer, new Vector3(spawnSphere * 2, spawnSphereHeight * 2, spawnSphereDepth * 2));
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireCube(thisT.position,
				new Vector3(positionSphere * 2 + spawnSphere * 2, positionSphereHeight * 2 + spawnSphereHeight * 2,
					positionSphereDepth * 2 + spawnSphereDepth * 2));
		}

		/// <summary>
		///     <para> AddChild </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="amount"></param>
		public void AddChild(int amount)
		{
			if (groupChildToNewTransform) InstantiateGroup();
			for (var i = 0; i < amount; i++)
			{
				var obj = Instantiate(childPrefab);
				obj.spawner = this;
				roamers.Add(obj);
				AddChildToParent(obj.transform);
			}
		}

		/// <summary>
		///     <para> AddChildToParent </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="obj"></param>
		public void AddChildToParent(Transform obj)
		{
			if (groupChildToFlock)
			{
				obj.parent = transform;
				return;
			}

			if (groupChildToNewTransform) obj.parent = groupTransform;
		}

		/// <summary>
		///     <para> RemoveChild </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="amount"></param>
		public void RemoveChild(int amount)
		{
			for (var i = 0; i < amount; i++)
			{
				var dObj = roamers[roamers.Count - 1];
				roamers.RemoveAt(roamers.Count - 1);
				Destroy(dObj.gameObject);
			}
		}

		/// <summary>
		///     <para> InstantiateGroup </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void InstantiateGroup()
		{
			if (groupTransform != null) return;
			var g = new GameObject();

			groupTransform = g.transform;
			groupTransform.position = thisT.position;

			if (groupName != "")
			{
				g.name = groupName;
				return;
			}

			g.name = thisT.name + " Fish Container";
		}

		/// <summary>
		///     <para> UpdateChildAmount </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void UpdateChildAmount()
		{
			if (childAmount >= 0 && childAmount < roamers.Count)
			{
				RemoveChild(1);
				return;
			}

			if (childAmount > roamers.Count) AddChild(1);
		}


		/// <summary>
		///     <para> SetFlockRandomPosition </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void SetFlockRandomPosition()
		{
			var t = Vector3.zero;
			var position = thisT.position;
			t.x = Random.Range(-positionSphere, positionSphere) + position.x;
			t.z = Random.Range(-positionSphereDepth, positionSphereDepth) + position.z;
			t.y = Random.Range(-positionSphereHeight, positionSphereHeight) + position.y;


			posBuffer = t;
			if (!forceChildWaypoints) return;

			foreach (var roamer in roamers)
				roamer.Wander(Random.value * forcedRandomDelay);
		}
		
		/// <summary>
		///     <para> DestroyBirds </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void DestroyBirds()
		{
			foreach (var roamer in roamers)
				Destroy(roamer.gameObject);

			childAmount = 0;
			roamers.Clear();
		}
	}
}