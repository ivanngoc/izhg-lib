using IziHardGames.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace IziHardGames.Ticking.Lib.ApplicationLevel
{
	/// <summary>
	/// Poll every frame various types of async operations. Keep executuion chain-queue for each of that operation. 
	/// After complete operation this object call continuation that were specified in methods-extansions like <see cref="ExtensionsForTracker"/>
	/// </summary>
	public class TrackerOperations : MonoBehaviour, IziHardGames.Core.IUpdatableDefault, IziHardGames.Core.IOrderable
	{
		/// <summary>
		/// ConstantsCore.GROUPE_CORE
		/// </summary>
		public int Priority { get => orderNumber; }
		public int orderNumber;

		public const int capacity = 16;

		private static readonly Dictionary<int, AsyncOperation> activeAsyncOperations = new Dictionary<int, AsyncOperation>(capacity);
		private static readonly Dictionary<int, Task> activeTasks = new Dictionary<int, Task>(capacity);

		private static readonly List<int> idsAsync = new List<int>(capacity);
		private static readonly List<int> idsTask = new List<int>(capacity);
		private static readonly Dictionary<int, Action> continuations = new Dictionary<int, Action>(capacity);
		private static readonly Dictionary<int, IContinuationState> continuationStates = new Dictionary<int, IContinuationState>(capacity);
		private static readonly Dictionary<int, Func<int, bool>> continuationPolls = new Dictionary<int, Func<int, bool>>(capacity);
		private static readonly Dictionary<int, int> continuationPollsValues = new Dictionary<int, int>(capacity);
		private static readonly Dictionary<int, Action> cancelations = new Dictionary<int, Action>(capacity);
		private static readonly Dictionary<int, Action<AggregateException>> errors = new Dictionary<int, Action<AggregateException>>(capacity);
		private static readonly Dictionary<int, Action<float>> progressReporters = new Dictionary<int, Action<float>>(capacity);
		private static readonly Dictionary<int, Type> actionsByTypes = new Dictionary<int, Type>(capacity);

		private static readonly List<int> toDelete = new List<int>(capacity);
		public static int idFree = int.MinValue;

		#region Unity Message	

		#endregion
		public void Initilize()
		{

		}
		public void ExecuteUpdate()
		{
			AddToQueue();
			CheckAsync();
			CheckTask();
			Delete();
		}
		public void AddToQueue()
		{
			foreach (var item in activeAsyncOperations)
			{
				if (!idsAsync.Contains(item.Key))
				{
					idsAsync.Add(item.Key);
				}
			}

			foreach (var item in activeTasks)
			{
				if (!idsTask.Contains(item.Key))
				{
					idsTask.Add(item.Key);
				}
			}
		}
		#region Async Operation
		public static int Track(AsyncOperation asyncOperation, Action continuation)
		{
			//Log.LogLime(nameof(Track) + $"  {continuation.Method.Name}");

			int id = GetFreeId();

			activeAsyncOperations.Add(id, asyncOperation);

			continuations.Add(id, continuation);

			return id;
		}
		public static int Track(AsyncOperation asyncOperation, Action continuation, Action cancelation)
		{
			int id = Track(asyncOperation, continuation);

			cancelations.Add(id, cancelation);

			return id;
		}
		public static int Track(AsyncOperation asyncOperation, Action continuation, Action cancelation, Action<float> progressReporter)
		{
			int id = Track(asyncOperation, continuation, cancelation);

			progressReporters.Add(id, progressReporter);

			return id;
		}
		public static int Track(AsyncOperation asyncOperation, Action continuation, IContinuationState continuationState)
		{
			int id = Track(asyncOperation, continuation);

			continuationStates.Add(id, continuationState);

			return id;
		}
		public static int Track(AsyncOperation asyncOperation, Action continuation, Func<int, bool> continuationPoll, int value)
		{
			int id = Track(asyncOperation, continuation);

			continuationPolls.Add(id, continuationPoll);

			continuationPollsValues.Add(id, value);

			return id;
		}

		private void CheckAsync()
		{
			//Log.LogBlue("CheckAsync start");

			foreach (var id in idsAsync)
			{
				AsyncOperation value = activeAsyncOperations[id];

				if (value.isDone)
				{
					if (continuationStates.TryGetValue(id, out IContinuationState continuationState))
					{
						if (!continuationState.IsReady) continue;
					}
					if (continuationPolls.TryGetValue(id, out Func<int, bool> poll))
					{
						if (!poll(continuationPollsValues[id]))
						{
							continue;
						}

					}
					toDelete.Add(id);
					//Log.LogRed(continuations[id].Method.Name, continuations[id].Target as Object);
					continuations[id].Invoke();

					continue;
				}

				if (progressReporters.TryGetValue(id, out Action<float> reporter))
				{
					reporter?.Invoke(value.progress);
				}
			}
			//Log.LogBlue("CheckAsync end");
		}
		#endregion

		#region Task
		public static int Track(Task task, Action continuation, Action cancelation, Action<AggregateException> error)
		{
			int id = GetFreeId();

			activeTasks.Add(id, task);
			continuations.Add(id, continuation);
			cancelations.Add(id, cancelation);
			errors.Add(id, error);
			return id;
		}
		public static int Track<T>(Task<T> task, Action<T> continuation, Action cancelation, Action<AggregateException> error)
		{
			int id = GetFreeId();

			activeTasks.Add(id, task);
			cancelations.Add(id, cancelation);
			errors.Add(id, error);

			actionsByTypes.Add(id, typeof(T));

			ContinuationsGeneric<T>.Instance.Add(id, continuation, task);

			return id;
		}
		private void CheckTask()
		{
			foreach (var id in idsTask)
			{
				Task value = activeTasks[id];

				if (value.IsFaulted)
				{
					errors[id]?.Invoke(value.Exception);

					Abort(id);

					continue;
				}

				if (value.IsCanceled)
				{
					if (cancelations.TryGetValue(id, out Action action))
					{
						action?.Invoke();
					}

					toDelete.Add(id);
					throw new System.NotImplementedException();
				}

				if (value.IsCompleted)
				{
					if (actionsByTypes.TryGetValue(id, out Type type))
					{
						Continuations.Invoke(id, type);
					}
					else
					{
						continuations[id].Invoke();
					}
					toDelete.Add(id);
				}
			}
		}

		#endregion

		public static void Untrack(int id, AsyncOperation asyncOperation)
		{
			activeAsyncOperations.Remove(id);

			continuations.Remove(id);

			idsAsync.Remove(id);

			if (cancelations.ContainsKey(id))
			{
				cancelations.Remove(id);
			}

			if (continuationStates.ContainsKey(id))
			{
				continuationStates.Remove(id);
			}

			if (continuationPolls.ContainsKey(id))
			{
				continuationPolls.Remove(id);
				continuationPollsValues.Remove(id);
			}
		}
		public static void Untrack(int id, Task task)
		{
			activeTasks.Remove(id);

			idsTask.Remove(id);

			if (actionsByTypes.TryGetValue(id, out Type type))
			{
				Continuations.Delete(id, type);

				actionsByTypes.Remove(id);
			}
			else
			{
				continuations.Remove(id);
			}
			cancelations.Remove(id);

			errors.Remove(id);
		}
		public static void Untrack(int id)
		{
			if (activeAsyncOperations.TryGetValue(id, out AsyncOperation asyncOperation))
			{
				Untrack(id, asyncOperation);
				return;
			}
			if (activeTasks.TryGetValue(id, out Task task))
			{
				Untrack(id, task);
				return;
			}
			throw new NullReferenceException($"Не найдена операция с id {id}");
		}
		public static void Abort(int id)
		{
			if (activeAsyncOperations.TryGetValue(id, out AsyncOperation asyncOperation))
			{
				Abort(id, asyncOperation);
				return;
			}
			if (activeTasks.TryGetValue(id, out Task task))
			{
				Abort(id, task);
				return;
			}
			throw new NullReferenceException($"Не найдена операция с id {id}");
		}
		public static void Abort(int id, AsyncOperation asyncOperation)
		{
			if (cancelations.TryGetValue(id, out Action cancel))
			{
				cancel.Invoke();
			}
			toDelete.Add(id);
		}
		public static void Abort(int id, Task task)
		{
			if (cancelations.TryGetValue(id, out Action cancel))
			{
				cancel?.Invoke();
			}
			toDelete.Add(id);
		}
		public void Delete()
		{
			for (int i = 0; i < toDelete.Count; i++)
			{
				int id = toDelete[i];

				toDelete.RemoveAt(i);

				Untrack(id);

				i--;
			}
		}
		private static int GetFreeId()
		{
			return ++idFree;
		}


		private class Continuations
		{
			protected static readonly Dictionary<Type, Continuations> instancesByType = new Dictionary<Type, Continuations>();

			public static void Invoke(int id, Type type)
			{
				if (instancesByType.TryGetValue(type, out Continuations continuations))
				{
					continuations.Invoke(id);
				}
			}
			protected virtual void Invoke(int id) { }
			protected virtual void Remove(int id) { }

			public static void Delete(int id, Type type)
			{
				if (instancesByType.TryGetValue(type, out Continuations continuations))
				{
					continuations.Remove(id);
				}
			}
		}

		private class ContinuationsGeneric<T> : Continuations
		{
			private static ContinuationsGeneric<T> instance;
			public static ContinuationsGeneric<T> Instance
			{
				get
				{
					if (instance == null)
					{
						instance = new ContinuationsGeneric<T>();

						instancesByType.Add(typeof(T), instance);
					}

					return instance;
				}
			}

			private readonly static Dictionary<int, Action<T>> continuations = new Dictionary<int, Action<T>>();

			private readonly static Dictionary<int, Task<T>> tasks = new Dictionary<int, Task<T>>();

			public void Add(int id, Action<T> continuation, Task<T> task)
			{
				continuations.Add(id, continuation);
				tasks.Add(id, task);
			}
			protected override void Invoke(int id)
			{
				base.Invoke(id);

				continuations[id]?.Invoke(tasks[id].Result);
			}
			protected override void Remove(int id)
			{
				base.Remove(id);

				continuations.Remove(id);

				tasks.Remove(id);
			}
		}
	}

	/// <summary>
	/// TODO: Extansion Tracker For Async And Parallel
	/// Completion
	/// Cancelation
	/// Exception
	/// </summary>
	public static class ExtensionsForTracker
	{
		#region AsyncOperation
		public static AsyncOperation ContinueWith(this AsyncOperation asyncOperation, Action continuation)
		{
			int id = TrackerOperations.Track(asyncOperation, continuation);

			return asyncOperation;
		}
		public static AsyncOperation ContinueWith(this AsyncOperation asyncOperation, Action continuation, Action cancelation)
		{
			int id = TrackerOperations.Track(asyncOperation, continuation, cancelation);

			return asyncOperation;
		}
		public static AsyncOperation ContinueWith(this AsyncOperation asyncOperation, Action continuation, Action cancelation, Action<float> progressReporter)
		{
			int id = TrackerOperations.Track(asyncOperation, continuation, cancelation, progressReporter);

			return asyncOperation;
		}
		public static AsyncOperation ContinueWith(this AsyncOperation asyncOperation, Action continuation, IContinuationState continuationState)
		{
			int id = TrackerOperations.Track(asyncOperation, continuation, continuationState);

			return asyncOperation;
		}
		public static AsyncOperation ContinueWith(this AsyncOperation asyncOperation, Action continuation, Func<int, bool> continuationPoll, int value)
		{
			int id = TrackerOperations.Track(asyncOperation, continuation, continuationPoll, value);
			return asyncOperation;
		}
		#endregion

		#region Task 
		public static Task CustomContinueWith(this Task task, Action continuation, Action cancelation, Action<AggregateException> error)
		{
			int id = TrackerOperations.Track(task, continuation, cancelation, error);
			return task;
		}
		public static Task<T> CustomContinueWith<T>(this Task<T> task, Action<T> continuation, Action cancelation, Action<AggregateException> error)
		{
			int id = TrackerOperations.Track<T>(task, continuation, cancelation, error);

			return task;
		}
		#endregion
	}
}