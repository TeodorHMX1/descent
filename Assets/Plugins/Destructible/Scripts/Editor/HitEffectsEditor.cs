using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Destructible
{
	[CustomEditor(typeof(HitEffects))]
	public class HitEffectsEditor : Editor
	{
		private Texture _deleteButton;
		private HitEffects _hitEffects;

		public void OnEnable()
		{
			_hitEffects = target as HitEffects;
			_deleteButton = Resources.Load("UI_Textures/delete-16x16") as Texture;

			if (!(_hitEffects is null) && _hitEffects.effects == null)
				_hitEffects.effects = new List<HitEffect>();

			//Default to Everything mask
			if (!(_hitEffects is null) && _hitEffects.effects.Count == 0)
				_hitEffects.effects.Add(new HitEffect() {hitBy = (HitBy) (-1)}); // -1 == "Everything"
		}

		public override void OnInspectorGUI()
		{
			GUIStyle style = new GUIStyle {padding = {top = 2}};

			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("When hit by:", style, GUILayout.Width(100));
			EditorGUILayout.LabelField("Use particle:", style, GUILayout.Width(100));
			EditorGUILayout.EndHorizontal();
			for (int i = 0; i < _hitEffects.effects.Count; i++)
			{
				HitEffect hitEffect = _hitEffects.effects[i];
				EditorGUILayout.BeginHorizontal();
				hitEffect.hitBy = (HitBy) EditorGUILayout.EnumFlagsField(hitEffect.hitBy, GUILayout.Width(100));
				hitEffect.effect =
					EditorGUILayout.ObjectField(hitEffect.effect, typeof(GameObject), false) as GameObject;

				if (GUILayout.Button(_deleteButton, style, GUILayout.Width(16)))
				{
					if (_hitEffects.effects.Count > 1)
						_hitEffects.effects.Remove(hitEffect);
					else
						Debug.Log("Cannot remove the last remaining Hit Effect.");
				}

				EditorGUILayout.EndHorizontal();
			}

			// Add Button
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("", GUILayout.Width(15));
			if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width(30)))
				_hitEffects.effects.Add(new HitEffect() {hitBy = (HitBy) (-1)}); // Add the first available tag.
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Separator();
			EditorGUILayout.EndVertical();

			if (_hitEffects == null || !GUI.changed || Application.isPlaying) return;

			EditorUtility.SetDirty(_hitEffects);
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
		}
	}
}