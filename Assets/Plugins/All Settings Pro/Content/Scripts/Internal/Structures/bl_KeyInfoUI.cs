using UnityEngine;
using UnityEngine.UI;

public class bl_KeyInfoUI : MonoBehaviour
{
	[SerializeField] private Text FunctionText;
	[SerializeField] private Text KeyText;

	private bl_KeyInfo _cacheInfo;
	private bl_KeyOptionsUI _keyOptions;

	public void Init(bl_KeyInfo info, bl_KeyOptionsUI koui)
	{
		_cacheInfo = info;
		_keyOptions = koui;
		FunctionText.text = info.description;
		KeyText.text = info.key.ToString();
	}

	public void SetKeyChange()
	{
		_keyOptions.SetWaitKeyProcess(_cacheInfo);
	}
}