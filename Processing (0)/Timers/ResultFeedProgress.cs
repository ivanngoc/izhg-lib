using System;
//using static IziHardGame.Libs.NonEngine.Collections.Chunks.ChunkedList<>

namespace IziHardGames.Libs.NonEngine.Progressing
{
	public readonly struct ResultFeedProgress
	{
		public readonly float progress01;
		public readonly int status;
		public readonly bool isComplete;
		public readonly bool isCancel;
		public readonly bool isPause;

		public ResultFeedProgress(float progress01, int status, bool isComplete, bool isCancel, bool isPause)
		{
			this.progress01 = progress01;
			this.status = status;
			this.isComplete = isComplete;
			this.isCancel = isCancel;
			this.isPause = isPause;
		}

		private bool IsComplete() => throw new NotImplementedException();
		private bool IsCancel() => throw new NotImplementedException();
		private bool IsPause() => throw new NotImplementedException();
	}
}