using IziHardGames.Core;
using IziHardGames.Core.Jobs;
using IziHardGames.Libs.NonEngine.Collections.Chunked;
using System;
using UnityEngine;

namespace IziHardGames.Libs.Engine.MovePatterns
{
	public readonly struct InputDataForMoveByTime : IInputDataForMove
	{
		public readonly Vector3 start;
		public readonly float time;
		public InputDataForMoveByTime(Vector3 start, float time)
		{
			this.start = start;
			this.time = time;
		}
		public InputDataForMoveByTime(ref Vector3 start, float time)
		{
			this.start = start;
			this.time = time;
		}
	}
	public readonly struct InputDataMoveFromPointToPoint : IInputDataForMove
	{
		public readonly Vector3 start;
		public readonly Vector3 end;
		public readonly float speed;
	}

	public readonly struct InputDataMoveForwardForDistance : IInputDataForMove
	{
		public readonly Vector3 start;
		public readonly Vector3 direction01;
		public readonly float distance;
		public readonly float speed;
	}
	public readonly struct InputDataMoveForwardForTime : IInputDataForMove
	{
		public readonly Vector3 start;
		public readonly Vector3 direction01;
		public readonly float distance;
		public readonly float speed;
	}

	public class SystemMove<TJob> : IMoveSystem
		where TJob : IMoveJobSelfContained
	{
		public static SystemMove<TJob> Shared { get => GetOrCreateDefault(); }
		private static SystemMove<TJob> shared;

		public const int MASS_UPDATE_THRESHOLD = 100;
		public ChunkedDictionaryWithStatesT2<TJob, IMoveDataRecieverByRef<TJob>> items;

		public SystemMove()
		{
			items = ChunkedDictionaryWithStatesT2<TJob, IMoveDataRecieverByRef<TJob>>.SharedT2;
			var method = System.Reflection.MethodBase.GetCurrentMethod();
		}

		public void InitilizeReverse()
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Insert into system for execution
		/// </summary>
		/// <typeparam name="TInit"></typeparam>
		/// <param name="initiator"></param>
		/// <returns></returns>
		public void MoveBegin<TInit>(TInit initiator, ref TJob initialData) where TInit : class, IMoveDataRecieverByRef<TJob>
		{
			IMoveDataRecieverByRef<TJob> initiatorAsInterface = initiator;
#if UNITY_EDITOR
			if (items.Contains(initiatorAsInterface))
			{
				throw new ArgumentException($"Duplicate T2. {initiator}");
			}
#endif
			items.AddT2WithPostAllocation(initiator, ref initialData, ref initiatorAsInterface);
		}
		/// <summary>
		/// Take from system for stop execution
		/// </summary>
		/// <typeparam name="TMove"></typeparam>
		/// <param name="initiator"></param>
		public void MoveEndByRequest<TInit>(TInit initiator) where TInit : class, ISingleChunkedKeyHolder
		{
			items.Remove(initiator);
		}

		public void Execute(float deltaTime)
		{
			CalculateMoveJob(deltaTime);
			CalculateCompletionAndRunCallbacks();
			ApplyChanges();
		}

		private void CalculateMoveJob(float deltaTime)
		{
			var chunks = items.GetChunksT2();

			for (int i = 0; i < chunks.Count; i++)
			{
				var chunk = chunks[i];
				for (int j = 0; j < chunk.Length; j++)
				{
					chunk.items[j].Execute(deltaTime);
				}
			}
		}


		private void CalculateCompletionAndRunCallbacks()
		{
			var chunks = items.GetChunksT2();

			for (int i = 0; i < chunks.Count; i++)
			{
				var chunk = chunks[i];
				for (int j = 0; j < chunk.Length; j++)
				{
					var endpoint = chunk.itemsT2[j];
					endpoint.RecieveMoveDataByRef(ref chunk.items[j]);

					if (chunk.items[j].IsCompleted)
					{
						items.MarkToRemove(i, j);
						endpoint.CompleteMoving();
					}
				}
			}
		}
		private void ApplyChanges()
		{
			items.ExecuteRemoves();
		}

		public static SystemMove<TJob> GetOrCreateDefault()
		{
			if (shared == null)
			{
				shared = CreateByDefaults();
			}
			return shared;
		}
		public static SystemMove<TJob> CreateByDefaults()
		{
			var systemMove = new SystemMove<TJob>();
			return systemMove;
		}
	}

