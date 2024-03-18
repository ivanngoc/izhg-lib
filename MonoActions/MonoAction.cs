using IziHardGames.Core.Tasks;
using IziHardGames.Libs.Engine.Memory;
using IziHardGames.Ticking.Lib;
using System;

namespace IziHardGames.Libs.Engine.ScenarioWorkflow.Actions
{
	public class MonoActionFactory
	{
		private readonly IObjectPool<MonoAction> objectPool;
		private readonly IUpdateService updateService;
		private readonly MonoActionsProcessor monoActionsProcessor;
	}

	/// <summary>
	/// <see cref="ITaskResult"/>
	/// <see cref="MonoActionsControl"/>
	/// </summary>
	public class MonoAction : IPoolable
	{
		public Action trigger;
		private readonly Action runUpdateFuncWithCompletePoll;

		public readonly Action updateStart;
		public Action updateCompleteCallback;
		private Func<bool> funcBool;
		public int updateGroupe;

		/// <summary>
		/// <see langword="true"/>- when call next after execution of current action the next action will not run.<br/>
		/// <see langword="false"/> - if current action is <see cref="isInstantExecution"/>=<see langword="true"/> after current action the next action will be called.
		/// </summary>
		public bool isPreventAutoNext;
		public bool isQueued;
		public bool isRunning;
		/// <summary>
		/// Пока что лишь покащывает места через референсы где оканчивается выполнение моно экшена
		/// </summary>
		public bool isComplete;
		/// <summary>
		/// <see langword="true"/> - after execution next action will be called if <see cref="isPreventAutoNext"/>=<see langword="false"/>
		/// </summary>
		public bool isInstantExecution;
		/// <summary>
		/// Not return to pool after execution complete
		/// </summary>
		public bool isPersistant;

		//continuation
		private bool isStartOnNextLoop;
		private bool isStartOnPreviousComplete;

		public MonoAction()
		{
			runUpdateFuncWithCompletePoll = RunUpdateFuncWithCompletePoll;
			updateStart = UpdateStart;
		}
		public void CleanToReuse()
		{
			trigger = default;
			isPreventAutoNext = default;
			isQueued = default;
			isRunning = default;

			updateCompleteCallback = default;
			funcBool = default;
			updateGroupe = default;

			isPersistant = default;
		}

		public void ReturnToPool()
		{
			CleanToReuse();
			PoolObjects<MonoAction>.Shared.Return(this);
		}
		public static MonoAction Wrap(Action action)
		{
			MonoAction monoAction = PoolObjects<MonoAction>.Shared.Rent();
			monoAction.trigger = action;

			return monoAction;
		}
		public static MonoAction Wrap(Func<bool> funcPollComplete, Action completeCallback, int updateGroupe)
		{
			MonoAction monoAction = PoolObjects<MonoAction>.Shared.Rent();
			monoAction.trigger = monoAction.updateStart;
			monoAction.funcBool = funcPollComplete;
			monoAction.updateCompleteCallback = completeCallback;
			monoAction.updateGroupe = updateGroupe;
			return monoAction;
		}

		private void UpdateStart()
		{
			//updateAction = ManagerUpdatePerGroupes.Regist(runUpdateFuncWithCompletePoll, EUpdateProcessAction.ActionUpdateLate, updateGroupe);
			throw new NotImplementedException();
		}
		private void UpdateEnd()
		{
			//ManagerUpdatePerGroupes.RegistReverse(updateAction);
			//updateProcess = default;
			throw new NotImplementedException();
		}

		private void RunUpdateFuncWithCompletePoll()
		{
			isComplete = funcBool();

			if (isComplete)
			{
				UpdateEnd();
				updateCompleteCallback();
				updateCompleteCallback = default;
			}
		}
	}
}