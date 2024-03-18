namespace IziHardGames.Libs.Engine.Memory
{
	public interface IPoolable
    {
        public void CleanToReuse();
        public void ReturnToPool();
    }
}