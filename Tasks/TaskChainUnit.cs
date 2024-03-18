using System;


namespace IziHardGames.Libs.NonEngine.Threading.Tasks
{

	/// <summary>
	/// Комлексаня Задача. Своего рода state machine для операций. <br/>
	/// На каждом шаг опроса выполняет метод и получает флаг завершенности операции. <br/>
	/// Если операция завершена приступает к следующей операции. <br/>
	/// Если все операции завершены и на очереди нет других, то комлексаня задача считается выполненой и возращается в пул объектов с предварительной очисткой <br/>
	/// </summary>
	/// <remarks>
	/// <see cref="Tasks.TaskDiscrete{T}"/>
	/// </remarks>
	/// <see cref="TasksChainUnitProcessor"/>
	public class TaskChainUnit
	{
		public TaskChainUnit next;
		public TaskChainUnit previous;
		public TaskChainUnit parallel;

		private readonly Action trigger;
		private readonly Action callback;
		/// <summary>
		/// Передача 
		/// </summary>
		private Action<Action> singleActionWithCallback;

		public TaskChainUnit()
		{
			trigger = Run;
			callback = Complete;
		}

		public void InitlizeAsSinglelAction(Action<Action> operation)
		{
			singleActionWithCallback = operation;
		}
		public void Run()
		{
			singleActionWithCallback(callback);

			if (parallel != null)
			{
				parallel.Run();
			}
		}
		public void Complete()
		{
			if (next != null)
			{
				next.Run();
			}
		}

		public TaskChainUnit ParallelWith(TaskChainUnit parallel)
		{
			this.parallel = parallel;
			return parallel;
		}
		public TaskChainUnit ContinueWith(TaskChainUnit continuation)
		{
			continuation.previous = this;
			next = continuation;
			return continuation;
		}

		public void Clean()
		{
			next = default;
			previous = default;
			parallel = default;
		}
	}
}