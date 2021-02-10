using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

// ReSharper disable once CheckNamespace
namespace ZeoFlow.Outline
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("OutlineObject/OutlineObject Camera Render")]
	public class OutlineRender : MonoBehaviour
	{
		[Range(1.0f, 6.0f)] public float lineThickness = 1.25f;
		[Range(0, 10)] public float lineIntensity = .5f;
		[Range(0, 1)] public float fillAmount = 0.2f;

		public Color[] lineColors;

		public bool additiveRendering;

		public bool backfaceCulling = true;

		public Color fillColor = Color.blue;
		public bool useFillColor;

		[Header("These settings can affect performance!")]
		public bool cornerOutlines;

		public bool addLinesBetweenColors;

		[Header("Advanced settings")] public bool scaleWithScreenSize = true;
		[Range(0.0f, 1.0f)] public float alphaCutoff = .5f;
		public bool flipY;
		public Camera sourceCamera;
		public bool autoEnableOutlines;

		[HideInInspector] public Camera outlineCamera;
		[HideInInspector] public Material outlineShaderMaterial;
		[HideInInspector] public RenderTexture renderTexture;
		[HideInInspector] public RenderTexture extraRenderTexture;

		private readonly LinkedSet<OutlineObject> _outlines = new LinkedSet<OutlineObject>();

		private CommandBuffer _commandBuffer;

		private readonly List<Material> _materialBuffer = new List<Material>();
		private List<Material> _outlineMaterials = new List<Material>();
		private Shader _outlineBufferShader;
		private Material _outlineEraseMaterial;
		private Shader _outlineShader;
		private bool _renderTheNextFrame;
		public static OutlineRender Instance { get; private set; }

		private void Awake()
		{
			if (Instance != null)
			{
				Destroy(this);
				throw new Exception("you can only have one outline camera in the scene");
			}

			Instance = this;
		}

		private void Start()
		{
			CreateMaterialsIfNeeded();
			UpdateMaterialsPublicProperties();

			if (sourceCamera == null)
			{
				sourceCamera = GetComponent<Camera>();

				if (sourceCamera == null)
					sourceCamera = Camera.main;
			}

			if (outlineCamera == null)
			{
				foreach (var c in GetComponentsInChildren<Camera>())
					if (c.name == "OutlineObject Camera")
					{
						outlineCamera = c;
						c.enabled = false;

						break;
					}

				if (outlineCamera == null)
				{
					var cameraGameObject = new GameObject("OutlineObject Camera");
					cameraGameObject.transform.parent = sourceCamera.transform;
					outlineCamera = cameraGameObject.AddComponent<Camera>();
					outlineCamera.enabled = false;
				}
			}

			if (renderTexture != null)
				renderTexture.Release();
			if (extraRenderTexture != null)
				renderTexture.Release();
			if (!(sourceCamera is null))
			{
				renderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16,
					RenderTextureFormat.Default);
				extraRenderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16,
					RenderTextureFormat.Default);
			}

			UpdateOutlineCameraFromSource();

			_commandBuffer = new CommandBuffer();
			outlineCamera.AddCommandBuffer(CameraEvent.BeforeImageEffects, _commandBuffer);
		}

		private void OnEnable()
		{
			var o = FindObjectsOfType<OutlineObject>();
			if (autoEnableOutlines)
				foreach (var oL in o)
				{
					oL.enabled = true;
				}
			else
				foreach (var oL in o)
				{
					if (!_outlines.Contains(oL))
						_outlines.Add(oL);
					oL.enabled = false;
				}
		}

		private void OnDestroy()
		{
			if (renderTexture != null)
				renderTexture.Release();
			if (extraRenderTexture != null)
				extraRenderTexture.Release();
			DestroyMaterials();
		}

		public void OnPreRender()
		{
			if (_commandBuffer == null)
				return;

			// The first frame during which there are no outlines, we still need to render 
			// to clear out any outlines that were being rendered on the previous frame
			if (_outlines.Count == 0)
			{
				if (!_renderTheNextFrame)
					return;

				_renderTheNextFrame = false;
			}
			else
			{
				_renderTheNextFrame = true;
			}

			CreateMaterialsIfNeeded();

			if (renderTexture == null || renderTexture.width != sourceCamera.pixelWidth ||
				renderTexture.height != sourceCamera.pixelHeight)
			{
				if (renderTexture != null)
					renderTexture.Release();
				if (extraRenderTexture != null)
					renderTexture.Release();
				renderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16,
					RenderTextureFormat.Default);
				extraRenderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16,
					RenderTextureFormat.Default);
				outlineCamera.targetTexture = renderTexture;
			}

			UpdateMaterialsPublicProperties();
			UpdateOutlineCameraFromSource();
			outlineCamera.targetTexture = renderTexture;
			_commandBuffer.SetRenderTarget(renderTexture);

			_commandBuffer.Clear();

			foreach (var outline in _outlines)
			{
				LayerMask l = sourceCamera.cullingMask;

				if (outline == null || l != (l | (1 << outline.gameObject.layer))) continue;

				if (!outline.enabled) continue;

				for (var v = 0; v < outline.SharedMaterials.Length; v++)
				{
					Material m = null;

					if (outline.SharedMaterials[v].HasProperty("_MainTex") &&
						outline.SharedMaterials[v].mainTexture != null && outline.SharedMaterials[v])
					{
						foreach (var g in _materialBuffer.Where(g =>
							g.mainTexture == outline.SharedMaterials[v].mainTexture))
							switch (outline.eraseRenderer)
							{
								case true when g.color == _outlineEraseMaterial.color:
								case false when g.color == GetMaterialFromID(outline.color).color:
									m = g;
									break;
							}

						if (m == null)
						{
							m = outline.eraseRenderer
								? new Material(_outlineEraseMaterial)
								: new Material(GetMaterialFromID(outline.color));

							m.mainTexture = outline.SharedMaterials[v].mainTexture;
							_materialBuffer.Add(m);
						}
					}
					else
					{
						m = outline.eraseRenderer ? _outlineEraseMaterial : GetMaterialFromID(outline.color);
					}

					if (backfaceCulling)
						// ReSharper disable once Unity.PreferAddressByIdToGraphicsParams
						m.SetInt("_Culling", (int) CullMode.Back);
					else
						// ReSharper disable once Unity.PreferAddressByIdToGraphicsParams
						m.SetInt("_Culling", (int) CullMode.Off);

					var mL = outline.MeshFilter;
					var sMr = outline.SkinnedMeshRenderer;
					var sR = outline.SpriteRenderer;
					if (mL)
					{
						if (mL.sharedMesh == null) continue;
						if (v < mL.sharedMesh.subMeshCount)
							_commandBuffer.DrawRenderer(outline.Renderer, m, v, 0);
					}
					else if (sMr)
					{
						if (sMr.sharedMesh == null) continue;
						if (v < sMr.sharedMesh.subMeshCount)
							_commandBuffer.DrawRenderer(outline.Renderer, m, v, 0);
					}
					else if (sR)
					{
						_commandBuffer.DrawRenderer(outline.Renderer, m, v, 0);
					}
				}
			}

			outlineCamera.Render();
		}

		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (outlineShaderMaterial == null) return;

			// ReSharper disable once Unity.PreferAddressByIdToGraphicsParams
			outlineShaderMaterial.SetTexture("_OutlineSource", renderTexture);

			if (addLinesBetweenColors)
				Graphics.Blit(source, extraRenderTexture, outlineShaderMaterial, 0);
			// outlineShaderMaterial.SetTexture("_OutlineSource", extraRenderTexture);

			Graphics.Blit(source, destination, outlineShaderMaterial, 1);
		}

		private Material GetMaterialFromID(int id)
		{
			return _outlineMaterials[id];
		}

		[SuppressMessage("ReSharper", "Unity.PreferAddressByIdToGraphicsParams")]
		[SuppressMessage("ReSharper", "StringLiteralTypo")]
		private Material CreateMaterial(Color emissionColor)
		{
			var m = new Material(_outlineBufferShader);
			m.SetColor("_Color", emissionColor);
			m.SetInt("_SrcBlend", (int) BlendMode.SrcAlpha);
			m.SetInt("_DstBlend", (int) BlendMode.OneMinusSrcAlpha);
			m.SetInt("_ZWrite", 0);
			m.DisableKeyword("_ALPHATEST_ON");
			m.EnableKeyword("_ALPHABLEND_ON");
			m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			m.renderQueue = 3000;
			return m;
		}

		private void CreateMaterialsIfNeeded()
		{
			if (_outlineShader == null)
				_outlineShader = Resources.Load<Shader>("Shaders/OutlineShader");
			if (_outlineBufferShader == null)
				_outlineBufferShader = Resources.Load<Shader>("Shaders/OutlineBufferShader");

			if (outlineShaderMaterial == null)
			{
				outlineShaderMaterial = new Material(_outlineShader) {hideFlags = HideFlags.HideAndDontSave};
				UpdateMaterialsPublicProperties();
			}

			if (_outlineEraseMaterial == null)
				_outlineEraseMaterial = CreateMaterial(new Color(0, 0, 0, 0));

			var colorIndex = 0;
			foreach (var color in lineColors)
			{
				if (_outlineMaterials.Count() == colorIndex)
				{
					_outlineMaterials.Add(CreateMaterial(new Color(color.r, color.g, color.b, color.a)));
				}

				colorIndex++;
			}
		}

		private void DestroyMaterials()
		{
			foreach (var m in _materialBuffer)
				DestroyImmediate(m);
			_materialBuffer.Clear();
			DestroyImmediate(outlineShaderMaterial);
			DestroyImmediate(_outlineEraseMaterial);
			foreach (var outlineMaterial in _outlineMaterials) DestroyImmediate(outlineMaterial);
			_outlineShader = null;
			_outlineBufferShader = null;
			outlineShaderMaterial = null;
			_outlineEraseMaterial = null;
			_outlineMaterials = new List<Material>();
		}

		[SuppressMessage("ReSharper", "Unity.PreferAddressByIdToGraphicsParams")]
		private void UpdateMaterialsPublicProperties()
		{
			if (!outlineShaderMaterial) return;

			float scalingFactor = 1;
			if (scaleWithScreenSize)
				// If Screen.height gets bigger, outlines gets thicker
				scalingFactor = Screen.height / 360.0f;

			// If scaling is too small (height less than 360 pixels), make sure you still render the outlines, but render them with 1 thickness
			if (scaleWithScreenSize && scalingFactor < 1)
			{
				if (XRSettings.isDeviceActive &&
					sourceCamera.stereoTargetEye != StereoTargetEyeMask.None)
				{
					outlineShaderMaterial.SetFloat("_LineThicknessX",
						1 / 1000.0f * (1.0f / XRSettings.eyeTextureWidth) * 1000.0f);
					outlineShaderMaterial.SetFloat("_LineThicknessY",
						1 / 1000.0f * (1.0f / XRSettings.eyeTextureHeight) * 1000.0f);
				}
				else
				{
					outlineShaderMaterial.SetFloat("_LineThicknessX",
						1 / 1000.0f * (1.0f / Screen.width) * 1000.0f);
					outlineShaderMaterial.SetFloat("_LineThicknessY",
						1 / 1000.0f * (1.0f / Screen.height) * 1000.0f);
				}
			}
			else
			{
				if (XRSettings.isDeviceActive &&
					sourceCamera.stereoTargetEye != StereoTargetEyeMask.None)
				{
					outlineShaderMaterial.SetFloat("_LineThicknessX",
						scalingFactor * (lineThickness / 1000.0f) *
						(1.0f / XRSettings.eyeTextureWidth) * 1000.0f);
					outlineShaderMaterial.SetFloat("_LineThicknessY",
						scalingFactor * (lineThickness / 1000.0f) *
						(1.0f / XRSettings.eyeTextureHeight) * 1000.0f);
				}
				else
				{
					outlineShaderMaterial.SetFloat("_LineThicknessX",
						scalingFactor * (lineThickness / 1000.0f) * (1.0f / Screen.width) * 1000.0f);
					outlineShaderMaterial.SetFloat("_LineThicknessY",
						scalingFactor * (lineThickness / 1000.0f) * (1.0f / Screen.height) * 1000.0f);
				}
			}

			outlineShaderMaterial.SetFloat("_LineIntensity", lineIntensity);
			outlineShaderMaterial.SetFloat("_FillAmount", fillAmount);
			outlineShaderMaterial.SetColor("_FillColor", fillColor);
			outlineShaderMaterial.SetFloat("_UseFillColor", useFillColor ? 1 : 0);
			var lineColorIndex = 1;
			foreach (var color in lineColors)
			{
				outlineShaderMaterial.SetColor("_LineColor" + lineColorIndex, color * color);
				lineColorIndex++;
			}

			outlineShaderMaterial.SetInt("_FlipY", flipY ? 1 : 0);
			outlineShaderMaterial.SetInt("_Dark", !additiveRendering ? 1 : 0);
			outlineShaderMaterial.SetInt("_CornerOutlines", cornerOutlines ? 1 : 0);

			Shader.SetGlobalFloat("_OutlineAlphaCutoff", alphaCutoff);
		}

		private void UpdateOutlineCameraFromSource()
		{
			outlineCamera.CopyFrom(sourceCamera);
			outlineCamera.renderingPath = RenderingPath.Forward;
			outlineCamera.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
			outlineCamera.clearFlags = CameraClearFlags.SolidColor;
			outlineCamera.rect = new Rect(0, 0, 1, 1);
			outlineCamera.cullingMask = 0;
			outlineCamera.targetTexture = renderTexture;
			outlineCamera.enabled = false;
			#if UNITY_5_6_OR_NEWER
			outlineCamera.allowHDR = false;
			#else
			outlineCamera.hdr = false;
			#endif
		}

		public void AddOutline(OutlineObject outline)
		{
			_outlines.Add(outline);
		}

		public void RemoveOutline(OutlineObject outline)
		{
			_outlines.Remove(outline);
		}
	}
}