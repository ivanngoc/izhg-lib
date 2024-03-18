using IziHardGames.Libs.NonEngine.Collections.Chunked;
using System;

namespace IziHardGames.Libs.NonEngine.Processing
{
	/// <summary>
	/// 
	/// </summary>
	public interface ITrackableByTimer : ISingleChunkedKeyHolder
	{
		/// <summary>
		/// Time for timer. Period of update
		/// </summary>
		public float TimeTrackTimer { get; }
		/// <summary>
		/// Callback after given time passed.
		/// </summary>
		public Action OnTimerEndHandler { get; }
	}
}