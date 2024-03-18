#if UNITY_EDITOR
using IziHardGames.Libs.NonEngine.String;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

namespace IziHardGames.Libs.Engine.AssetsUtils
{
	public static class HelperForAssets
	{
		public static List<T> GetAssetsAtDirectory<T>(string unityRelativePath) where T : UnityEngine.Object
		{
			string path = Path.Combine(Directory.GetCurrentDirectory(), unityRelativePath);
			// ordered with numbers as text
			string[] dirs = Directory.GetFiles(path);

			List<T> result = new List<T>(dirs.Length);

			foreach (var dir in dirs)
			{
				var assets = AssetDatabase.LoadAllAssetsAtPath(Path.Combine(unityRelativePath, Path.GetFileName(dir)));

				foreach (var asset in assets)
				{
					if (asset is T item)
					{
						result.Add(item);
						break;
					}
				}
			}
			return result;
		}
	}
}
#endif