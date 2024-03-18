#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using IziHardGames;


namespace IziHardGames.Libs.UnityEditor.AssetsManagment
{
	public static class ManagerForAssets
	{
		public static T LoadAsset<T>(string path) where T : UnityEngine.Object
		{
			return AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
		}
	}

}
#endif
