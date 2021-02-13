using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class bl_Input : ScriptableObject
{
	private static bl_Input _mInstance;
	public List<bl_KeyInfo> allKeys = new List<bl_KeyInfo>();

	public static bl_Input Instance
	{
		get
		{
			if (_mInstance != null) return _mInstance;
			
			_mInstance = Resources.Load("InputManager", typeof(bl_Input)) as bl_Input;

			return _mInstance;
		}
	}

	/// <summary>
	/// </summary>
	public void InitInput()
	{
		foreach (var key in allKeys)
		{
			var keyPrefix = "Controller." + $"Key.{key.function}";
			key.key = (KeyCode) PlayerPrefs.GetInt(keyPrefix, (int) key.key);
		}
	}

	/// <summary>
	/// </summary>
	public static bool GetKeyDown(string function)
	{
		return Input.GetKeyDown(Instance.GetKeyCode(function));
	}

	/// <summary>
	/// </summary>
	public static bool GetKey(string function)
	{
		return Input.GetKey(Instance.GetKeyCode(function));
	}

	/// <summary>
	/// </summary>
	public static bool GetKeyUp(string function)
	{
		return Input.GetKeyUp(Instance.GetKeyCode(function));
	}

	/// <summary>
	/// </summary>
	public bool SetKey(string function, KeyCode newKey)
	{
		foreach (var key in allKeys.Where(key => key.function == function))
		{
			key.key = newKey;
			var keyPrefix = "Controller." + $"Key.{key.function}";
			PlayerPrefs.SetInt(keyPrefix, (int) newKey);
			return true;
		}
		return false;
	}

	/// <summary>
	/// </summary>
	public KeyCode GetKeyCode(string function)
	{
		return allKeys.Where(key => key.function == function).Select(key => key.key).FirstOrDefault();
	}

	/// <summary>
	/// </summary>
	public bool IsKeyUsed(KeyCode newKey)
	{
		return allKeys.Any(key => key.key == newKey);
	}
}