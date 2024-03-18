using System;
using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.Ticking.Lib.ApplicationLevel
{
	/// <summary>
	/// Вместо корутинов. Опрос объекта на продолжение и вызов метода продолжения
	/// </summary>
	/// <remarks>
	/// <see cref="IziHardGames.Libs.NonEngine.Async.Await.AwaitExe"/>
	/// </remarks>
	public class SystemPollContinuation : MonoBehaviour
	{
		public static readonly Dictionary<int, Func<bool>> pollFunctions = new Dictionary<int, Func<bool>>(100);
		public static readonly Dictionary<int, Action> continuation      = new Dictionary<int, Action>(100);

		public static readonly List<int> toDelete  = new List<int>(100);
		public static readonly List<int> toExecute = new List<int>(100);
		/// <summary>
		/// ConstantsCore.GROUPE_CORE
		/// </summary>
		public int Priority { get => orderNumber; }

		public int orderNumber;

		public static int idFree;

		public void ExecuteUpdate()
		{
			Add();
			Check();
			Delete();
		}

		public static void Track(Action continuation, Func<bool> poll)
		{
			int id = GetNewId();
			SystemPollContinuation.continuation.Add(id, continuation);
			pollFunctions.Add(id, poll);
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
					continuation[item].Invoke();
					toDelete.Add(item);
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
		}
	}
}