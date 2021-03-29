using UnityEngine;

namespace Options.Filter
{
	/// <summary>
	///     <para> ImageEffectBase </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("")]
	public class ImageEffectBase : MonoBehaviour
	{
        /// Provides a shader property that is set in the inspector
        /// and a material instantiated from the shader
        public Shader shader;

		private Material m_Material;
		
		/// <summary>
		///     <para> Material </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		protected Material material
		{
			get
			{
				if (m_Material == null)
				{
					m_Material = new Material(shader);
					m_Material.hideFlags = HideFlags.HideAndDontSave;
				}

				return m_Material;
			}
		}

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		protected virtual void Start()
		{
			// Disable the image effect if the shader can't
			// run on the users graphics card
			if (!shader || !shader.isSupported)
				enabled = false;
		}
		
		/// <summary>
		///     <para> OnDisable </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		protected virtual void OnDisable()
		{
			if (m_Material) DestroyImmediate(m_Material);
		}
	}
}