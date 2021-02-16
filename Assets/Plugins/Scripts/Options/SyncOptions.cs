using Options.Filter;
using UnityEngine;

namespace Options
{
	/// <summary>
	///     <para> SyncOptions </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	[AddComponentMenu("Options/Options Synchronizer")]
	public class SyncOptions : MonoBehaviour
	{
		public BrightnessEffect brightnessEffect;
		private bool _isBrightnessEffectNotNull;

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Start()
		{
			_isBrightnessEffectNotNull = brightnessEffect != null;
		}

		/// <summary>
		///     <para> Update </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
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