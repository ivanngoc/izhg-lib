using IziHardGames.Core;
using System;

namespace IziHardGames.Libs.NonEngine.Processing
{
	[Serializable]
	public struct JobForTimer : IStateResetable<JobForTimer>
	{
		public float timeStart;
		public float timePassed;
		public float timeTarget;
		public float timeLeft;
		/// <summary>
		/// <see langword="true"/>- reset timer and start over
		/// <see langword="false"/> - delete after over
		/// </summary>
		public bool isPeriodic;

		public JobForTimer(float timeStart, float timePassed, float timeTarget, float timeLeft, bool isPeriodic)
		{
			this.timeStart = timeStart;
			this.timePassed = timePassed;
			this.timeTarget = timeTarget;
			this.timeLeft = timeLeft;
			this.isPeriodic = isPeriodic;
		}

		public JobForTimer StateReset()
		{
			//timePassed = default;
			//timeStart += timeTarget;
			//timeLeft = timeTarget;

			var v = new JobForTimer();
			v.timeStart = timeTarget;
			v.timeLeft = timeTarget;
			return v;
		}

		public void Countdown(float deltaTime)
		{

		}
	}
}