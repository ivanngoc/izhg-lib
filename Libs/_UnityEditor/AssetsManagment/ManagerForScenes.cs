#if UNITY_EDITOR


using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace IziHardGames.Libs.UnityEditor.AssetsManagment
{
	public static class ManagerForScenes
	{
		public static void EnsureSceneOpenedAdditive(params SceneAsset[] scenes)
		{
			for (int i = 0; i < scenes.Length; i++)
			{
				EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(scenes[i]), OpenSceneMode.Additive);
			}
		}
	}
}
#endif
