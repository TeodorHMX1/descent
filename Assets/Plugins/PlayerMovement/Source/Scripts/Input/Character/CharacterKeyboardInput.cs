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
			return useRawInput ? InputManager.GetAxisRaw(horizontalInputAxis) : InputManager.GetAxis(horizontalInputAxis);
		}

		public override float GetVerticalMovementInput()
		{
			return useRawInput ? InputManager.GetAxisRaw(verticalInputAxis) : InputManager.GetAxis(verticalInputAxis);
		}

		public override bool IsJumpKeyPressed()
		{
			return InputManager.GetButton("Jump");
		}

		public override bool IsRunKeyPressed()
		{
			return InputManager.GetButton("Run");
		}
	}
}