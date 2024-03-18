using System;
using UnityEngine;
using Algo = IziHardGames.PathFinding.AStarAlgoUniversal<IziHardGames.PathFinding.NodeQuadValues, IziHardGames.PathFinding.NodeLinks, IziHardGames.PathFinding.NodeQuadValues.NodeQuadEnumerator>;

namespace IziHardGames.PathFinding
{
	[Serializable]
	public class AStarAlgoUniversal<TData, TLink, TEnum> where TEnum : new()
	{
		/// <see cref="BoundsInt.allPositionsWithin"/>

		public TData[] datas;
		public TLink[] links;

		public interface INodeValue : IziHardGames.Core.IUnique
		{
			/// <summary>
			/// От старта до текущего узла
			/// </summary>
			float CostG { get; set; }
			/// <summary>
			/// От конца до текущего узла
			/// </summary>
			float CostH { get; set; }
			/// <summary>
			/// costG+costH
			/// </summary>
			float CostF { get; set; }

		}
		public interface INodeLinks : IziHardGames.Core.IUnique
		{
			/// <summary>
			/// <see cref=""/> 
			/// </summary>
			int PreviousNodeId { get; set; }
			int this[int direction] { get; set; }
		}

		TEnum t = new TEnum();

		public TEnum GetEnumerator()
		{
			return t;
		}

		void Test()
		{
			Algo aStarAlgoUniversal = new Algo();

			foreach (var item in aStarAlgoUniversal)
			{

			}
		}
	}



	/// <summary>
	/// Вершина графа с максимальным количество связей = 4.
	/// Сам граф выглядит как таблица. 
	/// Связи между вершинами может быть как прямая (вертикальная или горизонтальная) так и диагональная.
	/// </summary>
	[Serializable]
	public struct NodeQuadValues : Algo.INodeValue
	{
		public float CostG { get => costG; set => costG = value; }
		public float CostH { get => CostH; set => CostH = value; }
		public float CostF { get => CostF; set => CostF = value; }
		public int Id { get => id; set => id = value; }


		public static Vector2Int defaultSizeMap;

		public int id;
		/// <summary>
		/// От старта до текущего узла
		/// </summary>
		public float costG;
		/// <summary>
		/// От конца до текущего узла
		/// </summary>
		public float costH;
		/// <summary>
		/// costG+costH
		/// </summary>
		public float costF;

		public struct NodeQuadEnumerator
		{
			public NodeQuadValues Current { get => current; }

			NodeQuadValues current;
			public bool MoveNext()
			{
				throw new NotImplementedException();
			}

			public void Reset()
			{
				throw new NotImplementedException();
			}

			public void Dispose()
			{
				throw new NotImplementedException($"There is no unmanaged resources {Environment.NewLine}{this}");
			}
		}
	}

	public struct NodeLinks : Algo.INodeLinks
	{
		public int Id { get => id; set => id = value; }
		public int MaxLinksCount { get; set; }
		public int this[int direction] { get { return default; } set { } }
		int Algo.INodeLinks.PreviousNodeId { get; set; }

		public int id;
		public int previousNodeId;
	}
}