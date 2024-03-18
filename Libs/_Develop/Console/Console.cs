using System;
using UnityEngine;


namespace IziHardGames.Develop.Engine.TooolsForConsole
{
}

namespace IziHardGames
{
	public static class Console
	{
#if UNITY_EDITOR
		public static void Log(string message)
		{
			Debug.Log(message);
		}
		public static void Log(string message, UnityEngine.Object context)
		{
			Debug.Log(message, context);
		}
		public static void Log(string message, Color color)
		{
			throw new NotImplementedException();
		}
#endif
	}
}