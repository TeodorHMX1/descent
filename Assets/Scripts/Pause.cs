using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     <para> Pause </para>
///     <author> @TeodorHMX1 </author>
/// </summary>
public class Pause : MonoBehaviour
{
	public bool isPaused;
	public GameObject pauseMenu;
	public GameObject canvasOverride;
	public string newLevel;
	private bool _isCanvasOverrideNotNull;

	/// <summary>
	///     <para> Start </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	private void Start()
	{
		_isCanvasOverrideNotNull = canvasOverride != null;
		pauseMenu.SetActive(false);
	}

	/// <summary>
	///     <para> Update </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			isPaused = !isPaused;
			if (_isCanvasOverrideNotNull) canvasOverride.SetActive(isPaused);
		}

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
		if (_isCanvasOverrideNotNull) canvasOverride.SetActive(isPaused);
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