namespace IziHardGames.Libs.NonEngine.Consoles
{
	public abstract class Console
	{
		public static Console Default { get; }
		public abstract void WriteLine(string message, object source);
		public abstract void WriteLine(string message, object source, EConsoleColor eConsoleColor);
	}

	public enum EConsoleColor
	{
		Default,
		Red,
		Blue,
		Green,
		Magenta,
		Yellow,
		White,
		Black,
		Cyan,
		Orange,
	}
}