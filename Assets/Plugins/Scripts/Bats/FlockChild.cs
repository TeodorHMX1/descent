using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bats
{
	/// <summary>
	///     <para> Start </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class FlockChild : MonoBehaviour
	{
		private static int _updateNextSeed;

		[HideInInspector] public FlockController spawner;

		[HideInInspector] public Vector3 wayPoint;

		public float speed;

		[HideInInspector] public bool dived = true;

		[HideInInspector] public float stuckCounter;

		[HideInInspector] public float damping;

		[HideInInspector] public bool soar = true;

		[HideInInspector] public bool landing;

		[HideInInspector] public float targetSpeed;

		[HideInInspector] public bool move = true;

		public GameObject model;

		public Transform
			modelT;

		[HideInInspector] public float
			_avoidValue;

		[HideInInspector] public float _avoidDistance;

		[HideInInspector] public bool avoid = true;

		public Transform thisT;
		private bool _instantiated;
		private int _lerpCounter;
		private float _soarTimer;
		private int _updateSeed = -1;

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void Start()
		{
			FindRequiredComponents();
			Wander(0.0f);
			SetRandomScale();
			thisT.position = FindWaypoint();
			RandomizeStartAnimationFrame();
			InitAvoidanceValues();
			speed = spawner.minSpeed;
			spawner.activeChildren++;
			_instantiated = true;
			if (spawner.updateDivisor <= 1) return;
			var updateSeedCap = spawner.updateDivisor - 1;
			_updateNextSeed++;
			_updateSeed = _updateNextSeed;
			_updateNextSeed %= updateSeedCap;
		}

		/// <summary>
		///     <para> Update </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void Update()
		{
			if (spawner.updateDivisor > 1 && spawner.updateCounter != _updateSeed) return;
			SoarTimeLimit();
			CheckForDistanceToWaypoint();
			RotationBasedOnWaypointOrAvoidance();
			LimitRotationOfModel();
		}

		/// <summary>
		///     <para> OnEnable </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void OnEnable()
		{
			if (!_instantiated) return;
			spawner.activeChildren++;
			model.GetComponent<Animation>().Play(landing ? spawner.idleAnimation : spawner.flapAnimation);
		}

		/// <summary>
		///     <para> OnDisable </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void OnDisable()
		{
			CancelInvoke();
			spawner.activeChildren--;
		}

		/// <summary>
		///     <para> FindRequiredComponents </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void FindRequiredComponents()
		{
			if (thisT == null) thisT = transform;
			if (model == null) model = thisT.Find("Model").gameObject;
			if (modelT == null) modelT = model.transform;
		}

		/// <summary>
		///     <para> RandomizeStartAnimationFrame </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void RandomizeStartAnimationFrame()
		{
			foreach (AnimationState state in model.GetComponent<Animation>()) state.time = Random.value * state.length;
		}

		/// <summary>
		///     <para> InitAvoidanceValues </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void InitAvoidanceValues()
		{
			_avoidValue = Random.Range(.3f, .1f);
			if (Math.Abs(spawner.birdAvoidDistanceMax - spawner.birdAvoidDistanceMin) > 0)
			{
				_avoidDistance = Random.Range(spawner.birdAvoidDistanceMax, spawner.birdAvoidDistanceMin);
				return;
			}

			_avoidDistance = spawner.birdAvoidDistanceMin;
		}

		/// <summary>
		///     <para> SetRandomScale </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void SetRandomScale()
		{
			var sc = Random.Range(spawner.minScale, spawner.maxScale);
			thisT.localScale = new Vector3(sc, sc, sc);
		}

		/// <summary>
		///     <para> SoarTimeLimit </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void SoarTimeLimit()
		{
			if (!soar || !(spawner.soarMaxTime > 0)) return;
			if (_soarTimer > spawner.soarMaxTime)
			{
				Flap();
				_soarTimer = 0.0f;
			}
			else
			{
				_soarTimer += spawner.newDelta;
			}
		}

		/// <summary>
		///     <para> CheckForDistanceToWaypoint </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void CheckForDistanceToWaypoint()
		{
			switch (landing)
			{
				case false when (thisT.position - wayPoint).magnitude < spawner.waypointDistance + stuckCounter:
					Wander(0.0f);
					stuckCounter = 0.0f;
					break;
				case false:
					stuckCounter += spawner.newDelta;
					break;
				default:
					stuckCounter = 0.0f;
					break;
			}
		}

		/// <summary>
		///     <para> RotationBasedOnWaypointOrAvoidance </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void RotationBasedOnWaypointOrAvoidance()
		{
			var lookIt = wayPoint - thisT.position;
			if (targetSpeed > -1 && lookIt != Vector3.zero)
			{
				var rotation = Quaternion.LookRotation(lookIt);

				thisT.rotation = Quaternion.Slerp(thisT.rotation, rotation, spawner.newDelta * damping);
			}

			if (spawner.childTriggerPos)
				if ((thisT.position - spawner.posBuffer).magnitude < 1)
					spawner.SetFlockRandomPosition();
			speed = Mathf.Lerp(speed, targetSpeed, _lerpCounter * spawner.newDelta * .05f);
			_lerpCounter++;
			if (!move) return;
			thisT.position += thisT.forward * (speed * spawner.newDelta);
			if (avoid && spawner.birdAvoid)
				Avoidance();
		}

		/// <summary>
		///     <para> Avoidance </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private bool Avoidance()
		{
			var fwd = modelT.forward;
			var r = false;
			var pos = thisT.position;
			var rotation = thisT.rotation;
			var rot = rotation;
			var rotE = rotation.eulerAngles;
			if (Physics.Raycast(thisT.position, fwd + modelT.right * _avoidValue, out _, _avoidDistance,
				spawner.avoidanceMask))
			{
				rotE.y -= spawner.birdAvoidHorizontalForce * spawner.newDelta * damping;
				rot.eulerAngles = rotE;
				thisT.rotation = rot;
				r = true;
			}
			else if (Physics.Raycast(thisT.position, fwd + modelT.right * -_avoidValue, out _, _avoidDistance,
				spawner.avoidanceMask))
			{
				rotE.y += spawner.birdAvoidHorizontalForce * spawner.newDelta * damping;
				rot.eulerAngles = rotE;
				thisT.rotation = rot;
				r = true;
			}

			if (spawner.birdAvoidDown && !landing && Physics.Raycast(thisT.position, -Vector3.up, out _,
				_avoidDistance, spawner.avoidanceMask))
			{
				rotE.x -= spawner.birdAvoidVerticalForce * spawner.newDelta * damping;
				rot.eulerAngles = rotE;
				thisT.rotation = rot;
				pos.y += spawner.birdAvoidVerticalForce * spawner.newDelta * .01f;
				thisT.position = pos;
				r = true;
			}
			else if (spawner.birdAvoidUp && !landing && Physics.Raycast(thisT.position, Vector3.up, out _,
				_avoidDistance, spawner.avoidanceMask))
			{
				rotE.x += spawner.birdAvoidVerticalForce * spawner.newDelta * damping;
				rot.eulerAngles = rotE;
				thisT.rotation = rot;
				pos.y -= spawner.birdAvoidVerticalForce * spawner.newDelta * .01f;
				thisT.position = pos;
				r = true;
			}

			return r;
		}

		/// <summary>
		///     <para> LimitRotationOfModel </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void LimitRotationOfModel()
		{
			var rot = modelT.localRotation;
			var rotE = rot.eulerAngles;
			if ((soar && spawner.flatSoar || spawner.flatFly && !soar) && wayPoint.y > thisT.position.y ||
				landing)
			{
				rotE.x = Mathf.LerpAngle(modelT.localEulerAngles.x, -thisT.localEulerAngles.x,
					_lerpCounter * spawner.newDelta * .75f);
				rot.eulerAngles = rotE;
				modelT.localRotation = rot;
			}
			else
			{
				rotE.x = Mathf.LerpAngle(modelT.localEulerAngles.x, 0.0f, _lerpCounter * spawner.newDelta * .75f);
				rot.eulerAngles = rotE;
				modelT.localRotation = rot;
			}
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void Wander(float delay)
		{
			if (landing) return;
			damping = Random.Range(spawner.minDamping, spawner.maxDamping);
			targetSpeed = Random.Range(spawner.minSpeed, spawner.maxSpeed);
			_lerpCounter = 0;
			Invoke(nameof(SetRandomMode), delay);
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void SetRandomMode()
		{
			CancelInvoke(nameof(SetRandomMode));
			if (!dived && Random.value < spawner.soarFrequency)
				Soar();
			else if (!dived && Random.value < spawner.diveFrequency)
				Dive();
			else
				Flap();
		}

		/// <summary>
		///     <para> Flap </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void Flap()
		{
			if (!move) return;
			if (model != null) model.GetComponent<Animation>().CrossFade(spawner.flapAnimation, .5f);
			soar = false;
			AnimationSpeed();
			wayPoint = FindWaypoint();
			dived = false;
		}

		/// <summary>
		///     <para> FindWaypoint </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private Vector3 FindWaypoint()
		{
			var t = Vector3.zero;
			t.x = Random.Range(-spawner.spawnSphere, spawner.spawnSphere) + spawner.posBuffer.x;
			t.z = Random.Range(-spawner.spawnSphereDepth, spawner.spawnSphereDepth) + spawner.posBuffer.z;
			t.y = Random.Range(-spawner.spawnSphereHeight, spawner.spawnSphereHeight) + spawner.posBuffer.y;
			return t;
		}

		/// <summary>
		///     <para> Soar </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Soar()
		{
			if (!move) return;
			model.GetComponent<Animation>().CrossFade(spawner.soarAnimation, 1.5f);
			wayPoint = FindWaypoint();
			soar = true;
		}

		/// <summary>
		///     <para> Dive </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Dive()
		{
			if (spawner.soarAnimation != null)
				model.GetComponent<Animation>().CrossFade(spawner.soarAnimation, 1.5f);
			else
				foreach (AnimationState state in model.GetComponent<Animation>())
					if (thisT.position.y < wayPoint.y + 25)
						state.speed = 0.1f;
			wayPoint = FindWaypoint();
			wayPoint.y -= spawner.diveValue;
			dived = true;
		}

		/// <summary>
		///     <para> AnimationSpeed </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void AnimationSpeed()
		{
			foreach (AnimationState state in model.GetComponent<Animation>())
				if (!dived && !landing)
					state.speed = Random.Range(spawner.minAnimationSpeed, spawner.maxAnimationSpeed);
				else
					state.speed = spawner.maxAnimationSpeed;
		}
	}
}