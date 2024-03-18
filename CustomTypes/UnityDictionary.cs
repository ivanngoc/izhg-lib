using IziHardGames.Extensions.PremetiveTypes;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IziHardGames.Wrappers
{
	/// <summary>
	/// Changing value in inspector is not supported 2020/12/14
	/// TODO: Update values uppon changing inspector property
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	[Serializable]
	public class UnityDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
	{
		[SerializeField]
		private List<TKey> keys = new List<TKey>();

		[SerializeField]
		private List<TValue> values = new List<TValue>();
		[SerializeField]
		private int countBefore;
		public new void Add(TKey key, TValue value)
		{
			base.Add(key, value);

			keys.Add(key);

			values.Add(value);

			countBefore++;
		}
		public new void Remove(TKey key)
		{
			base.Remove(key);

			int index = keys.IndexOf(key);

			keys.RemoveAt(index);

			values.RemoveAt(index);

			countBefore--;
		}
		public new void Clear()
		{
			base.Clear();
			keys.Clear();
			values.Clear();
			countBefore = default;
		}

		public void OnBeforeSerialize()
		{
			//if (Count < 1) return;
			//if (countBefore == keys.Count && Count == keys.Count) return;
			//Debug.LogError("OnAfterDeserialize");
			keys.Clear();
			values.Clear();

			foreach (KeyValuePair<TKey, TValue> pair in this)
			{
				keys.Add(pair.Key);
				values.Add(pair.Value);
			}
			//countBefore = keys.Count;
		}

		public void AddSerilized(TKey key, TValue value)
		{
			keys.Add(key);
			values.Add(value);
		}

		public void OnAfterDeserialize()
		{
#if UNITY_EDITOR
			//if (UnityEditor.EditorApplication.isPlaying)
			//{
			//if (keys.Count < 1) return;

			//if (countBefore == keys.Count) return;


			//Debug.LogError("OnBeforeSerialize");

			base.Clear();

			if (keys.Count != values.Count)
				throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

			for (int i = 0; i != Math.Min(keys.Count, values.Count); i++)
			{
				base.Add(keys[i], values[i]);
			}
			//}
#endif
		}
	}

	/// <summary>
	/// Нет проверок на переполнение <br/>
	/// Простой линейный алгоритм поиска ключа<br/>
	/// Отлично подходит для небольших сетов. Основная цель - пресет сцены<br/>
	/// Нужно всегда учитывать общий размер capacity. Вручную расширять пересозданием<br/>
	/// </summary>
	/// <typeparam name="TValue"></typeparam>
	[Serializable]
	public class UnityDictionatySimple<TValue>
	{
		[SerializeField] public int[] keys;
		[SerializeField] public TValue[] values;

		public int count;
		public int capacity;

		public TValue this[int key]
		{
			get
			{
				return Get(key);
			}
			set
			{
				Set(key, value);
			}
		}

		public UnityDictionatySimple()
		{

		}

		public UnityDictionatySimple(int capacityVal)
		{
			capacity = capacityVal;
			count = 0;
		}

		public void Add(int key, TValue value)
		{
			keys[count] = key;
			values[count] = value;

			count++;
		}

		public void Remove(int key)
		{
			int i = GetIndex(key);

			int iLast = count - 1;

			keys[i] = keys[iLast];
			values[i] = values[iLast];

			count--;
		}

		public void Clear()
		{
			for (int i = 0; i < count; i++)
			{
				keys[i] = default;

				values[i] = default;
			}
			count = default;
		}

		private int GetIndex(int key)
		{
			for (int i = 0; i < count; i++)
			{
				if (keys[i] == key)
				{
					return i;
				}
			}
			throw new ArgumentException($"Cant find index for key {key} Not Founded. Typeof [{typeof(TValue)}]");
		}

		public TValue Get(int key)
		{
			for (int i = 0; i < count; i++)
			{
				if (keys[i] == key)
				{
					return values[i];
				}
			}
			throw new ArgumentException($"Object with key {key} Not Founded. Typeof [{typeof(TValue)}]");
		}

		public void Set(int key, TValue value)
		{
			int i = GetIndex(key);

			keys[i] = key;
			values[i] = value;
		}

		public void ResizeTight(int newSize)
		{
			Array.Resize(ref keys, newSize);
			Array.Resize(ref values, newSize);

			capacity = newSize;
			count = newSize;
		}
	}


#if UNITY_EDITOR
	[Unused]
	public class UnityDictionaryImmutable<TValue> where TValue : Object
	{
		[SerializeField] public int[] keys;
		[SerializeField] public TValue[] values;

		public UnityDictionaryImmutable(Dictionary<int, TValue> keyValuePairs)
		{

		}
		public UnityDictionaryImmutable(int[] keys, TValue[] values)
		{

		}
	}
#endif

}

namespace IziHardGames.Experimental
{
	[Serializable]

	public class DictionaryOptimized<TKey, TValue>
	{
		public TKey[] keys;
		public TValue[] values;
		public int capacity;
		public int count;

		public DictionaryOptimized(int capacityVal)
		{
			capacity = capacityVal;
			keys = new TKey[capacityVal];
			values = new TValue[capacityVal];
		}
	}
	public class DictionaryOptimizedInt<TValue>
	{
		/// <summary>
		/// 4096/4
		/// </summary>
		private const int SIZE_CHUNK_PAGE = 1024;

