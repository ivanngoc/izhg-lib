using UnityEngine;

namespace IziHardGames.View
{
	public static class ExtensionControlView
	{
		public static void SetHierarchyLayerEvryThing(this Transform transform)
		{
			int count = transform.childCount;

			transform.gameObject.layer = -1;

			for (int i = 0; i < count; i++)
			{
				transform.GetChild(i).SetHierarchyLayerEvryThing();
			}
		}
		public static void SetHierarchyLayerNothing(this Transform transform)
		{
			int count = transform.childCount;

			transform.gameObject.layer = 0;

			for (int i = 0; i < count; i++)
			{
				transform.GetChild(i).SetHierarchyLayerNothing();
			}
		}
		public static void SetHierarchyLayer0(this Transform transform)
		{
			int count = transform.childCount;

			transform.gameObject.layer = 1;

			for (int i = 0; i < count; i++)
			{
				transform.GetChild(i).SetHierarchyLayer0();
			}
		}
		public static void SetHierarchyLayer1(this Transform transform)
		{
			int count = transform.childCount;

			transform.gameObject.layer = 2;

			for (int i = 0; i < count; i++)
			{
				transform.GetChild(i).SetHierarchyLayer1();
			}
		}
		public static void SetHierarchyLayer2(this Transform transform)
		{
			int count = transform.childCount;

			transform.gameObject.layer = 4;

			for (int i = 0; i < count; i++)
			{
				transform.GetChild(i).SetHierarchyLayer2();
			}
		}
		public static void SetHierarchyLayer3(this Transform transform)
		{
			int count = transform.childCount;

			transform.gameObject.layer = 8;

			for (int i = 0; i < count; i++)
			{
				transform.GetChild(i).SetHierarchyLayer3();
			}
		}
		public static void SetHierarchyLayer4(this Transform transform)
		{
			int count = transform.childCount;

			transform.gameObject.layer = 16;

			for (int i = 0; i < count; i++)
			{
				transform.GetChild(i).SetHierarchyLayer4();
			}
		}
		public static void SetHierarchyLayer5(this Transform transform)
		{
			int count = transform.childCount;

			transform.gameObject.layer = 32;

			for (int i = 0; i < count; i++)
			{
				transform.GetChild(i).SetHierarchyLayer5();
			}
		}
		public static void SetHierarchyLayer6(this Transform transform)
		{
			int count = transform.childCount;

			transform.gameObject.layer = 64;

			for (int i = 0; i < count; i++)
			{
				transform.GetChild(i).SetHierarchyLayer6();
			}
		}
		public static void SetHierarchyLayer7(this Transform transform)
		{
			int count = transform.childCount;

			transform.gameObject.layer = 128;

			for (int i = 0; i < count; i++)
			{
				transform.GetChild(i).SetHierarchyLayer7();
			}
		}
	}
}