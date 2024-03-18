using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMark : MonoBehaviour
{
	private readonly static List<GameObject> gameObjects = new List<GameObject>();
	private readonly static List<MonoBehaviour> monoBehaviours = new List<MonoBehaviour>();

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		gameObjects.Add(gameObject);

		monoBehaviours.AddRange(gameObject.GetComponents<MonoBehaviour>());
		monoBehaviours.AddRange(gameObject.GetComponentsInChildren<MonoBehaviour>());
	}

	public static T FindComponent<T>()
		where T : Component
	{
		foreach (var item in gameObjects)
		{
			T component = item.GetComponent<T>();

			if (component != null) return component;
		}
		return default;
	}
	public static T FindMonoBehaviour<T>()
		where T : MonoBehaviour
	{
		foreach (var item in monoBehaviours)
		{
			if (item is T) return item as T;
		}
		return default;
	}
}
