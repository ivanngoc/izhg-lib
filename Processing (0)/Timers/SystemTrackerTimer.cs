using IziHardGames.Core;
using IziHardGames.Extensions.PremetiveTypes;
using IziHardGames.Libs.NonEngine.Collections.Chunked;
using IziHardGames.Libs.NonEngine.Helpers;
using System.Collections.Generic;
using Chunk = IziHardGames.Libs.NonEngine.Collections.Chunked.ChunkedDictionaryWithStatesT2<IziHardGames.Libs.NonEngine.Processing.JobForTimer, IziHardGames.Libs.NonEngine.Processing.ITrackableByTimer>;

namespace IziHardGames.Libs.NonEngine.Processing.Timers
{
	/// <summary>
	/// так как не зависит от других данных. Изменения в него вносятся либо им самим либо в конце фрейма. 
	/// Расчитанный этим трэкером результат потребляется познее всеми, кто встал в отслеживание
	/// </summary>
	public class SystemTrackerTimer : IUpdatableDeltaTime, IInitializable, IDeinitializable
	{
		public static SystemTrackerTimer Shared { get => GetOrCreateDefault(); }
		private static SystemTrackerTimer shared;

		public int Priority { get => orderNumber; }
		public int orderNumber = int.MinValue;

		public Chunk chunkedList;

		private List<ITrackableByTimer> toDelete = new List<ITrackableByTimer>(100);

		#region Unity Message


		#endregion

		public void Initilize()
		{
			chunkedList = Chunk.SharedT2;
		}

		public void InitilizeReverse()
		{
			chunkedList.Clean();
		}
		public void ExecuteUpdateWithDelta(float deltaTime)
		{
			var chunks = chunkedList.GetChunksT2();

			for (int i = 0; i < chunkedList.Count; i++)
			{
				var chunk = chunks[i].items;

				for (int j = 0; j < chunks[i].Length; j++)
				{
					chunk[j].Countdown(deltaTime);
				}
			}
			int resetCounts = default;

			// checks
			for (int i = 0; i < chunkedList.Count; i++)
			{
				var chunk = chunks[i].items;
				var chunk2 = chunks[i].itemsT2;

				for (int j = 0; j < chunks[i].Length; j++)
				{
					if (chunk[j].timeLeft < 0)
					{
						KeyForChunk chunkedKeyT1 = new KeyForChunk(i, j);

						if (chunk[j].isPeriodic)
						{
							chunkedList.MarkToReset(in chunkedKeyT1);
							resetCounts++;
						}
						else
						{
							toDelete.Add(chunk2[j]);
						}
						chunkedList.MarkToCallback(in chunkedKeyT1);
					}
				}
			}
			// callbacks
			for (int i = 0; i < chunkedList.Count; i++)
			{
				var callback = chunks[i].flagsToCallback;
				var chunk2 = chunks[i].itemsT2;

				for (int j = 0; j < chunks[i].Length; j++)
				{
					if (callback[j])
					{
						chunk2[j].OnTimerEndHandler.Invoke();
					}
				}
			}

			if (toDelete.Count > 0)
			{
				foreach (var movable in toDelete)
				{
					TrackReverse(movable);
				}
				toDelete.Clear();
			}
			chunkedList.UpdateCurrentPointer();

			if (resetCounts > 0)
			{
				SetResetFlagsToDefault();
			}
		}

		public void Insert<T>(T inititator, float timeStart) where T : class, ITrackableByTimer
		{
			chunkedList.AddT2WithPostAllocation(inititator, new JobForTimer(timeStart, default, inititator.TimeTrackTimer, inititator.TimeTrackTimer, default), inititator);
		}

		public void TrackReverse<T>(T inititator) where T : ITrackableByTimer
		{
			chunkedList.Remove(inititator);
		}

		public void SetResetFlagsToDefault()
		{
			var chunksT1 = chunkedList.GetChunksT1();

			int countChunks = chunksT1.Count;

			for (int i = 0; i < countChunks; i++)
			{
				Chunk.ChunkWithStateT1 chunkT1 = chunksT1[i];

				for (int j = 0; j < chunkT1.Length; j++)
				{
					if (chunkT1.flagsStatePerElement[j].IsBitIsOne(ChunkConstants.MASK_BIT_FLAG_RESET))
					{
						chunkT1.items[j] = chunkT1.items[j].StateReset();
						chunkT1.flagsStatePerElement[j] = HelperFlagsInt.BitFlagWithMaskReset(ChunkConstants.MASK_BIT_FLAG_RESET, chunkT1.flagsStatePerElement[j]);
					}
				}
			}
		}

		public static SystemTrackerTimer GetOrCreateDefault()
		{
			if (shared == default)
			{
				shared = CreateDefault();
			}
			return shared;
		}

		public static SystemTrackerTimer CreateDefault()
		{
			SystemTrackerTimer trackerTimer = new SystemTrackerTimer();

			trackerTimer.Initilize();

			return trackerTimer;
		}
	}
}