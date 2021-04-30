using Lovatto.SceneLoader;
using UnityEditor;
using UnityEngine;

namespace Walls
{
	/// <summary>
	///     <para> WallTeleporter </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class WallTeleporter : MonoBehaviour
	{
		public bool byName = true;
		public string sceneName;
		public int sceneID = 0;
		
		private Collider _collider;
		private bool _wasCollided;

		/// <summary>
		///     <para> Start </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void Start()
		{
			_collider = GetComponent<Collider>();
		}

		/// <summary>
		///     <para> OnCollisionEnter </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="collision"></param>
		private void OnCollisionEnter(Collision collision)
		{
			Physics.IgnoreCollision(collision.collider, _collider);
		}

		/// <summary>
		///     <para> OnTriggerEnter </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		/// <param name="collision"></param>
		private void OnTriggerEnter(Collider collision)
		{
			if (collision.gameObject.name != "Player") return;
			_wasCollided = !_wasCollided;
			LoadScene();
		}

        /// <summary>
        ///     <para> LoadScene </para>
        ///     <author> @TeodorHMX1 </author>
        /// </summary>
		private void LoadScene()
        {
			if (byName)
			{
				if (string.IsNullOrEmpty(sceneName)) return;
				bl_SceneLoaderUtils.GetLoader.LoadLevel(sceneName);
			}
			else
			{
				sceneName = bl_SceneLoaderManager.Instance.List[sceneID].SceneName;
				bl_SceneLoaderUtils.GetLoader.LoadLevel(sceneName);
			}
		}
	}
	
	

#if UNITY_EDITOR

	/// <summary>
	///     <para> WallTeleporterEditor </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	[CustomEditor(typeof(WallTeleporter))]
	public class WallTeleporterEditor : Editor
	{
		WallTeleporter script;
		string[] sceneNames;
		
		/// <summary>
		///     <para> OnEnable </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		private void OnEnable()
		{
			script = (WallTeleporter)target;
			sceneNames = bl_SceneLoaderManager.Instance.GetSceneNames();
		}

		/// <summary>
		///     <para> OnInspectorGUI </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.BeginVertical("box");
			{
				script.byName = EditorGUILayout.ToggleLeft("By Name", script.byName, EditorStyles.toolbarButton);
				GUILayout.Space(2);
				if (script.byName)
				{
					script.sceneName = EditorGUILayout.TextField("Scene Name", script.sceneName);
				}
				else
					script.sceneID = EditorGUILayout.Popup("Scene", script.sceneID, sceneNames, EditorStyles.toolbarPopup);
			}
			GUILayout.Space(2);
			EditorGUILayout.EndVertical();

			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
				EditorUtility.SetDirty(target);
			}
		}
	}
#endif
}