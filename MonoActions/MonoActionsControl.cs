using IziHardGames.Libs.NonEngine.Async.Await;
using IziHardGames.Libs.NonEngine.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IziHardGames.Libs.Engine.ScenarioWorkflow.Actions
{
	/// <summary>
	/// State Machine c послежовательными состояниями. Состояния в данном контексте это действия, которые нужно выполнить последовательно и поочередно.<br/>
	/// Набор стандартных действий для объектов с <see cref="Transform"/>.
	/// Используется для изменения <see cref="MonoBehaviour"/> объектов в сценариях без отражение от данных. То есть например когда объект данных уже удален, но View объекту нужно выполнить какие-то остаточные действия для визуализации эффекта или статуса.
	/// </summary>	
	/// <remarks>
	/// Update order or groupe is not configurable <br/>
	/// Alternative <see cref="IziHardGames.Libs.Engine.Tasks.TaskUnity"/>
	/// Alternative <see cref="NonEngine.Updating.UpdateStep"/>
	/// </remarks>
	public class MonoActionsControl : MonoBehaviour, IStateMachine
	{
		public const int ACTION_MOVE_TO = 1 << 0;
		public const int ACTION_ROTATE_TO = 1 << 1;
		/// <summary>
		/// Remove From Scene with custom delete action <see cref="delete"/>
		/// </summary>
		public const int ACTION_DELETE = 1 << 2;

		// Predefined actions
		public MonoActionMoveWithTransform monoActionMoveWithTransform;
		public MonoActionDelete monoActionDelete;


		public float speedRotation;


		public MonoAction monoActionCurrent;
		public Queue<MonoAction> queue = new Queue<MonoAction>(32);


		public bool isInitOnce;
		public bool isAwaitCallbackScheduled;
		public bool isFinishedExecutionOfQueudActions;

		private AwaitToken awaitToken;

		private Action actionComplete;


		public int configuration;

		#region Unity Message


		#endregion

		public void Initilize()
		{
			if (!isInitOnce)
			{
				isInitOnce = true;
				Configure();
			}
		}
		public void Initilize(float speedMoveArg, Action deleteArg, Action<Vector3> setPositionArg, Func<Vector3> getPositionArg)
		{
			Initilize();

			monoActionDelete.actionDelete = deleteArg;
			monoActionDelete.trigger = deleteArg;

			monoActionMoveWithTransform.speedMove = speedMoveArg;
			monoActionMoveWithTransform.actionPositionSet = setPositionArg;
			monoActionMoveWithTransform.funcPositionGet = getPositionArg;

			actionComplete = MonoActionComplete;
		}


		public void UpdateFrom<TData>(TData data)
		{
			throw new NotImplementedException();
		}

		private void Configure()
		{
			if ((configuration & ACTION_MOVE_TO) > 0)
			{
				monoActionMoveWithTransform = new MonoActionMoveWithTransform(transform);
				monoActionMoveWithTransform.isPersistant = true;
				monoActionMoveWithTransform.control = this;
			}
			if ((configuration & ACTION_DELETE) > 0)
			{
				monoActionDelete = new MonoActionDelete();
				monoActionDelete.isPersistant = true;
			}
		}

		/// <summary>
		/// </summary>
		/// <example>
		/// While move rotate around
		/// </example>
		/// <returns></returns>
		public MonoActionsControl While()
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Free Action Execution
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public MonoActionsControl Do(Action action)
		{
			MonoAction monoAction = Queue(action);
			monoAction.isInstantExecution = true;
			return this;
		}
		public MonoActionsControl Then(Action action)
		{
			MonoAction lastAction = queue.Last();
			return Do(action);
		}
		/// <summary>
		/// While func return <see langword="true"/> do. When return <see langword="false"/> break AND NOT EXECUTE LAST TIME
		/// </summary>
		/// <param name="poll"></param>
		/// <returns></returns>
		public MonoActionsControl DoWhile(Action action, Func<bool> poll)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// While func return <see langword="true"/> do. When return <see langword="false"/> break AND EXECUTE LAST TIME
		/// </summary>
		/// <param name="func"></param>
		/// <returns></returns>
		public MonoActionsControl DoWhileAndLast(Func<bool> func)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// While func return <see langword="false"/> do. When return <see langword="true"/> break AND NOT EXECUTE LAST TIME
		/// </summary>
		/// <param name="func"></param>
		/// <returns></returns>
		public MonoActionsControl DoUntil(Action action, Func<bool> func)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// While func return <see langword="false"/> do. When return <see langword="true"/> break AND EXECUTE LAST TIME
		/// </summary>
		/// <param name="func"></param>
		/// <returns></returns>
		public MonoActionsControl DoUntilAndLast(Func<bool> func)
		{
			throw new NotImplementedException();
		}
		public MonoActionsControl WaitFor(Func<bool> func, int updateGroupe)
		{
			MonoAction monoAction = Queue(func, updateGroupe);
			monoAction.isInstantExecution = false;
			return this;
		}
		/// <summary>
		/// After this command all next command will start executing in new frame (not current frame)
		/// </summary>
		/// <example>
		/// this.Move(Vector3.Zero).Schedule().Move(Vector3.One).Delete().Run();
		/// First Move Will execute Instantly
		/// </example>
		/// <returns></returns>
		public MonoActionsControl Schedule()
		{
			throw new NotImplementedException();
		}
		public MonoActionsControl Hide()
		{
			throw new NotImplementedException();
		}

		#region Move
		public MonoActionsControl ScheduleMove(Vector3 end)
		{
			return ScheduleMove(transform.position, end);
		}
		public MonoActionsControl ScheduleMove(Vector3 start, Vector3 end)
		{
			monoActionMoveWithTransform.SetInitialValues(ref start, ref end);
			var action = Queue(monoActionMoveWithTransform.triggerMove);
			action.isInstantExecution = false;
			return this;
		}


		#endregion

		private void TriggerDelete()
		{
			monoActionDelete.actionDelete();
		}

		public MonoActionsControl SheduleDelete()
		{
			var action = Queue(monoActionDelete);
			action.isInstantExecution = true;
			return this;
		}

		#region Execution control
		public MonoActionsControl Run(AwaitToken awaitToken)
		{
			this.awaitToken = awaitToken;
			isAwaitCallbackScheduled = true;
			isFinishedExecutionOfQueudActions = false;
			Next();
			return this;
		}
		public MonoActionsControl Run()
		{
			isAwaitCallbackScheduled = false;
			isFinishedExecutionOfQueudActions = false;
			Next();
			return this;
		}
		private bool TryNext()
		{
			if (queue.Count > 0)
			{
				Next();
				return true;
			}
			return false;
		}

		private void Next()
		{
#if DEBUG
			if (monoActionCurrent != null && monoActionCurrent.isRunning)
			{
				Debug.LogError($"Calling next action while there is another monoaction running", this);
			}
#endif
			MonoAction monoAction = queue.Dequeue();
			monoAction.trigger();
			isFinishedExecutionOfQueudActions = queue.Count <= 0;

			if (monoAction.isInstantExecution)
			{
				MonoActionComplete(monoAction);
			}
			else
			{
				monoAction.isRunning = true;
				monoActionCurrent = monoAction;
			}
		}
		public MonoActionsControl PreventNext()
		{
			queue.Peek().isPreventAutoNext = true;
			return this;
		}
		public void MonoActionComplete()
		{
			var action = monoActionCurrent;
			monoActionCurrent = default;
			MonoActionComplete(action);

			//var method = System.Reflection.MethodBase.GetCurrentMethod();
			//Debug.Log($"{Time.frameCount}	{GetType().FullName}.{method.Name}() {monoActionCurrent}| isFinishedExecutionOfQueudActions:{isFinishedExecutionOfQueudActions}| isAwaitCallbackScheduled {isAwaitCallbackScheduled}", this);
		}
		public void MonoActionComplete(MonoAction action)
		{
			action.isComplete = true;

			if (!action.isPersistant)
			{
				action.ReturnToPool();
			}
			else
			{
				action.isRunning = false;
			}

			if (isFinishedExecutionOfQueudActions)
			{
				if (isAwaitCallbackScheduled)
				{
					awaitToken.Callback();
					awaitToken = default;
				}
			}
			else
			{
				if (!action.isPreventAutoNext)
				{
					Next();
				}
			}
		}
		private MonoAction Queue(MonoAction monoAction)
		{
			monoAction.isQueued = true;
			queue.Enqueue(monoAction);
			return monoAction;
		}
		private MonoAction Queue(Action action)
		{
			MonoAction monoAction = MonoAction.Wrap(action);
			monoAction.isQueued = true;

			queue.Enqueue(monoAction);
			return monoAction;
		}
		private MonoAction Queue(Func<bool> func, int updateGroupe)
		{
			MonoAction monoAction = MonoAction.Wrap(func, actionComplete, updateGroupe);
			monoAction.isQueued = true;
			queue.Enqueue(monoAction);
			return monoAction;
		}
		#endregion
	}
}