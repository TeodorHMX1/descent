using System;
using UnityEngine;
using UnityEngine.UI;
using ZeoFlow;

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

		[Header("Cancel Action")]
		public GameObject elementToShow;

		private bool _isElementToShowNotNull;


		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Start()
		{
			_isElementToShowNotNull = elementToShow != null;
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
				sensitivityVertical.onValueChanged.AddListener(IOnSensitivityVertical);
			}
			if (sensitivityHorizontal != null)
			{
				sensitivityHorizontal.value = PlayerPrefs.GetFloat(Constants.Options.SensitivityVertical, 1.0f);
				sensitivityHorizontal.onValueChanged.AddListener(IOnSensitivityHorizontal);
			}
			if (brightness != null)
			{
				brightness.value = PlayerPrefs.GetFloat(Constants.Options.Brightness, 1.0f);
				brightness.onValueChanged.AddListener(IOnBrightness);
			}
			if (contrast != null)
			{
				contrast.value = PlayerPrefs.GetFloat(Constants.Options.Contrast, 1.0f);
				contrast.onValueChanged.AddListener(IOnContrast);
			}
			if (sound != null)
			{
				sound.value = PlayerPrefs.GetFloat(Constants.Options.Sound, 1.0f);
				sound.onValueChanged.AddListener(IOnSound);
			}
		}
		
		/// <summary>
		///     <para> Update </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Update()
		{
			if (InputManager.GetButtonDown("PauseMenu"))
			{
				if (_isElementToShowNotNull)
				{
					elementToShow.SetActive(true);
					gameObject.SetActive(false);
				}
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
		private void IOnSensitivityVertical(float value)
		{
			PlayerPrefs.SetFloat(Constants.Options.SensitivityVertical, value);
		}

		/// <summary>
		///     <para> IOnSensitivityHorizontal </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void IOnSensitivityHorizontal(float value)
		{
			PlayerPrefs.SetFloat(Constants.Options.SensitivityHorizontal, value);
		}

		/// <summary>
		///     <para> IOnBrightness </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void IOnBrightness(float value)
		{
			PlayerPrefs.SetFloat(Constants.Options.Brightness, value);
		}

		/// <summary>
		///     <para> IOnContrast </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void IOnContrast(float value)
		{
			PlayerPrefs.SetFloat(Constants.Options.Contrast, value);
		}

		/// <summary>
		///     <para> IOnSound </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void IOnSound(float value)
		{
			PlayerPrefs.SetFloat(Constants.Options.Sound, value);
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
			PlayerPrefs.SetFloat(Constants.Options.Sound, 1);
			sensitivityVertical.value = PlayerPrefs.GetFloat(Constants.Options.SensitivityVertical, 1.0f);
			sensitivityHorizontal.value = PlayerPrefs.GetFloat(Constants.Options.SensitivityVertical, 1.0f);
			brightness.value = PlayerPrefs.GetFloat(Constants.Options.Brightness, 1.0f);
			contrast.value = PlayerPrefs.GetFloat(Constants.Options.Contrast, 1.0f);
			sound.value = PlayerPrefs.GetFloat(Constants.Options.Sound, 1.0f);
		}
	}
}