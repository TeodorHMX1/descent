using UnityEngine;
using UnityEngine.SceneManagement;
using ZeoFlow;
using ZeoFlow.PlayerMovement;

namespace Override
{
	/// <summary>
	///     <para> VSync </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class VSync : MonoBehaviour
	{

		/// <summary>
		///     <para> Update </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Update()
		{
			// VSync must be disabled
			QualitySettings.vSyncCount = 0;
			// Set the frame rate to 60 on all devices
			Application.targetFrameRate = 60;
		}
	}
}