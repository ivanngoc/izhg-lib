using IziHardGames.Core;
using IziHardGames.Core.Jobs;
using IziHardGames.Libs.NonEngine.Collections.Chunked;
using System;
using UnityEngine;
//IziHardGame.Libs.Engine.MovePatterns
namespace IziHardGames.Libs.Engine.MovePatterns
{
	public readonly struct DataTakMove
	{
		public readonly int id;
		public readonly Vector3 start;
		public readonly Vector3 end;
	}
	public readonly struct DataTakMoveWithDirection
	{
		public readonly int id;
		public readonly Vector3 start;
		public readonly Vector3 end;
		public readonly Vector3 direction;
	}
	public readonly struct JobMoveByDistanceResult : IJobResultCompletable
	{
		public bool IsComplete { get => distanceLeft < 0; }

		public readonly Vector3 position;
		public readonly float distanceLeft;

		public JobMoveByDistanceResult(Vector3 position, float distanceLeft)
		{
			this.position = position;
			this.distanceLeft = distanceLeft;
		}

		public override string ToString()
		{
			return $"distanceLeft {distanceLeft} | Pos ({position.x},{position.y},{position.z}) | IsComplete {IsComplete}";
		}
	}
	public readonly struct JobMoveResult : IJobResultCompletable
	{
		public bool IsComplete { get => isComplete; }
		public readonly bool isComplete;
		/// <summary>
		/// 
		/// </summary>
		public readonly Vector3 position;
		/// <summary>
		/// какая то величина (оставшееся время/дистанция и т.д.)
		/// </summary>
		public readonly float value;

		public JobMoveResult(bool isComplete, Vector3 position, float value)
		{
			this.isComplete = isComplete;
			this.position = position;
			this.value = value;
		}
	}

	/// <summary>
	/// Нет точного позиционирования кнечной точки. Расчет может перескочить конечную точку, подходит для пуш движений. 
	/// Экономия ресурсов процессора и памяти за счет отсутствия clamp функций
	/// </summary>
	public readonly struct JobMoveByDirectionAndSpeed : IJobStackable<float, JobMoveByDistanceResult>, IStateResetable<JobMoveByDirectionAndSpeed>
	{
		public readonly Vector3 directionNormilized;
		public readonly float speedPerSec;

		public JobMoveByDirectionAndSpeed(Vector3 directionNormilized, float speedPerSec)
		{
			this.directionNormilized = directionNormilized;
			this.speedPerSec = speedPerSec;
		}

		public JobMoveByDistanceResult JobExecute(float deltaTime, in JobMoveByDistanceResult jobMoveResult)
		{
			float length = speedPerSec * deltaTime;

			return new JobMoveByDistanceResult(jobMoveResult.position + directionNormilized * (length), jobMoveResult.distanceLeft - length);
		}
		public JobMoveByDirectionAndSpeed StateReset()
		{
			return new JobMoveByDirectionAndSpeed();
		}
	}

	#region Self contained. Solid
	#endregion

	[Serializable]
	public struct JobMoveByTimeAndDirection
	{
		public Vector3 direction;
		public float speed;
	}

	[Serializable]
	public struct JobMoveByTimeAndDirectionResult
	{
		public float timeLeft;
		public Vector3 position;
	}
	/// <summary>
	/// Движение по времени подходит для линейных простоых движений или в случаая постоянства времени за которое нужно пройти расстояние
	/// В процессе выполнения движения скорость по факту будет регулироваться дельтой времени
	/// </summary>
	[Serializable]
	public struct JobMoveByTimeAndDistance : IMoveJobToTarget<JobMoveByTimeAndDistance>, IExecutable<float>
	{
		public float time;
		public float timePass;
		public float progress;
		public float distance;

		public void Set(float timeArg, float velocityArg)
		{
			time = timeArg;
			distance = timeArg * velocityArg;
		}
		public void Set(ref Vector3 start, ref Vector3 end, float velocityArg)
		{
			distance = (end - start).magnitude;
			time = distance / velocityArg;
		}

		public bool MoveChecked(float deltaTime)
		{
			Execute(deltaTime);

			return CompleteCheck();
		}

		public bool CompleteCheck()
		{
			return progress >= 1f;
		}

		public void SetPositionToTarget<T>(T target) where T : IMovable
		{
			//target.Position = Vector3.Lerp(target.StartPos, target.EndPos, progress);
			target.Position = target.StartPos + target.DirectionNormalized * (distance * progress);
		}

		public void SetCompleteToTarget<T>(T target) where T : IMovable
		{
			target.Position = target.EndPos;
		}

		public JobMoveByTimeAndDistance StateReset()
		{
			return new JobMoveByTimeAndDistance();
		}
		public void Execute(float arg)
		{
			timePass += arg;

			progress = Mathf.Clamp01(timePass / time);
		}
	}

