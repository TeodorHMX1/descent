using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Filters
{
	/// <summary>
	///     <para> FilterParanoia </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	[ExecuteInEditMode]
	[AddComponentMenu("Filters/Paranoia Filter")]
	public class FilterParanoia : MonoBehaviour
	{
		#region Properties

		/// <summary>
		///     <para> material type object </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private Material Material
		{
			get
			{
				if (_scMaterial == null) _scMaterial = new Material(scShader) {hideFlags = HideFlags.HideAndDontSave};

				return _scMaterial;
			}
		}

		#endregion

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Start()
		{
			scShader = Shader.Find("Filter_Paranoia");
			#pragma warning disable 618
			if (SystemInfo.supportsImageEffects) return;

			enabled = false;
		}

		#region Variables

		public Shader scShader;
		private float _timeX = 1.0f;

		private Material _scMaterial;
		[Range(0.0f, 10.0f)] public float brightness = 2.0f;
		[Range(0.0f, 10.0f)] public float saturation = 1.5f;
		[Range(0.0f, 10.0f)] public float contrast = 1.5f;

		#endregion

		/// <summary>
		///     <para> OnRenderImage </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="sourceTexture"></param>
		/// <param name="destTexture"></param>
		[SuppressMessage("ReSharper", "Unity.PreferAddressByIdToGraphicsParams")]
		private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
		{
			if (scShader != null)
			{
				_timeX += Time.deltaTime;
				if (_timeX > 100) _timeX = 0;
				Material.SetFloat("_Brightness", brightness);
				Material.SetFloat("_Saturation", saturation);
				Material.SetFloat("_Contrast", contrast);
				Material.SetFloat("_TimeX", _timeX);
				Material.SetVector("_ScreenResolution",
					new Vector4(sourceTexture.width, sourceTexture.height, 0.0f, 0.0f));
				Graphics.Blit(sourceTexture, destTexture, Material);
			}
			else
			{
				Graphics.Blit(sourceTexture, destTexture);
			}
		}

		/// <summary>
		///     <para> Update </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Update()
		{
			#if UNITY_EDITOR
			if (Application.isPlaying != true) scShader = Shader.Find("Filter_Paranoia");
			#endif
		}

		/// <summary>
		///     <para> OnDisable </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void OnDisable()
		{
			if (_scMaterial) DestroyImmediate(_scMaterial);
		}
	}
}