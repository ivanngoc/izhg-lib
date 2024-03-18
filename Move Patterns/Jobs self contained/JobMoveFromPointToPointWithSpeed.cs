using System;
using UnityEngine;
namespace IziHardGames.Libs.Engine.MovePatterns.Jobs
{
	/// <summary>
	/// Подхожит в ситуация когда направление или конечная точка не изменяется
	/// </summary>
	[Serializable]
	public struct JobMoveFromPointToPointWithSpeed : IMoveJobSelfContained
	{
		public bool IsCompleted { get => isCompleted; }

		public Vector3 start;
		public Vector3 end;
		public Vector3 position;
		/// <summary>
		/// Общий прогресс движения
		/// </summary>
		public float progress01;
		/// <summary>
		/// Скорость движения юнитов/сек
		/// </summary>
		public float speedPerSec;
		/// <summary>
		/// проденное расстояние в текущую итерацию
		/// </summary>
		public float lengthToMove;
		/// <summary>
		/// Сколько нужно пройти всего
		/// </summary>
		public float distanceTotal;
		/// <summary>
		/// сколько осталось пройти
		/// </summary>
		public float distanceLeft;

		public bool isCompleted;

		public JobMoveFromPointToPointWithSpeed(ref Vector3 start, ref Vector3 end, float speedPerSec)
		{
			this.start = start;
			this.end = end;
			this.speedPerSec = speedPerSec;

			position = start;
			distanceTotal = (start - end).magnitude;
			distanceLeft = distanceTotal;
			progress01 = default;
			lengthToMove = default;
			isCompleted = default;
		}

		public void Execute(float deltaTime)
		{
			var lengthToMoveUnclamped = deltaTime * speedPerSec;

			if (distanceLeft > lengthToMoveUnclamped)
			{
				lengthToMove = lengthToMoveUnclamped;
				distanceLeft -= lengthToMove;
				progress01 = 1f - (distanceLeft / distanceTotal);
				position = Vector3.Lerp(start, end, progress01);
			}
			else
			{
				lengthToMove = distanceLeft;
				isCompleted = true;
				distanceLeft = 0;
				progress01 = 1;
				position = end;
			}
		}

	}
}