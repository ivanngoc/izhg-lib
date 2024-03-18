using IziHardGames.Libs.Engine.Memory;
using System;
using System.Collections.Generic;

namespace IziHardGames.Ticking.Lib
{
	/// <summary>
	/// Task for update. Wrapping handler and periodicly execute.
	/// 1 object = 1 handler. <br/>
	/// Сделан для регистраций методов от систем, а не для регистрации каждого объекта который нужно обновлять. Системы должны обрабатывать массивы объектов внутри себя.
	/// Используется для вызовов обработчиков с высокой продолжительностью жизни (НЕ update untill, while, etc)
	/// </summary>
	/// <remarks>
	/// Related:<br/>
	/// <see cref="UpdateGroupe"/><br/>
	/// <see cref="UpdateChannelV1"/><br/>
	/// <see cref="UpdateStep"/><br/>
	/// <see cref="UpdateWithChannels"/><br/>
	/// <see cref="UpdateService{TDataProvider}"/><br/>
	/// </remarks>
	public class UpdateStep : IPoolable, IComparable<UpdateStep>
	{
		public static int idInitial;
		public readonly int id;
		public int Order => updateGroupe.IndexOf(this);
		public int priority;

		public bool isEnabled;
		public bool isFilterByTime;
		public bool isFilterByFrame;
		public bool isScaledDeltaTime;

		public int countFrameTarget;

		public float timeTarget;
		/// <summary>
		/// Сумма <see cref="timeDelta"/> c момента последней засечки
		/// </summary>
		public float timeAccomulated;

		public List<UpdateJob> updateJobs = new List<UpdateJob>();

		/// <summary>
		/// Action that will be called outside. One of member method of this object
		/// </summary>
		public Action trigger;
		private Action triggerWrap0;
		private Action triggerWrap1;
		private Action triggerWrap2;
		private Action triggerWrap3;
		private Action triggerWrap4;
		private readonly Action triggerFilteredByTime;
		private readonly Action triggerFilteredByFrame;
		private readonly Action run;

		private Action actionStop;
		public readonly Action callbackToStop;
		private readonly Action actionStopDefault;

		public Func<bool> funcCondition;
		public Func<bool> funcFilter;
		private readonly Func<bool> conditionReverse;

		private readonly Action actionJoinedCondition;
		private readonly Action actionDoExclusive;
		private readonly Action actionDoInclusive;
		private readonly Action actionFilterByFlag;
		private readonly Action actionFilterByFunc;

		public UpdateChannelV1 updateChannel;
		public UpdateGroupe updateGroupe;
		public UpdateControlToken updateToken;

		static UpdateStep()
		{
			idInitial = 1400;
		}
		public UpdateStep()
		{
			triggerFilteredByTime = TriggerFilteredByTime;
			triggerFilteredByFrame = TriggerFilteredByFrame;

			actionJoinedCondition = Do;
			actionDoExclusive = DoExclusive;
			actionDoInclusive = DoInclusive;
			actionFilterByFlag = DoIfIsEnabled;
			actionFilterByFunc = DoIfIsFunc;

			conditionReverse = ConditionReverse;
			actionStopDefault = Stop;
			actionStop = actionStopDefault;
			callbackToStop = actionStopDefault;

			run = Run;

			id = idInitial;
			idInitial++;
		}
		public void Initilize(UpdateControlToken token)
		{
			funcCondition = token.Condition;
			funcFilter = token.Filter;

			updateToken = token;
			ref var updateOptions = ref token.updateOptions;
			isFilterByFrame = updateOptions.frameRate != default;
			isFilterByTime = updateOptions.time != default;
			timeTarget = updateOptions.time;
			countFrameTarget = updateOptions.frameRate;
			isEnabled = true;

			BuildPipeline();
		}
		public void InitilizeReverse()
		{
			updateToken = default;
			throw new NotImplementedException();
		}
		private void BuildPipeline()
		{
			ref var updateOptions = ref updateToken.updateOptions;

			switch (updateOptions.eUpdateComposition)
			{
				case EUpdateComposition.None: break;
				case EUpdateComposition.Action:
					{
						trigger = run;
						break;
					}
				case EUpdateComposition.ActionJoinedCondition:
					{
						trigger = actionJoinedCondition;
						break;
					}
				case EUpdateComposition.ActionSeparateCondition:
					{
						trigger = run;
						break;
					}
			}

			switch (updateOptions.eUpdateLifespan)
			{
				case EUpdateLifespan.None: break;
				case EUpdateLifespan.Persistent: break;
				case EUpdateLifespan.WhileFunc: break;
				case EUpdateLifespan.UntilFunc: break;
				case EUpdateLifespan.FixedFrameCount: break;
				case EUpdateLifespan.FixedTime: break;
				default: break;
			}

			switch (updateOptions.eUpdateInclusive)
			{
				case EUpdateInclusive.None:
					break;
				case EUpdateInclusive.Inclusive:
					{
						triggerWrap0 = trigger;
						trigger = actionDoExclusive;
						break;
					}
				case EUpdateInclusive.Exclusive:
					{
						triggerWrap0 = trigger;
						trigger = actionDoInclusive;
						break;
					}
				default:
					break;
			}

			if (isFilterByFrame)
			{
				triggerWrap1 = trigger;
				trigger = triggerFilteredByFrame;
			}
			if (isFilterByTime)
			{
				triggerWrap2 = trigger;
				trigger = triggerFilteredByTime;
			}

			if (updateOptions.eUpdateFiltering.HasFlag(EUpdateFiltering.FlagEnabled))
			{
				triggerWrap3 = trigger;
				trigger = actionFilterByFlag;
			}
			if (updateOptions.eUpdateFiltering.HasFlag(EUpdateFiltering.FuncFilter))
			{
				triggerWrap4 = trigger;
				trigger = actionFilterByFunc;
			}
		}

