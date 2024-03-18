using System;
using UnityEngine;

namespace IziHardGames.Libs.Engine.GameEvents
{
	public abstract class GameEventWithTarget<T> : GameEvent
	{
		public event Action<T> OnEvent;
		[NonSerialized] protected T target;

		public override void Execute()
		{
			base.Execute();
			OnEvent?.Invoke(target);
		}
		public void SetTarget(T target)
		{
			this.target = target;
		}
	}
}