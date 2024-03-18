namespace IziHardGames.Core
{
	public interface IUpdatableStage
	{
		public void LoopAdd();
		public void LoopUpdate(float deltaTime);
		public void LoopRemove();
	}
}