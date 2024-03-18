using System.Buffers;
using UnityEngine;

namespace IziHardGames.Buffers
{
	[CreateAssetMenu(fileName = "Beffer", menuName = "IziHardGames/Memory/Buffer")]
	public class BuffersAllocate : ScriptableObject
	{
		[SerializeField] private int[] buuferInt0 = new int[1024];
		[SerializeField] private int[] buuferInt1 = new int[1024];
		[SerializeField] private int[] buuferInt2 = new int[1024];
		[SerializeField] private int[] buuferInt3 = new int[1024];
		[SerializeField] private int[] buuferInt4 = new int[1024];


		private void Awake()
		{
			ArrayPool<int>.Shared.Return(buuferInt0);
			ArrayPool<int>.Shared.Return(buuferInt1);
			ArrayPool<int>.Shared.Return(buuferInt2);
			ArrayPool<int>.Shared.Return(buuferInt3);
			ArrayPool<int>.Shared.Return(buuferInt4);
		}
	}
}