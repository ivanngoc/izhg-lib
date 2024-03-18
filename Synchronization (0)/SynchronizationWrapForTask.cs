using System;
using System.Threading.Tasks;

namespace IziHardGames.Libs.NonEngine.Synchronization
{
	internal class SynchronizationWrapForTask : SynchronizationWrap<Task>
	{
		public readonly Action<Task> callback;
		public SynchronizationWrapForTask()
		{
			callback = Complete;
		}
		private void Complete(Task task)
		{
			isComplete = true;
			syncPoint.RunComplete();
		}
		public override SyncPoint Sync(Task item)
		{
			throw new NotImplementedException();
		}
	}
}