using System;

namespace IziHardGames.Libs.Engine.GameEvents
{
	public class GameEventParameterless : GameEvent
	{
		public event Action OnEvent;
		public override void Execute()
		{
			base.Execute();
		}
	}

}