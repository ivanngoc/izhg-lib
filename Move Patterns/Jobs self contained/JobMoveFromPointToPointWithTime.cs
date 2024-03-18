using IziHardGames.Core;
using System;
using UnityEngine;
//IziHardGame.Libs.Engine.MovePatterns
namespace IziHardGames.Libs.Engine.MovePatterns.Jobs
{
	/// <summary>
	/// Выполнить перемещение до указанной точки
	/// </summary>
	[Serializable]
	public struct JobMoveFromPointToPointWithTime : IExecutable<float>, IMoveJobSelfContained
	{
		public bool IsCompleted { get => isCompleted; }

		public Vector3 start;
		public Vector3 end;
		public Vector3 directionNormilized;
		public Vector3 position;

		public float timeTotal;
		public float timeLeft;

		public bool isCompleted;
		public float progress01;
		public JobMoveFromPointToPointWithTime(ref Vector3 startArg, ref Vector3 endArg, float time)
		{
			start = startArg;
			end = endArg;
			position = startArg;
			directionNormilized = (end - start).normalized;
			timeTotal = time;
			timeLeft = time;
			isCompleted = false;
			progress01 = 0;
		}
		public void Execute(float deltaTime)
		{
			if (timeLeft > deltaTime)
			{
				timeLeft -= deltaTime;
				progress01 = 1f - timeLeft / timeTotal;
				position = Vector3.Lerp(start, end, progress01);
			}
			else
			{
				isCompleted = true;
				progress01 = 1;
				position = end;
			}
		}
	}
}