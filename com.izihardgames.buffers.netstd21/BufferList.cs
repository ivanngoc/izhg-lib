using System.Collections.Generic;

namespace IziHardGames.Buffers
{
	public class BufferList<T> where T : class
	{
		private static readonly List<List<T>> buffers = new List<List<T>>();

		private static BufferList<T> instance;
		public static BufferList<T> Shared
		{
			get
			{
				if (instance == null)
				{
					instance = new BufferList<T>();
				}
				return instance;
			}
		}

		public BufferList()
		{
			buffers.Add(new List<T>(8));
			buffers.Add(new List<T>(64));
			buffers.Add(new List<T>(128));
		}

		public List<T> Rent(int capacity)
		{
			for (int i = 0; i < buffers.Count; i++)
			{
				if (buffers[i].Capacity >= capacity)
				{
					var buf = buffers[i];

					buffers.RemoveAt(i);

					return buf;
				}
			}
			List<T> newBuffer = new List<T>(capacity + capacity / 2);

			return newBuffer;
		}
		public List<T> RentCapacityMax()
		{
			int max = int.MinValue;

			int maxIndex = int.MinValue;

			for (int i = 0; i < buffers.Count; i++)
			{
				if (buffers[i].Capacity > max)
				{
					max = buffers[i].Capacity;

					maxIndex = i;
				}
			}

			if (maxIndex < 0)
			{
				return buffers[maxIndex];
			}
			else
			{
				return Rent(1024);
			}
		}

		public void Return(List<T> list, bool isToClear = default)
		{
			if (isToClear)
			{
				list.Clear();
			}

			buffers.Add(list);
		}

		public static void Release()
		{
			buffers.Clear();
		}
	}
}