using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IziHardGames.Libs.Engine.Helpers
{
	public static class HelperGameObject
	{
		/// <summary>
		/// Find all objects on Scene
		/// Find Inactive Game Objects
		/// </summary>
		/// <remarks>
		/// если ошибиться с параметорм totalGountInScene то будет ошибкаа 
		/// </remarks>
		/// <param name="scene"></param>
		/// <param name="buffer"></param>
		/// <param name="from"></param>
		/// <param name="totalGountInScene"></param>
		/// <returns> count of objects on scene</returns>
		public static int GameObjectsAtSceneAll(Scene scene, GameObject[] buffer, int from, int totalGountInScene = int.MinValue)
		{
			var rootCount = scene.rootCount;

			var listRoot = Buffers.BufferList<GameObject>.Shared.Rent(rootCount);

			scene.GetRootGameObjects(listRoot);

			var countGameObjects = rootCount;

			if (totalGountInScene < 0)
			{
				for (var i = 0; i < rootCount; i++)
				{
					countGameObjects += listRoot[i].transform.CountHierarchy();
				}
			}
			else
			{
				countGameObjects = totalGountInScene;
			}

			if (buffer.Length - from < countGameObjects)
			{
				throw new System.ArgumentException("Размер буфера слишком мал");
			}

			var listAll = Buffers.BufferList<GameObject>.Shared.Rent(countGameObjects);

			listAll.AddRange(listRoot);

			for (var i = 0; i < rootCount; i++)
			{
				listRoot[i].transform.GetHiararchyGameObjects(listAll);
			}

			var endCount = countGameObjects + from;

			int current = default;

			for (var i = from; i < endCount; i++)
			{
				buffer[i] = listAll[current];

				current++;
			}

			Buffers.BufferList<GameObject>.Shared.Return(listRoot);
			Buffers.BufferList<GameObject>.Shared.Return(listAll);

			return countGameObjects;
		}

		public static int GameObjectsCountAtScene(Scene scene)
		{
			var rootCount = scene.rootCount;

			var listRoot = Buffers.BufferList<GameObject>.Shared.Rent(rootCount);

			scene.GetRootGameObjects(listRoot);

			var countGameObjects = rootCount;

			for (var i = 0; i < rootCount; i++)
			{
				countGameObjects += listRoot[i].transform.CountHierarchy();
			}

			Buffers.BufferList<GameObject>.Shared.Return(listRoot);

			return countGameObjects;
		}

		/// <summary>
		/// Find first gameobject with given name in given scene
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static GameObject FindGameObjectWithNameIncludeInactive(Scene scene, string name, out int indent, out int indexSibl)
		{
			return Select(scene, (x) => x.name == name, out indent, out indexSibl);
		}
		public static GameObject Select(Scene scene, Func<Transform, bool> func, out int indent, out int indexSibling)
		{
			var gameObjects = scene.GetRootGameObjects();
			indent = default;
			indexSibling = default;

			for (int i = 0; i < gameObjects.Length; i++)
			{
				Transform current = gameObjects[i].transform;

				if (ItterateRecusivly(current, func, ref indent, ref indexSibling, out Transform result))
				{
					return result.gameObject;
				}
			}
			return default;
		}
		private static bool ItterateRecusivly(Transform current, Func<Transform, bool> func, ref int depth, ref int indexSibl, out Transform result)
		{
			int depthAtBegining = depth;
			depth++;

			if (func(current))
			{
				result = current;
				return true;
			}
			indexSibl = default;
			for (int i = 0; i < current.childCount; i++)
			{
				indexSibl = i;
				if (ItterateRecusivly(current.GetChild(i), func, ref depth, ref indexSibl, out result))
				{
					return true;
				}
			}
			depth = depthAtBegining;
			result = default;
			return false;
		}
	}


	public struct IteratorForGameObjectsAtScene
	{
		public Transform Current { get => transform; set => transform = value; }

		/// <summary>
		/// Глубина поиска
		/// </summary>
		public int indentCurrent;
		/// <summary>
		/// Одноранговая позиция 
		/// </summary>
		public int indexSibling;

		private Transform transform;

		public IteratorForGameObjectsAtScene(Scene scene) : this()
		{

		}
	}
}