using IziHardGames.Libs.NonEngine.Consoles;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IziHardGames.Libs.Engine.Consoles
{
	public class UnityConsole : Libs.NonEngine.Consoles.Console
	{
		public static UnityConsole Default => GetOrCreate();
		public static UnityConsole singleton;

#if UNITY_EDITOR
		public ConsolePostInvokeControl WriteLike(string message)
		{
			return new ConsolePostInvokeControl(this, message, null);
		}
		public void WriteLine(string message, Object target)
		{
			Debug.Log(FormatByDefault(message), target);
		}
#endif
		public override void WriteLine(string message, object source)
		{
#if UNITY_EDITOR
			if (source is UnityEngine.Object unityTarget)
			{
				Debug.Log(FormatByDefault(message), unityTarget);
			}
			else
			{
				Debug.Log(FormatByDefault(message));
			}
#endif
		}
		public override void WriteLine(string message, object source, EConsoleColor eConsoleColor)
		{
			throw new NotImplementedException();
		}

		public static UnityConsole GetOrCreate()
		{
			if (singleton == null)
			{
				singleton = new UnityConsole();
			}
			return singleton;
		}

		private string FormatByDefault(string input)
		{
			return $"<color=lime>[{Time.frameCount}]</color>";
		}
	}

	public struct ConsolePostInvokeControl
	{
		private string message;
		private UnityConsole unityConsole;
		private UnityEngine.Object target;

		public ConsolePostInvokeControl(UnityConsole unityConsole, string message, Object target)
		{
			this.message = message;
			this.unityConsole = unityConsole;
			this.target = target;
		}
		public void Red()
		{
			unityConsole.WriteLine($"<color=red>{message}</color>", target);
		}
		public void Green()
		{
			unityConsole.WriteLine($"<color=green>{message}</color>", target);
		}
		public void Cyan()
		{
			unityConsole.WriteLine($"<color=cyan>{message}</color>", target);
		}
		public void Lime()
		{
			unityConsole.WriteLine($"<color=lime>{message}</color>", target);
		}
	}
}