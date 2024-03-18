using IziHardGames.Core;
using System;
using System.Collections.Generic;

namespace IziHardGames.Libs.NonEngine.StateMachines.Variant1
{
	/// <summary>
	/// Управление переходами между состояниями
	/// </summary>
	public class ControlStateMachinesTransitions : IUpdatableDeltaTime
	{
		public int Priority { get; set; } = int.MaxValue;
		public static ControlStateMachinesTransitions Shared { get => GetOrCreate(); }
		private static ControlStateMachinesTransitions shared;

		public List<StateMachineControl> systemStateMachinesActive;
		public List<StateMachineControl> systemStateMachinesStateOut;


		/// <summary>
		/// Выполняемые в данный момент переходы
		/// </summary>
		public List<StateTransition> stateTransitionsActive;
		/// <summary>
		/// переходы из <see cref="stateTransitionsActive"/> для удаление оттуда же
		/// </summary>
		public List<StateTransition> stateTransitionsCompleted;


		/// <summary>
		/// Все выходные переходы всех активных состояний
		/// </summary>
		public List<StateTransition> transitionsTracked;
		public List<StateTransition> transitionsTrackedToAdd;
		/// <summary>
		/// очередь на удаление <see cref="transitionsTracked"/>
		/// </summary>
		public List<StateTransition> transitionsTrackedToDelete;


		public List<StateNode> stateActive;
		public List<StateNode> statesToDelete;


		public List<JobState> jobStatesActive;

		public void ExecuteUpdateWithDelta(float timeDelta)
		{
			return;
			ExecuteSheduledStateMachineStops();
			PerfomJobs(timeDelta);
			ExecuteScheduledTransitionsProceeds(timeDelta);
			StartTriggeredTransitions();
			DeleteActiveStates();
			ExecuteScheduledStateExitsThenEnters();
			ExecuteScheduledScenarios();
		}

		private void ExecuteSheduledStateMachineStops()
		{
			foreach (var item in systemStateMachinesActive)
			{
				if (item.isScheduledStop)
				{
					item.isScheduledStop = false;
					item.StopImmediate();
				}
			}
		}
		private void ExecuteScheduledTransitionsProceeds(float timeDelta)
		{
			foreach (var transition in stateTransitionsActive)
			{
				if (!transition.isBreak)
					transition.Proceed();
			}

			foreach (var transition in stateTransitionsCompleted)
			{
				stateTransitionsActive.Remove(transition);
			}
			stateTransitionsCompleted.Clear();
		}

		private void PerfomJobs(float timeDelta)
		{
			foreach (var item in jobStatesActive)
			{
				item.ActionDo();
			}
		}

		private void ExecuteScheduledStateExitsThenEnters()
		{
			foreach (var item in systemStateMachinesActive)
			{
				if (item.isScheduledExitBreak)
				{
					StateNode state = item.stateScheduledExitComplete;
					item.ScheduleCompleteExitBreak();
					state.ExitBreak();
				}
			}
			foreach (var item in systemStateMachinesActive)
			{
				if (item.isScheduledExitComplete)
				{
					StateNode state = item.stateScheduledExitComplete;
					item.ScheduleCompleteExitComplete();
					state.ExitComplete();
				}
			}

			foreach (var item in systemStateMachinesActive)
			{
				if (item.isScheduledEnter)
				{
					StateNode state = item.stateScheduledEnter;
					item.ScheduleCompleteEnter();
					state.Enter();
				}
			}
		}
		/// <summary>
		/// Остлеживание изменений в активных состояниях на предмет изменения и переход в другое состояние например при новом инпуте
		/// </summary>
		private void StartTriggeredTransitions()
		{
			#region Track Transitions

			foreach (var item in transitionsTrackedToAdd)
			{
				transitionsTracked.Add(item);
			}
			transitionsTrackedToAdd.Clear();

			foreach (var item in transitionsTrackedToDelete)
			{
				transitionsTracked.Remove(item);
			}
			transitionsTrackedToDelete.Clear();

			foreach (var item in transitionsTracked)
			{
				if (item.isTriggered)
				{
					item.Start();
				}
			}
			#endregion


		}
		private void DeleteActiveStates()
		{
			foreach (var item in statesToDelete)
			{
				stateActive.Remove(item);
			}
			statesToDelete.Clear();
		}
		private void ExecuteScheduledScenarios()
		{
			foreach (var item in systemStateMachinesActive)
			{
				/// пропускаем если запланированы переходы и выходы, на слайчай например если во время входа выше был запланирован выход, 
				/// который выполняется до входа <see cref="ExecuteScheduledStateExitsThenEnters"/>, но после указанного метода идет этот метод, то есть выход еще не произведен
				if (item.isScenarioScheduled && !item.isScheduledEnter && !item.isScheduledExitComplete && !item.isScheduledExitBreak)
				{
					item.isScenarioScheduled = false;
					item.scenarioScheduled.Start();
				}
			}
		}
		public void Track(StateNode arg)
		{
#if UNITY_EDITOR
			if (stateActive.Contains(arg))
				throw new Exception($"Duplicate active state {arg.idState}");
#endif
			stateActive.Add(arg);

			foreach (var item in arg.transitionsOut)
			{
#if UNITY_EDITOR
				if (transitionsTracked.Contains(item))
					throw new Exception($"Duplicate track transition machine {arg.stateMachine.idStateMachine} |state: {arg.idState}| transition {item.idStateTransition}");
#endif
				transitionsTrackedToAdd.Add(item);
			}
		}
		public void Track_De(StateNode arg)
		{
			statesToDelete.Add(arg);

			foreach (var item in arg.transitionsOut)
			{
				transitionsTrackedToDelete.Add(item);
			}
		}
		public void Track(StateTransition arg)
		{
			stateTransitionsActive.Add(arg);
		}
		public void Track_De(StateTransition arg)
		{
			stateTransitionsCompleted.Add(arg);
		}
		public void Track(StateMachineControl arg)
		{
			systemStateMachinesActive.Add(arg);
		}
		public void Track_De(StateMachineControl arg)
		{
			systemStateMachinesActive.Remove(arg);
		}

		public void Track(JobState jobState)
		{
			jobStatesActive.Add(jobState);
		}

		public void Track_De(JobState jobState)
		{
			jobStatesActive.Remove(jobState);
		}

		private static ControlStateMachinesTransitions GetOrCreate()
		{
			if (shared == null)
			{
				shared = CreateDefault();
			}

			return shared;
		}

		private static ControlStateMachinesTransitions CreateDefault()
		{
			return new ControlStateMachinesTransitions()
			{
				Priority = int.MaxValue,
				systemStateMachinesActive = new List<StateMachineControl>(50),
				systemStateMachinesStateOut = new List<StateMachineControl>(50),
				stateTransitionsActive = new List<StateTransition>(50),
				stateTransitionsCompleted = new List<StateTransition>(50),

				transitionsTracked = new List<StateTransition>(100),
				transitionsTrackedToAdd = new List<StateTransition>(100),
				transitionsTrackedToDelete = new List<StateTransition>(100),

				stateActive = new List<StateNode>(50),
				statesToDelete = new List<StateNode>(50),

				jobStatesActive = new List<JobState>(50),
			};
		}
	}
}