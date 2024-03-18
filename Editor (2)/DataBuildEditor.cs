#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace IziHardGames.ProjectManagment.Build.EditorGUI
{

	[UnityEditor.CustomEditor(typeof(DataBuild))]
	public class DataBuildEditor : Editor
	{
		#region Unity Message

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			GUI.enabled = false;
			EditorGUILayout.ObjectField("Script Editor:", MonoScript.FromScriptableObject(this), typeof(DataBuildEditor), true);
			GUI.enabled = true;

			DataBuild instance = target as DataBuild;

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button($"Подставить"))
			{
				instance.ReplaceBuildOptions();
			}

			if (GUILayout.Button($"Скопировать"))
			{
				instance.CopyFrom();
			}

			EditorGUILayout.EndHorizontal();
		}
		#endregion
	}
}
#endif
