using IziHardGames.Libs.Engine.Memory;
using System;
using System.Collections.Generic;

namespace IziHardGames.Libs.NonEngine.Graphs.QuadTile.Ground
{
	/// <summary>
	/// Контур из ячеек. Каждая ячейка должна непосредственно граничить с другой ячейкой, входящей в контур
	/// </summary>
	public class Circuit : IPoolable
	{
		public int idCircuit;
		public int value;
		public readonly List<int> associatedTileIndexes = new List<int>(64);

		public Circuit()
		{
			idCircuit = GetHashCode();
		}

		public void Add(int indexMap)
		{
#if UNITY_EDITOR
			//Develop.log
			if (associatedTileIndexes.Contains(indexMap)) throw new ArgumentException($"Add contained indexMap={indexMap} idCircuit {idCircuit}");
#endif
			associatedTileIndexes.Add(indexMap);
		}
		public void Remove(int indexMap)
		{
#if UNITY_EDITOR
			if (!associatedTileIndexes.Contains(indexMap)) throw new ArgumentException($"Remove non contained indexMap={indexMap} idCircuit {idCircuit}");
#endif
			associatedTileIndexes.Remove(indexMap);
		}

		public void CleanToReuse()
		{
			associatedTileIndexes.Clear();
			value = default;
		}

		public void ReturnToPool()
		{
			CleanToReuse();
			PoolObjects<Circuit>.Shared.Return(this);
		}

		public static Circuit Rent(int value, int inexMap)
		{
			var v = Rent();
			v.value = value;
			v.associatedTileIndexes.Add(inexMap);
			return v;
		}
		public static Circuit Rent()
		{
			return PoolObjects<Circuit>.Shared.Rent();
		}
		/// <summary>
		/// Проверка на утилизацию
		/// </summary>
		/// <returns>
		/// <see langword="true"/> - emoty, returned to pool<br/>
		/// <see langword="false"/> - at least 1 remain
		/// </returns>
		public bool CheckDispose()
		{
			bool isEmpty = associatedTileIndexes.Count < 1;

			if (isEmpty)
			{
				ReturnToPool();
			}
			return isEmpty;
		}
	}
	//public readonly struct CircuiteGroupe
	//{
	//	public readonly int indexStart;
	//	public readonly int indexEnd;
	//	public readonly int idGroupe;
	//}
}//namespace
