using System;
using UnityEngine;

namespace IziHardGames.MemoryManagment
{
	public class ManagerMemory : MonoBehaviour
	{
		[SerializeField] long totalMemoryBytes;
		[SerializeField] long totalMemoryKBytes;
		[SerializeField] long totalMemoryMBytes;
		[SerializeField] long totalMemoryGBytes;
		[SerializeField] int countCollectGen0;
		[SerializeField] int countCollectGen1;
		[SerializeField] int countCollectGen2;
		public void Initilize()
		{
			UnityEngine.Application.lowMemory += HandleLowMemoryEvent;
		}
		public void Initilize_De()
		{
			UnityEngine.Application.lowMemory -= HandleLowMemoryEvent;
		}
		#region Unity Message
		private void OnDestroy()
		{
			UnityEngine.Application.lowMemory -= HandleLowMemoryEvent;
		}

		#endregion
		public void ExecuteUpdate()
		{
			CollectMemoryInfo();
		}
		public void HandleLowMemoryEvent()
		{
			// unload buffers

			// unload pools

			// unload scenes
		}

		public void AnalyzHeapSize()
		{

		}

		public void CollectMemoryInfo()
		{
			return;

			totalMemoryBytes = GC.GetTotalMemory(false);

			totalMemoryKBytes = totalMemoryBytes / 1024;
			totalMemoryMBytes = totalMemoryKBytes / 1024;
			totalMemoryGBytes = totalMemoryMBytes / 1024;

			countCollectGen0 = GC.CollectionCount(0);
			countCollectGen1 = GC.CollectionCount(1);
			countCollectGen2 = GC.CollectionCount(2);
		}



		public int Priority { get; }
	}
}