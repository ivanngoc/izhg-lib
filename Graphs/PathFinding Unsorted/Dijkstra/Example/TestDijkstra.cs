using IziHardGames.Extensions.PremetiveTypes;
using IziHardGames.Libs.Graphs.PathFinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IziHardGames.GameProject1
{
	public class TestDijkstra : MonoBehaviour
	{
		[SerializeField] public ExampleDijkstra exampleDijkstra;

		[SerializeField] public ExampleNode[] exampleNodes;
		[SerializeField] public ExampleEdge[] exampleEdges;

		[SerializeField] public GameObject prefabNode;
		[SerializeField] public GameObject prefabEdge;
		[Space]
		[SerializeField] public List<ExampleNode> result;
		[SerializeField] public Material material;
		private void Awake()
		{
			exampleDijkstra = new ExampleDijkstra(exampleNodes, exampleNodes.Length, exampleEdges, exampleEdges.Length);

			ICollection<ExampleNode> collection = result;

			exampleDijkstra.FindPath(exampleNodes[11], exampleNodes[88], ref collection);

			foreach (var item in collection)
			{
				item.GetComponent<Renderer>().material = material;
			}

			for (int i = 1; i < result.Count; i++)
			{
				var v = result[i];

				result[i - 1].GetJointEdge(ref v).GetComponent<LineRenderer>().material = default;
			}
		}

		private void OnEnable()
		{

		}

		[ContextMenu("—генерить")]
		public void Generate()
		{
			exampleNodes = new ExampleNode[100];
			exampleEdges = new ExampleEdge[400];

			int count = default;

			for (int y = 0; y < 10; y++)
			{
				for (int x = 0; x < 10; x++)
				{
					GameObject gameObject = Instantiate(prefabNode);

					gameObject.name += $"[{count}]";

					gameObject.transform.position = new Vector3(x, y, 0);

					exampleNodes[count] = gameObject.GetComponent<ExampleNode>();

					exampleNodes[count].id = count;

					count++;
				}
			}

			int X = default;
			int Y = default;

			int edgeCount = default;

			for (int i = 0; i < 100; i++)
			{
				if (exampleNodes[i].transform.position.x < 9)
				{
					GameObject edgeRight = Instantiate(prefabEdge);

					ExampleNode from = exampleNodes[i];

					ExampleNode to = exampleNodes.First(x => x.transform.position == new Vector3(X + 1, Y));

					ExampleEdge exampleEdge = edgeRight.GetComponent<ExampleEdge>();

					from.AddEdge(exampleEdge);

					to.AddEdge(exampleEdge);

					exampleEdge.AddNodes(from, to);

					exampleEdges[edgeCount] = exampleEdge;

					edgeRight.name += $"[{edgeCount}]";

					edgeCount++;
				}

				if (exampleNodes[i].transform.position.y < 9)
				{
					ExampleNode from = exampleNodes[i];

					GameObject edgeTop = Instantiate(prefabEdge);

					edgeTop.name += $"[{edgeCount}]";

					ExampleNode toTop = exampleNodes.First(x => x.transform.position == new Vector3(X, Y + 1));

					ExampleEdge exampleEdge = edgeTop.GetComponent<ExampleEdge>();

					from.AddEdge(exampleEdge);

					toTop.AddEdge(exampleEdge);

					exampleEdge.AddNodes(from, toTop);

					exampleEdges[edgeCount] = exampleEdge;

					edgeCount++;
				}

				X++;

				if (X > 9)
				{
					X = 0;
					Y++;
				}
			}

			X = default;
			Y = default;

			for (int i = 0; i < 100; i++)
			{
				ExampleNode exampleNode = exampleNodes[i];
				// top
				if (!exampleNodes[i].transform.position.y.IsEqualDeci(9))
				{
					ExampleNode toAdd = exampleNodes.First(x => x.transform.position == new Vector3(X, Y + 1));

					exampleNode.AddNode(toAdd);
				}
				// right
				if (!exampleNodes[i].transform.position.x.IsEqualDeci(9))
				{
					ExampleNode toAdd = exampleNodes.First(x => x.transform.position == new Vector3(X + 1, Y));

					exampleNode.AddNode(toAdd);
				}

				// bot
				if (!exampleNodes[i].transform.position.y.IsEqualDeci(0))
				{
					ExampleNode toAdd = exampleNodes.First(x => x.transform.position == new Vector3(X, Y - 1));

					exampleNode.AddNode(toAdd);
				}
				// left
				if (!exampleNodes[i].transform.position.x.IsEqualDeci(0))
				{
					ExampleNode toAdd = exampleNodes.First(x => x.transform.position == new Vector3(X - 1, Y));

					exampleNode.AddNode(toAdd);
				}
				X++;

				if (X > 9)
				{
					X = 0;
					Y++;
				}
			}

			for (int i = 0; i < 100; i++)
			{
				exampleNodes[i].exampleEdges = exampleNodes[i].exampleEdges.Where(x => x != null).ToArray();
				exampleNodes[i].exampleNodes = exampleNodes[i].exampleNodes.Where(x => x != null).ToArray();
			}

			exampleEdges = exampleEdges.Where(x => x != null).ToArray();

			for (int i = 0; i < exampleEdges.Length; i++)
			{
				exampleEdges[i].length = Random.Range(1, 10);
			}
		}
		[ContextMenu("—копировать данные вершины из дейкстры")]
		public void CopyDataNode()
		{
			for (int i = 0; i < exampleDijkstra.Nodes.Length; i++)
			{
				exampleNodes[i].dataNode = exampleDijkstra.dataNodes[i];
			}
		}
	}
}