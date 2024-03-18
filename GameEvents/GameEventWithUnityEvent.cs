using UnityEngine;
using UnityEngine.Events;

namespace IziHardGames.Libs.Engine.GameEvents
{
	[CreateAssetMenu(menuName = "IziHardGames/Libs/Engine/GameEvents/GameEventWithUnityEvent", fileName = "GameEventWithUnityEvent")]
	public class GameEventWithUnityEvent : GameEvent
	{
		public UnityEvent unityEvent;

		public override void Execute()
		{
			base.Execute();
			unityEvent?.Invoke();
		}
	}

}