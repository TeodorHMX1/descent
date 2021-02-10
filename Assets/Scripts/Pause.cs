using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
	public bool isPaused;
	public GameObject pauseMenu;
	public GameObject canvasOverride;
	public string newLevel;
	private bool _isCanvasOverrideNotNull;

	private void Start()
	{
		_isCanvasOverrideNotNull = canvasOverride != null;
		pauseMenu.SetActive(false);
	}

	// Update is called once per frame
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			isPaused = !isPaused;
			if (_isCanvasOverrideNotNull)
			{
				canvasOverride.SetActive(isPaused);
			}
		}

		if (isPaused)
		{
			Paused();
		}
		else
		{
			Resume();
		}
	}

	public void OnResume()
	{
		Debug.Log("fgdfh");
		isPaused = false;
		if (_isCanvasOverrideNotNull)
		{
			canvasOverride.SetActive(isPaused);
		}
	}

	private void Resume()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		pauseMenu.SetActive(false);
		Time.timeScale = 1f;
	}

	private void Paused()
	{
		pauseMenu.SetActive(true);
		Time.timeScale = 0f;
	}

	public void LoadScene()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(newLevel);
	}
}