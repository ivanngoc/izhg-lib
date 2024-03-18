using IziHardGames.Libs.NonEngine.Collections.Chunked.MemoryBased;
using System;
using List2 = IziHardGames.Libs.NonEngine.Collections.Chunked.ChunkedDictionaryWithStatesT2<IziHardGames.Libs.NonEngine.Processing.JobForTimerSimple, System.Action>;
using List = IziHardGames.Libs.NonEngine.Collections.Dictionaries.WithOwnKey.DictionaryByIndexT2<IziHardGames.Libs.NonEngine.Processing.JobForTimerSimple, System.Action>;

namespace IziHardGames.Libs.NonEngine.Processing.Timers
{
	/// <summary>
	/// Simple System for delayed action executing.
	/// No callbacks. Only execution without break.
	/// Suitable for example for VFX that were triggered and needed to be utilized
	/// </summary>
	public class SystemCountdownTimer
	{
		public List list;
		public float deltaTime;

		public SystemCountdownTimer(int capacity)
		{
			list = new List(capacity);
		}

		public void Update()
		{
			Update(deltaTime);
		}
		public void Update(float deltaTime)
		{
			int count = list.Count;
			var jobs = list.GetT1();
			var rawActions = list.GetT2();

			for (int i = 0; i < count; i++)
			{
				jobs[i].Execute(deltaTime);
			}

			for (int i = 0; i < list.Count; i++)
			{
				if (jobs[i].isComplete)
				{
					rawActions[i].Invoke();
					list.RemoveByKeyInternal(i, true);
					i++;
				}
			}
		}

		public void Begin(Action actionToPerform, float time)
		{
			var job = new JobForTimerSimple(time);
			list.AddByRef(ref job, actionToPerform);
		}

		public void EndByForce(int externalKey)
		{
			list.Remove(externalKey, true);
		}
	}
}