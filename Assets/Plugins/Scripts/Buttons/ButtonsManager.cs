using System;
using Override;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Buttons
{
	/// <summary>
	///     <para> OnDisable </para>
	/// </summary>
	public class ButtonsManager : MonoBehaviour
	{
		/// <summary>
		///     <para> ChangeScene </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="newLevel"></param>
		public void ChangeScene(string newLevel)
		{
			Resume();
			LoadScene.LoadingOperation = SceneManager.LoadSceneAsync(newLevel);
		}

		/// <summary>
		///     <para> Exitgame </para>
		/// </summary>
		public void Exitgame()
		{
			Debug.Log("Exiting application");
			Application.Quit();
		}

		/// <summary>
		///     <para> Resume </para>
		/// </summary>
		private static void Resume()
		{
			Time.timeScale = 1f;
		}
	}
}