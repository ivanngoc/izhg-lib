using System;

namespace IziHardGames.Libs.Graphs.PathFinding
{
	[Serializable]
	public class ExampleDijkstra : Dijkstra<ExampleNode, ExampleEdge, float>
	{
		public ExampleDijkstra(ExampleNode[] nodes, int countNodes, ExampleEdge[] edges, int countEdges) : base(nodes, countNodes, edges, countEdges)
		{

		}

		public override bool ContestWeight(ref DataNode dataNode, ref ExampleEdge edge, ref float numeric)
		{
			float sum = dataNode.weight + edge.Length;

			if (sum < numeric)
			{
				numeric = sum;

				return true;
			}
			else
			{
				return false;
			}
		}

		protected override void Setup()
		{
			for (int i = 0; i < dataNodes.Length; i++)
			{
				dataNodes[i].weight = float.MaxValue;
			}
			// обязательно внизу так как вес начального элемента перезаписывается
			base.Setup();
		}

		//override 
	}
}