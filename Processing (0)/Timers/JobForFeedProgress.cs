using IziHardGames.Core;
//using static IziHardGame.Libs.NonEngine.Collections.Chunks.ChunkedList<>

namespace IziHardGames.Libs.NonEngine.Progressing
{
	public struct JobForFeedProgress : IStateResetable<JobForFeedProgress>
	{
		public float timeNeed;
		public float timeLeftToComplete;
		public float timeLeftToFeedCheck;
		///// <summary>
		///// Флаг ожидания возможности скармливания для продолжения.<br/>
		///// <see langword="true"/> - ожидается. Будут пропущены все лупы кроме проверки этого флага<br/>
		///// <see langword="false"/> - будут выполнены остальные лупы и проверки<br/> 
		///// </summary>
		//public bool isFeedAwait;
		public bool isFeedTime;

		public JobForFeedProgress(float timeNeed, float timeLeftToComplete, float timeLeftToFeedCheck, bool isFeedTime)
		{
			this.timeNeed = timeNeed;
			this.timeLeftToFeedCheck = timeLeftToFeedCheck;
			this.timeLeftToComplete = timeLeftToComplete;
			this.isFeedTime = isFeedTime;
		}

		//public bool isAllowed;

		public void UpdateTime(float deltaTime)
		{
			timeLeftToFeedCheck -= deltaTime;
			timeLeftToComplete -= deltaTime;
		}
		public void ResetTime(float time)
		{
			timeLeftToFeedCheck = time;
			isFeedTime = false;
		}
		public JobForFeedProgress StateReset()
		{
			return new JobForFeedProgress();
		}

		public ResultFeedProgress Calculate()
		{
			float progress01 = 1 - (timeLeftToComplete / timeNeed);
			isFeedTime = timeLeftToFeedCheck < 0;
			//isAllowed = !isFeedTime;

			return new ResultFeedProgress(progress01, default, progress01 >= 1f, default, default);
		}

	}
}