using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IziHardGames.Libs.Engine.Tasks
{
	/// <summary>
	/// Объект для синхронизации задач <see cref="System.Threading.Tasks.Task"/>, <see cref="TaskUnity"/> и прочих оберток обработчиков (методов).
	/// По сути является агрегатором <see cref="Func{TResult}"/> c типом <see cref="bool"/>. Функции опрашиваются по умолчанию через определенный интервал, но может и по требовани, до тех пор пока не получит <see langword="true"/>
	/// Когда все задачи будут выполнены без ошибок то вызывается метод продолжения, иначае метод ошибки и т.п. как по <see cref="System.Threading.Tasks.Task"/>.
	/// Аналог - <see cref="System.Threading.Tasks.Task.WhenAll(IEnumerable{System.Threading.Tasks.Task})"/><br/>
	/// объекты синхронизации:
	/// polling (repeatable execution)
	/// await
	/// task
	/// coroutine	
	/// CAN BE EXTANDED WITH UNITY COROUTINE
	/// </summary>
	public class TaskSyncPoint
	{
		public readonly List<Func<bool>> checkers = new List<Func<bool>>(8);

		public TaskSyncPoint Add(IEnumerator enumerator)
		{
			throw new NotImplementedException();
		}
		public TaskSyncPoint Add(Task task)
		{
			throw new NotImplementedException();
		}
		public TaskSyncPoint Add(Func<bool> func)
		{
			throw new NotImplementedException();
		}
	}
	/// <summary>
	/// для типов вне стандартного c#. Наприме Coroutine в Unity или AsyncOperation
	/// </summary>
	public class WrapSyncPoint
	{

	}
}