using IziHardGames.Libs.NonEngine.Collections.Chunked;
using IziHardGames.Libs.NonEngine.Processing.Progressing;

namespace IziHardGames.Libs.NonEngine.Progressing
{
	public interface IProgressCallbackRecieverByIn<TData> : ISingleChunkedKeyHolder
	{
		/// <summary>
		/// Get result from <see cref="SystemProgressByTime"/> to local variable
		/// </summary>
		/// <param name="progressValue01"></param>
		public void CopyProgressingResultByIn(in TData data);
		public void ProgressingCompleteCalback();
	}
}