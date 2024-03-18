using IziHardGames.Core;
using IziHardGames.Libs.Engine.Memory;
using IziHardGames.Libs.NonEngine.Graphs.SerilizableGraph;
using IziHardGames.Libs.NonEngine.Graphs.SimpleVersion1;
using System;
using System.Buffers;
using System.Collections.Generic;

namespace IziHardGames.Libs.NonEngine.Graphs.SimpleVersion1
{
	public class GraphCommon
	{
		#region Unity Message

		#endregion

		/// Механизмы графов:
		/// 1. Обход вершин (по граням или через ссылки в самих вершинах друг на друга)
	}

	public interface IConnection : IUnique
	{

	}
	public interface IEdge : IUnique
	{

	}
	public interface INodeData : IUnique
	{

	}
	public interface INodeRank : IUnique
	{
		int NodeRank { get; set; }

		void SetRank(int rank);
	}
	public interface INode : IUnique
	{

	}
}

namespace IziHardGames.Libs.NonEngine.Graphs.ValueType
{
	public interface INodeValueType : INode
	{
		int ConnectionsIndex { get; }
		int ConnectionsCount { get; }
		///// <summary>
		///// Max 32 connection. Each bit of int32 represents sequental connection presents. 
		///// 0 - got connection, 1 - don't.
		///// Flags going from right to left
		///// </summary>
		//int ConnectionsFlag { get; }

		//bool IsVisited { get; set; }
	}

	public interface IConnectionValueType : IConnection
	{
		bool TryGetNodeOnConnection(int i, out int indexNode);
	}