	/// <summary>
	/// ДВижение в направлении. Не отслеживает начальную позицию. Не определяет прогресс
	/// </summary>
	[Serializable]
	public struct JobMoveByDirection : IStateResetable<JobMoveByDirection>, IMoveJobToTarget<JobMoveByDirection>, IExecutable<float>
	{
		public Vector3 direction01;
		public Vector3 curPos;
		public Vector3 endPos;

		public float speedPerSec;
		public void Set(ref Vector3 start, ref Vector3 end, float speedPerSecArg)
		{
			direction01 = (end - start).normalized;
			curPos = start;
			endPos = end;
			speedPerSec = speedPerSecArg;
		}

		public void Reset()
		{
			direction01 = default;
			curPos = default;
			endPos = default;
			speedPerSec = default;
		}

		public bool MoveChecked(float deltaTime)
		{
			curPos += direction01 * (deltaTime * speedPerSec);

			if (CompleteCheck())
			{
				curPos = endPos;

				return true;
			}
			return false;
		}

		public void Execute(float deltaTime)
		{
			curPos += direction01 * (deltaTime * speedPerSec);
		}

		public bool TryComplete()
		{
			if (Vector3.Dot(endPos - curPos, direction01) < Mathf.Epsilon)
			{
				curPos = endPos;

				return true;
			}
			return false;
		}

		public JobMoveByDirection StateReset()
		{
			return new JobMoveByDirection();
		}

		public bool CompleteCheck()
		{
			return Vector3.Dot(endPos - curPos, direction01) < Mathf.Epsilon;
		}

		public void SetPositionToTarget<T>(T target) where T : IMovable
		{
			throw new NotImplementedException();
		}

		public void SetCompleteToTarget<T>(T target) where T : IMovable
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// Move job в котором результат input и output хранятся внутри одного объекта
	/// </summary>
	public interface IMoveJobSelfContained
	{
		public bool IsCompleted { get; }
		public void Execute(float deltaTime);
	}
	/// <summary>
	/// Move Job который устанавливает результат объекту по связи.
	/// Подзодит в ситуации когда внешние данные постоянно менятся и для расчета постоянно требуется вытягивать актуальные данные в каждой иттерации. 
	/// Мало проиводительный паттерн
	/// </summary>
	/// <typeparam name="TJob"></typeparam>
	public interface IMoveJobToTarget<TJob> : IStateResetable<TJob>
	{
		/// <summary>
		/// Variants: <br/>
		/// Start Pos/End Pos/Velocity
		/// Start Pos/Direction/Velocity
		/// </summary>
		/// <param name="startPos"></param>
		/// <param name="endPos"></param>
		/// <param name="velocityArg"></param>
		void Set(ref Vector3 startPos, ref Vector3 endPos, float velocityArg);
		bool MoveChecked(float deltaTime);
		bool CompleteCheck();
		void SetPositionToTarget<T>(T target) where T : IMovable;
		void SetCompleteToTarget<T>(T target) where T : IMovable;
	}
	public interface IMoveJobProvider
	{
		ref Vector3 StartPos { get; }
		ref Vector3 StartEndPos { get; }
		ref Vector3 Direction { get; }
		float Velocity { get; }

		ref (Vector3, Vector3, Vector3, float) DataProvide();
	}
	public interface IMovable : ISingleChunkedKeyHolder
	{
		bool IsMoving { get; set; }
		Vector3 Position { get; set; }
		Vector3 DirectionNormalized { get; set; }
		Vector3 StartPos { get; set; }
		Vector3 EndPos { get; set; }
		Action MovingCompleteCallback { get; }
	}
	/// <summary>
	/// Object that will recieve signal of moving completion.
	/// </summary>
	public interface IMoveCollbackReciever : ISingleChunkedKeyHolder
	{
		void CompleteMoving();
	}
	/// <summary>
	/// Object that will recieve signal of moving completion and after each round result of that round for this object. 
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public interface IMoveDataReciever<TResult> : IMoveCollbackReciever
	{
		public void RecieveResultOfMovingAtRound(TResult result);
	}
	/// <summary>
	/// Object that will recieve signal of moving completion and after each round result of that round for this object. 
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public interface IMoveDataRecieverByIn<TResult> : IMoveCollbackReciever
	{
		public void RecieveResultOfMovingAtRound(in TResult result);
	}
	/// <summary>
	/// Object that will recieve signal of moving completion and after each round result of that round for this object. 
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public interface IMoveDataRecieverByRef<TResult> : IMoveCollbackReciever
	{
		public void RecieveMoveDataByRef(ref TResult result);
	}
}