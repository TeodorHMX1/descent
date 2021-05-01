#region [Copyright (c) 2021 ZeoFlow S.R.L.]

//	Distributed under the terms of an MIT-style license:
//
//	The MIT License
//
//	Copyright (c) 2021 ZeoFlow S.R.L.
//
//	Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
//	and associated documentation files (the "Software"), to deal in the Software without restriction, 
//	including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
//	and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
//	subject to the following conditions:
//
//	The above copyright notice and this permission notice shall be included in all copies or substantial 
//	portions of the Software.
//
//	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//	INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
//	PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
//	FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//	ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

using UnityEngine;

namespace ZeoFlow
{
	public partial class InputManager : MonoBehaviour
	{
		public static Vector3 acceleration => Input.acceleration;
		public static int accelerationEventCount => Input.accelerationEventCount;
		public static AccelerationEvent[] accelerationEvents => Input.accelerationEvents;
		public static bool anyKey => Input.anyKey;
		public static bool anyKeyDown => Input.anyKeyDown;
		public static Compass compass => Input.compass;
		public static string compositionString => Input.compositionString;
		public static DeviceOrientation deviceOrientation => Input.deviceOrientation;
		public static Gyroscope gyro => Input.gyro;
		public static bool imeIsSelected => Input.imeIsSelected;
		public static string inputString => Input.inputString;
		public static LocationService location => Input.location;
		public static Vector2 mousePosition => Input.mousePosition;
		public static bool mousePresent => Input.mousePresent;
		public static Vector2 mouseScrollDelta => Input.mouseScrollDelta;
		public static bool touchSupported => Input.touchSupported;
		public static int touchCount => Input.touchCount;
		public static Touch[] touches => Input.touches;

		public static bool compensateSensors
		{
			get => Input.compensateSensors;
			set => Input.compensateSensors = value;
		}

		public static Vector2 compositionCursorPos
		{
			get => Input.compositionCursorPos;
			set => Input.compositionCursorPos = value;
		}

		public static IMECompositionMode imeCompositionMode
		{
			get => Input.imeCompositionMode;
			set => Input.imeCompositionMode = value;
		}

		public static bool multiTouchEnabled
		{
			get => Input.multiTouchEnabled;
			set => Input.multiTouchEnabled = value;
		}

		public static AccelerationEvent GetAccelerationEvent(int index)
		{
			return Input.GetAccelerationEvent(index);
		}

		public static float GetAxis(string name, PlayerID playerID = PlayerID.One)
		{
			var action = GetAction(playerID, name);
			if (action != null)
			{
				return action.GetAxis();
			}

			Debug.LogError(string.Format(
				"An axis named \'{0}\' does not exist in the active input configuration for player {1}", name,
				playerID));
			return 0.0f;
		}

		public static float GetAxisRaw(string name, PlayerID playerID = PlayerID.One)
		{
			var action = GetAction(playerID, name);
			if (action != null)
			{
				return action.GetAxisRaw();
			}

			Debug.LogError(string.Format(
				"An axis named \'{0}\' does not exist in the active input configuration for player {1}", name,
				playerID));
			return 0.0f;
		}

		public static bool GetButton(string name, PlayerID playerID = PlayerID.One)
		{
			var action = GetAction(playerID, name);
			if (action != null)
			{
				return action.GetButton();
			}

			Debug.LogError(string.Format(
				"A button named \'{0}\' does not exist in the active input configuration for player {1}", name,
				playerID));
			return false;
		}

		public static bool GetButtonDown(string name, PlayerID playerID = PlayerID.One)
		{
			var action = GetAction(playerID, name);
			if (action != null) return action.GetButtonDown();

			Debug.LogError(string.Format(
				"A button named \'{0}\' does not exist in the active input configuration for player {1}", name,
				playerID));
			return false;
		}

		public static bool GetButtonUp(string name, PlayerID playerID = PlayerID.One)
		{
			var action = GetAction(playerID, name);
			if (action != null)
			{
				return action.GetButtonUp();
			}

			Debug.LogError(string.Format(
				"An button named \'{0}\' does not exist in the active input configuration for player {1}", name,
				playerID));
			return false;
		}

		public static bool GetKey(KeyCode key)
		{
			return Input.GetKey(key);
		}

		public static bool GetKeyDown(KeyCode key)
		{
			return Input.GetKeyDown(key);
		}

		public static bool GetKeyUp(KeyCode key)
		{
			return Input.GetKeyUp(key);
		}

		public static bool GetMouseButton(int index)
		{
			return Input.GetMouseButton(index);
		}

		public static bool GetMouseButtonDown(int index)
		{
			return Input.GetMouseButtonDown(index);
		}

		public static bool GetMouseButtonUp(int index)
		{
			return Input.GetMouseButtonUp(index);
		}

		public static Touch GetTouch(int index)
		{
			return Input.GetTouch(index);
		}

		public static string[] GetJoystickNames()
		{
			return Input.GetJoystickNames();
		}

		public static void ResetInputAxes()
		{
			Input.ResetInputAxes();
		}
	}
}