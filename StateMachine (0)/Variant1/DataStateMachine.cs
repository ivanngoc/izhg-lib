using IziHardGames.Libs.Engine.Memory;
using IziHardGames.Libs.NonEngine.StateMachines.Datas;
using System;

namespace IziHardGames.Libs.NonEngine.StateMachines.Variant1
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class StateMachineGraph
	{
		public int[] idsStates;
		public int[] idsTransitions;
		public int[] idsScenarios;
		public int[] idsJobs;

		//[NonSerialized] 
		public StateNode[] states;
		//[NonSerialized]
		public JobState[] jobStates;
		//[NonSerialized] 
		public StateTransition[] stateTransitions;
		//[NonSerialized]
		public StateMachineScenario[] stateMachineScenarios;
	}

	/// <summary>
	/// Хранит данные по сотояниям объекта и их взаимосвязи. 
	/// Хранить общме данные о машине состояний: кому принадлежит, общие для состояний этого объекта данные
	/// Также может хранить данные по переключениям статусов. Fallback, ReturnState, IddleState и т.п.
	/// </summary>
	[Serializable]
	public class DataStateMachine : IDataOfStateMachine, IPoolable
	{
		public int idDataState;
		public int idStateMachine;

		[NonSerialized] public StateMachineControl stateMachine;

		public virtual void CleanToReuse()
		{
			idDataState = default;
		}

		public virtual void ReturnToPool()
		{

		}
		//public int state;
	}
}