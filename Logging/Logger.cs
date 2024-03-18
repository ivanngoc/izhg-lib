using System;

namespace IziHardGames.ApplicationLevel
{
	public class Logger
	{
		public static Action<string, object> actionLog;
		public static Action<string, object> actionLogWarning;
		public static Action<string, object> actionLogError;
		public static Action<Exception, object> actionLogException;
		public static void Log(string message, object target = default)
		{
			actionLog(message, target);
		}
		public static void LogWarning(string message, object target = default)
		{
			actionLogWarning(message, target);
		}
		public static void LogError(string message, object target = default)
		{
			actionLogError(message, target);
		}
		public static void LogError(Exception exception, object target = default)
		{
			actionLogException(exception, target);
		}
	}
}