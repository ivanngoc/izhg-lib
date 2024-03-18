using System;
using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.Ticking.Lib.ApplicationLevel
{
	/// <summary>
	/// Выполнение цепочки задач (асинхронных или смешанных как линейных так и асинхронных)
	/// </summary>
	public class SystemChainExecution : MonoBehaviour, IziHardGames.Core.IUpdatableDefault
	{
		/// <summary>
		/// Функции опроса на прекращение выполнения. Пока <see langword="false"/> выполнять 
		/// </summary>
		public static readonly Dictionary<int, Func<bool>> pollFunctions = new Dictionary<int, Func<bool>>(100);
		public static readonly Dictionary<int, Action> continuation      = new Dictionary<int, Action>(100);
		public static readonly Dictionary<int, Action> destroy           = new Dictionary<int, Action>(100);
		public static readonly List<int> toDelete  = new List<int>(100);
		public static readonly List<int> toExecute = new List<int>(100);

		/// <summary>
		/// ConstantsCore.GROUPE_CORE
		/// </summary>
		public int Priority { get => orderNumber; }

		public int orderNumber;

		public static int idFree;

		#region Unity Message

		#endregion
		public void ExecuteUpdate()
		{
			Add();
			Check();
			Delete();
		}

		public static void Track(Action action, Func<bool> poll, Action destroyAction)
		{
			int id = GetNewId();
			continuation.Add(id, action);
			pollFunctions.Add(id, poll);
			destroy.Add(id, destroyAction);
		}
		/// <summary>
		/// выполнить вставку нового отслеживания (связано с неизменяемостью списка во время иттерации)
		/// </summary>
		private void Add()
		{
			foreach (var item in pollFunctions)
			{
				if (!toExecute.Contains(item.Key))
				{
					toExecute.Add(item.Key);
				}
			}
		}
		/// <summary>
		/// Проверить каждую отслеживаемую задачу на завершение
		/// </summary>
		private void Check()
		{
			foreach (var item in toExecute)
			{
				if (pollFunctions[item].Invoke())
				{
					toDelete.Add(item);

					destroy[item].Invoke();
				}
				else
				{
					continuation[item].Invoke();
				}
			}
		}

		private void Delete()
		{
			foreach (var item in toDelete)
			{
				pollFunctions.Remove(item);
				continuation.Remove(item);
				toExecute.Remove(item);
				destroy.Remove(item);
			}
			toDelete.Clear();
		}

		private static int GetNewId()
		{
			idFree++;

			while (pollFunctions.ContainsKey(idFree))
			{
				idFree++;
			}

			return idFree;
		}

		public void Initilize()
		{
			idFree = int.MinValue;
		}

		public void InitilizeReverse()
		{
			pollFunctions.Clear();
			continuation.Clear();
			toDelete.Clear();
			toExecute.Clear();
			destroy.Clear();
		}
	}
}