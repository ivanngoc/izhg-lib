using IziHardGames.Libs.Engine.MovePatterns;
using IziHardGames.Libs.NonEngine.Collections.Chunked;
using System;
using UnityEngine;
using JobMoveData = IziHardGames.Libs.Engine.MovePatterns.Jobs.JobMoveFromPointToPointWithSpeed;
using SystemMove = IziHardGames.Libs.Engine.MovePatterns.SystemMove<IziHardGames.Libs.Engine.MovePatterns.Jobs.JobMoveFromPointToPointWithSpeed>;

namespace IziHardGames.Libs.Engine.ScenarioWorkflow.Actions
{
	/// <summary>
	/// Используеься в контроллере предопределенных действий <see cref="MonoActionsControl"/> (<see cref="control"/>).
	/// </summary>
	public class MonoActionMoveWithTransform : MonoAction, IMoveDataRecieverByRef<JobMoveData>
	{
		public static SystemMove systemMove;

		public MonoActionsControl control;

		public Action<Vector3> actionPositionSet;
		public Func<Vector3> funcPositionGet;

		private Transform transform;

		public readonly Action triggerMove;

		public bool isMoving;
		public float speedMove;
		private KeyForChunk moveKey;
		private Vector3 start;
		private Vector3 end;

		public MonoActionMoveWithTransform(Transform transform) : base()
		{
			this.transform = transform;
			triggerMove = TriggerMove;
			trigger = triggerMove;
		}

		public ref KeyForChunk RefKey()
		{
			return ref moveKey;
		}

		public void UpdateKey(in KeyForChunk newKey)
		{
			moveKey = newKey;
		}

		public void SetInitialValues(ref Vector3 start, ref Vector3 end)
		{
			var delta = end - start;
			//if (delta.sqrMagnitude == 0) return;

			this.start = start;
			this.end = end;
		}
		public void CompleteMoving()
		{
			isMoving = false;
			control.MonoActionComplete();
		}
		private void TriggerMove()
		{
			var init = new JobMoveData(ref start, ref end, speedMove);
			systemMove.MoveBegin(this, ref init);
		}
		public void RecieveMoveDataByRef(ref JobMoveData result)
		{
			actionPositionSet(result.position);
		}
	}
}