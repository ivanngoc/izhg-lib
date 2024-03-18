using System;
using UnityEngine.Tilemaps;

namespace UnityEngine
{
	public static partial class ExtensionBehaviour
	{
		public static Func<bool> GetEnabledFunc(this Behaviour self)
		{
			return () => self.enabled;
		}
	}

	public static partial class ExtensionUnityObject
	{
		public static Tilemap AsTilemap(this Object self)
		{
			return self as Tilemap;
		}

		public static T AsType<T>(this Object self) where T : Object
		{
			return self as T;
		}
	}

	public static partial class ExtensionsForGameObject
	{
		/// <summary>
		/// Contigination of names of gameobjects hierarchy to given object. Separator is "/" char
		/// </summary>
		/// <param name="gameObject"></param>
		/// <returns></returns>
		public static string GetPathTillRoot(this GameObject gameObject)
		{
			Transform parent = gameObject.transform.parent;

			string path = gameObject.name;

			while (parent != null)
			{
				path = $"{parent.gameObject.name}/{path}";
				parent = parent.parent;
			}
			return path;
		}
	}
}