		public UpdateJob CreateJob(UpdateControlToken token)
		{
			UpdateJob updateJob = UpdateJob.Wrap(token);
			updateJob.updateStep = this;
			updateJobs.Add(updateJob);
			return updateJob;
		}

		private void Run()
		{
			for (int i = 0; i < updateJobs.Count; i++)
			{
				updateJobs[i].isComplete = false;
				updateJobs[i].Execute();
			}

			while (true)
			{
				for (int i = 0; i < updateJobs.Count; i++)
				{
					if (!updateJobs[i].isComplete) continue;
				}
				break;
			}
		}

		#region Trigger
		private void TriggerFilteredByTime()
		{
			timeAccomulated += updateChannel.timeDelta;

			if (timeAccomulated > timeTarget)
			{
				triggerWrap2();
				timeAccomulated = default;
			}
		}
		private void TriggerFilteredByFrame()
		{
			if ((updateChannel.countFrames % countFrameTarget) == 0)
			{
				triggerWrap1();
			}
		}
		#endregion

		#region Update
		private void Do()
		{
			if (funcCondition())
			{
				actionStop();
			}
		}
		private void DoInclusive()
		{
			triggerWrap0();

			if (funcCondition())
			{
				actionStop();
			}
		}

		private void DoExclusive()
		{
			if (funcCondition())
			{
				actionStop();
			}
			else
			{
				triggerWrap0();
			}
		}
		#endregion

		#region Conditions for pipeline
		private void DoIfIsEnabled()
		{
			if (isEnabled)
			{
				triggerWrap3();
			}
		}
		private void DoIfIsFunc()
		{
			if (funcFilter())
			{
				triggerWrap4();
			}
		}
		#endregion

		private bool ConditionReverse() => !funcCondition.Invoke();
		public void Stop()
		{
			updateGroupe.RemoveSheduled(this);
		}

		public void CleanToReuse()
		{
			funcCondition = default;
			trigger = default;
		}
		public void ReturnToPool()
		{
			CleanToReuse();
			PoolObjects<UpdateStep>.Shared.Return(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="to"></param>
		/// <returns>
		/// -1 <br/>
		///  0 <br/>
		///  1 <br/>
		/// </returns>
		public int CompareTo(UpdateStep to)
		{
			ref var updateOptions = ref updateToken.updateOptions;

			if (updateOptions.priority < to.updateToken.updateOptions.priority)
			{
				return -1;
			}
			if (updateOptions.priority > to.updateToken.updateOptions.priority)
			{
				return 1;
			}
			return 0;
		}

		#region Static
		public static UpdateStep Wrap(UpdateControlToken token)
		{
			UpdateStep item = UpdateStep.Rent();
			item.Initilize(token);
			return item;
		}

		public static UpdateStep Rent()
		{
			UpdateStep updateAction = PoolObjects<UpdateStep>.Shared.Rent();
			return updateAction;
		}
		#endregion
	}
}