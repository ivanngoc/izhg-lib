using IziHardGames.Ticking.Lib;

namespace IziHardGames.Core
{
	public interface IUpdatableDeltaTime : IUpdatable
	{
		public void ExecuteUpdateWithDelta(float timeDelta);
	}
}