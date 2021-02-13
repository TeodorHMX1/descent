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
	public GameObject optionsMenu;
	public string newLevel;
	public MouseCursorLock mouseCursorLock;

	/// <summary>
	///     <para> Start </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	private void Start()
	{
		pauseMenu.SetActive(false);
		optionsMenu.SetActive(false);
	}

	/// <summary>
	///     <para> Update </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	private void Update()
	{
		if (InputManager.GetButtonDown("PauseMenu")) isPaused = !isPaused;
		mouseCursorLock.SetPaused(isPaused);
		
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
		pauseMenu.SetActive(false);
	}

	/// <summary>
	///     <para> OnOptions </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public void OnOptions()
	{
		pauseMenu.SetActive(false);
		optionsMenu.SetActive(true);
	}

	/// <summary>
	///     <para> OnOptionsClosed </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public void OnOptionsClosed()
	{
		pauseMenu.SetActive(true);
		optionsMenu.SetActive(false);
	}

	/// <summary>
	///     <para> Resume </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	private void Resume()
	{
		if (pauseMenu.activeSelf)
		{
			pauseMenu.SetActive(false);
		}
		Time.timeScale = 1f;
	}

	/// <summary>
	///     <para> Paused </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	private void Paused()
	{
		if (!optionsMenu.activeSelf)
		{
			pauseMenu.SetActive(true);
		}
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