	/// <summary>
	/// Обычный граф из вершин, ребер и соединений. Соеднинение - это набор ребер для конкретной вершины
	/// Цель: быстрый поиск объектов(локальность по кэшу)
	/// Задачи: обход графа без выделения памяти
	/// </summary>
	/// <typeparam name="TNode">Тип узла</typeparam>
	/// <typeparam name="TEdge">Тип ребра</typeparam>
	/// <typeparam name="TCon">Тип соединения </typeparam>
	/// <typeparam name="TData">Данные узла</typeparam>
	/// Для многопотока можно создать для каждого потока отдельный экземпляр, но тогда придется прорабатывать синхронизацию
	/// Лучше сделать отдельный блок массивов в которые будут производиться записи и он у потока будет каждый свой и не будут друг друга перекрывтаь
	/// на данный момент поле <see cref="TNode.IsVisited"/> является общим и конкурирующим если к нему будут обращаться из нескольких потоков
	[Serializable]
	public class Graph<TNode, TEdge, TCon, TData>
		where TNode : unmanaged, INodeValueType
		where TEdge : unmanaged, IEdge
		where TCon : unmanaged, IConnectionValueType
	{
		private TNode[] rentedNodes;
		private TEdge[] rentedEdges;
		private TCon[] rentedConnections;

		private TData[] rentedDatas;
		private TData[] rentedListDataOpen;

		private int[] rentedlistNodesOpen;
		private int[] rentedlistNodesOpenTemp;
		private int[] rentedlistNodesClose;

		private bool[] rentedFlagsVisited;

		private Memory<TNode> nodes;
		private Memory<TEdge> edges;
		private Memory<TCon> connections;

		private Memory<TData> datas;
		private Memory<TData> listDataOpen;

		private Memory<int> listNodesOpen;
		private Memory<int> listNodesOpenTemp;
		private Memory<int> listNodesClose;

		private Memory<bool> flagsVisited;

		private int indexListOpenStart;
		private int indexListOpenEnd;

		private int indexListCloseEnd;

		private int countListOpen;
		private int countListClose;
		private int countNodes;
		private int countEdges;

		#region Traversal itterational
		/// <summary>
		/// Глуюина поиска от центра
		/// </summary>
		private int depthCurrent;
		#endregion

#if UNITY_EDITOR
		public void Debug_Inspect()
		{

		}
#endif
		#region Setups. Startup
		public void Allocate(int countNodesArg, int countEdgesArg, int countConnections, int countDatas)
		{
			countNodes = countNodesArg;
			countEdges = countEdgesArg;

			#region Rent
			rentedNodes = ArrayPool<TNode>.Shared.Rent(countNodesArg);
			rentedlistNodesOpen = ArrayPool<int>.Shared.Rent(countNodesArg);
			rentedlistNodesOpenTemp = ArrayPool<int>.Shared.Rent(countNodesArg);
			rentedlistNodesClose = ArrayPool<int>.Shared.Rent(countNodesArg);
			rentedFlagsVisited = ArrayPool<bool>.Shared.Rent(countNodesArg);

			rentedEdges = ArrayPool<TEdge>.Shared.Rent(countEdgesArg);
			#endregion


			#region Wrap

			nodes = new Memory<TNode>(rentedNodes, 0, countNodesArg);
			listNodesOpen = new Memory<int>(rentedlistNodesOpen, 0, countNodesArg);
			listNodesOpenTemp = new Memory<int>(rentedlistNodesOpenTemp, 0, countNodesArg);
			listNodesClose = new Memory<int>(rentedlistNodesClose, 0, countNodesArg);
			flagsVisited = new Memory<bool>(rentedFlagsVisited, 0, countNodesArg);

			edges = new Memory<TEdge>(rentedEdges, 0, countEdgesArg);
			#endregion


			throw new NotImplementedException();
		}
		public void Resize()
		{
			throw new NotImplementedException();
		}
		public void Extend()
		{
			throw new NotImplementedException();
		}
		public void Free()
		{
			ArrayPool<TNode>.Shared.Return(rentedNodes);
			ArrayPool<TEdge>.Shared.Return(rentedEdges);

			ArrayPool<int>.Shared.Return(rentedlistNodesOpen);
			ArrayPool<int>.Shared.Return(rentedlistNodesOpenTemp);
			ArrayPool<int>.Shared.Return(rentedlistNodesClose);
			ArrayPool<bool>.Shared.Return(rentedFlagsVisited);

			throw new NotImplementedException();
		}
		#endregion


		#region Main. Traversal Process
		/// <summary>
		/// Начать обход графа
		/// </summary>
		public void TraversalBegin(int indexNodeStart)
		{
			depthCurrent = 0;
			indexListCloseEnd = -1;

			throw new NotImplementedException();
		}
		/// <summary>
		/// ПРодвинуться в ширину графа
		/// </summary>
		public bool MoveNext()
		{
			depthCurrent++;

			int cachedOpenListSize = countListOpen;
			int cachedOpenListIndexStart = indexListOpenStart;

			indexListOpenStart = default;
			indexListOpenEnd = -1;
			countListOpen = default;

			TraversalExecute(cachedOpenListIndexStart, cachedOpenListSize);

			listNodesOpenTemp = new Memory<int>(rentedlistNodesOpenTemp, indexListOpenStart, countListOpen);

			Span<int> to = listNodesOpenTemp.Span;
			Span<int> from = listNodesOpen.Span;

			from.CopyTo(to);

			return countListOpen > 0;
		}
		/// <summary>
		/// получить данные обхода полученного после послежнего вызова <see cref="MoveNext"/>
		/// </summary>
		/// <returns></returns>
		public Memory<TData> GetRoundData()
		{
			return listDataOpen;
		}
		/// <summary>
		/// Закончить обхд графа
		/// </summary>
		public void TraversalEnd()
		{
			TraversalReset();
			throw new NotImplementedException();
		}
		#endregion


		#region Traversal logic
		private void TraversalExecute(int start, int count)
		{
			Span<int> span = listNodesOpenTemp.Span.Slice(start, count);

			for (int i = 0; i < count; i++)
			{
				VisitNode(span[i]);
			}
			listNodesClose = new Memory<int>(rentedlistNodesClose, 0, countListClose);
		}
		private void VisitNode(int indexNodeCenter)
		{
			Span<TCon> spanConnections = connections.Span;
			Span<TNode> spanNodes = nodes.Span;
			Span<bool> flags = flagsVisited.Span;

			ref TNode nodeCenter = ref spanNodes[indexNodeCenter];

			ref TCon con = ref spanConnections[nodeCenter.ConnectionsIndex];

			int count = nodeCenter.ConnectionsCount;

			for (int i = 0; i < count; i++)
			{
				if (con.TryGetNodeOnConnection(i, out int indexNode))
				{
					ref TNode node = ref spanNodes[indexNode];

					if (flags[indexNode])
					{
						continue;
					}
					else
					{
						flags[indexNode] = true;
						indexListCloseEnd = countListClose;
						countListClose++;
						rentedlistNodesClose[indexListCloseEnd] = indexNode;
					}

					indexListOpenEnd = countListOpen;
					countListOpen++;

					rentedlistNodesOpen[indexListOpenEnd] = indexNode;
					rentedListDataOpen[indexListOpenEnd] = rentedDatas[indexNode];
				}
			}
		}

		private void TraversalReset()
		{
			Span<bool> spanFlags = flagsVisited.Span;

			for (int i = 0; i < countNodes; i++)
			{
				spanFlags[i] = false;
			}
		}
		#endregion

		public TEdge GetEdge(TNode left, TNode right)
		{
			throw new NotImplementedException();
		}
		public bool TryGetEdge(TNode left, TNode right, out TEdge edge)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// Node with 2 Connection
	/// </summary>
	public struct Node : INodeValueType
	{
		public int Id { get => index; set => index = value; }
		public int ConnectionsIndex { get => indexConnections; }
		public int ConnectionsCount { get => countConnectionsFormed; }
		public bool IsVisited { get => isVisited; set => isVisited = value; }

		public int index;
		public int indexConnections;
		public int countConnectionsFormed;
		public bool isVisited;
	}
	public struct Edge : IEdge
	{
		public int Id { get => index; set => index = value; }
		public int index;
		public bool isOn;
		public bool isDuplex;
	}
	/// <summary>
	/// Для создания треугольного графа
	/// </summary>
	public struct Connections2 : IConnectionValueType
	{
		public int Id { get => index; set => index = value; }
		public int index;

		public int left;
		public int right;

		public bool TryGetNodeOnConnection(int i, out int indexNode)
		{
			throw new NotImplementedException();
		}
	}
	/// <summary>
	/// Для создания прямоугольного графа
	/// </summary>
	public struct Connections4 : IConnectionValueType
	{
		public int top;
		public int right;
		public int bot;
		public int left;

		public int index;

		public Connections4(int top, int right, int bot, int left, int index)
		{
			this.top = int.MinValue;
			this.right = int.MinValue;
			this.bot = int.MinValue;
			this.left = int.MinValue;
			this.index = int.MinValue;
		}

		public unsafe int this[int index]
		{
			get
			{
#if UNITY_EDITOR
				if (index < 0 || index > 4) throw new ArgumentOutOfRangeException();
#endif
				fixed (Connections4* pointer = &this)
				{
					int* p = (int*)pointer;
					return p[index];
				}
			}
			set
			{
#if UNITY_EDITOR
				if (index < 0 || index > 4) throw new ArgumentOutOfRangeException();
#endif
				fixed (Connections4* pointer = &this)
				{
					int* p = (int*)pointer;

					p[index] = value;
				}
			}
		}

		public int Id { get => index; set => index = value; }

		public bool TryGetNodeOnConnection(int i, out int indexNode)
		{
			indexNode = this[i];

			return indexNode >= 0;
		}

		public void Clean(int top, int right, int bot, int left, int index)
		{
			this.top = int.MinValue;
			this.right = int.MinValue;
			this.bot = int.MinValue;
			this.left = int.MinValue;
			this.index = int.MinValue;
		}
	}
	/// <summary>
	/// Связи с элементами вокруг Гексагона или кубик-рубик: вверх, вниз и 4 связи на горизонтали
	/// </summary>
	public struct Connections6 : IConnection
	{
		public int Id { get => index; set => index = value; }
		public int index;
	}
	/// <summary>
	/// Связи по сторонам розы ветров. Или Квадрат на шахматном поле, со всеми примыкающими квадратами
	/// </summary>
	public struct Connections8 : IConnection
	{
		public int Id { get => index; set => index = value; }
		public int index;

		public int N;
		public int NE;
		public int E;
		public int SE;
		public int S;
		public int SW;
		public int W;
		public int NW;
	}
}

namespace IziHardGames.Libs.NonEngine.Graphs.SerilizableGraph
{
	[Serializable]
	public class DataGraph
	{
		public DataNode[] nodes;
		public DataEdge[] edges;
		public DataConnection[] connections;
	}
	[Serializable]
	public struct DataNode : IUnique
	{
		public int Id { get => idNode; set => idNode = value; }
		public int idNode;
		public int idValue;
		public int countConnection;
	}
	[Serializable]
	public struct DataEdge
	{
		public int idConnection;
		public int idEdge;
	}
	[Serializable]
	public struct DataConnection
	{
		public int idConnection;
		public int idNodeStart;
		public int idNodeEnd;
		public int idEdge;
	}
}


namespace IziHardGames.Libs.NonEngine.Graphs.RefType
{
	[Serializable]
	public class Graph
	{
		public static PoolObjects<Graph> poolObjectsGraph;
		/// <summary>
		/// key = <see cref="GraphNode.idValue"/>
		/// </summary>
		public Dictionary<int, GraphNode> nodes;
		public Dictionary<int, GraphEdge> edges;
		public Dictionary<int, GraphConnection> connections;

