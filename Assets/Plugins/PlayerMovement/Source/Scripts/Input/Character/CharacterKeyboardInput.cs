using UnityEngine;

namespace ZeoFlow.PlayerMovement
{
	//This character movement input class is an example of how to get input from a keyboard to control the character;
	public class CharacterKeyboardInput : CharacterInput
	{
		public string horizontalInputAxis = "Horizontal";
		public string verticalInputAxis = "Vertical";
		public KeyCode jumpKey = KeyCode.Space;
		public KeyCode runKey = KeyCode.LeftShift;

		//If this is enabled, Unity's internal input smoothing is bypassed;
		public bool useRawInput = true;

		public override float GetHorizontalMovementInput()
		{
			return useRawInput ? Input.GetAxisRaw(horizontalInputAxis) : Input.GetAxis(horizontalInputAxis);
		}

		public override float GetVerticalMovementInput()
		{
			return useRawInput ? Input.GetAxisRaw(verticalInputAxis) : Input.GetAxis(verticalInputAxis);
		}

		public override bool IsJumpKeyPressed()
		{
			const string keyPrefix = "Controller.Key.Jump";
			return Input.GetKey((KeyCode) PlayerPrefs.GetInt(keyPrefix, (int) jumpKey));
		}

		public override bool IsRunKeyPressed()
		{
			const string keyPrefix = "Controller.Key.Run";
			return Input.GetKey((KeyCode) PlayerPrefs.GetInt(keyPrefix, (int) runKey));
		}
	}
}