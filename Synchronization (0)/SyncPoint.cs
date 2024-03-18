using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IziHardGames.Libs.NonEngine.Synchronization
{
	public class SyncPoint
	{
		public List<SynchronizationWrap> wraps;
		public Action actionAfterSync;

		/// <summary>
		/// Записать метод продолжения после синхронизации
		/// </summary>
		/// <param name="action"></param>
		public void ContinueWith(Action action)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Записать метод продолжения после синхронизации с передачей этого объекта в случаях когда после завершения нужно использовать данные из вычисленных задач
		/// </summary>
		/// <param name="action"></param>
		public void ContinueWith(Action<SyncPoint> action)
		{
			throw new NotImplementedException();
		}
		public static SyncPoint NewSyncPoint()
		{
			throw new NotImplementedException();
		}
		internal void Add(SynchronizationWrap synchronizationWrap)
		{
			wraps.Add(synchronizationWrap);
		}
		public void RunComplete()
		{
			foreach (var wrap in wraps)
			{
				if (!wrap.isComplete) return;
			}
			actionAfterSync();
		}

#if UNITY_EDITOR
		public void TestSyntax()
		{
			var task = Task.Run(() => { });

			SyncPoint.NewSyncPoint().Sync(task);
		}
#endif
	}
}