using IziHardGames.Libs.Engine.Memory;
using System;
using System.Threading.Tasks;

namespace IziHardGames.Libs.NonEngine.Synchronization
{
	internal static class ExtensionSynchronizationWrapForTask
	{
		public static SyncPoint Sync(this SyncPoint syncPoint, Task task)
		{
			SynchronizationWrapForTask synchronizationWrapForTask = PoolObjects<SynchronizationWrapForTask>.Shared.Rent();
			syncPoint.Add(synchronizationWrapForTask);
			task.ContinueWith(synchronizationWrapForTask.callback);


			throw new NotImplementedException();
		}
	}
}