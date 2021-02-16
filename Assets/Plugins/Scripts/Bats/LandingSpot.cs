using System.Collections;
using UnityEngine;

namespace Bats
{
	
	/// <summary>
	///     <para> LandingSpot </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class LandingSpot : MonoBehaviour
	{
		[HideInInspector] public FlockChild landingChild;

		[HideInInspector] public bool landing;

		[HideInInspector] public LandingSpotController controller;

		public Transform thisT;
		private bool _idle;
		private int _lerpCounter;

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void Start()
		{
			if (thisT == null) thisT = transform;
			if (controller == null)
				controller = thisT.parent.GetComponent<LandingSpotController>();
			if (controller.autoCatchDelay.x > 0)
				StartCoroutine(GetFlockChild(controller.autoCatchDelay.x, controller.autoCatchDelay.y));
			RandomRotate();
		}

		/// <summary>
		///     <para> LateUpdate </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void LateUpdate()
		{
			if (controller.flock.gameObject.activeInHierarchy && landing && landingChild != null)
			{
				if (!landingChild.gameObject.activeInHierarchy) StartCoroutine(ReleaseFlockChild(0.0f, 0.0f));

				var distance = Vector3.Distance(landingChild._thisT.position, thisT.position);

				if (distance < 5 && distance > .5f)
				{
					if (controller.soarLand)
					{
						landingChild.model.GetComponent<Animation>()
							.CrossFade(landingChild.spawner.soarAnimation, .5f);
						if (distance < 2)
							landingChild.model.GetComponent<Animation>()
								.CrossFade(landingChild.spawner.flapAnimation, .5f);
					}

					landingChild.targetSpeed = landingChild.spawner.maxSpeed * .5f;
					landingChild.wayPoint = thisT.position;
					landingChild.damping = controller.landingTurnSpeedModifier;
					landingChild._avoid = false;
				}
				else if (distance <= .5f)
				{
					landingChild.wayPoint = thisT.position;

					if (distance < .1f && !_idle)
					{
						_idle = true;
						landingChild.model.GetComponent<Animation>()
							.CrossFade(landingChild.spawner.idleAnimation, .55f);
					}

					if (distance > .01f)
					{
						landingChild.targetSpeed = landingChild.spawner.minSpeed * controller.landingSpeedModifier;
						landingChild._thisT.position += (thisT.position - landingChild._thisT.position) *
														Time.deltaTime * landingChild.speed *
														controller.landingSpeedModifier;
					}

					landingChild.move = false;
					_lerpCounter++;

					var rot = landingChild._thisT.rotation;
					var rotE = rot.eulerAngles;
					rotE.y = Mathf.LerpAngle(landingChild._thisT.rotation.eulerAngles.y, thisT.rotation.eulerAngles.y,
						_lerpCounter * Time.deltaTime * .005f);
					rot.eulerAngles = rotE;
					landingChild._thisT.rotation = rot;

					landingChild.damping = controller.landingTurnSpeedModifier;
				}
				else
				{
					landingChild.wayPoint = thisT.position;
					landingChild.damping = 1.0f;
				}
			}
		}

		/// <summary>
		///     <para> OnDrawGizmos </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void OnDrawGizmos()
		{
			if (thisT == null) thisT = transform;
			if (controller == null)
				controller = thisT.parent.GetComponent<LandingSpotController>();

			Gizmos.color = Color.yellow;

			if (landingChild != null && landing)
				Gizmos.DrawLine(thisT.position, landingChild._thisT.position);
			if (thisT.rotation.eulerAngles.x != 0 || thisT.rotation.eulerAngles.z != 0)
				thisT.eulerAngles = new Vector3(0.0f, thisT.eulerAngles.y, 0.0f);
			Gizmos.DrawWireCube(new Vector3(thisT.position.x, thisT.position.y, thisT.position.z),
				new Vector3(.2f, .2f, .2f));
			Gizmos.DrawWireCube(thisT.position + thisT.forward * .2f, new Vector3(.1f, .1f, .1f));
			Gizmos.color = new Color(1.0f, 1.0f, 0.0f, .05f);
			Gizmos.DrawWireSphere(thisT.position, controller.maxBirdDistance);
		}

		/// <summary>
		///     <para> GetFlockChild </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="minDelay"></param>
		/// <param name="maxDelay"></param>
		/// <returns></returns>
		public IEnumerator GetFlockChild(float minDelay, float maxDelay)
		{
			yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
			if (controller.flock.gameObject.activeInHierarchy && landingChild == null)
			{
				RandomRotate();

				FlockChild fChild = null;

				for (var i = 0; i < controller.flock.roamers.Count; i++)
				{
					var child = controller.flock.roamers[i];
					if (!child.landing && !child.dived)
					{
						if (!controller.onlyBirdsAbove)
						{
							if (fChild == null &&
								controller.maxBirdDistance >
								Vector3.Distance(child._thisT.position, thisT.position) &&
								controller.minBirdDistance < Vector3.Distance(child._thisT.position, thisT.position))
							{
								fChild = child;
								if (!controller.takeClosest) break;
							}
							else if (fChild != null && Vector3.Distance(fChild._thisT.position, thisT.position) >
								Vector3.Distance(child._thisT.position, thisT.position))
							{
								fChild = child;
							}
						}
						else
						{
							if (fChild == null && child._thisT.position.y > thisT.position.y &&
								controller.maxBirdDistance >
								Vector3.Distance(child._thisT.position, thisT.position) &&
								controller.minBirdDistance < Vector3.Distance(child._thisT.position, thisT.position))
							{
								fChild = child;
								if (!controller.takeClosest) break;
							}
							else if (fChild != null && child._thisT.position.y > thisT.position.y &&
									 Vector3.Distance(fChild._thisT.position, thisT.position) >
									 Vector3.Distance(child._thisT.position, thisT.position))
							{
								fChild = child;
							}
						}
					}
				}

				if (fChild != null)
				{
					landingChild = fChild;
					landing = true;
					landingChild.landing = true;
					StartCoroutine(ReleaseFlockChild(controller.autoDismountDelay.x, controller.autoDismountDelay.y));
				}
				else if (controller.autoCatchDelay.x > 0)
				{
					StartCoroutine(GetFlockChild(controller.autoCatchDelay.x, controller.autoCatchDelay.y));
				}
			}
		}

		/// <summary>
		///     <para> RandomRotate </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void RandomRotate()
		{
			if (controller.randomRotate)
			{
				var rot = thisT.rotation;
				var rotE = rot.eulerAngles;
				rotE.y = Random.Range(0, 360);
				rot.eulerAngles = rotE;
				thisT.rotation = rot;
			}
		}

		/// <summary>
		///     <para> InstantLand </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void InstantLand()
		{
			if (controller.flock.gameObject.activeInHierarchy && landingChild == null)
			{
				FlockChild fChild = null;

				for (var i = 0; i < controller.flock.roamers.Count; i++)
				{
					var child = controller.flock.roamers[i];
					if (!child.landing && !child.dived) fChild = child;
				}

				if (fChild != null)
				{
					landingChild = fChild;
					landing = true;

					landingChild.landing = true;
					landingChild._thisT.position = thisT.position;
					landingChild.model.GetComponent<Animation>().Play(landingChild.spawner.idleAnimation);
					StartCoroutine(ReleaseFlockChild(controller.autoDismountDelay.x, controller.autoDismountDelay.y));
				}
				else if (controller.autoCatchDelay.x > 0)
				{
					StartCoroutine(GetFlockChild(controller.autoCatchDelay.x, controller.autoCatchDelay.y));
				}
			}
		}

		/// <summary>
		///     <para> ReleaseFlockChild </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="minDelay"></param>
		/// <param name="maxDelay"></param>
		/// <returns></returns>
		public IEnumerator ReleaseFlockChild(float minDelay, float maxDelay)
		{
			yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
			if (controller.flock.gameObject.activeInHierarchy && landingChild != null)
			{
				_lerpCounter = 0;
				if (controller.featherPS != null)
				{
					controller.featherPS.position = landingChild._thisT.position;
					controller.featherPS.GetComponent<ParticleSystem>().Emit(Random.Range(0, 3));
				}

				landing = false;
				_idle = false;
				landingChild._avoid = true;

				landingChild.damping = landingChild.spawner.maxDamping;
				landingChild.model.GetComponent<Animation>().CrossFade(landingChild.spawner.flapAnimation, .2f);
				landingChild.dived = true;
				landingChild.speed = 0.0f;
				landingChild.move = true;
				landingChild.landing = false;
				landingChild.Flap();
				landingChild.wayPoint =
					new Vector3(landingChild.wayPoint.x, thisT.position.y + 10, landingChild.wayPoint.z);
				yield return new WaitForSeconds(.1f);
				if (controller.autoCatchDelay.x > 0)
					StartCoroutine(GetFlockChild(controller.autoCatchDelay.x, controller.autoCatchDelay.y));
				landingChild = null;
			}
		}
	}
}