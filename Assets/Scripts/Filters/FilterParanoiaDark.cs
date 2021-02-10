using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Filters
{
	[ExecuteInEditMode]
	[AddComponentMenu("Filters/Paranoia Filter Dark")]
	public class FilterParanoiaDark : MonoBehaviour
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
				if (_scMaterial == null)
				{
					_scMaterial = new Material(scShader);
					_scMaterial.hideFlags = HideFlags.HideAndDontSave;
				}

				return _scMaterial;
			}
		}

		#endregion

		private void Start()
		{
			scShader = Shader.Find("Filter_ParanoiaDark");
			#pragma warning disable 618
			if (SystemInfo.supportsImageEffects) return;

			enabled = false;
		}

		#region Variables

		public Shader scShader;
		private float _timeX = 1.0f;

		private Material _scMaterial;
		[Range(-5f, 5f)] public float alpha = 1f;
		[Range(0f, 16f)] private const float Colors = 11f;
		[Range(-1f, 1f)] private const float GreenMod = 1f;
		[Range(0f, 10f)] private const float Value4 = 1f;

		#endregion

		[SuppressMessage("ReSharper", "Unity.PreferAddressByIdToGraphicsParams")]
		private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
		{
			if (scShader != null)
			{
				_timeX += Time.deltaTime;
				if (_timeX > 100) _timeX = 0;
				Material.SetFloat("_TimeX", _timeX);
				Material.SetFloat("_Value", alpha);
				Material.SetFloat("_Value2", Colors);
				Material.SetFloat("_Value3", GreenMod);
				Material.SetFloat("_Value4", Value4);
				Material.SetVector("_ScreenResolution",
					new Vector4(sourceTexture.width, sourceTexture.height, 0.0f, 0.0f));
				Graphics.Blit(sourceTexture, destTexture, Material);
			}
			else
			{
				Graphics.Blit(sourceTexture, destTexture);
			}
		}

		private void Update()
		{
			#if UNITY_EDITOR
			if (Application.isPlaying != true) scShader = Shader.Find("Filter_ParanoiaDark");
			#endif
		}

		private void OnDisable()
		{
			if (_scMaterial) DestroyImmediate(_scMaterial);
		}
	}
}