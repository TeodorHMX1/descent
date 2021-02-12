using System.Collections.Generic;
using UnityEngine;

namespace Bats
{
	public class LandingButtons : MonoBehaviour
	{
		public List<LandingSpotController> landingSpotsController = new List<LandingSpotController>();
		public FlockController flockController;
		public float hSliderValue = 250.0f;

		public void OnGUI()
		{
			GUI.Label(new Rect(20.0f, 20.0f, 125.0f, 18.0f), "Landing Spots");
			if (GUI.Button(new Rect(20.0f, 40.0f, 125.0f, 18.0f), "Scare All"))
				foreach (var landingSpot in landingSpotsController)
					landingSpot.ScareAll();
			if (GUI.Button(new Rect(20.0f, 60.0f, 125.0f, 18.0f), "Land In Reach"))
				foreach (var landingSpot in landingSpotsController)
					landingSpot.LandAll();
			if (GUI.Button(new Rect(20.0f, 80.0f, 125.0f, 18.0f), "Land Instant"))
				foreach (var landingSpot in landingSpotsController)
					StartCoroutine(landingSpot.InstantLand(0.01f));
			if (GUI.Button(new Rect(20.0f, 100.0f, 125.0f, 18.0f), "Destroy")) flockController.DestroyBirds();
			GUI.Label(new Rect(20.0f, 120.0f, 125.0f, 18.0f), "Bird Amount: " + flockController.childAmount);
			flockController.childAmount = (int) GUI.HorizontalSlider(new Rect(20.0f, 140.0f, 125.0f, 18.0f),
				flockController.childAmount, 0.0f, 250.0f);
		}
	}
}