using System;

namespace IziHardGames.UserControl.Lib.Contexts
{

	[Flags]
	public enum EContextSources
	{
		All = -1,
		None = 0,
		FocusOfPointer = 1,
		FocusOfForms = 2,
	}
}