		private const int SIZE_CHUNK = 32;
		/// <summary>
		/// INT.MAX / 32
		/// </summary>
		private const int COUNT_CHUNKS_MAX = 67108863;

		public int[] keys;
		public TValue[] values;
		public int capacity;
		public int count;

		public int countChunks;

		public int[] chunksMasks;
		public List<TValue[]> chunks;

		public DictionaryOptimizedInt(int capacityVal)
		{
			capacity = capacityVal;
			keys = new int[capacityVal];
			values = new TValue[capacityVal];

			countChunks = capacityVal / 32 + 1;
			chunks = new List<TValue[]>(countChunks);
		}


	}
	public class Dictionary32<TValue>
	{
		public int[] keys = new int[32];
		public TValue[] values = new TValue[32];
		public int mask;

		public TValue this[int mask]
		{
			get
			{
				return values[GetIndex(mask)];
			}
			set
			{
				values[GetIndex(mask)] = value;
			}
		}

		public int Add(TValue value)
		{
			int id = GetFreeIndex();

			values[id] = value;

			return id;
		}
		public void Remove(int key)
		{

		}
		/// <summary>
		/// В конечном итоге будет создана хэш таблица которые все равно будет выполнять % 
		/// </summary>
		/// <param name="mask"></param>
		/// <returns></returns>
		private int GetIndex(int mask)
		{
			switch (mask)
			{
				case 0b0000_0000_0000_0000: return 0;
				case 0b0000_0000_0000_0001: return 1;
				case 0b0000_0000_0000_0010: return 2;
				case 0b0000_0000_0000_0100: return 3;
				case 0b0000_0000_0000_1000: return 4;
				case 0b0000_0000_0001_0000: return 5;
				case 0b0000_0000_0010_0000: return 6;
				case 0b0000_0000_0100_0000: return 7;
				case 0b0000_0000_1000_0000: return 8;
				default: break;
			}

			throw new ArgumentException("Wrong mask. Must Be Pow of 2");
		}
		/// <summary>
		/// Слишком много операций алу
		/// в худшем случае выдасть 32 иттерации
		/// Нужно найти способ компоратора
		/// Идеальный способ это смещение на x, где икс вычислен наиболее оптимально
		/// </summary>
		public int GetFreeIndex()
		{
			int result = default;

			int i = 0b1011_1111;

			while (i.IsOdd())
			{
				i = i >> 1;

				result++;
			}

			return result;
		}
	}
	/// <summary>
	/// В целях оптимизации id должен начинаться с 0 и заполняться сначала до бсконеччность.
	/// Потом от 0 до -бесконечности
	/// </summary>
	/// <typeparam name="TValue"></typeparam>
	public class DictionaryMemoryPage<TValue> where TValue : class
	{
		/// <summary>
		/// 2147483648/1024 = 2,097152 MB
		/// </summary>
		public const int COUNT_MAX_CHUNKS = 2097152;

		public TValue this[int key]
		{
			get
			{
				return GetValue(key);
			}
			set
			{

			}
		}
		/// <summary>
		/// <see cref="Array"/>
		/// </summary>
		private class ChunkSet
		{
			public int[] keys = new int[1024];
			/// <summary>
			/// 
			/// </summary>
			public int[] freeIds = new int[1024];

			public TValue[] values = new TValue[1024];
			/// <summary>
			/// Последний индекс свободных id
			/// </summary>
			public int indexFree;
			public int countFree;
			public int countTaken;


			public ChunkSet()
			{
				for (int i = 0; i < freeIds.Length; i++)
				{
					freeIds[i] = i;
				}

				indexFree = 1023;
				countFree = 1024;
				countTaken = 0;
			}

			public TValue Get(int index)
			{
				return values[index];
			}

			public void Add(int index, TValue value)
			{
				values[index] = value;

				values.Swap<TValue[], TValue>(index, indexFree);

				indexFree--;

				countFree--;

				countTaken++;
			}
		}

		private ChunkSet[] chunkSets = new ChunkSet[COUNT_MAX_CHUNKS];

		/// <summary>
		/// (val =2147483648 ) 30 bit / (val=1024) 9 = ~3
		/// </summary>
		public TValue[][] valuesChunk = new TValue[3][];

		private void AddChunk()
		{
			int[] keys = new int[1024];

			TValue[] values = new TValue[1024];
			/// лево => право = свободные/занятные
			int[] freeSlots = new int[1024];
		}

		public bool TryGetValue(int key, out TValue value)
		{
			int reminder = key % 1024;

			int times = key / 1024;

			TValue[] values = valuesChunk[times];

			if (values != null)
			{
				if (times > 0)
				{
					value = values[reminder];
				}
				else
				{
					value = values[key];
				}

				return true;
			}
			else
			{
				value = default;

				return false;
			}
		}

		private TValue GetValue(int key)
		{
			int times = key / 1024;

			int index = (times > 0) ? key - times * 1024 : key;

			ChunkSet chunkSet = chunkSets[times];

			return chunkSet.Get(index);
		}

		private void SetValue(int key, TValue value)
		{

		}
	}
}
