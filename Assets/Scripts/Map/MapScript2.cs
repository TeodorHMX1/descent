using Menu;
using UnityEngine;
using ZeoFlow;
using ZeoFlow.Pickup;

namespace Map
{
	/// <summary>
	///     <para> MapScript2 </para>
	/// </summary>
	public class MapScript2 : MonoBehaviour
	{
		public static bool ColArea1;
		public static bool ColArea2;
		public static bool ColArea3;
		public static bool Controlsvisible;
		public bool mapOpen;
		public Pause pause;
		public RigidbodyPickUp rigidbodyPickUp;
		public GameObject map;
		public GameObject area1;
		public GameObject area2;
		public GameObject area3;
		public GameObject controlprompt;
		public GameObject Controls;
		private bool _isarea1Null;
		private bool _isarea2Null;
		private bool _isarea3Null;
		private bool _isrigidbodyPickUpNotNull;
		private bool _ispauseNotNull;

		/// <summary>
		///     <para> Start </para>
		/// </summary>
		private void Start()
		{
			_ispauseNotNull = pause != null;
			_isrigidbodyPickUpNotNull = rigidbodyPickUp != null;
			_isarea1Null = area1 == null;
			_isarea2Null = area2 == null;
			_isarea3Null = area3 == null;
			mapOpen = false;
			ColArea1 = false;
			ColArea2 = false;
			ColArea3 = false;
			Controlsvisible = false;
			controlprompt.SetActive(true);
			Controls.SetActive(false);

			if (!_isarea1Null)
			{
				area1.SetActive(false);
			}
			if (!_isarea2Null)
			{
				area2.SetActive(false);
			}
			if (!_isarea3Null)
			{
				area3.SetActive(false);
			}
		}

		/// <summary>
		///     <para> Update </para>
		/// </summary>
		private void Update()
		{
			if (_isarea1Null || _isarea2Null || _isarea3Null) return;
			
			area1.SetActive(ColArea1);

			area2.SetActive(ColArea2);

			area3.SetActive(ColArea3);

			if (InputManager.GetButtonDown("Map")) mapOpen = !mapOpen;
			
			if (_isrigidbodyPickUpNotNull) rigidbodyPickUp.hideCrosshair = mapOpen;

			if (_ispauseNotNull)
			{
				if (pause.isPaused)
				{
					area1.SetActive(false);
					area2.SetActive(false);
					area3.SetActive(false);
					map.SetActive(false);
				}
				else if (mapOpen)
				{
					Open_Map();
				}
				else
				{
					area1.SetActive(false);
					area2.SetActive(false);
					area3.SetActive(false);
					Exit_Map();
				}
			}
			else
			{
				if (mapOpen)
				{
					Open_Map();
				}
				else
				{
					area1.SetActive(false);
					area2.SetActive(false);
					area3.SetActive(false);
					Exit_Map();
				}
			}
			if (InputManager.GetButtonDown("ToggleControls"))
				{

				Controlsvisible = !Controlsvisible;

			}
			if (Controlsvisible == true)
			{
				controlprompt.SetActive(false);
				Controls.SetActive(true);
			}
			else
			{
				controlprompt.SetActive(true);
				Controls.SetActive(false);
			}
		}

		/// <summary>
		///		<para> OnTriggerEnter </para>
		/// </summary>
		/// <param name="collision"></param>
		// public void OnTriggerEnter(Collision collision)
		// {
		// 	if (collision.gameObject.name == "Area1")
		// 	{
		// 		Debug.Log("Collision Detected");
		// 	}
		// }

		/// <summary>
		///     <para> Exit_Map </para>
		/// </summary>
		private void Exit_Map()
		{
			map.SetActive(false);
			mapOpen = false;
			// Debug.Log("Exiting map");
		}

		/// <summary>
		///     <para> Open_Map </para>
		/// </summary>
		private void Open_Map()
		{
			map.SetActive(true);
			mapOpen = true;
			// Debug.Log("opening map");
		}
	}
}