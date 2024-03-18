namespace System
{
	public static class ExtensionSpan
	{
		/// <summary>
		/// Сортировка выборкой
		/// <see cref="IziHardGames.Libs.Sorting.SortingIList.SortSelectionAscending"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name=""></param>
		public static void SortSelectionAscending<TElem, TSelect>(this Span<TElem> span, Func<TElem, TSelect> selector) where TSelect : IComparable<TSelect>
		{
			int count = span.Length;

			for (int i = 0; i < count; i++)
			{
				int index = i;

				TSelect minCurrent = selector(span[i]);

				for (int j = i + 1; j < count; j++)
				{
					TSelect val = selector(span[j]);

					if (minCurrent.CompareTo(val) > 0)
					{
						index = j;
						minCurrent = val;
					}
				}
				span.Swap(i, index);
			}
		}
		public static void SortAscending(this Span<int> span)
		{
			var size = span.Length;

			var max = span[0];

			for (var i = 1; i < size; i++)
			{
				if (span[i] > max)
					max = span[i];
			}

			for (var rank = 1; max / rank > 0; rank *= 10)
			{
				int i;

				Span<int> count = stackalloc int[10];

				/// для каждой цифры в порядке exp подсчитать количество совпадений
				for (i = 0; i < size; i++)
				{
					count[span[i] / rank % 10]++;
				}
				for (i = 1; i < 10; i++)
				{
					count[i] += count[i - 1];

				}
				Span<int> output = stackalloc int[size];

				for (i = size - 1; i >= 0; i--)
				{
					var index = span[i] / rank % 10;

					output[count[index] - 1] = span[i];

					count[index]--;
				}
				for (i = 0; i < size; i++)
				{
					span[i] = output[i];
				}
			}
		}
		public static void SortDescending(this Span<int> span)
		{
			var size = span.Length;

			var max = span[0];

			for (var i = 1; i < size; i++)
			{
				if (span[i] > max)
					max = span[i];
			}

			for (var rank = 1; max / rank > 0; rank *= 10)
			{
				int i;

				Span<int> count = stackalloc int[10];

				/// для каждой цифры в порядке exp подсчитать количество совпадений
				for (i = 0; i < size; i++)
				{
					count[span[i] / rank % 10]++;
				}
				for (i = 1; i < 10; i++)
				{
					count[i] += count[i - 1];

				}
				Span<int> output = stackalloc int[size];

				for (i = size - 1; i >= 0; i--)
				{
					var index = span[i] / rank % 10;

					output[count[index] - 1] = span[i];

					count[index]--;
				}
				for (i = 0; i < size; i++)
				{
					span[i] = output[i];
				}
			}

			span.Reverse();
		}
		public static void Inverse<T>(this Span<T> span)
		{
			var count = span.Length / 2;

			for (var i = 0; i < count; i++)
			{
				var temp = span[i];
				var inverseIndex = span.Length - 1 - i;
				span[i] = span[inverseIndex];
				span[inverseIndex] = temp;
			}
		}
		public static void Swap<T>(this Span<T> span, int left, int right)
		{
			var temp = span[left];
			span[left] = span[right];
			span[right] = temp;
		}
		public static bool Contains(this Span<int> span, int x)
		{
			for (var i = 0; i < span.Length; i++)
			{
				if (x == span[i]) return true;
			}

			return false;
		}
	}
}