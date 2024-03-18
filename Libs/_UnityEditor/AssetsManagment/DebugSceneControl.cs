#if UNITY_EDITOR


using IziHardGames.Libs.Engine.Helpers;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IziHardGames.Libs.UnityEditor.AssetsManagment
{
	public static class DebugSceneControl
	{
		public static void PrintAllComponentsAtScene(Scene scene)
		{
			foreach (var item in HelperForComponents.ItterateComponentsAtScene<Component>(scene))
			{
				Debug.Log($"GO:{item.gameObject.name}	Type:{item.GetType().FullName}", item);
			}
		}
		[MenuItem("*IziHardGames/Active Scene/Print Components")]
		public static void PrintAllComponentsAtActiveScene()
		{
			PrintAllComponentsAtScene(SceneManager.GetActiveScene());
		}
		public static T FindComponent<T>(string gameObjectName) where T : Component
		{
			return HelperForComponents.AtSceneFindComponent<T>(SceneManager.GetActiveScene(), gameObjectName);
		}
	}
}
#endif
