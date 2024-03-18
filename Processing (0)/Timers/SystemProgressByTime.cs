using IziHardGames.Core;
using IziHardGames.Extensions.PremetiveTypes;
using IziHardGames.Libs.NonEngine.Collections.Chunked;
using IziHardGames.Libs.NonEngine.Helpers;
using IziHardGames.Libs.NonEngine.Progressing;
using IziHardGames.Libs.NonEngine.StateMachines;
using System;
using ChunkedList = IziHardGames.Libs.NonEngine.Collections.Chunked.ChunkedDictionaryWithStatesT2<
IziHardGames.Libs.NonEngine.StateMachines.DataOfProgressingByTime,
IziHardGames.Libs.NonEngine.Progressing.IProgressCallbackRecieverByIn<IziHardGames.Libs.NonEngine.StateMachines.DataOfProgressingByTime>>;

namespace IziHardGames.Libs.NonEngine.Processing.Progressing
{
	/// <summary>
	/// Централизованное движение прогресса по времени.
	/// </summary>
	/// <remarks>
	/// Флаги для значений Int <see cref="ChunkedDictionaryWithStatesT1{T1}.chunksElementsStateFlags"/><br/>
	/// Справа налево по индексу разрядов:<br/>
	/// 0 - завершено?<br/>
	/// 1 - сбросить?<br/>
	/// 2 - удалить?<br/>
	/// </remarks>
	[Serializable]
	public class SystemProgressByTime : IInitializable, IDeinitializable
	{
		public static SystemProgressByTime Shared { get => GetOrCreateDefault(); }
		private static SystemProgressByTime shared;

		public ChunkedList items;

		private HandlerChunkedKeyT1 markingWithDelete;
		private HandlerChunkedKeyT1 markingWithReset;

		public void Initilize()
		{
			items = ChunkedList.SharedT2;
		}

		public void InitilizeReverse()
		{

		}

		public void ExecuteByDefault(float deltaTime)
		{
			ExecuteCalculation(deltaTime);
			CopyResultsAndCheckCompletion();
			items.ExecuteRemoves();
		}

		private void ExecuteCalculation(float deltaTime)
		{
			var chunks = items.GetChunksT2();

			for (int i = 0; i < items.Count; i++)
			{
				var chunk = chunks[i];
				for (int j = 0; j < chunk.Length; j++)
				{
					chunk.items[j].Execute(deltaTime);
				}
			}
		}
		/// <summary>
		/// Copy results to <see cref="IProgressCallbackReciever"/>
		/// </summary>
		private void CopyResultsAndCheckCompletion()
		{
			var chunks = items.GetChunksT2();

			for (int i = 0; i < items.Count; i++)
			{
				var chunk = chunks[i];

				for (int j = 0; j < chunk.Length; j++)
				{
					ref readonly var data = ref chunk.items[j];

					chunk.itemsT2[j].CopyProgressingResultByIn(in data);

					if (data.isComplete)
					{
						chunk.itemsT2[j].ProgressingCompleteCalback();
						items.MarkToRemove(i, j);
					}
				}
			}
		}


		/// <summary>
		/// Ставит прцоесс на отслеживание. 
		/// </summary>
		/// <typeparam name="TInit"></typeparam>
		/// <param name="initiator"> объект с ключом <see cref="KeyForChunk"/></param>
		/// <param name="time">время выполнения прогрессирования</param>
		/// <param name="pollToDelete"> 
		/// метод заверщения. Также определяет действие продолжения или конца.<br/>
		/// <see langword="true"/> - удаление из ослеживания / delete<br/>
		/// <see langword="false"/> - сбрасывает и начинает отсчет сначала /reset<br/>
		/// </param>
		/// <returns></returns>
		public void Begin<TInit>(TInit initiator, ref DataOfProgressingByTime initialProgress) where TInit : class, IProgressCallbackRecieverByIn<DataOfProgressingByTime>
		{
			IProgressCallbackRecieverByIn<DataOfProgressingByTime> trackableProgress = initiator;
			items.AddT2WithPostAllocation(initiator, ref initialProgress, ref trackableProgress);
		}
		public void EndByForce<TInit>(TInit initiator) where TInit : ISingleChunkedKeyHolder
		{
			ref KeyForChunk refKey = ref initiator.RefKey();
			items.Remove(initiator);
		}


		public ref DataOfProgressingByTime GetTaskProgress(in KeyForChunk chunkedKeyT1)
		{
			return ref items.RefGetT1(in chunkedKeyT1);
		}
		public float GetProgress(in KeyForChunk chunkedKeyT1)
		{
			ref DataOfProgressingByTime taskprogress = ref items.RefGetT1(in chunkedKeyT1);

			return taskprogress.progress01;
		}

		public void SetResetFlagsToDefault()
		{
			var chunksT1 = items.GetChunksT1();

			int countChunks = chunksT1.Count;

			for (int i = 0; i < countChunks; i++)
			{
				ChunkedList.ChunkWithStateT1 chunkT1 = chunksT1[i];

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

		public static SystemProgressByTime GetOrCreateDefault()
		{
			if (shared == null)
			{
				shared = CreateDefault();
			}
			return shared;
		}
		public static SystemProgressByTime CreateDefault()
		{
			SystemProgressByTime progressTracker = new SystemProgressByTime();

			progressTracker.Initilize();

			return progressTracker;
		}

	}
}