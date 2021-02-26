using UnityEngine;
using UnityEngine.SceneManagement;
using ZeoFlow;
using ZeoFlow.PlayerMovement;

namespace Override
{
	/// <summary>
	///     <para> TimeScale </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class TimeScale : MonoBehaviour
	{

		/// <summary>
		///     <para> Update </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Update()
		{
			Time.timeScale = 1f;
		}
	}
}