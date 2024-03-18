using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.Libs.Engine.GameEvents
{
	public abstract class GameEvent : ScriptableObject
	{
		public virtual void Execute()
		{

		}
	}
}