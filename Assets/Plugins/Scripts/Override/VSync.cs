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
			// Sync framerate to monitors refresh rate
			QualitySettings.vSyncCount = 1;
		}
	}
}