		public static DataGraph CreateSerilizableVersion(Graph graph)
		{
			DataGraph dataGraph = new DataGraph();

			dataGraph.connections = new DataConnection[graph.connections.Count];
			dataGraph.edges = new DataEdge[graph.edges.Count];
			dataGraph.nodes = new DataNode[graph.nodes.Count];

			int i = default;
			foreach (var item in graph.connections)
			{
				dataGraph.connections[i] = item.Value;
				i++;
			}

			i = default;
			foreach (var item in graph.edges)
			{
				dataGraph.edges[i] = item.Value;
				i++;
			}

			i = default;
			foreach (var item in graph.nodes)
			{
				dataGraph.nodes[i] = item.Value;
				i++;
			}
			return dataGraph;
		}

		public static Graph CreateFromSerilizableVersion(DataGraph dataGraph)
		{
			Graph graph = new Graph()
			{
				connections = new Dictionary<int, GraphConnection>(dataGraph.connections.Length),
				edges = new Dictionary<int, GraphEdge>(dataGraph.edges.Length),
				nodes = new Dictionary<int, GraphNode>(dataGraph.nodes.Length),
			};

			PoolObjects<GraphEdge>.Shared.Rent();

			for (int i = 0; i < dataGraph.nodes.Length; i++)
			{
				GraphNode graphNode = PoolObjects<GraphNode>.Shared.Rent();

				graphNode.Initilize(dataGraph.nodes[i]);

				graph.nodes.Add(dataGraph.nodes[i].idNode, graphNode);
			}
			for (int i = 0; i < dataGraph.edges.Length; i++)
			{
				GraphEdge graphEdge = PoolObjects<GraphEdge>.Shared.Rent();

				graph.edges.Add(dataGraph.edges[i].idEdge, graphEdge);
			}

			for (int i = 0; i < dataGraph.connections.Length; i++)
			{
				GraphConnection graphConnection = PoolObjects<GraphConnection>.Shared.Rent();

				graphConnection.Initilize(graph, ref dataGraph.connections[i]);

				graph.connections.Add(dataGraph.connections[i].idConnection, graphConnection);

				graphConnection.start.connections.Add(graphConnection.Id, graphConnection);
				graphConnection.end.connections.Add(graphConnection.Id, graphConnection);
				graphConnection.edge.connection = graphConnection;
			}

			return graph;
		}
	}
	[Serializable]
	public class GraphEdge : IEdge
	{
		public static PoolObjects<GraphEdge> poolObjectsGraphEdge;

