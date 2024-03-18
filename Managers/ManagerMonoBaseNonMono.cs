using IziHardGames.Core;
using IziHardGames.Libs.Engine.Memory;
using IziHardGames.View;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.Libs.Engine.Managers
{
	[Serializable]
	public class ManagerMonoBaseNonMono<T> where T : Component, IUnique, IVisionControllable, IInitializable, IDeinitializable
	{
		public static ManagerMonoBaseNonMono<T> singleton;
		public bool isInitilized;
		[Header("Preset")]
		[SerializeField] protected Transform parent;
		[SerializeField] protected GameObject prefab;
		[Header("Managers")]
		[SerializeField] protected List<T> toDelete;

		protected ManagerWithDictionary<T> manager;
		protected PoolGameObjects poolGameObjects;

		public Dictionary<int, T> Monos => manager.keyValuePairs;

		public virtual void Initilize()
		{
			singleton = this;

			manager = new ManagerWithDictionary<T>();
			poolGameObjects = new PoolGameObjects(100, InstantiateGameObject, DestroyGameObject);
			poolGameObjects.ConfigureStandrat();
			poolGameObjects.Initilize(100, 0, 0);
			isInitilized = true;
		}

		public virtual void Initilize_De()
		{
			singleton = default;

			Clean();
			isInitilized = false;
		}

		public T GetExisted(int key)
		{
			return manager.GetItem(key);
		}
		public T GetNew(int id, bool isVisible = default)
		{
			var gameObject = poolGameObjects.Rent();

			var comp = gameObject.GetComponent<T>();

			comp.Initilize();

			comp.Id = id;

			manager.Add(comp);

			if (isVisible)
			{
				comp.ControlView.Show();
			}
			return comp;
		}

		public T Return(T obj)
		{
			return Return(Monos[obj.Id].Id);
		}
		public T Return(int key)
		{
			var val = manager.Remove(key);

			val.ControlView.Hide();

			val.InitilizeReverse();

			poolGameObjects.Return(val.gameObject);

			return val;
		}

		public GameObject InstantiateGameObject()
		{
			if (parent != null)
			{
				return GameObject.Instantiate(prefab, parent);
			}
			else
			{
				return GameObject.Instantiate(prefab);
			}
		}
		public virtual void Clean()
		{
#if UNITY_EDITOR
			//if (!isInitilized) Debug.LogError($"Repeated call [{this}]", this);
#endif
			foreach (var item in Monos)
			{
				toDelete.Add(item.Value);
			}

			for (var i = 0; i < toDelete.Count; i++)
			{
				Return(toDelete[i]);
			}

			toDelete.Clear();
		}

		/// <summary>
		/// Выгрузка объектов и освобождение памяти
		/// </summary>
		public virtual void Unload()
		{
			manager.Clear();
			poolGameObjects.Unload();
		}

		public void DestroyGameObject(GameObject toDestroy)
		{
#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlaying)
			{
				GameObject.DestroyImmediate(toDestroy);
			}
			else
#endif
			{
				GameObject.Destroy(toDestroy);
			}
		}
	}

}