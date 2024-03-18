using System;

namespace IziHardGames.Libs.NonEngine.Applications
{
    public static class Logging
    {
        public static LoggerSelector Select { get; set; }
        public static IziLogger Debug { get; set; }
        public static IziLogger Default { get; set; }

        public static void Log(string msg, object source = default)
        {
            Default.Log(msg, source);
        }
    }

    public class LoggerSelector
    {

    }
	public abstract class IziLogger
	{
        public abstract void Log(string msg, object source);
	}
}