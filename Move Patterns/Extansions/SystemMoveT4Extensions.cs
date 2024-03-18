using IziHardGames.Libs.NonEngine.Collections.Chunked;

namespace IziHardGames.Libs.Engine.MovePatterns
{
	public static class SystemMoveT4Extensions
	{
		public static void ExecuteResultCalculation<TLink>(this SystemMove<InputDataForMoveByTime, DataMoveConfigBySpeedAndDirection, JobMoveResult, TLink> system, float deltaTime)
			where TLink : IMoveCollbackReciever, IMoveDataRecieverByIn<JobMoveResult>
		{
			var items = system.items;
			var chunks = items.GetChunksT4();

			for (int i = 0; i < items.Count; i++)
			{
				var chunk = chunks[i];

				for (int j = 0; j < chunk.Length; j++)
				{
					ref var input = ref chunk.items[j];
					ref var config = ref chunk.itemsT2[j];
					ref var prevResult = ref chunk.itemsT3[j];

					if (prevResult.value > deltaTime)
					{
						float timeLeft = prevResult.value - deltaTime;
						chunk.itemsT3[j] = new JobMoveResult(false, prevResult.position + (chunk.itemsT2[j].directionNormilized * (config.speedPerSec * deltaTime)), timeLeft);
					}
					else
					{
						float timeLeft = prevResult.value;
						chunk.itemsT3[j] = new JobMoveResult(true, prevResult.position + (chunk.itemsT2[j].directionNormilized * (config.speedPerSec * timeLeft)), 0);
						chunk.flagsToCallback[j] = true;
						chunk.flagsStatePerElement[j] |= ChunkConstants.MASK_BIT_FLAG_REMOVE;
					}
				}
			}
		}

		public static void CopyResult<TLink>(this SystemMove<InputDataForMoveByTime, DataMoveConfigBySpeedAndDirection, JobMoveResult, TLink> system)
				where TLink : IMoveCollbackReciever, IMoveDataRecieverByIn<JobMoveResult>
		{
			var items = system.items;
			var chunks = items.GetChunksT4();

			for (int i = 0; i < items.Count; i++)
			{
				var chunk = chunks[i];

				for (int j = 0; j < chunk.Length; j++)
				{


				}
			}
		}
	}
}