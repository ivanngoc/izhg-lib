using System;
using System.Collections.Generic;
using System.Linq;

namespace IziHardGames.Libs.NonEngine.StateMachines.Variant1
{
	/// <summary>
	/// Машина переключения состояний
	/// </summary>
	[Serializable]
	public class StateMachineControl
	{
		public int idStateMachine;
		public int idStateCurrent;
		public int idScenarioOnInitilize = SCENARIO_INITILIZE;

		public bool isTransitionActive;
		public bool isTransitionTriggered;

		public bool isScenarioScheduled;
		public bool isScenarioActive;

		public bool isScheduledStop;
		public bool isScheduledEnter;
		public bool isScheduledExitComplete;
		public bool isScheduledExitBreak;

		public bool isAutoAI;

		[NonSerialized] public bool isGraphBuilded;

		public const int STATE_ENTER = 1;
		public const int STATE_IDLE = 2;
		public const int STATE_COOLDOWN_REAPEAT = 11;

		public const int TRANSITION_TEMP = 4;

		public const int SCENARIO_INITILIZE = 4;

		[NonSerialized] public IStateMachineAssociatedData stateMachineAssociatedData;
		[NonSerialized] public DataStateMachine dataStateMachine;
		[NonSerialized] public StateMachineGraph stateMachineGraph;

		[NonSerialized] public StateNode stateLastExit;
		[NonSerialized] public StateNode stateLastEnter;

		[NonSerialized] public StateNode stateIdle;
		[NonSerialized] public StateNode stateCooldownRepeat;

		[NonSerialized] public StateNode stateScheduledEnter;
		[NonSerialized] public StateNode stateScheduledExitComplete;
		[NonSerialized] public StateNode stateScheduledExitBreak;

		/// <summary>
		/// Используется для сохздания временного перехода между состояниями если между ними нет явного перехода
		/// </summary>
		[NonSerialized] public StateTransition stateTransitionTemp;
		[NonSerialized] public StateTransition stateTransitionTriggered;
		[NonSerialized] public StateTransition stateTransitionActive;
		[NonSerialized] public StateMachineScenario scenarioActive;
		[NonSerialized] public StateMachineScenario scenarioScheduled;

		[NonSerialized] public Dictionary<int, StateNode> statesById;
		[NonSerialized] public Dictionary<int, JobState> jobsById;
		[NonSerialized] public Dictionary<int, StateTransition> transitionsById;
		[NonSerialized] public Dictionary<int, StateMachineScenario> scenarios;

#if UNITY_EDITOR
		public static Action<StateMachineControl> debug_Scheduled_Enter_Complete;
#endif
		public static int count;
		public void Initilize(IStateMachineAssociatedData dataArg)
		{
			count++;
			idStateMachine = count;

			stateMachineAssociatedData = dataArg;

			foreach (var item in jobsById)
			{
				item.Value.InitilizeData();
			}

			ScenarioStart(idScenarioOnInitilize);

			idScenarioOnInitilize = SCENARIO_INITILIZE;
		}

		#region Scheduling
		public void ScheduleScenarioStart(int idScenario)
		{
			StateMachineScenario stateMachineScenario = scenarios[idScenario];

			scenarioScheduled = stateMachineScenario;

			isScenarioScheduled = true;
		}

		internal void ScheduleCompleteExitBreak()
		{
			stateScheduledExitBreak = default;
			isScheduledExitBreak = false;
		}

		public void ScheduleStateExitBreak(StateNode state)
		{
			stateScheduledExitBreak = state;
			isScheduledExitBreak = true;
		}

		internal void ScheduleCompleteExitComplete()
		{
			stateScheduledExitComplete = default;
			isScheduledExitComplete = false;
		}

		public void ScheduleStateExitComplete(StateNode state)
		{
			stateScheduledExitComplete = state;
			isScheduledExitComplete = true;
		}

		internal void ScheduleCompleteEnter()
		{
#if UNITY_EDITOR
			debug_Scheduled_Enter_Complete?.Invoke(this);
#endif
			stateScheduledEnter = default;
			isScheduledEnter = false;
		}

		public void ScheduleStateEnter(int idState)
		{
			stateScheduledEnter = statesById[idState];
			isScheduledEnter = true;
		}

		#endregion

		#region Scenario
		public void ScenarioStart(int idScenario)
		{
			StateMachineScenario stateMachineScenario = scenarios[idScenario];

			ScenarioStart(stateMachineScenario);
		}
		/// <summary>
		/// К началу выполнения сценария должен быть произведен выход из действующего состояния <see cref="stateLastEnter"/>
		/// Не должно быть других активных сценариев (все остановлены)
		/// </summary>
		/// <param name="stateMachineScenario"></param>
		public void ScenarioStart(StateMachineScenario stateMachineScenario)
		{
			stateMachineScenario.Start();
		}

		public void ScenarionEnd()
		{
			scenarioActive.End();

			StateTransitStraightDefault();
		}

		public void ScenarionMoveToNext()
		{
			StateNode state = scenarioActive.GetNextState();

			StateTransit(state);
		}
		#endregion

