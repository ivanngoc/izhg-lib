/*
 Job - менее сложная задача, в которой не нужно отслеживать состояние процесса. Если job (работа) запущена, то она гарантировано закончится успехом. 
 Это может быть например вычисление координаты. из за природы структуры  и значимых типов ошибком быть не может
 Job не ветвиться и не выстраивается в цепочки.
 
Task - более сложный процесс, который требует больше контроля. Упор делается на гибкость, а не производительность

 */

namespace IziHardGames.Core.Tasks
{
	public interface ITaskResult
	{
		bool IsEnded { get; }
		bool IsCompleted { get; }
		bool IsCanceled { get; }
		bool IsFailed { get; }
		bool IsRunning { get; }
		bool IsScheduled { get; }
	}
}
namespace IziHardGames.Core.Jobs
{
	/// <summary>
	/// Имеет флаг на проверку завершения
	/// </summary>
	public interface IJobResultCompletable : IJobResult
	{
		bool IsComplete { get; }
	}
	public interface IJobResult
	{

	}
	public interface IJob<TData, TDataEssential, TResult>
	{
		TResult JobExecute(TData data, in TDataEssential dataEssential);
	}
	/// <summary>
	/// Using previous calculation result for current calculation
	/// </summary>
	/// <typeparam name="TData"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	public interface IJobStackable<TData, TResult>
	{
		TResult JobExecute(TData data, in TResult prevResult);
	}
	public interface IJob<TData, TResult>
	{
		TResult JobExecute(TData data);
	}

	public interface IJob<TResult>
	{
		TResult JobExecute();
	}
	/// <summary>
	/// Data Struct with essential Data (origin, not changable like start and end positions for lerp)
	/// </summary>
	/// <typeparam name="TJob"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	public interface IJobDataProvider<TJob, TResult> where TJob : IJob<TResult>
	{

	}
}
