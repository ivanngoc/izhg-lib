using IziHardGames.Libs.NonEngine.Math.Float;

namespace IziHardGames.Libs.NonEngine.Processing
{
	public struct JobForTimerSimple
	{
		public bool isComplete;
		public float timeLeft;
		public float timeTotal;

		public JobForTimerSimple(float time) : this()
		{
			this.timeLeft = time;
			this.timeTotal = time;
		}
		internal void Execute(float deltaTime)
		{
			timeLeft -= deltaTime;
			isComplete = timeLeft < MathForFloat.Epsilon;
		}
	}
}