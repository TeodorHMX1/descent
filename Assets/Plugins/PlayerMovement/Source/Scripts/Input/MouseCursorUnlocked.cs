using UnityEngine;

namespace ZeoFlow.PlayerMovement
{
	//This script provides simple mouse cursor locking functionality;
	public class MouseCursorUnlocked : MonoBehaviour
	{
		//Whether to lock the mouse cursor at the start of the game;
		public bool lockCursorAtGameStart;

		//Key used to unlock mouse cursor;
		public bool useKeyToUnlock;

		//Key used to unlock mouse cursor;
		public KeyCode unlockKeyCode = KeyCode.Escape;

		//Start;
		private void Start()
		{
			if (lockCursorAtGameStart)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		//Update;
		private void Update()
		{
			if (useKeyToUnlock)
			{
				if (Input.GetKeyDown(unlockKeyCode))
				{
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
				}
			}
			else
			{
				
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}
	}
}