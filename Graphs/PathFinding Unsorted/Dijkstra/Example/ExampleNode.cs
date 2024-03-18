using System;
using UnityEngine;

namespace IziHardGames.Libs.Graphs.PathFinding
{
	public class ExampleNode : MonoBehaviour, Graph<ExampleNode, ExampleEdge, float, Dijkstra<ExampleNode, ExampleEdge, float>>.IGraphNode
	{
		public int id;
		public int Index { get => id; }
		public bool IsAccessible { get => true; }

		public ref ExampleNode this[int id] { get => ref exampleNodes[id]; }

		public ExampleNode[] exampleNodes = new ExampleNode[4];

		public ExampleEdge[] exampleEdges = new ExampleEdge[4];

		private int edgeCount;
		private int NodeCount;

		[SerializeField] public Dijkstra<ExampleNode, ExampleEdge, float>.DataNode dataNode;

		public int CountNeighbors { get => exampleNodes.Length; }

		public ExampleEdge GetJointEdge(ref ExampleNode node)
		{
			return exampleEdges.IntersectFirst(node.exampleEdges, (x, y) => x == y);
		}

		public void AddEdge(ExampleEdge exampleEdge)
		{
			exampleEdges[edgeCount] = exampleEdge;

			edgeCount++;
		}

		public void AddNode(ExampleNode exampleNode)
		{
			exampleNodes[NodeCount] = exampleNode;

			NodeCount++;
		}
	}
}