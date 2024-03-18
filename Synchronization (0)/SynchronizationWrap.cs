namespace IziHardGames.Libs.NonEngine.Synchronization
{
	public abstract class SynchronizationWrap
	{
		public bool isComplete;
		public SyncPoint syncPoint;
	}

	public abstract class SynchronizationWrap<T> : SynchronizationWrap
	{
		public abstract SyncPoint Sync(T item);
	}
}