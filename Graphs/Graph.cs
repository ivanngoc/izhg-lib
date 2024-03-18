using System;
using System.Collections.Generic;

namespace IziHardGames.Libs.Graphs
{
	public partial class Graph<TNode, TEdge, TNumeric, TGraphAlgo>

		where TNode : Graph<TNode, TEdge, TNumeric, TGraphAlgo>.IGraphNode
		where TEdge : Graph<TNode, TEdge, TNumeric, TGraphAlgo>.IGraphEdge<TNumeric>, IEquatable<TNumeric>
		where TNumeric : IEquatable<TNumeric>, IComparable<TNumeric>
		where TGraphAlgo : Graph<TNode, TEdge, TNumeric, TGraphAlgo>.IGraphAlgo

	{
		public IEnumerable<TNode> Nodes { get; }
		public IEnumerable<TEdge> Edges { get; }
		public IEnumerable<TNode> WorkingSetNodes { get; }
		public IEnumerable<TEdge> WorkingSetEdges { get; }

		public TGraphAlgo graphAlgo;

		#region Unity Message	

		#endregion

		public void Create()
		{

		}

		public interface IGraphAlgo
		{
			public int CountEdges { get; set; }
			public int CountNodes { get; set; }
			public TNode[] Nodes { get; set; }
			public TEdge[] Edges { get; set; }
			bool FindPath(TNode start, TNode end, ref ICollection<TNode> result);
			bool FindPath(TNode start, TNode end, ICollection<TNode> nodesExcluded, ref ICollection<TNode> result);
			bool FindPath(TNode start, TNode end, ICollection<TEdge> edgesExcluded, ref ICollection<TNode> result);
			bool FindPath(TNode start, TNode end, ICollection<TNode> nodesExcluded, ICollection<TEdge> edgesExcluded, ref ICollection<TNode> result);
		}


		public interface IGraphNode
		{
			int Index { get; }
			bool IsAccessible { get; }

			ref TNode this[int index] { get; }

			int CountNeighbors { get; }

			TEdge GetJointEdge(ref TNode node);
		}
		public interface IGraphEdge<T>
		{
			bool IsAccessible { get; }
			public T Length { get; }
		}

		public interface IGraphNodeHolder
		{
			TNode Node { get; set; }
		}

	}

}