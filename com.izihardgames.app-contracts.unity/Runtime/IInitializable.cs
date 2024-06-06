namespace IziHardGames.Core
{
	public interface IInitializable
	{
		void Initilize();
	}
    public interface IInitializable<in T>
    {
        void Initilize(T t);
    }
    public interface IInitializable<T1, T2>
    {
        void Initilize(T1 t1, T2 t2);
    }
}