using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DungeonCorp
{
	public class BrokerTaskContinuation
	{
		private readonly List<Task> tasks = new List<Task>();
		private readonly List<Action> continuations = new List<Action>();

		#region Unity Message
		private void Update()
		{
			Execute();
		}
		#endregion

		public void Execute()
		{
			for (int i = 0; i < tasks.Count; i++)
			{
				Task task = tasks[i];

				if (task.IsCompleted)
				{
					tasks.RemoveAt(i);

					i--;

					continue;
				}
				if (task.IsFaulted)
				{

				}
				if (task.IsCanceled)
				{

				}
			}
		}

		public void QueueContinuation(Task task, Action continuation)
		{

		}
	}
}