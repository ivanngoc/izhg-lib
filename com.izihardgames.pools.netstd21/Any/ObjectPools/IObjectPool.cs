namespace IziHardGames.Libs.Engine.Memory
{
	public interface IObjectPool<T>
	{
		T Rent();
		void Return(T item);
	}
}