using IziHardGames.Libs.NonEngine.Collections.Chunked;
using IziHardGames.Libs.NonEngine.Processing.Progressing;
using System;

namespace IziHardGames.Libs.NonEngine.StateMachines.Variant1
{
	/// <summary>
	/// Ребро в графе машины состояний<br/>
	/// В анимации нужен для сведения переходов двух состояний. В основном для наложения.
	/// При раюоте с данными может быть этапом передачи данных
	/// Два режима исполнения: растянутый и мгновенный. <see cref="isImmediate"/>
	/// </summary>
	/// <remarks>
	/// Ограничения ДЕЙСТВИЙ:<br/>
	/// При выполнении переходов, выполняется выход из предыдущего и вход в следующее состоние. При этом нужно обеспечить правильность выполнения послежовательности вызовов<br/>
	/// Последовательность может нарушиться если во время вызова перехода произоешь вход в первое состояние а из первого произошел вход во второе состояние при этом стэк вызова перехода еще не завершился
	/// Затем происходит выход из второго состояния и очистка данных, а стэк все еще продолжает выход из первого состояния и т.п.
	/// </remarks>
	[Serializable]
	public class StateTransition //: IProgressCallbackReciever
	{
		public int idStateTransition;
		public int idStateFrom;
		public int idStateTo;
		/// <summary>
		/// Выполняется мгновенно без времени выполнения [<see cref="transitionTime"/> = 0]
		/// </summary>
		public bool isImmediate;
		public bool isBreak;
		/// <summary>
		/// Флаг: переход был запущен
		/// </summary>
		public bool isTriggered;
		///// <summary>
		///// Флаг: переход отслеживается
		///// </summary>
		//public bool isListening;
		public float transitionTime;
		public float transitionTimeLeft;

		public float transitionProgress01;
		public float transitionPreviousEnd01;
		public float transitionNextStart01;

		private KeyForChunk chunkedKeyT1;
		/// <summary>
		/// Флаг: выход из <see cref="from"/> состоялся
		/// </summary>
		private bool isStatePreviousEnded;
		/// <summary>
		/// вход в <see cref="to"/> состоялся
		/// </summary>
		private bool isStateNextStarted;

		public static SystemProgressByTime progressTracker;
		public static ControlStateMachinesTransitions controlStateMachinesTransitions;

		[NonSerialized] private Func<bool> FuncCompleteHandler;
		//public bool Test() { throw new NotImplementedException(); }
		[NonSerialized] public StateNode from;
		[NonSerialized] public StateNode to;
		[NonSerialized] public DataStateMachine dataStateMachine;
		[NonSerialized] public StateMachineControl stateMachine;
		[NonSerialized] public bool isGraphBilded;
		//public Func<bool> Trans

		public void Initilize(StateMachineControl stateMachineArg)
		{
			if (!isGraphBilded && idStateTransition != StateMachineControl.TRANSITION_TEMP)
			{
				stateMachine = stateMachineArg;

				from = stateMachineArg.statesById[idStateFrom];
				to = stateMachineArg.statesById[idStateTo];

				FuncCompleteHandler = HandleProgressComplete;

				isGraphBilded = true;
			}
		}
		public ref KeyForChunk RefKey()
		{
			return ref chunkedKeyT1;
		}

		public void UpdateKey(in KeyForChunk newKey)
		{
			chunkedKeyT1 = newKey;
		}

		public void Start()
		{
			stateMachine.isTransitionTriggered = false;
			stateMachine.isTransitionActive = true;
			stateMachine.stateTransitionActive = this;
			// instanet execution
			if (isImmediate)
			{
				from.ExitComplete();
				to.Enter();
				End();
				Clean();
			}
			else
			{
				controlStateMachinesTransitions.Track(this);
				//progressTracker.Insert(this, FuncCompleteHandler, new ProgressByTime.TaskProgress(default, transitionTime));
			}
		}

		public void End()
		{
			if (isImmediate)
			{

			}
			else
			{
				controlStateMachinesTransitions.Track_De(this);
			}
			//to.stateComeFrom = from;
			stateMachine.stateTransitionTriggered = default;
			stateMachine.isTransitionActive = false;

			if (stateMachine.stateTransitionActive == this)
			{
				stateMachine.stateTransitionActive = this;
			}
		}

		public void Proceed()
		{
			transitionProgress01 = progressTracker.GetProgress(in chunkedKeyT1);

			if (transitionPreviousEnd01 <= transitionProgress01 && !isStatePreviousEnded)
			{
				isStatePreviousEnded = true;
				from.ExitComplete();
			}
			if (transitionNextStart01 <= transitionProgress01 && !isStateNextStarted)
			{
				isStateNextStarted = true;
				to.Enter();
			}
		}

		private bool HandleProgressComplete()
		{   //prevent skip call before 1f.
			Proceed();

			End();

			Clean();

			return false;
		}

		public void Break()
		{
			isBreak = true;

			if (isImmediate)
			{

			}
			else
			{
				controlStateMachinesTransitions.Track_De(this);
				//progressTracker.Extract(this);
			}
			Clean();

			throw new NotImplementedException();
		}

		public StateTransition CopyTo(StateTransition to)
		{
			throw new NotImplementedException();

			//return to;
		}

		public void Trigger()
		{
			isTriggered = true;
		}

		private void Clean()
		{
			transitionProgress01 = default;
			isStatePreviousEnded = default;
			isStateNextStarted = default;

			chunkedKeyT1 = default;
			isBreak = default;
			isTriggered = default;
		}

		public void CopyProgressingResult(float progressValue01)
		{
			transitionProgress01 = progressValue01;
		}
	}
}