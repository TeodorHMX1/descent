using UnityEngine;
using UnityEngine.SceneManagement;
using ZeoFlow;
using ZeoFlow.PlayerMovement;

/// <summary>
///     <para> Pause </para>
///     <author> @TeodorHMX1 </author>
/// </summary>
public class Pause : MonoBehaviour
{
	public bool isPaused;
	public GameObject pauseMenu;
	public string newLevel;

	/// <summary>
	///     <para> Start </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	private void Start()
	{
		pauseMenu.SetActive(false);
	}

	/// <summary>
	///     <para> Update </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	private void Update()
	{
		if (InputManager.GetButtonDown("PauseMenu")) isPaused = !isPaused;

		if (isPaused)
			Paused();
		else
			Resume();
	}

	/// <summary>
	///     <para> OnResume </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public void OnResume()
	{
		isPaused = false;
	}

	/// <summary>
	///     <para> Resume </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	private void Resume()
	{
		pauseMenu.SetActive(false);
		Time.timeScale = 1f;
	}

	/// <summary>
	///     <para> Paused </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	private void Paused()
	{
		pauseMenu.SetActive(true);
		Time.timeScale = 0f;
	}

	/// <summary>
	///     <para> LoadScene </para>
	/// </summary>
	public void LoadScene()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(newLevel);
	}
}