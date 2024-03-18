using IziHardGames.Libs.Engine.Memory;
using System;
using System.Collections.Generic;

namespace IziHardGames.Libs.NonEngine.StateMachines.Variant1
{
	/// <summary>
	/// Набор работ и данных характеризующих конкретное состояние<br/>
	/// Вуршина в графе машины состояний
	/// </summary>
	[Serializable]
	public class StateNode : IState, IPoolable
	{
		public int idState;
		public int idJob;
		/// <summary>
		/// <see cref="Fallback"/>
		/// </summary>
		public int idStateFallback;
		/// <summary>
		/// <see langword="true"/> use <see cref="StateTransition"/><br/>
		/// <see langword="false"/> use direct <see cref="Enter"/>
		/// </summary>
		public bool isStateFallbackWithTransition;

		public int repeatCount;
		public int repeatCountLeft;
		/// <summary>
		/// Определяет что будет делать метод <see cref="CompleteJobCallback"/><br/>
		/// 0 - не инициализирован<br/>
		/// 1 - <see cref="Proceed"/> -продолжить сценарий. Переход в <see cref="nextState"/><br/>
		/// 2 - <see cref="Fallback"/> - вернуться в предыдущее состояние <see cref="idStateFallback"/><br/>
		/// 3 - <see cref="Repeat"/><br/>
		/// 4 - <see cref="ExitToDefaultState"/><br/>
		/// 5 - <see cref="ExitFromScenario"/><br/>
		/// 6 - <see cref="ProceedNextScenarioState"/><br/>
		/// 7 - <see cref=""/> cooldown then repeat
		/// 8 - <see cref=""/> Enter Scheduled and will executed in <see cref="ControlStateMachinesTransitions.ExecuteUpdateWithDelta(float)"/>
		/// </summary>
		public int kindNextProceed;
		/// <summary>
		/// Повторять указанное количество раз
		/// </summary>
		public bool isRepeat;
		/// <summary>
		/// В случае если переход из текущего (выход) невозможен - повторить текущее состояние.  
		/// </summary>
		public bool isRepeatOnTransitionFail;
		/// <summary>
		/// Повторять бесконечно
		/// </summary>
		public bool isRepeatInfinite;
		///// <summary>
		///// Выход из состояния запланирован
		///// </summary>
		//public bool isExitScheduled;

		public bool isEnterScheduled;

		public int[] idsTransitionsIn;
		public int[] idsTransitionsOut;

		[NonSerialized] public bool isGraphBuilded;
		[NonSerialized] public List<StateTransition> transitionsIn;
		[NonSerialized] public List<StateTransition> transitionsOut;

		[NonSerialized] public Action actionCompleteJobCallback;
		///// <summary>
		///// Состояние для возврата после выполнения
		///// </summary>
		//[JsonIgnore, NonSerialized] private State fallback;
		///// <summary>
		///// Выполняется 1 раз при входе в состояние. Оповещает о входе в состояние
		///// </summary>
		//[JsonIgnore, NonSerialized] private JobState jobEnter;
		///// <summary>
		///// Выполняется 1 раз при выходе из состояния. Оповещает о выходе из состояния
		///// </summary>
		//[JsonIgnore, NonSerialized] private JobState jobExit;
		/// <summary>
		/// Выполняется после <see cref="jobEnter"/> каждый последующий раз до момента выхода
		/// </summary>
		[NonSerialized] public JobState jobMain;
		[NonSerialized] public StateNode stateComeFrom;
		/// <summary>
		/// Если состояния задействовано в сценарии то во время старта этого сценария в это поле записывается следующее за этим состояние
		/// </summary>
		[NonSerialized] public StateNode nextState;
		/// <summary>
		/// Переход который нужно осуществить немедля
		/// </summary>
		[NonSerialized] public StateTransition stateTransitionTriggered;
		[NonSerialized] public StateMachineControl stateMachine;

		public static ControlStateMachinesTransitions controlStateMachinesTransitions;
#if UNITY_EDITOR
		public static Action<StateNode> debugMathodEnter;
		public static Action<StateNode> debugMathodExit;
		public static Action<StateNode> debugMathodFallback;
#endif



		public void Initilize(StateMachineControl stateMachineArg)
		{
			if (!isGraphBuilded)
			{
				stateMachine = stateMachineArg;
				actionCompleteJobCallback = CompleteJobCallback;

				transitionsIn = new List<StateTransition>(10);
				transitionsOut = new List<StateTransition>(10);

				foreach (var item in idsTransitionsIn)
				{
					transitionsIn.Add(stateMachineArg.transitionsById[item]);
				}
				foreach (var item in idsTransitionsOut)
				{
					transitionsOut.Add(stateMachineArg.transitionsById[item]);
				}
				isGraphBuilded = true;
			}
		}


