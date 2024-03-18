using IziHardGames.Core;
using IziHardGames.Libs.Engine.Memory;
using IziHardGames.View;
using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.Libs.Engine.Managers
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TComp"></typeparam>
	public class ManagerForMonoComponents<TComp> : MonoBehaviour, IInitializable, IDeinitializable, IUpdatableDefault
	where TComp : Component, IUnique, IVisionControllable, IDeinitializable
	{
		public static ManagerForMonoComponents<TComp> singleton;
		public virtual int Priority { get => int.MaxValue; }
		public Dictionary<int, TComp> itemsByKey => manager.keyValuePairs;
		public bool isInitilized;
		[Header("Managers")]
		[SerializeField] protected ManagerWithDictionary<TComp> manager;
		[SerializeField] protected PoolGameObjects poolGameObjects;
		[SerializeField] protected List<TComp> toDelete;
		[Header("Preset")]
		[SerializeField] protected Transform parent;
		[SerializeField] protected GameObject prefab;

		#region Unity Messages
		public virtual void Awake() { }
		public virtual void Reset() { }
		public virtual void OnEnable() { }
		public virtual void OnDisable() { }
		public virtual void OnDestroy() { }
		#endregion

		public virtual void Initilize()
		{
			singleton = this;

			manager = new ManagerWithDictionary<TComp>();
			poolGameObjects = new PoolGameObjects(100, InstantiateGameObject, DestroyGameObject);
			poolGameObjects.ConfigureStandrat();
			poolGameObjects.Initilize(100, 0, 0);
			isInitilized = true;
		}

		public virtual void InitilizeReverse()
		{
			singleton = default;

			Clean();

			isInitilized = false;
		}

		public virtual void ExecuteUpdate()
		{
			enabled = !poolGameObjects.DoTask();
		}

		public TComp GetExisted(int key)
		{
			return manager.GetItem(key);
		}

		public TComp Rent(int id)
		{
			var gameObject = poolGameObjects.Rent();
			var comp = gameObject.GetComponent<TComp>();
			comp.Id = id;
			manager.Add(comp);
			return comp;
		}
		public TComp Rent(int id, bool isVisible)
		{
			TComp comp = Rent(id);
			if (isVisible)
			{
				comp.ControlView.Show();
			}
			return comp;
		}
		public THeir Rent<THeir>(int id, bool isVisible) where THeir : TComp, IInitializable
		{
			var comp = Rent(id, isVisible) as THeir;
			comp.Initilize();
			return comp;
		}
		public THeir Rent<THeir, TData>(TData data, bool isVisible)
			where THeir : TComp, IInitializable<TData>
			where TData : IUnique
		{
			var comp = Rent(data.Id, isVisible) as THeir;
			comp.Initilize(data);
			return comp;
		}

		public TComp Return(TComp component, bool isHideView)
		{
			if (isHideView)
			{
				component.ControlView.Hide();
			}
			var comp = manager.Remove(component.Id);
			poolGameObjects.Return(comp.gameObject);
			return component;
		}

		public THeir Return<THeir>(THeir obj, bool isHideView) where THeir : TComp, IDeinitializable
		{
			TComp comp = obj;
			Return(comp, isHideView);
			obj.InitilizeReverse();
			return obj;
		}

		public THeir Return<THeir>(int key, bool isRendererTurnOff) where THeir : TComp, IDeinitializable
		{
			return Return(itemsByKey[key] as THeir, isRendererTurnOff);
		}

		public GameObject InstantiateGameObject()
		{
			if (parent != null)
			{
				return Instantiate(prefab, parent);
			}
			else
			{
				return Instantiate(prefab);
			}
		}
		public virtual void Clean()
		{
#if UNITY_EDITOR
			if (!isInitilized) Debug.LogError($"Repeated call [{this}]", this);
#endif
			foreach (var item in itemsByKey)
			{
				toDelete.Add(item.Value);
			}

			for (var i = 0; i < toDelete.Count; i++)
			{
				Return<TComp>(toDelete[i], true);
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
				DestroyImmediate(toDestroy);
			}
			else
#endif
			{
				Destroy(toDestroy);
			}
		}
	}

}