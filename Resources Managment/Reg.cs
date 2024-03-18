using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IziHardGames.Ticking.Lib.ApplicationLevel
{
	/// <summary>
	/// Регистр систем / подключенных модулей / сервисов
	/// </summary>
	public static class Reg
	{
		private static readonly Dictionary<Type, object> systems = new Dictionary<Type, object>(1000);
		/// <summary>
		/// Ключ <see cref="Object.GetInstanceID"/>
		/// Значение  <see cref="Component"/> статический на сцене
		/// </summary>
		private static readonly Dictionary<int, Component> components = new Dictionary<int, Component>(1000);

		private static readonly Dictionary<int, object> objectsByGUID = new Dictionary<int, object>(1000);

		private static Dictionary<int, ProjectGUID> guids = new Dictionary<int, ProjectGUID>(100);

		#region Unity Message

		#endregion

		public static T GetSingleton<T>() where T : class
		{
			if (systems.TryGetValue(typeof(T), out object val))
			{
				return val as T;
			}

			foreach (var item in systems)
			{
				var value = item.Value;

				if (value is T)
				{
					return value as T;
				}
			}
			throw new NullReferenceException($"Объект типа {typeof(T).FullName} не добавлен в список");
		}
		public static T GetComponent<T>(int key) where T : Component
		{
			return components[key] as T;
		}
		public static T GetByGuid<T>(int guid) where T : class
		{
			return objectsByGUID[guid] as T;
		}

		public static void SingletonAdd<T>(T item, Type type) where T : class
		{
			systems.Add(type, item);
		}
		public static void SingletonAdd<T>() where T : class, new()
		{
			SingletonAdd<T>(new T());
		}

		/// <summary>
		/// Регистрация По типу 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		public static void SingletonAdd<T>(T obj) where T : class
		{
			Debug.Log($"Type:{obj.GetType().FullName}\t SingletonAdd");
			if (obj == null) throw new NullReferenceException();

			//if (obj is Component)
			//{
			//             Debug.Log($"<color=red> Regist {obj}</color>", obj as Component);

#if UNITY_EDITOR

			try
			{
#endif
				systems.Add(typeof(T), obj);
#if UNITY_EDITOR
			}
			catch (Exception ex)
			{
				if (obj is UnityEngine.Object)
				{
					Debug.LogException(ex, obj as UnityEngine.Object);
				}
				else
				{
					Debug.LogError($"Type:{typeof(T).FullName}\r\n{ex}");
				}
			}            //         }
#endif
		}
		public static void SingletonRemove(Type type)
		{
			systems.Remove(type);
		}
		public static void SingletonRemove<T>() where T : class
		{
			systems.Remove(typeof(T));
		}
		public static void SingletonRemove<T>(T obj) where T : class
		{
			if (obj == null) throw new NullReferenceException();
			SingletonRemove<T>();
		}

		public static void SingletonByGetTypeAdd<T>(T obj)
		{
#if UNITY_EDITOR
			try
			{
#endif
				systems.Add(obj.GetType(), obj);
#if UNITY_EDITOR
			}
			catch (Exception ex)
			{
				if (obj is UnityEngine.Object)
				{
					Debug.LogException(ex, obj as UnityEngine.Object);
				}
				else
				{
					Debug.LogException(ex);
				}
			}            //         }
#endif
		}
		public static void SingletonByGetTypeRemove<T>(T obj)
		{
			if (obj == null) throw new NullReferenceException();

			systems.Remove(obj.GetType());
		}
		/// <summary>
		/// Регистрация по <see cref="Object.GetInstanceID"/>
		/// </summary>
		/// <param name="component"></param>
		public static void ComponentAdd(Component component)
		{
			int key = component.GetInstanceID();

			if (components.TryGetValue(key, out Component existed))
			{
				components[key] = component;
			}
			else
			{
				components.Add(key, component);
			}
		}
		public static void ComponentRemove(Component component)
		{
			int key = component.GetInstanceID();

			if (components.ContainsKey(key))
			{
				components.Remove(key);
			}
		}

		public static void AddByGuidAsInt(int guid, object obj)
		{
			objectsByGUID.Add(guid, obj);
		}
		public static void AddByGuidAsIntReverse(int guid)
		{
			objectsByGUID.Remove(guid);
		}
		public static void RegistGUID(ProjectGUID projectGUID, object obj)
		{
			guids.Add(projectGUID.guid, projectGUID);
		}
		public static void RegistGUID_De(ProjectGUID projectGUID, object obj)
		{
			guids.Remove(projectGUID.guid);
		}

		public static void ClearNullRef()
		{
			List<Type> toDel = new List<Type>();

			foreach (var item in systems)
			{
				if (item.Value == null)
				{
					toDel.Add(item.Key);
				}
			}

			foreach (var item in toDel)
			{
				systems.Remove(item);
			}
		}

#if UNITY_EDITOR
		[Unused]
		public class Record
		{
			public int id;
			public Type type;
			public Object target;
		}
#endif

	}
	public static class RegActions
	{
		public static Dictionary<int, Action> actions = new Dictionary<int, Action>(256);

		public static void Clear()
		{
			actions.Clear();
		}
		public static void Reg(int key, Action action)
		{
			actions.Add(key, action);
		}
		public static void Reg_De(int key, Action action)
		{
			actions.Remove(key);
		}
		public static Action Get(int key)
		{
			return actions[key];
		}
	}
}