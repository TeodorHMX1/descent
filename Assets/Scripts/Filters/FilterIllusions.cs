using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Filters
{
	[ExecuteInEditMode]
	[AddComponentMenu("Filters/Illusions Filter")]
	public class FilterIllusions : MonoBehaviour
	{
		#region Properties

		/**
		 * @object: material
		 * @description: material type object
		 * @author: teodorhmx1
		 */
		private Material Material
		{
			get
			{
				if (_scMaterial != null) return _scMaterial;
				_scMaterial = new Material(scShader) {hideFlags = HideFlags.HideAndDontSave};

				return _scMaterial;
			}
		}

		#endregion

		private void Start()
		{
			scShader = Shader.Find("Filter_Illusions");
			#pragma warning disable 618
			if (!SystemInfo.supportsImageEffects)
				#pragma warning restore 618
				enabled = false;
		}

		private void Update()
		{
			#if UNITY_EDITOR
			if (Application.isPlaying != true) scShader = Shader.Find("Filter_Illusions");
			#endif
		}

		private void OnDisable()
		{
			if (_scMaterial) DestroyImmediate(_scMaterial);
		}

		[SuppressMessage("ReSharper", "Unity.PreferAddressByIdToGraphicsParams")]
		private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
		{
			if (scShader != null)
			{
				_timeX += Time.deltaTime;
				if (_timeX > 100) _timeX = 0;
				Material.SetFloat("_TimeX", _timeX);
				Material.SetFloat("_Value", value);
				Material.SetFloat("_Speed", speed);
				Material.SetFloat("_Distortion", distortion);
				Material.SetFloat("_DistortionWave", distortionWave);
				Material.SetFloat("_Wavy", wavy);
				Material.SetFloat("_Fade", fade);

				Material.SetFloat("_ColoredChange", coloredChange);
				Material.SetFloat("_ChangeRed", changeRed);
				Material.SetFloat("_ChangeGreen", changeGreen);
				Material.SetFloat("_ChangeBlue", changeBlue);

				Material.SetFloat("_Colored", coloredSaturate);
				Material.SetVector("_ScreenResolution",
					new Vector4(sourceTexture.width, sourceTexture.height, 0.0f, 0.0f));
				Graphics.Blit(sourceTexture, destTexture, Material);
			}
			else
			{
				Graphics.Blit(sourceTexture, destTexture);
			}
		}

		#region Variables

		public Shader scShader;
		private float _timeX = 1.0f;

		private Material _scMaterial;
		[HideInInspector] [Range(0, 20)] public float value = 6.0f;
		[Range(0, 10)] public float speed = 1.0f;
		[Range(0, 1)] public float wavy = 1f;
		[Range(0, 1)] public float distortion;
		[Range(0, 1)] public float distortionWave = 1.0f;
		[Range(0, 1)] public float fade = 0.79f;
		[Range(-2, 2)] public float coloredSaturate = 1.0f;
		[Range(-1, 2)] public float coloredChange;
		[Range(-1, 1)] public float changeRed;
		[Range(-1, 1)] public float changeGreen;
		[Range(-1, 1)] public float changeBlue;

		#endregion
	}
}