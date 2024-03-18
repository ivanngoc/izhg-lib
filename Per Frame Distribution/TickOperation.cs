using IziHardGames.Ticking.Lib;
using System;
using System.Collections.Generic;

namespace IziHardGames.Libs.Engine.PerFrameDistribution
{
	/// <summary>
	/// Single action
	/// </summary>
	public interface IOperation
	{

	}

	public class Operation : IOperation
	{
		public bool isDone;
		public Action handler;
	}

	public class OperationDistributedInFrames : Operation
	{

	}
	public class OperationDistributedInFramesWithCallback : Operation
	{

	}

	/// <summary>
	/// Операция с гарантирвоанным завершением и выполняющаяся в нескольких кадрах.
	/// Single object mode.
	/// </summary>
	public class TickOperation
	{
		public Action handler;
		public int startTick;
	}

	public class TickOperationState
	{
		public int ticksLeft;
		public bool isRepeat;
		public bool isRepeatWhile;
		public bool isRepeatUntil;
		public Func<bool> conditionWhile;
		public Func<bool> conditionUntil;
		public TickOperation tickOperation;
		public Action callback;
		public TickOperationsProcessor processor;
		public bool isCompleted;

		/// <summary>
		/// Delete Self after execution over
		/// </summary>
		public void Discard()
		{

		}
	}

	public class TickOperationsFactory
	{
		public TickOperationsProcessor tickOperationsProcessor;
	}
	public class TickOperationsProcessor
	{
		public IUpdateService updateService;
		public List<TickOperationState> tickOperationStates;
		public List<TickOperationState> tickOperationStatesToAdd;
		public List<TickOperationState> tickOperationStatesToRemove;
	}
}