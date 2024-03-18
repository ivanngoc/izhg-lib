#if UNITY_EDITOR
using IziHardGames.Libs.Engine.CustomTypes;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IziHardGames.CustomEditor
{
	public static class MenuUtility
	{

		[MenuItem("Utility*/Set All Dirty Selection")]
		public static void SetAllDirtySelection()
		{

			var selections = Selection.objects;

			for (int i = 0; i < selections.Length; i++)
			{
				EditorUtility.SetDirty(selections[i]);
			}
		}

		[MenuItem("Utility*/Set All Dirty")]
		public static void SetAllDirty()
		{
			int countScene = SceneManager.sceneCount;

			GameObject[] roots = default;

			for (int i = 0; i < countScene; i++)
			{
				Scene scene = SceneManager.GetSceneAt(i);

				roots = scene.GetRootGameObjects();
			}

			List<Component> components = new List<Component>(1000);

			roots.First().transform.TryGetAllComponentsOnScene(components);

			for (int i = 0; i < components.Count; i++)
			{
				EditorUtility.SetDirty(components[i]);
			}
		}


		[MenuItem("Utility*/Ui/Set Anchors to Corners")] // ctrl+shift+a
		public static void SetAnchors()
		{
			var items = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

			for (int i = 0; i < items.Length; i++)
			{
				RectTransform target = items[i];

				RectTransform parent = target.parent as RectTransform;

				if (parent == null) continue;

				Rect rect = parent.rect;

				Vector2 pos = target.position;

				RectPosLocal rectPos = RectPosLocal.Create(target);

				target.anchorMin = rectPos.anchorMin;
				target.anchorMax = rectPos.anchorMax;

				target.position = pos;
				target.offsetMin = Vector2.zero;
				target.offsetMax = Vector2.zero;
			}
		}

		//[MenuItem("Utility*/Ui/Distribute Spacing Vertical")] // ctrl+shift+a
		//public static void DistributeSpacingVertical()
		//{
		//    var items = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

		//    if (items.Length < 0) return;

		//    RectPosGlobal rectPosGlobal = UtilityRect.CalculateBounds(items);

		//    float totalFreeSpace = items.Sum(x => x.rect.width);

		//    float rectWidth = rectPosGlobal.Width;

		//    if (totalFreeSpace > rectWidth)
		//    {
		//        // distribute free space
		//    }
		//    else
		//    {
		//        // distribute objects 
		//    }

		//    for (int i = 0; i < items.Length; i++)
		//    {
		//        RectTransform rectTransform = items[i];

		//        //rectTransform.position
		//    }
		//}

		//public static void DistributeObjectsVerticalCenter(RectTransform[] rectTransforms, in RectPosGlobal rectPos)
		//{
		//    var enumaration = rectTransforms.OrderBy(x => x.position.y);


		//}
		//public static void DistributeObjectsVerticalLeft(RectTransform[] rectTransforms, in RectPosGlobal rectPos)
		//{

		//}
	}
}
#endif
