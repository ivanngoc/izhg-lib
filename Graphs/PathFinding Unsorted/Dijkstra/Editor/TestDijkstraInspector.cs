#if UNITY_EDITOR
using IziHardGames.Libs.Graphs.PathFinding;
using UnityEditor;
using UnityEngine;

namespace IziHardGames.GameProject1
{
	[UnityEditor.CustomEditor(typeof(TestDijkstra))]
	public class TestDijkstraInspector : Editor
	{
		bool isShowTitles;
		#region Unity Message
		private void OnSceneGUI()
		{
			if (isShowTitles)
			{
				TestDijkstra testDijkstra = target as TestDijkstra;

				if (testDijkstra.exampleDijkstra.dataNodes.Length == testDijkstra.exampleNodes.Length)
				{
					for (int i = 0; i < testDijkstra.exampleNodes.Length; i++)
					{
						ExampleNode item = testDijkstra.exampleNodes[i];

						Handles.Label(item.transform.position, $"Id:{item.id}|{testDijkstra.exampleDijkstra.dataNodes[i].weight}|{testDijkstra.exampleDijkstra.dataNodes[i].proceedIndex}");
					}
				}

				for (int i = 0; i < testDijkstra.exampleEdges.Length; i++)
				{
					Handles.Label(testDijkstra.exampleEdges[i].transform.position, $"{i}|{testDijkstra.exampleEdges[i].Length}");
				}
			}
		}
		public override void OnInspectorGUI()
		{
			GUI.enabled = false;
			EditorGUILayout.ObjectField("Script Editor:", MonoScript.FromScriptableObject(this), typeof(TestDijkstraInspector), true);
			GUI.enabled = true;
			DrawDefaultInspector();

			if (GUILayout.Button("Включить надписи"))
			{
				isShowTitles = !isShowTitles;
			}
		}
		#endregion
	}
}
#endif