		#region State Changing
		/// <summary>
		/// Запуск машины состояний после остановки <see cref="Stop"/>
		/// </summary>
		public void Start()
		{
			stateIdle.Enter();
		}
		public void StopSchedule()
		{
			isScheduledStop = true;
		}
		public void StopImmediate()
		{
			Stop();
		}
		/// <summary>
		/// Остановка машины, выход из всех состояний
		/// </summary>
		private void Stop()
		{
			if (isScenarioScheduled)
			{
				isScenarioScheduled = default;
				scenarioScheduled = default;
			}

			if (isScenarioActive)
			{
				scenarioActive.Stop();
				scenarioActive = default;
			}

			if (isTransitionActive)
			{
				stateTransitionActive.Break();
				isTransitionActive = false;
				stateTransitionActive = default;
			}

			stateLastEnter.ExitBreak();
			stateLastEnter = default;
		}
		public void StateTransitTrigger(StateTransition stateTransition)
		{
			isTransitionTriggered = true;

			stateTransitionTriggered = stateTransition;

			stateTransition.Trigger();
		}
		public void StateTransit(int idState)
		{
			StateTransit(statesById[idState]);
		}
		public void StateTransit(StateNode stateNew)
		{
			StateNode current = stateLastEnter;

			StateTransition stateTransition = GetTransition(current, stateNew);

			StateTransitTrigger(stateTransition);
		}
		public void StateTransitStraightDefault()
		{
			StateTransitStraight(statesById[STATE_IDLE]);
		}
		/// <summary>
		/// Перход из текущего состояния в указанное без перехода
		/// </summary>
		public void StateTransitStraight(StateNode stateArg)
		{
			stateArg.Enter();
		}
		#endregion

		#region Transitions
		public StateTransition GetTransition(StateNode from, StateNode to)
		{
			try
			{
				return from.transitionsOut.First(x => x.idStateTo == to.idState);
			}
			catch (Exception ex)
			{
				throw new Exception($"from{from.idState}| to {to.idState}| state machine {idStateMachine}", ex);
			}
			//throw new NotImplementedException();
		}
		/// <summary>
		/// Прервавать переход без отмены состояния. Машина зависает в том положении в котором произведена остановка. 
		/// При этом данные очищвются чтобы при повторном вызове прерываемого перехода все было бы как по новой
		/// </summary>
		/// <param name="stateTransition"></param>
		public void TransitonBreak(StateTransition stateTransition)
		{
			stateTransition.Break();
		}
		#endregion
		public void RecreateNonSerilizedFields()
		{
			scenarios = new Dictionary<int, StateMachineScenario>(5);

			statesById = new Dictionary<int, StateNode>(15);
			jobsById = new Dictionary<int, JobState>(15);
			transitionsById = new Dictionary<int, StateTransition>(15);
		}
		/// <summary>
		/// Создавть связи между несерилизованными полями
		/// </summary>
		public void Rebind(StateMachineGraph stateMachineGraphArg, DataStateMachine dataStateMachineArg)
		{
			dataStateMachine = dataStateMachineArg;

			stateMachineGraph = stateMachineGraphArg;

			dataStateMachine.stateMachine = this;

			for (int i = 0; i < stateMachineGraph.stateTransitions.Length; i++)
			{
				StateTransition stateTransition = stateMachineGraph.stateTransitions[i];

				transitionsById.Add(stateTransition.idStateTransition, stateTransition);
			}

			for (int i = 0; i < stateMachineGraph.jobStates.Length; i++)
			{
				JobState jobState = stateMachineGraph.jobStates[i];

				jobsById.Add(jobState.idJobState, jobState);
			}

			for (int i = 0; i < stateMachineGraph.states.Length; i++)
			{
				StateNode state = stateMachineGraph.states[i];

				state.Initilize(this);

				statesById.Add(state.idState, state);

				state.jobMain = jobsById[state.idJob];

				state.jobMain.stateMachine = this;

				state.jobMain.state = state;

				state.jobMain.InitilizeGraph(state, dataStateMachine, this);
			}

			foreach (var item in transitionsById)
			{
				item.Value.Initilize(this);
			}

			stateTransitionTemp = transitionsById[TRANSITION_TEMP];

			for (int i = 0; i < stateMachineGraph.stateMachineScenarios.Length; i++)
			{
				StateMachineScenario stateMachineScenario = stateMachineGraph.stateMachineScenarios[i];

				scenarios.Add(stateMachineScenario.idScenario, stateMachineScenario);

				stateMachineScenario.InitilizeGraph(this);

				stateMachineScenario.states = new StateNode[stateMachineScenario.idStates.Length];

				for (int j = 0; j < stateMachineScenario.idStates.Length; j++)
				{
					stateMachineScenario.states[j] = statesById[stateMachineScenario.idStates[j]];
				}
				stateMachineScenario.stateLast = stateMachineScenario.states.Last();
			}

			isGraphBuilded = true;

			stateIdle = statesById[STATE_IDLE];
			stateCooldownRepeat = statesById[STATE_COOLDOWN_REAPEAT];
		}
	}
}