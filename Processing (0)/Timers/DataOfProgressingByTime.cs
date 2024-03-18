using IziHardGames.Core;
using IziHardGames.Libs.NonEngine.Math.Float;
using System;

namespace IziHardGames.Libs.NonEngine.StateMachines
{
	/// <summary>
	/// Input data + result
	/// </summary>
	[Serializable]
	public struct DataOfProgressingByTime : IStateResetable<DataOfProgressingByTime>
	{
		/// <summary>
		/// Total time of progressing
		/// </summary>
		public float time;
		public float timeLeft;
		/// <summary>
		/// Normalized progress. 0=0%, 1=100%
		/// </summary>
		public float progress01;
		public bool isComplete;
		/// <summary>
		/// If complete than reset to initial state and start again
		/// </summary>
		public bool isResetable;

		public static Func<DataOfProgressingByTime, DataOfProgressingByTime> funcReset;

		static DataOfProgressingByTime()
		{
			funcReset = ResetTime;
		}

		public DataOfProgressingByTime(float progress01, float time) : this()
		{
			this.progress01 = progress01;
			this.time = time;
			timeLeft = (1 - progress01) * time;
		}
		public DataOfProgressingByTime(float progress01, float time, bool isResetable) : this(progress01, time)
		{
			this.isResetable = isResetable;
		}
		public DataOfProgressingByTime(bool isResetable, float time) : this()
		{
			this.time = time;
			timeLeft = time;
			this.isResetable = isResetable;
		}

		public void Init(float time)
		{
			this.time = time;
			timeLeft = time;
			progress01 = 0;
		}

		public void Execute(float timeArg)
		{
			timeLeft -= timeArg;
			isComplete = timeLeft < MathForFloat.Epsilon;
			progress01 = 1f - HelperMath.Clamp01(timeLeft / time);
		}

		public DataOfProgressingByTime StateReset()
		{
			var v = new DataOfProgressingByTime();
			v.timeLeft = time;
			return v;
		}
		public static DataOfProgressingByTime ResetTime(DataOfProgressingByTime taskProgress)
		{
			taskProgress.timeLeft = taskProgress.time;
			taskProgress.progress01 = default;
			return taskProgress;
		}

		public void SetCurrentTimes(float progressArg)
		{
			progress01 = progressArg;
			timeLeft = (1 - progress01) * time;
		}
		public void UpdateTime(float timeArg)
		{
			SetCurrentTimes(timeArg, progress01);
		}
		public void SetCurrentTimes(float timeArg, float progressArg)
		{
			time = timeArg;

			SetCurrentTimes(progressArg);
		}
	}
}