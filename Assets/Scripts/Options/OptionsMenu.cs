using UnityEngine;
using UnityEngine.UI;

namespace Options
{
	/// <summary>
	///     <para> OptionsMenu </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	[AddComponentMenu("Options/Options Menu")]
	public class OptionsMenu : MonoBehaviour
	{
	
		[Header("Content Holder")]
		public GameObject graphicsContent;
		public GameObject keyboardControllerContent;

		[Header("Sliders")]
		[SerializeField] private Slider sensitivityVertical;
		[SerializeField] private Slider sensitivityHorizontal;
		[SerializeField] private Slider brightness;
		[SerializeField] private Slider contrast;
		[SerializeField] private Slider sound;

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Start()
		{
			if (graphicsContent != null)
			{
				graphicsContent.SetActive(true);
			}
			if (keyboardControllerContent != null)
			{
				keyboardControllerContent.SetActive(false);
			}
			
			if (sensitivityVertical != null)
			{
				sensitivityVertical.value = PlayerPrefs.GetFloat(Constants.Options.SensitivityVertical, 1.0f);
			}
			if (sensitivityHorizontal != null)
			{
				sensitivityHorizontal.value = PlayerPrefs.GetFloat(Constants.Options.SensitivityVertical, 1.0f);
			}
			if (brightness != null)
			{
				brightness.value = PlayerPrefs.GetFloat(Constants.Options.Brightness, 1.0f);
			}
			if (contrast != null)
			{
				contrast.value = PlayerPrefs.GetFloat(Constants.Options.Contrast, 1.0f);
			}
			if (sound != null)
			{
				sound.value = PlayerPrefs.GetFloat(Constants.Options.Sound, 1.0f);
			}
		}

		/// <summary>
		///     <para> OnGraphicsTabClicked </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void OnGraphicsTabClicked()
		{
			if (graphicsContent != null)
			{
				graphicsContent.SetActive(true);
			}
			if (keyboardControllerContent != null)
			{
				keyboardControllerContent.SetActive(false);
			}
		}

		/// <summary>
		///     <para> OnControllerTabClicked </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void OnControllerTabClicked()
		{
			if (graphicsContent != null)
			{
				graphicsContent.SetActive(false);
			}
			if (keyboardControllerContent != null)
			{
				keyboardControllerContent.SetActive(true);
			}
		}

		/// <summary>
		///     <para> IOnSensitivityVertical </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void IOnSensitivityVertical()
		{
			PlayerPrefs.SetFloat(Constants.Options.SensitivityVertical, sensitivityVertical.value);
		}

		/// <summary>
		///     <para> IOnSensitivityHorizontal </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void IOnSensitivityHorizontal()
		{
			PlayerPrefs.SetFloat(Constants.Options.SensitivityHorizontal, sensitivityHorizontal.value);
		}

		/// <summary>
		///     <para> IOnBrightness </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void IOnBrightness()
		{
			PlayerPrefs.SetFloat(Constants.Options.Brightness, brightness.value);
		}

		/// <summary>
		///     <para> IOnContrast </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void IOnContrast()
		{
			PlayerPrefs.SetFloat(Constants.Options.Contrast, contrast.value);
		}

		/// <summary>
		///     <para> IOnSound </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void IOnSound()
		{
			PlayerPrefs.SetFloat(Constants.Options.Sound, contrast.value);
		}

		/// <summary>
		///     <para> IOnDefaultGraphics </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public void IOnDefaultGraphics()
		{
			PlayerPrefs.SetFloat(Constants.Options.SensitivityVertical, 1);
			PlayerPrefs.SetFloat(Constants.Options.SensitivityHorizontal, 1);
			PlayerPrefs.SetFloat(Constants.Options.Brightness, 1);
			PlayerPrefs.SetFloat(Constants.Options.Contrast, 1);
			sensitivityVertical.value = PlayerPrefs.GetFloat(Constants.Options.SensitivityVertical, 1.0f);
			sensitivityHorizontal.value = PlayerPrefs.GetFloat(Constants.Options.SensitivityVertical, 1.0f);
			brightness.value = PlayerPrefs.GetFloat(Constants.Options.Brightness, 1.0f);
			contrast.value = PlayerPrefs.GetFloat(Constants.Options.Contrast, 1.0f);
		}
	}
}