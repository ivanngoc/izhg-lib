using System;
using System.Collections.Generic;
using System.Linq;

namespace IziHardGames.Libs.NonEngine.StateMachines.Variant1
{
	/// <summary>
	/// Машина состояний для сценарных и одиночных переходов<br/>
	/// Ветка переходов (линейных) для какого-то процесса. <br/>
	/// Нужен для автоматизации переходов по состояниям. Своего рода скрипттинг сценарий<br/>
	/// Должна быть возможность вставлять ситуативные сценарии. <br/>
	/// Например при проверке перед какого то состояния сценария вдруг необходимо выполнить другой сценарий не прерывая текущий<br/>
	/// А затем либо продолжить выполнение текущего либо выполнить его заново<br/>
	/// </summary>
	[Serializable]
	public class StateMachineScenario
	{
		public int idScenario;
		public int[] idStates;

		public int stageCurrent;

		[NonSerialized] public bool isGraphBuilded;
		[NonSerialized] public Queue<StateNode> statesQueue;
		[NonSerialized] public StateNode[] states;
		[NonSerialized] public StateNode stateLast;

		[NonSerialized] public StateMachineControl stateMachine;
		[NonSerialized] public Stack<StateMachineScenario> scenarios;
		[NonSerialized] public List<StateNode> statesList = new List<StateNode>();
#if UNITY_EDITOR
		public string name;
#endif

		public void SetIds()
		{
			idStates = statesList.Select(x => x.idState).ToArray();
		}
		public void InitilizeGraph(StateMachineControl arg)
		{
			if (!isGraphBuilded)
			{
				statesQueue = new Queue<StateNode>(5);
				scenarios = new Stack<StateMachineScenario>(2);

				isGraphBuilded = true;
				stateMachine = arg;
			}
		}

		public void Start()
		{
			stateMachine.scenarioActive = this;

			stageCurrent = default;

			statesQueue.Clear();

			for (int i = 1; i < states.Length; i++)
			{
				StateNode prev = states[i - 1];
				StateNode next = states[i];

				prev.nextState = next;
			}

			int count = states.Length - 1;

			for (int i = 0; i < count; i++)
			{
				states[i].kindNextProceed = 6;
			}
			///<see cref="StateNode.kindNextProceed"/>
			states.Last().kindNextProceed = 5;

			foreach (var item in states)
			{
				statesQueue.Enqueue(item);
			}
			GetNextState().Enter();
		}
		public void End()
		{
			stageCurrent = default;

			stateMachine.scenarioActive = default;

			stateLast.ExitComplete();
		}
		public void Stop()
		{
			statesQueue.Clear();
			stageCurrent = default;
		}
		public StateNode GetNextState()
		{
			stageCurrent++;

			return statesQueue.Dequeue();
		}
	}
}