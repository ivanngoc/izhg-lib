using IziHardGames.Libs.NonEngine.Collections.Chunked;
//using static IziHardGame.Libs.NonEngine.Collections.Chunks.ChunkedList<>

namespace IziHardGames.Libs.NonEngine.Progressing
{
	public interface IProgressFeed : ISingleChunkedKeyHolder
	{
		/// <summary>
		/// Break,Set to Default, Start again
		/// </summary>
		void Restart();
		/// <summary>
		/// Start again
		/// </summary>
		void Repeat();
		/// <summary>
		/// after <see cref="IsFeedAwait"/>=<see langword="true"/> periodicly check if can resume feeding  (outside). If can resume then call this method
		/// </summary>
		void Retry();
		/// <summary>
		/// Decrease some value as consumption fees
		/// </summary>
		/// <returns>
		/// <see langword="true"/> feed succesfull. go to next tick <br/>
		/// <see langword="false"/> feed NOT succesfull. set <see cref="IsFeedAwait"/>. Skeep <br/>
		/// </returns>
		bool TryFeed();
		/// <summary>
		/// Получить время до очередного опроса на кормление
		/// </summary>
		float GetTime();
		/// <summary>
		/// <see langword="true"/> - ожидается. Будут пропущены все лупы кроме проверки этого флага<br/>
		/// <see langword="false"/> - будут выполнены остальные лупы и проверки<br/> 		
		/// </summary>
		bool IsFeedAwait { get; set; }
		/// <summary>
		/// Restart progressing
		/// </summary>
		bool IsToRepeatProgressing { get; set; }

		void Complete(ResultFeedProgress resultFeedProgress);
	}
}