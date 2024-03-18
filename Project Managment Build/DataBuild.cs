#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace IziHardGames.ProjectManagment.Build
{
	[CreateAssetMenu(menuName = "IziHardGames/Develop/ProjectManagment/Build/BuildPreset", fileName = "VariantBuild")]
	public class DataBuild : ScriptableObject
	{
		[SerializeField] public List<SceneAsset> sceneAssets;

		#region Unity Message


		#endregion

		public void CopyFrom()
		{
			EditorBuildSettingsScene[] editorBuildSettingsScene = EditorBuildSettings.scenes;

			sceneAssets.Clear();

			for (int i = 0; i < editorBuildSettingsScene.Length; i++)
			{
				var sceneSetting = editorBuildSettingsScene[i];

				SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath(sceneSetting.path, typeof(SceneAsset)) as SceneAsset;

				sceneAssets.Add(sceneAsset);
			}
		}

		public void ReplaceBuildOptions()
		{
			EditorBuildSettingsScene[] editorBuildSettingsScene = new EditorBuildSettingsScene[sceneAssets.Count];

			for (int i = 0; i < sceneAssets.Count; i++)
			{
				editorBuildSettingsScene[i] = new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(sceneAssets[i]), true);
			}

			EditorBuildSettings.scenes = editorBuildSettingsScene;
		}
	}
}
#endif
