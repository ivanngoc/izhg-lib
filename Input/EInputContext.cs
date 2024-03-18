using System;

namespace IziHardGames.UserControl.InputSystem.Unity
{
	/// <summary>
	/// Список контекстов указателя или пользовательского фокуса.
	/// Определяет контекст события в котором <see cref="BroadcastEventConsumer"/> выполнится или не выполняется (по месту реализации логики)
	/// </summary>
	[Flags]
	public enum EInputContext
	{
		All = -1,
		None = 0,
		/// <summary>
		/// Context's object is Detected by Graphic Raycaster
		/// </summary>
		UI = 1,
		/// <summary>
		/// Context's object is Detected by Physic2d raycast
		/// </summary>
		World2D = 2,
		/// <summary>
		/// Context's object is Detected by Physic3d raycast
		/// </summary>
		World3D = 4,
		/// <summary>
		/// Nothing under cursor
		/// </summary>
		Void = 8,
		/// <summary>
		/// Text editing element witch carriage on
		/// </summary>
		TextBox = 16,
		CustomRaycast = 32,
	}
}
































