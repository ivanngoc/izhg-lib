using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Component = UnityEngine.Component;

namespace IziHardGames.Libs.Engine.Helpers
{
	public static class HelperForComponents
	{
		public static IEnumerable<T> ItterateComponentsAtScene<T>(Scene scene) where T : Component
		{
			var roots = scene.GetRootGameObjects();

			foreach (var root in roots)
			{
				var iters = ItterateComponentsAtScene<T>(root);

				foreach (var iter in iters)
				{
					yield return iter;
				}
			}
		}
		public static IEnumerable<T> ItterateComponentsAtScene<T>(GameObject gameObject) where T : Component
		{
			var components = gameObject.GetComponents<T>();

			foreach (var component in components)
			{
				try
				{
					EnsureValidComponent(component, gameObject);
				}
				catch (NullReferenceException ex)
				{
					yield break;
				}
				yield return component;
			}

			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				var goChild = gameObject.transform.GetChild(i).gameObject;

				var iters = ItterateComponentsAtScene<T>(goChild);

				foreach (var iter in iters)
				{
					try
					{
						EnsureValidComponent(iter, goChild);
					}
					catch (NullReferenceException ex)
					{
						yield break;
					}
					yield return iter;
				}
			}
		}
		private static void EnsureValidComponent(Component component, GameObject go)
		{
			if (component == null)
			{
				var ex = new NullReferenceException($"Invalid Component at GameObject");
#if UNITY_EDITOR
				Debug.LogException(ex, go);
#endif
				throw ex;
			}
		}

		public static T AtSceneFindComponent<T>(Scene scene, string goName) where T : Component
		{
			foreach (var component in ItterateComponentsAtScene<T>(scene))
			{
				if (component is T result)
				{
					if (component.name == goName)
					{
						return result;
					}
				}
			}
			throw new ArgumentOutOfRangeException("No Match Founded");
		}
		public static List<T> ForScenesFindComponents<T>() where T : Component
		{
			List<T> result = new List<T>();

			ForScenesFindComponents(ref result);

			return result;
		}
		public static void ForScenesFindComponents<T>(ref List<T> result) where T : Component
		{
			int sceneCount = SceneManager.sceneCount;

			for (int i = 0; i < sceneCount; i++)
			{
				Scene scene = SceneManager.GetSceneAt(i);

				AtSceneFindComponents(scene, ref result);
			}
		}
		public static bool AtSceneFindComponents<T>(Scene scene, ref List<T> result) where T : Component
		{
			GameObject[] roots = scene.GetRootGameObjects();

			GameObject gameObject = roots.FirstOrDefault();

			if (gameObject != null)
			{
				if (gameObject.transform.TryGetAllComponentsOnScene<T>(result))
				{
					return true;
				}
			}
			return false;
		}
	}
}