		public int Id { get; set; }
		public GraphConnection connection;

		public static implicit operator DataEdge(GraphEdge graphEdge) => new DataEdge() { idConnection = graphEdge.connection.Id, idEdge = graphEdge.Id };

	}
	[Serializable]
	public class GraphNode : INode
	{
		public static PoolObjects<GraphNode> poolObjectsGraphNode;

		public int Id { get => value.Id; set => this.value.Id = value; }
		public int idNode;
		public int idValue;
		public INodeData value;
		/// <summary>
		/// Key = <see cref="GraphConnection.Id"/>
		/// </summary>
		public Dictionary<int, GraphConnection> connections;
		/// <summary>
		/// key = <see cref="idNode"/>
		/// </summary>
		public Dictionary<int, GraphNode> nodes;

		public bool IsGotConnectionWith(GraphNode with, out GraphConnection graphConnection)
		{
			foreach (var item in connections)
			{
				if (item.Value.IsGotNode(with))
				{
					graphConnection = item.Value;

					return true;
				}
			}
			graphConnection = default;

			return false;
		}
		public bool IsGotConnectionWith(GraphNode with)
		{
			return nodes.ContainsKey(with.idNode);
		}
		public bool IsConnectedWith(GraphNode with)
		{
			return nodes.ContainsKey(with.Id) || with.nodes.ContainsKey(idValue);
		}
		public void Initilize(DataNode dataNode)
		{
			idValue = dataNode.idValue;
			idNode = dataNode.idNode;

			if (connections == null) connections = new Dictionary<int, GraphConnection>(dataNode.countConnection);
			if (nodes == null) nodes = new Dictionary<int, GraphNode>(dataNode.countConnection);
		}

