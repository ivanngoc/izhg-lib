using System;

namespace IziHardGames.UserControl.Lib
{
	/// <summary>
	/// Определяет в каком контексте после выполнения будет прервано продолжение передачи <see cref="BroadcastEvent"/>
	/// </summary>
	[Flags]
	public enum EEventConsumeOption
	{
		All = -1,
		None = 0,
		UI = 1,
		World2D = 2,
		World3D = 4,
		/// <summary>
		/// No objects under cursor
		/// </summary>
		World3DVoid = 8,
		Typing = 16,
	}
}
































