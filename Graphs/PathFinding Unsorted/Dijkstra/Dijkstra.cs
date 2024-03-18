using System;
using System.Collections.Generic;
using System.Linq;

namespace IziHardGames.Libs.Graphs.PathFinding
{
	/// <summary>
	/// </summary>
	/// <remarks>
	///// Количество соединений с 1 узлом не более 256 т.к. <see cref="Dijkstra{TNode, TEdge, TNumeric}.DataNode.countVisit"/> is Byte 
	/// </remarks>
	/// <typeparam name="TNode"></typeparam>
	/// <typeparam name="TEdge"></typeparam>
	/// <typeparam name="TNumeric"></typeparam>
	[Serializable]
	public class Dijkstra<TNode, TEdge, TNumeric> : Graph<TNode, TEdge, TNumeric, Dijkstra<TNode, TEdge, TNumeric>>.IGraphAlgo
		where TNode : Graph<TNode, TEdge, TNumeric, Dijkstra<TNode, TEdge, TNumeric>>.IGraphNode
		where TEdge : Graph<TNode, TEdge, TNumeric, Dijkstra<TNode, TEdge, TNumeric>>.IGraphEdge<TNumeric>, IEquatable<TNumeric>
		where TNumeric : IEquatable<TNumeric>, IComparable<TNumeric>
	{
		public int CountEdges { get; set; }
		public int CountNodes { get; set; }

		private int countResult;

		private int indexStart;

		private int indexEnd;

		//private int countNodesToVisit;
		public TNode[] Nodes { get; set; }
		public TEdge[] Edges { get; set; }

		public DataNode[] dataNodes;

		private List<int> openList;
		//private Queue<int> openList;
		private List<int> closeList;

		private int[] result;

		#region IGraphAlgo
		public bool FindPath(TNode start, TNode end, ref ICollection<TNode> resultOut)
		{
			indexStart = start.Index;

			indexEnd = end.Index;

			Setup();

			Execute();

			if (Reconstruct())
			{
				for (int i = countResult - 1; i >= 0; i--)
				{
					resultOut.Add(Nodes[result[i]]);
				}
			}
			return countResult > 0;
		}

		public bool FindPath(TNode start, TNode end, ICollection<TNode> nodesExcluded, ref ICollection<TNode> result)
		{
			throw new System.NotImplementedException();
		}

		public bool FindPath(TNode start, TNode end, ICollection<TEdge> edgesExcluded, ref ICollection<TNode> result)
		{
			throw new System.NotImplementedException();
		}

		public bool FindPath(TNode start, TNode end, ICollection<TNode> nodesExcluded, ICollection<TEdge> edgesExcluded, ref ICollection<TNode> result)
		{
			throw new System.NotImplementedException();
		}

		#endregion

		public Dijkstra(TNode[] nodesIn, int countNodes, TEdge[] edgesIn, int countEdges)
		{
			CountNodes = countNodes;

			CountEdges = countEdges;

			Nodes = nodesIn;

			Edges = edgesIn;

			Initilize();
		}
		/// <summary>
		/// Инициализация после создания гарфа
		/// </summary>
		private void Initilize()
		{
			dataNodes = new DataNode[CountNodes];

			openList = new List<int>(CountNodes);

			closeList = new List<int>(CountNodes);

			result = new int[CountNodes];

			for (int i = 0; i < CountNodes; i++)
			{
				dataNodes[i].index = Nodes[i].Index;
			}
		}
		/// <summary>
		/// Установка значений для нового поиска
		/// </summary>
		protected virtual void Setup()
		{
			countResult = default;

			closeList.Clear();

			dataNodes[indexStart].weight = default;
		}

		private void Execute()
		{
			openList.Add(indexStart);

			int count = 0;

			while (openList.Count > 0)
			{
				ProceedOpenList();

				count++;

				if (count > 200) throw new Exception();
			}
		}

		private void ExecuteYield()
		{
			throw new NotImplementedException();
		}

		private void ProceedOpenList()
		{
			int proceedIndex = openList.First();

			openList.RemoveAt(0);

			if (VisitNeighbors(ref Nodes[proceedIndex]))
			{

			}
		}
		int counter = default;
		private bool VisitNeighbors(ref TNode node)
		{
			int visitCount = default;

			//Debug.Log($"{node.Index} | {dataNodes[node.Index].isIterated}| {counter}");
			dataNodes[node.Index].proceedIndex = (byte)counter;

			counter++;

			dataNodes[node.Index].isIterated = true;

			closeList.Add(node.Index);

			for (int i = 0; i < node.CountNeighbors; i++)
			{
				ref TNode neighbor = ref node[i];

				if (neighbor.IsAccessible)
				{
					ref DataNode dataNodeTo = ref dataNodes[neighbor.Index];

					if (!dataNodeTo.isIterated && !closeList.Contains(neighbor.Index) && !openList.Contains(neighbor.Index))
					{
						//Log.LogRed($"Pushed {neighbor.Index}");

						OpenListInsert(neighbor.Index);
					}

					VisitNode(ref node, ref neighbor);

					visitCount++;

					//Log.LogLime($"End Pushed");
				}
			}


			return visitCount > 0;
		}

		private void VisitNode(ref TNode from, ref TNode to)
		{
			ref DataNode dataNodeTo = ref dataNodes[to.Index];

			TEdge edge = from.GetJointEdge(ref to);
#if UNITY_EDITOR
			if (edge == null)
			{
				throw new NullReferenceException($"Между вершинами Index: {from.Index} и  {to.Index} есть связьб но нету грани");
			}
#endif

			if (ContestWeight(ref dataNodes[from.Index], ref edge, ref dataNodeTo.weight))
			{
				dataNodeTo.indexNodeComeFrom = from.Index;

				if (closeList.Contains(dataNodeTo.index))
				{
					closeList.Remove(dataNodeTo.index);
				}
				if (!openList.Contains(dataNodeTo.index))
				{
					OpenListInsert(dataNodeTo.index);
				}
			}
		}

		private void OpenListInsert(int idNode)
		{
			bool isInserted = default;

			for (int i = 0; i < openList.Count; i++)
			{
				//int v1 = 10.CompareTo(20);    //<  -1
				//int v2 = 10.CompareTo(-20);   //>   1
				//int v3 = 10.CompareTo(10);    //==  0
				if (dataNodes[idNode].weight.CompareTo(dataNodes[openList[i]].weight) < 0)
				{
					isInserted = true;

					openList.Insert(i, idNode);

					break;
				}
			}

			if (!isInserted)
			{
				openList.Add(idNode);
			}
		}

		private bool Reconstruct()
		{
			ref DataNode dataNode = ref dataNodes[indexEnd];

			if (dataNode.isIterated)
			{
				int i = 0;

				int indexCurrent = indexEnd;

				while (indexCurrent != indexStart)
				{
					result[i] = indexCurrent;

					indexCurrent = dataNodes[indexCurrent].indexNodeComeFrom;

					i++;
				}

				result[i] = indexStart;

				countResult = i + 1;

				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="from"></param>
		/// <param name="joint"></param>
		/// <param name="numericTo"></param>
		/// <returns>
		/// <see cref="true"/> если значение было перезаписано
		/// </returns>
		public virtual bool ContestWeight(ref DataNode from, ref TEdge joint, ref TNumeric numericTo)
		{
			throw new NotImplementedException();
		}
		[Serializable]
		public struct DataNode
		{
			public int index;                   //4 bytes
			public int indexNodeComeFrom;       //4 bytes
			public TNumeric weight;             //4 bytes
			public bool isIterated;              //1 bytes
			public byte proceedIndex;             //1 bytes
		}

	}
}