		/// <summary>
		/// <see cref="jobEnter"/> 
		/// Оповещение связанных объектов о входе
		/// </summary>
		public void Enter()
		{
#if UNITY_EDITOR
			debugMathodEnter?.Invoke(this);
#endif
			stateMachine.idStateCurrent = idState;
			stateMachine.stateLastEnter = this;
			stateComeFrom = stateMachine.stateLastExit;

			controlStateMachinesTransitions.Track(this);

			jobMain.ActionStart();
		}
		/// <summary>
		/// Вызывается в работе по окончанию. метод решает повторить работу или переключиться в другое состояние
		/// </summary>
		public void CompleteJobCallback()
		{
			switch (kindNextProceed)
			{
				case 0: throw new ArgumentException($"Wrong value = [{kindNextProceed}]");
				case 1: Proceed(); break;
				case 2: Fallback(); break;
				case 3: Repeat(); break;
				case 4: goto case 0;
				case 5: stateMachine.ScenarionEnd(); break;
				case 6: ProceedNextScenarioState(); break;
				case 7: CooldownReapeat(); break;
				case 8: break;
				default: goto case 0;
			}
		}
		/// <summary>
		/// Выход из состояния не дожидаясь завершения состояния
		/// </summary>
		public void ExitBreak()
		{
#if UNITY_EDITOR
			debugMathodExit?.Invoke(this);
#endif
			if (jobMain.isActive)
			{
				stateMachine.stateLastExit = this;

				jobMain.ActionBreak();

				jobMain.isActive = false;

				controlStateMachinesTransitions.Track_De(this);
			}
			else
			{
				ExitComplete();
			}
		}
		/// <summary>
		/// <see cref="jobExit"/>
		/// Оповещение связанных объектов о выходе
		/// </summary>
		public void ExitComplete()
		{
			stateMachine.stateLastExit = this;
#if UNITY_EDITOR
			debugMathodExit?.Invoke(this);
#endif
			jobMain.ActionEnd();

			controlStateMachinesTransitions.Track_De(this);
		}

		#region CompleteJobCallback handlers
		private void ProceedNextScenarioState()
		{
			stateMachine.ScenarionMoveToNext();
		}
		/// <summary>
		/// Перейти к состоянию в <see cref="nextState"/>. Значение в поле пишет <see cref="StateMachineScenario"/>
		/// </summary>
		private void Proceed()
		{
			if (IsHasTransitionTo(nextState, out StateTransition stateTransition))
			{
				stateMachine.StateTransitTrigger(stateTransition);
			}
			else
			{
				stateMachine.StateTransitStraight(nextState);
			}
		}
		/// <summary>
		/// Возврат в последнее состояние откуда пришел
		/// </summary>
		public void Fallback()
		{
#if UNITY_EDITOR
			debugMathodFallback?.Invoke(this);
#endif
			if (isStateFallbackWithTransition)
			{
				stateMachine.StateTransit(idStateFallback);
			}
			else
			{
				stateMachine.ScheduleStateExitComplete(this);
				stateMachine.ScheduleStateEnter(idStateFallback);
			}
		}

		private void Repeat()
		{
			throw new NotImplementedException();
		}

		private void CooldownReapeat()
		{
			stateMachine.ScheduleStateExitComplete(this);

			stateMachine.ScheduleStateEnter(StateMachineControl.STATE_COOLDOWN_REAPEAT);
		}

		#endregion
		public void TriggerTransition(int idStateTransition)
		{
			foreach (var item in transitionsOut)
			{
				if (item.idStateTransition == idStateTransition)
				{
					stateTransitionTriggered = item;

					item.Trigger();

					return;
				}
			}
			throw new NullReferenceException($"NOT FOUNDED TRANSITION WITH ID {idStateTransition}");
		}

		public StateNode CopyTo(StateNode to)
		{
			to.idState = idState;
			to.idJob = idJob;

			//to.time = this.time;
			//to.timeLeft = this.timeLeft;
			//to.progress01 = this.progress01;
			to.repeatCount = repeatCount;
			to.repeatCountLeft = repeatCountLeft;
			to.isRepeat = isRepeat;
			to.isRepeatOnTransitionFail = isRepeatOnTransitionFail;
			to.isRepeatInfinite = isRepeatInfinite;
			//to.isExitScheduled = this.isExitScheduled;

			to.transitionsIn.AddRange(transitionsIn);
			to.transitionsOut.AddRange(transitionsOut);
			// nonserilized
			to.jobMain = jobMain;

			return to;
		}

		public bool IsHasTransitionTo(int to)
		{
			for (int i = 0; i < idsTransitionsOut.Length; i++)
			{
				if (idsTransitionsOut[i] == to) return true;
			}
			return false;
		}
		public bool IsHasTransitionTo(StateNode to, out StateTransition stateTransition)
		{
			return IsHasTransitionTo(to.idState, out stateTransition);
		}
		public bool IsHasTransitionTo(int idState, out StateTransition stateTransition)
		{
			for (int i = 0; i < idsTransitionsOut.Length; i++)
			{
				if (idsTransitionsOut[i] == idState)
				{
					stateTransition = transitionsOut[i];

					return true;
				}
			}
			stateTransition = default;

			return false;
		}
		public void CleanToReuse()
		{
			idState = default;
			idJob = default;


			repeatCount = default;
			repeatCountLeft = default;
			isRepeat = default;
			isRepeatOnTransitionFail = default;
			isRepeatInfinite = default;
			//isExitScheduled = default;

			transitionsIn.Clear();
			transitionsOut.Clear();

			jobMain = default;
			stateTransitionTriggered = default;
		}
		public void ReturnToPool()
		{
			PoolObjects<StateNode>.Shared.ReturnReusable(this);
		}
	}
}