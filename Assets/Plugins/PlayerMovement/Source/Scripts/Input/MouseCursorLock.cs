using UnityEngine;

namespace ZeoFlow.PlayerMovement
{
	//This script provides simple mouse cursor locking functionality;
	public class MouseCursorLock : MonoBehaviour
	{
		//Whether to lock the mouse cursor at the start of the game;
		public bool lockCursorAtGameStart = true;

		//Key used to unlock mouse cursor;
		public KeyCode unlockKeyCode = KeyCode.Escape;

		//Key used to lock mouse cursor;
		public KeyCode lockKeyCode = KeyCode.Mouse0;

		private bool gameIsPaused = false;

		//Start;
		private void Start()
		{
			if (lockCursorAtGameStart)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}

		//Update;
		private void Update()
		{
			if (InputManager.GetButtonDown("PauseMenu"))
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			else if (!gameIsPaused && Input.GetKeyDown(lockKeyCode))
			{
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
		}

		public void SetPaused(bool isPaused)
		{
			if (gameIsPaused && !isPaused)
			{
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
			gameIsPaused = isPaused;
		}
	}
}