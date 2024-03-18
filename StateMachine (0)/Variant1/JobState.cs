using IziHardGames.Libs.Engine.Memory;
using System;

namespace IziHardGames.Libs.NonEngine.StateMachines.Variant1
{
	/// <summary>
	/// Класс для наследования. Каждая операция пишется программистом
	/// </summary>
	[Serializable]
	public class JobState : IJobState, IPoolable
	{
		public int idJobState;
		/// <summary>
		///<see langword="true"/> - Выполняется мгновенно и не растянуто выполнение по кадрам и прочим иттерациям
		/// </summary>
		public bool isImmediate;
		[Obsolete("Unused")] public bool isEnded;
		/// <summary>
		/// Выполняется ли данная операция в текущий момент. Доступен только при <see cref="isImmediate"/>=<see langword="false"/>
		/// </summary>
		public bool isActive;
		/// <summary>
		/// Сколько длиться цикл. Зависит от <see cref="JobState"/>
		/// </summary>
		public float time;
		public float timeLeft;
		public float progress01;

		[NonSerialized] public bool isGraphBuilded;

		[NonSerialized] public StateNode state;
		[NonSerialized] public StateMachineControl stateMachine;
		[NonSerialized] public DataStateMachine dataStateMachine;

		public Action ActionStart { get; set; } = () => throw new NotImplementedException("Field is Not Set");
		public Action ActionEnd { get; set; } = () => throw new NotImplementedException("Field is Not Set");
		public Action ActionDo { get; set; } = () => throw new NotImplementedException("Field is Not Set");
		public Action ActionFallback { get; set; } = () => throw new NotImplementedException("Field is Not Set");
		public Action ActionBreak { get; set; } = () => throw new NotImplementedException("Field is Not Set");

		public virtual void InitilizeData()
		{

		}
		public virtual void InitilizeGraph(StateNode stateArg, DataStateMachine arg, StateMachineControl stateMachineArg)
		{
			if (!isGraphBuilded)
			{
				state = stateArg;
				dataStateMachine = arg;
				stateMachine = stateMachineArg;
			}
		}

		protected void ScheduleStartScenario(int idScenario)
		{
			//state.isExitScheduled = true;

			stateMachine.ScheduleScenarioStart(idScenario);
		}
		public virtual void Clean()
		{
			isEnded = default;

			timeLeft = default;
			progress01 = default;
		}

		public void CleanToReuse()
		{
			time = default;
			timeLeft = default;
			progress01 = default;
		}

		public void ReturnToPool()
		{
			throw new NotImplementedException();
		}
	}
}