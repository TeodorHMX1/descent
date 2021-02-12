using System.Collections;
using UnityEngine;

namespace Bats
{
	/// <summary>
	///     <para> LandingSpotController </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class LandingSpotController : MonoBehaviour
	{
		public bool randomRotate = true;
		public Vector2 autoCatchDelay = new Vector2(10.0f, 20.0f);
		public Vector2 autoDismountDelay = new Vector2(10.0f, 20.0f);
		public float maxBirdDistance = 20.0f;
		public float minBirdDistance = 5.0f;
		public bool takeClosest;
		public FlockController flock;
		public bool landOnStart;
		public bool soarLand = true;
		public bool onlyBirdsAbove;
		public float landingSpeedModifier = .5f;
		public float landingTurnSpeedModifier = 5.0f;
		public Transform featherPS;
		public Transform thisT;

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void Start()
		{
			if (thisT == null) thisT = transform;
			if (flock == null)
			{
				flock = (FlockController) FindObjectOfType(typeof(FlockController));
			}

			if (landOnStart) StartCoroutine(InstantLandOnStart(.1f));
		}

		/// <summary>
		///     <para> ScareAll </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void ScareAll()
		{
			for (var i = 0; i < thisT.childCount; i++)
				if (thisT.GetChild(i).GetComponent<LandingSpot>() != null)
				{
					var spot = thisT.GetChild(i).GetComponent<LandingSpot>();
					StartCoroutine(spot.ReleaseFlockChild(0.0f, 1.0f));
				}
		}

		/// <summary>
		///     <para> ScareAll </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="minDelay"></param>
		/// <param name="maxDelay"></param>
		public void ScareAll(float minDelay, float maxDelay)
		{
			for (var i = 0; i < thisT.childCount; i++)
				if (thisT.GetChild(i).GetComponent<LandingSpot>() != null)
				{
					var spot = thisT.GetChild(i).GetComponent<LandingSpot>();
					StartCoroutine(spot.ReleaseFlockChild(minDelay, maxDelay));
				}
		}

		/// <summary>
		///     <para> LandAll </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void LandAll()
		{
			for (var i = 0; i < thisT.childCount; i++)
				if (thisT.GetChild(i).GetComponent<LandingSpot>() != null)
				{
					var spot = thisT.GetChild(i).GetComponent<LandingSpot>();
					StartCoroutine(spot.GetFlockChild(0.0f, 2.0f));
				}
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="delay"></param>
		/// <returns></returns>
		public IEnumerator InstantLandOnStart(float delay)
		{
			yield return new WaitForSeconds(delay);
			for (var i = 0; i < thisT.childCount; i++)
				if (thisT.GetChild(i).GetComponent<LandingSpot>() != null)
				{
					var spot = thisT.GetChild(i).GetComponent<LandingSpot>();
					spot.InstantLand();
				}
		}

		/// <summary>
		///     <para> InstantLand </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="delay"></param>
		/// <returns></returns>
		public IEnumerator InstantLand(float delay)
		{
			yield return new WaitForSeconds(delay);
			for (var i = 0; i < thisT.childCount; i++)
				if (thisT.GetChild(i).GetComponent<LandingSpot>() != null)
				{
					var spot = thisT.GetChild(i).GetComponent<LandingSpot>();
					spot.InstantLand();
				}
		}
	}
}