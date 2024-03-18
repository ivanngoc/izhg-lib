using System;
using UnityEngine;

namespace IziHardGames.Libs.Graphs.PathFinding
{
	public class ExampleEdge : MonoBehaviour, IEquatable<float>, Graph<ExampleNode, ExampleEdge, float, Dijkstra<ExampleNode, ExampleEdge, float>>.IGraphEdge<float>
	{
		public ExampleNode first;
		public ExampleNode second;

		[SerializeField] LineRenderer lineRenderer;
		public bool Equals(float other)
		{
			throw new NotImplementedException();
		}

		public bool IsAccessible { get; }

		public float Length { get => length; }

		public float length;

		public void AddNodes(ExampleNode val1, ExampleNode val2)
		{
			first = val1;
			second = val2;

			lineRenderer.SetPosition(0, val1.transform.position);

			lineRenderer.SetPosition(1, val2.transform.position);

			transform.position = Vector3.Lerp(val1.transform.position, val2.transform.position, 0.5f);
		}
	}
}