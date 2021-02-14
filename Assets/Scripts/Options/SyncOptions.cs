using Options.Filter;
using UnityEngine;

namespace Options
{
	[AddComponentMenu("Options/Options Synchronizer")]
	public class SyncOptions : MonoBehaviour
	{
		public BrightnessEffect brightnessEffect;
		private bool _isBrightnessEffectNotNull;

		private void Start()
		{
			_isBrightnessEffectNotNull = brightnessEffect != null;
		}

		private void Update()
		{
			if (_isBrightnessEffectNotNull)
			{
				brightnessEffect.SetBrightness(PlayerPrefs.GetFloat(Constants.Options.Brightness, 1.0f));
				brightnessEffect.SetContrast(PlayerPrefs.GetFloat(Constants.Options.Contrast, 1.0f));
			}
			// PlayerPrefs.GetFloat(Constants.Options.Sound, 1.0f)
		}

	}
}