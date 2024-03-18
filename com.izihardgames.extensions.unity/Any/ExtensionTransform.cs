using System.Collections.Generic;

namespace UnityEngine
{

	/// <summary>
	/// 
	/// </summary>
	public static class ExtensionTransform
	{
		public static T GetComponentInChildren<T>(this Transform transform, string gameObjectName)
			where T : Component
		{
			var components = transform.GetComponentsInChildren<T>();

			for (int i = 0; i < components.Length; i++)
			{
				if (components[i].gameObject.name == gameObjectName)
					return components[i];
			}
			return default;
		}
		public static void SetChilds(this Transform self, Transform[] childs)
		{
			for (int i = 0; i < childs.Length; i++)
			{
				childs[i].SetParent(self);
			}
		}
		public static void SetChilds(this Transform self, Transform[] childs, int from, int to)
		{
			for (int i = from; i < to; i++)
			{
				childs[i].SetParent(self);
			}
		}
		public static int CountHierarchy(this Transform self)
		{
			int childCount = self.childCount;

			int result = childCount;

			for (int i = 0; i < childCount; i++)
			{
				Transform child = self.GetChild(i);

				result += child.CountHierarchy();
			}

			return result;
		}
		public static void GetHiararchyGameObjects(this Transform self, List<GameObject> buffer)
		{
			int childCount = self.childCount;

			for (int i = 0; i < childCount; i++)
			{
				Transform child = self.GetChild(i);

				buffer.Add(child.gameObject);

				child.GetHiararchyGameObjects(buffer);
			}
		}
	}
}