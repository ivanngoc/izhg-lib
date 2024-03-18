using System;
using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.Ticking.Lib.ApplicationLevel
{
	public class ManagerScriptableObjects : MonoBehaviour
	{
		[SerializeField] List<ScriptableObject> scriptableObjects;

		private static readonly Dictionary<Type, ScriptableObject> scriptableObjectsByType = new Dictionary<Type, ScriptableObject>(100);

		public void Initilize_De()
		{
			scriptableObjectsByType.Clear();
		}
		public void Initilize()
		{
			foreach (var item in scriptableObjects)
			{
				scriptableObjectsByType.Add(item.GetType(), item);
			}
		}

		#region Unity Message


		#endregion
		public static T Get<T>() where T : ScriptableObject
		{
			return scriptableObjectsByType[typeof(T)] as T;
		}
	}
}