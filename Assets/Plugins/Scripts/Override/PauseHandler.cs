using Menu;
using UnityEngine;

namespace Override
{
	/// <summary>
	///     <para> PauseHandler </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class PauseHandler : MonoBehaviour
	{

		public bool active;
		
		/// <summary>
		///     <para> OnApplicationFocus </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="hasFocus"></param>
		private void OnApplicationFocus(bool hasFocus)
		{
			if (!active) return;
			if (hasFocus) return;
			
			Pause.instance.IOnPause();
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			Time.timeScale = 0f;
		}

		/// <summary>
		///     <para> OnApplicationPause </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="pauseStatus"></param>
		private void OnApplicationPause(bool pauseStatus)
		{
			if (!active) return;
		}
	}
}