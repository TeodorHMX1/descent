using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bl_KeyOptionsUI : MonoBehaviour
{
	[Header("Settings")] [SerializeField] private bool DetectIfKeyIsUse = true;

	[Header("References")] [SerializeField]
	private GameObject KeyOptionPrefab;

	[SerializeField] private Transform KeyOptionPanel;
	[SerializeField] private GameObject WaitKeyWindowUI;
	[SerializeField] private Text WaitKeyText;
	private readonly List<bl_KeyInfoUI> _cacheKeysInfoUI = new List<bl_KeyInfoUI>();

	private bool _waitForKey;
	private bl_KeyInfo _waitFunctionKey;

	public bool InteractableKey { get; set; } = true;

	/// <summary>
	/// </summary>
	private void Start()
	{
		InstanceKeysUI();
		WaitKeyWindowUI.SetActive(false);
	}

	/// <summary>
	/// </summary>
	private void Update()
	{
		if (_waitForKey && InteractableKey) DetectKey();
	}

	/// <summary>
	/// </summary>
	private void InstanceKeysUI()
	{
		var keys = bl_Input.Instance.allKeys;

		foreach (var key in keys)
		{
			var kui = Instantiate(KeyOptionPrefab, KeyOptionPanel, false);
			kui.GetComponent<bl_KeyInfoUI>().Init(key, this);
			kui.gameObject.name = key.function;
			_cacheKeysInfoUI.Add(kui.GetComponent<bl_KeyInfoUI>());
		}
	}

	/// <summary>
	/// </summary>
	private void ClearList()
	{
		foreach (var kui in _cacheKeysInfoUI) Destroy(kui.gameObject);
		_cacheKeysInfoUI.Clear();
	}

	/// <summary>
	/// </summary>
	private void DetectKey()
	{
		foreach (KeyCode vKey in Enum.GetValues(typeof(KeyCode)))
		{
			if (!Input.GetKey(vKey)) continue;

			if (DetectIfKeyIsUse && bl_Input.Instance.IsKeyUsed(vKey) && vKey != _waitFunctionKey.key)
			{
				WaitKeyText.text =
					$"KEY <b>'{vKey.ToString().ToUpper()}'</b> IS ALREADY USE, \n PLEASE PRESS ANOTHER KEY FOR REPLACE <b>{_waitFunctionKey.description.ToUpper()}</b>";
			}
			else
			{
				KeyDetected(vKey);
				_waitForKey = false;
			}
		}
	}

	/// <summary>
	/// </summary>
	public void SetWaitKeyProcess(bl_KeyInfo info)
	{
		if (_waitForKey)
			return;

		_waitFunctionKey = info;
		_waitForKey = true;
		WaitKeyText.text = $"PRESS A KEY FOR REPLACE <b>{info.description.ToUpper()}</b>";
		WaitKeyWindowUI.SetActive(true);
	}

	/// <summary>
	/// </summary>
	private void KeyDetected(KeyCode keyPressed)
	{
		if (_waitFunctionKey == null) return;
		if (!bl_Input.Instance.SetKey(_waitFunctionKey.function, keyPressed)) return;
		
		ClearList();
		InstanceKeysUI();
		_waitFunctionKey = null;
		WaitKeyWindowUI.SetActive(false);
	}

	/// <summary>
	/// </summary>
	public void CancelWait()
	{
		_waitForKey = false;
		_waitFunctionKey = null;
		WaitKeyWindowUI.SetActive(false);
		InteractableKey = true;
	}
}