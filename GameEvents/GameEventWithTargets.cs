using System;
using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.Libs.Engine.GameEvents
{
	public abstract class GameEventWithTargets<T> : GameEvent
	{
		public event Action<T> OnEvent;
		[NonSerialized] protected List<T> targets = new List<T>();

		public override void Execute()
		{
			base.Execute();

			foreach (var target in targets)
			{
				OnEvent?.Invoke(target);
			}
		}
	}
}