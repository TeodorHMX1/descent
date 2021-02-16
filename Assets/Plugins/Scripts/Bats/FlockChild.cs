using UnityEngine;

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

		[HideInInspector] public bool _avoid = true;

		public Transform _thisT;
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
			_thisT.position = findWaypoint();
			RandomizeStartAnimationFrame();
			InitAvoidanceValues();
			speed = spawner.minSpeed;
			spawner.activeChildren++;
			_instantiated = true;
			if (spawner.updateDivisor > 1)
			{
				var _updateSeedCap = spawner.updateDivisor - 1;
				_updateNextSeed++;
				_updateSeed = _updateNextSeed;
				_updateNextSeed = _updateNextSeed % _updateSeedCap;
			}
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void Update()
		{
			if (spawner.updateDivisor <= 1 || spawner.updateCounter == _updateSeed)
			{
				SoarTimeLimit();
				CheckForDistanceToWaypoint();
				RotationBasedOnWaypointOrAvoidance();
				LimitRotationOfModel();
			}
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void OnEnable()
		{
			if (_instantiated)
			{
				spawner.activeChildren++;
				if (landing)
					model.GetComponent<Animation>().Play(spawner.idleAnimation);
				else
					model.GetComponent<Animation>().Play(spawner.flapAnimation);
			}
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void OnDisable()
		{
			CancelInvoke();
			spawner.activeChildren--;
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void FindRequiredComponents()
		{
			if (_thisT == null) _thisT = transform;
			if (model == null) model = _thisT.Find("Model").gameObject;
			if (modelT == null) modelT = model.transform;
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void RandomizeStartAnimationFrame()
		{
			foreach (AnimationState state in model.GetComponent<Animation>()) state.time = Random.value * state.length;
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void InitAvoidanceValues()
		{
			_avoidValue = Random.Range(.3f, .1f);
			if (spawner.birdAvoidDistanceMax != spawner.birdAvoidDistanceMin)
			{
				_avoidDistance = Random.Range(spawner.birdAvoidDistanceMax, spawner.birdAvoidDistanceMin);
				return;
			}

			_avoidDistance = spawner.birdAvoidDistanceMin;
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void SetRandomScale()
		{
			var sc = Random.Range(spawner.minScale, spawner.maxScale);
			_thisT.localScale = new Vector3(sc, sc, sc);
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		//Soar Timeout - Limits how long a bird can soar
		public void SoarTimeLimit()
		{
			if (soar && spawner.soarMaxTime > 0)
			{
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
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void CheckForDistanceToWaypoint()
		{
			if (!landing && (_thisT.position - wayPoint).magnitude < spawner.waypointDistance + stuckCounter)
			{
				Wander(0.0f);
				stuckCounter = 0.0f;
			}
			else if (!landing)
			{
				stuckCounter += spawner.newDelta;
			}
			else
			{
				stuckCounter = 0.0f;
			}
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void RotationBasedOnWaypointOrAvoidance()
		{
			var lookit = wayPoint - _thisT.position;
			if (targetSpeed > -1 && lookit != Vector3.zero)
			{
				var rotation = Quaternion.LookRotation(lookit);

				_thisT.rotation = Quaternion.Slerp(_thisT.rotation, rotation, spawner.newDelta * damping);
			}

			if (spawner.childTriggerPos)
				if ((_thisT.position - spawner.posBuffer).magnitude < 1)
					spawner.SetFlockRandomPosition();
			speed = Mathf.Lerp(speed, targetSpeed, _lerpCounter * spawner.newDelta * .05f);
			_lerpCounter++;
			if (move)
			{
				_thisT.position += _thisT.forward * speed * spawner.newDelta;
				if (_avoid && spawner.birdAvoid)
					Avoidance();
			}
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public bool Avoidance()
		{
			var hit = new RaycastHit();
			var fwd = modelT.forward;
			var r = false;
			var rot = Quaternion.identity;
			var rotE = Vector3.zero;
			var pos = Vector3.zero;
			pos = _thisT.position;
			rot = _thisT.rotation;
			rotE = _thisT.rotation.eulerAngles;
			if (Physics.Raycast(_thisT.position, fwd + modelT.right * _avoidValue, out hit, _avoidDistance,
				spawner.avoidanceMask))
			{
				rotE.y -= spawner.birdAvoidHorizontalForce * spawner.newDelta * damping;
				rot.eulerAngles = rotE;
				_thisT.rotation = rot;
				r = true;
			}
			else if (Physics.Raycast(_thisT.position, fwd + modelT.right * -_avoidValue, out hit, _avoidDistance,
				spawner.avoidanceMask))
			{
				rotE.y += spawner.birdAvoidHorizontalForce * spawner.newDelta * damping;
				rot.eulerAngles = rotE;
				_thisT.rotation = rot;
				r = true;
			}

			if (spawner.birdAvoidDown && !landing && Physics.Raycast(_thisT.position, -Vector3.up, out hit,
				_avoidDistance, spawner.avoidanceMask))
			{
				rotE.x -= spawner.birdAvoidVerticalForce * spawner.newDelta * damping;
				rot.eulerAngles = rotE;
				_thisT.rotation = rot;
				pos.y += spawner.birdAvoidVerticalForce * spawner.newDelta * .01f;
				_thisT.position = pos;
				r = true;
			}
			else if (spawner.birdAvoidUp && !landing && Physics.Raycast(_thisT.position, Vector3.up, out hit,
				_avoidDistance, spawner.avoidanceMask))
			{
				rotE.x += spawner.birdAvoidVerticalForce * spawner.newDelta * damping;
				rot.eulerAngles = rotE;
				_thisT.rotation = rot;
				pos.y -= spawner.birdAvoidVerticalForce * spawner.newDelta * .01f;
				_thisT.position = pos;
				r = true;
			}

			return r;
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void LimitRotationOfModel()
		{
			var rot = Quaternion.identity;
			var rotE = Vector3.zero;
			rot = modelT.localRotation;
			rotE = rot.eulerAngles;
			if ((soar && spawner.flatSoar || spawner.flatFly && !soar) && wayPoint.y > _thisT.position.y ||
				landing)
			{
				rotE.x = Mathf.LerpAngle(modelT.localEulerAngles.x, -_thisT.localEulerAngles.x,
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
			if (!landing)
			{
				damping = Random.Range(spawner.minDamping, spawner.maxDamping);
				targetSpeed = Random.Range(spawner.minSpeed, spawner.maxSpeed);
				_lerpCounter = 0;
				Invoke("SetRandomMode", delay);
			}
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void SetRandomMode()
		{
			CancelInvoke("SetRandomMode");
			if (!dived && Random.value < spawner.soarFrequency)
				Soar();
			else if (!dived && Random.value < spawner.diveFrequency)
				Dive();
			else
				Flap();
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void Flap()
		{
			if (move)
			{
				if (model != null) model.GetComponent<Animation>().CrossFade(spawner.flapAnimation, .5f);
				soar = false;
				animationSpeed();
				wayPoint = findWaypoint();
				dived = false;
			}
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public Vector3 findWaypoint()
		{
			var t = Vector3.zero;
			t.x = Random.Range(-spawner.spawnSphere, spawner.spawnSphere) + spawner.posBuffer.x;
			t.z = Random.Range(-spawner.spawnSphereDepth, spawner.spawnSphereDepth) + spawner.posBuffer.z;
			t.y = Random.Range(-spawner.spawnSphereHeight, spawner.spawnSphereHeight) + spawner.posBuffer.y;
			return t;
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void Soar()
		{
			if (move)
			{
				model.GetComponent<Animation>().CrossFade(spawner.soarAnimation, 1.5f);
				wayPoint = findWaypoint();
				soar = true;
			}
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void Dive()
		{
			if (spawner.soarAnimation != null)
				model.GetComponent<Animation>().CrossFade(spawner.soarAnimation, 1.5f);
			else
				foreach (AnimationState state in model.GetComponent<Animation>())
					if (_thisT.position.y < wayPoint.y + 25)
						state.speed = 0.1f;
			wayPoint = findWaypoint();
			wayPoint.y -= spawner.diveValue;
			dived = true;
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void animationSpeed()
		{
			foreach (AnimationState state in model.GetComponent<Animation>())
				if (!dived && !landing)
					state.speed = Random.Range(spawner.minAnimationSpeed, spawner.maxAnimationSpeed);
				else
					state.speed = spawner.maxAnimationSpeed;
		}
	}
}