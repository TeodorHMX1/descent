using UnityEngine;
using UnityEngine.SceneManagement;

namespace Buttons
{
	public class ButtonsManager : MonoBehaviour
	{
		public void ChangeScene(string newLevel)
		{
			Resume();
			SceneManager.LoadScene(newLevel);
		}

		public void Exitgame()
		{
			Debug.Log("Exiting application");
			Application.Quit();
		}

		private static void Resume()
		{
			Time.timeScale = 1f;
		}
	}
}