	/// <summary>
	/// После каждой иттерации движения вызывается: <see cref="IMoveDataReciever.RecieveResultMove(TResult)"/> <br/>
	/// После достижения конечной точки вызывается: <see cref="IMoveCollbackReciever.CompleteMoving"/>
	/// </summary>
	/// <remarks>
	/// От ECS отличается наличием постоянного буфера результата
	/// </remarks>
	/// <typeparam name="TJob"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	public class SystemMove<TJob, TResult> : IInitializable, IDeinitializable, IMoveSystem
		where TJob : IJobStackable<float, TResult>, IStateResetable<TJob>
		where TResult : IJobResultCompletable
	{
		public static SystemMove<TJob, TResult> Shared => GetOrCreateDefault();
		private static SystemMove<TJob, TResult> shared;

		public ChunkedDictionaryWithStatesT3<TJob, TResult, IMoveDataReciever<TResult>> chunkedList;

		public void Initilize()
		{
			chunkedList = ChunkedDictionaryWithStatesT3<TJob, TResult, IMoveDataReciever<TResult>>.SharedT3;
		}
		public void InitilizeReverse()
		{
			ChunkedDictionaryWithStatesT3<TJob, TResult, IMoveCollbackReciever>.SharedT3.Clean();
			throw new NotImplementedException();
		}
		public void LoopExecute(float deltaTime)
		{
			var chunks = chunkedList.GetChunksT3();

			for (int i = 0; i < chunkedList.Count; i++)
			{
				var chunk1 = chunks[i].items;
				var chunk2 = chunks[i].itemsT2;
				var flagsSkip = chunks[i].flagsToSkip;

				for (int j = 0; j < chunks[i].Length; j++)
				{
					if (!flagsSkip[j])
					{
						chunk2[j] = chunk1[j].JobExecute(deltaTime, in chunk2[j]);
						Debug.Log(chunk2[j].ToString());
					}
				}
			}
		}
		public void LoopMark()
		{
			var chunks = chunkedList.GetChunksT3();

			for (int i = 0; i < chunkedList.Count; i++)
			{
				var chunk2 = chunks[i].itemsT2;
				var flags = chunks[i].flagsToSkip;

				for (int j = 0; j < chunks[i].Length; j++)
				{
					if (!flags[j])
					{
						if (chunk2[j].IsComplete)
						{
							chunkedList.MarkToRemove(i, j);
							chunkedList.MarkToCallback(i, j);
						}
					}
				}
			}
		}
		/// <summary>
		/// Transfer result 
		/// </summary>
		public void LoopCopyResultsOfRound()
		{
			var chunks = chunkedList.GetChunksT3();

			for (int i = 0; i < chunkedList.Count; i++)
			{
				var chunkt3 = chunks[i].itemsT3;
				var chunkt2 = chunks[i].itemsT2;
				var flags = chunks[i].flagsToSkip;

				for (int j = 0; j < chunks[i].Length; j++)
				{
					if (!flags[j])
					{
						chunkt3[j].RecieveResultOfMovingAtRound(chunkt2[j]);
					}
				}
			}
		}
		/// <summary>
		/// For each object that complete move signal of completion will be sended. 
		/// </summary>
		public void LoopCallbacks()
		{
			var chunks = chunkedList.GetChunksT3();

			for (int i = 0; i < chunkedList.Count; i++)
			{
				var callback = chunks[i].flagsToCallback;
				var chunkt3 = chunks[i].itemsT3;

				for (int j = 0; j < chunks[i].Length; j++)
				{
					if (callback[j])
					{
						chunkt3[j].CompleteMoving();
					}
				}
			}
		}
		public void LoopMakeListChanges()
		{
			chunkedList.ExecuteClearCallbacks();
			chunkedList.ExecuteRemoves();
		}
		public void Insert<T>(T initiator, TJob job, TResult initialState) where T : class, IMoveDataReciever<TResult>
		{
#if UNITY_EDITOR
			if (chunkedList.Contains(initiator))
			{
				throw new ArgumentException($"Duplicate T3. {initiator}");
			}
#endif
			chunkedList.AddT3(initiator, job, initialState, initiator);
		}
		public void Extract<T>(T initiator) where T : class, IMoveDataReciever<TResult>
		{
			chunkedList.Remove(initiator);
		}

		#region Static
		public static SystemMove<TJob, TResult> GetOrCreateDefault()
		{
			if (shared == null)
			{
				shared = CreateDefault();
			}

			return shared;
		}

		public static SystemMove<TJob, TResult> CreateDefault()
		{
			var v = new SystemMove<TJob, TResult>();
			v.Initilize();
			return v;
		}
		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TInput">Data that remains unchange during whole process</typeparam>
	/// <typeparam name="TConfig">Data that contain variable that affect result</typeparam>
	/// <typeparam name="TOutput">Result of moving</typeparam>
	public class SystemMove<TInput, TConfig, TOutput, TLink>
		where TInput : IInputDataForMove
		where TConfig : IMoveConfig
		where TOutput : struct, IJobResultCompletable
		where TLink : IMoveCollbackReciever, IMoveDataRecieverByIn<TOutput>
	{
		public ChunkedDictionaryWithStatesT4<TInput, TConfig, TOutput, TLink> items;
		public Action<float> actionExecute;
		public Action[] executionPipeline;

		public SystemMove(int capacity)
		{
			items = new ChunkedDictionaryWithStatesT4<TInput, TConfig, TOutput, TLink>(capacity);
			items.Initilize();
		}

		public void BuildPipeline(Action[] actions)
		{
			executionPipeline = actions;
		}

		public void ExecuteWithPipeline(float deltaTime)
		{
			actionExecute(deltaTime);
			for (int i = 0; i < executionPipeline.Length; i++)
			{
				executionPipeline[i].Invoke();
			}
		}
		public void Execute(float deltaTime)
		{
			actionExecute(deltaTime);
			CopyResult();
			ExecuteCallbacks();
			items.ExecuteRemoves();
		}

		/// <summary>
		/// 
		/// </summary>
		public void CopyResult()
		{
			var chunks = items.GetChunksT4();

			for (int i = 0; i < items.Count; i++)
			{
				var chunk = chunks[i];

				for (int j = 0; j < chunk.Length; j++)
				{
					chunk.itemsT4[j].RecieveResultOfMovingAtRound(in chunk.itemsT3[j]);
				}
			}
		}
		public void ExecuteCallbacks()
		{
			var chunks = items.GetChunksT4();
			for (int i = 0; i < items.Count; i++)
			{
				var chunk = chunks[i];
				for (int j = 0; j < chunk.Length; j++)
				{
					/// deletes suppose to be marked at <see cref="actionExecute"/>
					if (chunk.flagsToCallback[j])
					{
						chunk.itemsT4[j].CompleteMoving();
					}
				}
			}
		}


		public void MoveBegin<TInit>(ref TInput input, ref TConfig config, ref TOutput initialToStack, TLink link, TInit initiator) where TInit : ISingleChunkedKeyHolder
		{
#if UNITY_EDITOR
			if (items.Contains(link))
			{
				throw new ArgumentException($"Duplicate T4. {initiator}");
			}
#endif
			items.AddWithPreAlocT4(initiator, ref input, ref config, ref initialToStack, ref link);
		}
		public void MoveEndByRequest<TInit>(TInit item) where TInit : ISingleChunkedKeyHolder
		{
			items.Remove(item);
		}
	}

	public struct DataMoveConfigBySpeedAndDirection : IMoveConfig
	{
		public float speedPerSec;
		public Vector3 directionNormilized;

		public DataMoveConfigBySpeedAndDirection(float speedPerSec, Vector3 direction)
		{
			this.speedPerSec = speedPerSec;
			directionNormilized = direction;
		}
	}

	public interface IMoveConfig
	{

	}
}