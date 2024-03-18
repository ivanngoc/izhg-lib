#if UNITY_EDITOR
using IziHardGames.Libs.Engine.Helpers;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace IziHardGames.MenuEditor
{
	public class MenuHierarchy
	{
		#region Unity Message

		[MenuItem("*Hierarchy/Selection/Set pickability Selected")]
		public static void MakeSelectedPickAbility()
		{
			SceneVisibilityManager.instance.EnablePicking(Selection.gameObjects, false);
		}
		#endregion
		[MenuItem("*Hierarchy/Selection/Vision/Switch Scene Vision")] // ctrl+shift+a
		public static void SelectionSwitchViewSceneVisible()
		{
			var selection = Selection.gameObjects;

			foreach (var item in selection)
			{
				SceneVisibilityManager.instance.ToggleVisibility(item, true);
			}
		}
		[MenuItem("*Hierarchy/Selection/Sort sibling position Ascending")]
		public static void SelectionSortAlphabeticalAscending()
		{
			var transforms = Selection.gameObjects.Select(x => x.transform);

			var array = transforms.OrderBy(x => x.name).ToArray();

			if (HelperTransforms.IsSiblings(array, 0, array.Length, out Transform parent))
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetSiblingIndex(i);
				}
			}
		}

		[MenuItem("*Hierarchy/Copy GameObject Path")]
		public static void CopyPathGameObject()
		{
			GameObject gameObject = Selection.activeGameObject;

			if (gameObject != null)
			{
				EditorGUIUtility.systemCopyBuffer = gameObject.GetPathTillRoot();
			}
		}

	}
}
#endif
