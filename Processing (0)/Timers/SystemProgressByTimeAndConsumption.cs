using IziHardGames.Core;
using IziHardGames.Libs.NonEngine.Collections.Chunked;
using IziHardGames.Libs.NonEngine.Progressing;
using System;
//using static IziHardGame.Libs.NonEngine.Collections.Chunks.ChunkedList<>

namespace IziHardGames.Libs.NonEngine.Processing.Progressing
{
	/// <summary>
	/// Прогресс зависящий от времени и от потребления неких ресурсов для продвижения прогресса с определенным временным интервалом опроса
	/// Например: тратить ресурс золота раз в 0.5 сек. Если не хватает ресурса то прогресса не будет, как только хватит прогресс снова начнет идти
	/// </summary>
	/// 1 TEndpoint = 1 process
	public class SystemProgressByTimeAndConsumption : IUpdatableStage
	{
		public static SystemProgressByTimeAndConsumption Shared => GetOrCreate();
		private static SystemProgressByTimeAndConsumption shared;

		private ChunkedDictionaryWithStatesT3<JobForFeedProgress, IProgressFeed, ResultFeedProgress> chunkedList;

		public void Initilize()
		{
			chunkedList = ChunkedDictionaryWithStatesT3<JobForFeedProgress, IProgressFeed, ResultFeedProgress>.SharedT3;
		}

		#region Control
		public void Start<TEndpoint>(TEndpoint endpoint, JobForFeedProgress initial) where TEndpoint : class, IProgressFeed
		{
			chunkedList.AddT3(endpoint, initial, endpoint, default);
		}

		#endregion

		#region Processing
		/// <summary>
		/// Добавляет новые объекты в список
		/// </summary>
		public void LoopAdd()
		{
			throw new NotImplementedException();
		}
		public void LoopRemove()
		{
			throw new NotImplementedException();
		}

		public void LoopUpdate(float deltaTime)
		{
			var chunks = chunkedList.GetChunksT3();

			for (int i = 0; i < chunkedList.Count; i++)
			{
				var chunk = chunks[i].items;
				var chunk2 = chunks[i].itemsT2;
				var chunk3 = chunks[i].itemsT3;
				var skip = chunks[i].flagsToSkip;

				for (int j = 0; j < chunks[i].Length; j++)
				{
					if (!skip[j])
					{
						chunk[j].UpdateTime(deltaTime);
						chunk3[j] = chunk[j].Calculate();
					}
				}
			}

		}
		public void LoopFeed()
		{
			var chunks = chunkedList.GetChunksT3();

			for (int i = 0; i < chunkedList.Count; i++)
			{
				var chunk = chunks[i].items;
				var chunk2 = chunks[i].itemsT2;
				var skip = chunks[i].flagsToSkip;

				for (int j = 0; j < chunks[i].Length; j++)
				{
					if (!skip[j])
					{
						if (chunk[j].isFeedTime)
						{
							if (chunk2[j].TryFeed())
							{
								chunk[j].isFeedTime = false;
								chunk[j].ResetTime(chunk2[j].GetTime());
							}
							else
							{
								chunk2[j].IsFeedAwait = true;
								chunkedList.MarkToSkip(i, j);
							}
						}
					}
				}
			}
		}

		public void LoopCallback()
		{
			var chunks = chunkedList.GetChunksT3();

			for (int i = 0; i < chunkedList.Count; i++)
			{
				var chunk2 = chunks[i].itemsT2;
				var chunk3 = chunks[i].itemsT3;

				for (int j = 0; j < chunks[i].Length; j++)
				{
					if (chunk3[j].isComplete)
					{
						chunk2[i].Complete(chunk3[j]);

						if (chunk2[i].IsToRepeatProgressing)
						{
							chunk2[i].Repeat();
						}
						else
						{
							chunkedList.MarkToRemove(i, j);
						}
					}
				}
			}
			chunkedList.ExecuteRemoves();
		}
		#endregion
		public void Resume<TEndpoint>(TEndpoint endpoint) where TEndpoint : class, IProgressFeed
		{
			ref var key = ref endpoint.RefKey();
			chunkedList.MarkToSkipReverse(in key);
		}

		public static SystemProgressByTimeAndConsumption GetOrCreate()
		{
			if (shared == null)
			{
				shared = CreateDefault();
			}
			return shared;
		}
		public static SystemProgressByTimeAndConsumption CreateDefault()
		{
			SystemProgressByTimeAndConsumption progressByTimeAndFeed = new SystemProgressByTimeAndConsumption();

			progressByTimeAndFeed.Initilize();

			return progressByTimeAndFeed;
		}
	}
}