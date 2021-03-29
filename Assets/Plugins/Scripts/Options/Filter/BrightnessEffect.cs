using UnityEngine;

namespace Options.Filter
{
	/// <summary>
	///     <para> BrightnessEffect </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Custom/Brightness Effect")]
	public class BrightnessEffect : ImageEffectBase
	{
		[Range(0f, 2f)] public float _Brightness = 1f;
		[Range(0f, 2f)] public float _Contrast = 1f;

		/// <summary>
		///     <para> OnRenderImage - Called by camera to apply image effect </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="source"></param>
		/// <param name="destination"></param>
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			material.SetFloat("_Brightness", _Brightness);
			material.SetFloat("_Contrast", _Contrast);
			Graphics.Blit(source, destination, material);
		}

		/// <summary>
		///     <para> SetBrightness </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="value"></param>
		public void SetBrightness(float value)
		{
			_Brightness = value;
		}

		/// <summary>
		///     <para> SetContrast </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="value"></param>
		public void SetContrast(float value)
		{
			_Contrast = value;
		}
	}
}