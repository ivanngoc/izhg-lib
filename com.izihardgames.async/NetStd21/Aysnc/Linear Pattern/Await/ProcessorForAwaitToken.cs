using System;
using Dictionary = IziHardGames.Libs.NonEngine.Collections.Dictionaries.WithOwnKey.DictionaryByIndexOnMemory<IziHardGames.Libs.NonEngine.Async.Await.DataForAwaitProcess>;

namespace IziHardGames.Libs.NonEngine.Async.Await
{
	public class ProcessorForAwaitToken
	{
		internal Dictionary items;
		public ProcessorForAwaitToken()
		{
			int capacity = 1;
			items = new Dictionary(capacity);
		}
		public void Execute()
		{
			var span = items.Span;

			for (int i = 0; i < items.Count; i++)
			{
				if (span[i].isAwaitCompleted)
				{
					span[i].action.Invoke();
					items.RemoveByInternalKeyWithSetDefault(i);
					i--;
				}
			}
		}

		internal void AccomulateCallback(AwaitToken awaitToken)
		{
			ref var data = ref items.GetRef(awaitToken.idAsIndex);
			data.Increment();
		}

		public AwaitToken Insert(Action action, int callbackCount)
		{
			return new AwaitToken(items.Add(new DataForAwaitProcess(action, callbackCount)));
		}
	}

	internal struct DataForAwaitProcess
	{
		public bool isAwaitCompleted;
		public int countTotal;
		public int countCurrent;
		public Action action;

		public DataForAwaitProcess(Action action, int callbackCount) : this()
		{
			this.action = action;
			countTotal = callbackCount;
		}

		internal void Increment()
		{
			countCurrent++;
			isAwaitCompleted = countTotal == countCurrent;
#if UNITY_EDITOR
			if (countCurrent > countTotal) throw new OverflowException($"Количество колбэков превышает максимальное");
#endif
		}
	}
}