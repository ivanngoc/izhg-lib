using System;

namespace IziHardGames.Ticking.Lib
{
	public interface IUpdateDataProvider
	{
		/// <summary>
		/// Общее количество обновлений с момента запуска
		/// </summary>
		int FrameCount { get; }
		/// <summary>
		/// Время последнего обнолвения. Время между прошлым и позапрошлым кадром относительного текущего
		/// </summary>
		float DeltaTime { get; }
	}

	/// <summary>
	/// Поставщик сигналов. Генератор сигнала или медиатор, перебрасывающий сигнал
	/// </summary>
	public interface IUpdateProvider<T>
	{
		EUpdateChannel EUpdateChannel { get; set; }
		void ConsumerAdd(EUpdateChannel eUpdateTypess, Action<T> consumer);
		void ConsumerRemove(EUpdateChannel eUpdateTypess, Action<T> consumer);
	}
}