		public void BindFrom(GraphNode graphNode, GraphConnection graphConnection)
		{
			nodes.Add(graphNode.idNode, graphNode);
			connections.Add(graphConnection.Id, graphConnection);
		}

		public static implicit operator DataNode(GraphNode graphNode) => new DataNode() { idNode = graphNode.idNode, idValue = graphNode.idValue, countConnection = graphNode.connections.Count };
	}
	[Serializable]
	public class GraphConnection : IUnique
	{
		public static PoolObjects<GraphConnection> poolObjectsGraphConnection;
		public int Id { get; set; }

		public GraphNode start;
		public GraphNode end;
		public GraphEdge edge;

		public void Initilize(Graph graphArg, ref DataConnection dataConnection)
		{
			Id = dataConnection.idConnection;
			start = graphArg.nodes[dataConnection.idNodeStart];
			end = graphArg.nodes[dataConnection.idNodeEnd];
			edge = graphArg.edges[dataConnection.idEdge];
		}

		public bool IsGotNode(GraphNode graphNode)
		{
			return start == graphNode || end == graphNode;
		}
		public GraphNode GetOppositeNode(GraphNode graphNode)
		{
			if (graphNode == start) return end;
			if (graphNode == end) return start;

			throw new ArgumentException($"No Match");
		}
		public static implicit operator DataConnection(GraphConnection graphConnection) => new DataConnection()
		{
			idConnection = graphConnection.Id,
			idEdge = graphConnection.edge.Id,
			idNodeEnd = graphConnection.end.Id,
			idNodeStart = graphConnection.start.Id,
		};
	}

	[Serializable]
	public class GraphEdge4
	{
		public GraphConnection top;
		public GraphConnection right;
		public GraphConnection bot;
		public GraphConnection left;

		public GraphConnection this[int index]
		{
			get
			{
				switch (index)
				{
					case 0: return top;
					case 1: return right;
					case 2: return bot;
					case 3: return left;
					default: throw new ArgumentOutOfRangeException();
				}
			}
			set
			{
				switch (index)
				{
					case 0: top = value; break;
					case 1: right = value; break;
					case 2: bot = value; break;
					case 3: left = value; break;
					default: throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
	/// <summary>
	/// Обходит граф и получает определенные данные
	/// </summary>
	public class GraphVisitor
	{
		public static List<GraphNode> listClose = new List<GraphNode>(512);
		public static Queue<GraphNode> listOpen = new Queue<GraphNode>(256);
		public static Queue<GraphNode> nextRang = new Queue<GraphNode>(256);
		public static bool GetDatasWithRangRang(Graph graph, int rang, GraphNode center, ref List<GraphNode> graphNodes)
		{
			if (rang == 0)
			{
				graphNodes.Add(center);

				return true;
			}
			int startCount = graphNodes.Count;

			listClose.Clear();
			listOpen.Clear();

			int rangCurrent = rang;

			listOpen.Enqueue(center);

			while (rangCurrent < rang)
			{
				rangCurrent++;

				while (listOpen.Count > 0)
				{
					GraphNode current = listOpen.Dequeue();

					listClose.Add(current);

					foreach (var pair in current.nodes)
					{
						GraphNode node = pair.Value;

						if (listClose.Contains(node) || nextRang.Contains(node)) continue;

						nextRang.Enqueue(node);
					}
				}

				if (rangCurrent < rang)
				{
					while (nextRang.Count > 0)
					{
						listOpen.Enqueue(nextRang.Dequeue());
					}
				}
				else
				{
					while (nextRang.Count > 0)
					{
						graphNodes.Add(nextRang.Dequeue());
					}
				}
			}

			return startCount != graphNodes.Count;
		}

		public static void ExecuteEachRang<T>(Graph graph, GraphNode center) where T : class, INodeData, INodeRank
		{
			listClose.Clear();
			listOpen.Clear();

			int rangCurrent = default;

			listOpen.Enqueue(center);

			while (listOpen.Count > 0)
			{
				while (listOpen.Count > 0)
				{
					GraphNode current = listOpen.Dequeue();

					(current.value as T).SetRank(rangCurrent);

					listClose.Add(current);

					foreach (var pair in current.nodes)
					{
						GraphNode node = pair.Value;

						if (listClose.Contains(node) || nextRang.Contains(node)) continue;

						nextRang.Enqueue(node);
					}
				}

				while (nextRang.Count > 0)
				{
					listOpen.Enqueue(nextRang.Dequeue());
				}
				rangCurrent++;
			}
		}
	}
}
