#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
// using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace IziHardGames.Libs.Engine.LevelDesign.Tilemapping
{
	public class TileRegionSelectorWindow : EditorWindow
	{
	// 	//List<TilemapRegion> tilemapRegions;
	// 	private Material material;
	// 	private TilemapRegionScriptable tilemapRegion;
	// 	private Tilemap tilemap;
	// 	private Vector3 mousPos;
	// 	//private TileBase tileBaseSelection;

	// 	private static GameObject tempGameObject;
	// 	private static MeshRenderer tempMeshrenderer;
	// 	private static MeshFilter tempMeshFilter;
	// 	private static Mesh tempMesh;
	// 	///// <summary>
	// 	///// Служебная копия на служебном ГО
	// 	///// </summary>
	// 	//private static Tilemap tilemapView;

	// 	private Vector3Int[] selectionCoords = new Vector3Int[4];
	// 	private Vector3[] selectionVerts = new Vector3[4];
	// 	private int[] triangles;

	// 	#region Unity Message
	// 	private void OnEnable()
	// 	{
	// 		SceneView.duringSceneGui += SceneGUIHandler;

	// 		SetupTilemap();
	// 	}
	// 	private void OnDisable()
	// 	{
	// 		SceneView.duringSceneGui -= SceneGUIHandler;

	// 		if (tempGameObject != null)
	// 		{
	// 			DestroyImmediate(tempGameObject);
	// 			tempGameObject = default;
	// 			tempMeshrenderer = default;
	// 			tempMeshFilter = default;
	// 		}
	// 	}

	// 	private void OnGUI()
	// 	{
	// 		if (Event.current.type == EventType.MouseMove)
	// 		{
	// 			Repaint();
	// 		}

	// 		GUI.enabled = false;
	// 		EditorGUILayout.ObjectField("Script Editor:", MonoScript.FromScriptableObject(this), typeof(TileRegionSelectorWindow), true);
	// 		mousPos = EditorGUILayout.Vector3Field("MousePos:", mousPos);
	// 		var someI = EditorGUILayout.TextField("DateTime:", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:zz"));
	// 		tempMesh = EditorGUILayout.ObjectField("Mesh:", tempMesh, typeof(Mesh), true) as Mesh;
	// 		GUI.enabled = true;

	// 		EditorGUILayout.Space();

	// 		//tileBaseSelection = EditorGUILayout.ObjectField("selection Tile:", tileBaseSelection, typeof(TileBase), true) as TileBase;

	// 		var newObj = EditorGUILayout.ObjectField("Edit:", tilemapRegion, typeof(TilemapRegionScriptable), true) as TilemapRegionScriptable;

	// 		material = EditorGUILayout.ObjectField("Material:", material, typeof(Material), true) as Material;

	// 		if (tilemapRegion != newObj)
	// 		{
	// 			tilemapRegion = newObj;
	// 		}

	// 		var newTilemap = EditorGUILayout.ObjectField("Tilemap:", tilemap, typeof(Tilemap), true) as Tilemap;

	// 		if (tilemap != newTilemap)
	// 		{
	// 			tilemap = newTilemap;

	// 			SetupTilemap();
	// 		}

	// 		EditorGUILayout.Space();

	// 		if (GUILayout.Button("Сбросит рабочую копию"))
	// 		{
	// 			if (tempGameObject != null)
	// 			{
	// 				DestroyImmediate(tempGameObject);

	// 				tempGameObject = default;
	// 				tempMeshrenderer = default;
	// 				tempMeshFilter = default;
	// 			}
	// 			SetupTilemap();
	// 		}
	// 	}

	// 	#endregion
	// 	[MenuItem("*IziHardGames/Libs/Engine/LevelDesign/Tilemapping/Tile Region Selector Window")]
	// 	[MenuItem("Window/2D/Level Design/Tile Region Selector Window")]
	// 	private static void Init()
	// 	{
	// 		TileRegionSelectorWindow window = (TileRegionSelectorWindow)EditorWindow.GetWindow(typeof(TileRegionSelectorWindow));
	// 		window.titleContent = new GUIContent("Tile Region Selector Window");
	// 		window.CreateElements();
	// 		window.Show();
	// 	}
	// 	private void CreateElements()
	// 	{
	// 		//AssetDatabase.find
	// 	}
	// 	private void SceneGUIHandler(SceneView obj)
	// 	{
	// 		Handles.BeginGUI();
	// 		Handles.EndGUI();

	// 		mousPos = Event.current.mousePosition;
	// 		//mousPos = Input.mousePosition;
	// 		mousPos = Mouse.current.position.ReadValue();

	// 		if (tilemap != null && tilemapRegion != null && Event.current.type == EventType.MouseDown && Event.current.button == 0)
	// 		{
	// 			Highlight();
	// 		}
	// 		Repaint();
	// 	}
	// 	/// <summary>
	// 	/// Посдветить объекты в окне сцены
	// 	/// </summary>
	// 	public void Highlight()
	// 	{
	// 		//Debug.Log("Highlight", this);

	// 		Camera camera = SceneView.lastActiveSceneView.camera;

	// 		var cells = tilemapRegion.vector3s;

	// 		var cellPosVInt = tilemap.ScreenToCell(camera, Mouse.current.position.ReadValue().ScreenOppositeY());

	// 		var worldCellCenter = tilemap.GetCellCenterWorld(cellPosVInt);

	// 		//var bound = tilemap.GetBoundsLocal(cellPosVInt);

	// 		//selectionVerts[0] = bound.min;
	// 		//selectionVerts[1] = new Vector3(bound.min.x, bound.max.y);
	// 		//selectionVerts[2] = bound.max;
	// 		//selectionVerts[3] = new Vector3(bound.max.x, bound.min.y);

	// 		//selectionVerts[0] = Vector3.zero;
	// 		//selectionVerts[1] = Vector3.up;
	// 		//selectionVerts[2] = Vector3.up + Vector3.right;
	// 		//selectionVerts[3] = Vector3.right;

	// 		selectionCoords[0] = cellPosVInt;
	// 		selectionCoords[1] = cellPosVInt + Vector3Int.up;
	// 		selectionCoords[2] = selectionCoords[1] + Vector3Int.right;
	// 		selectionCoords[3] = selectionCoords[2] + Vector3Int.down;

	// 		tempGameObject.transform.position = worldCellCenter;
	// 		// рисует 2d
	// 		//Handles.DrawSolidRectangleWithOutline(selectionVerts, Color.white, Color.yellow);
	// 		//Handles.DrawAAPolyLine(selectionVerts[0], selectionVerts[1], selectionVerts[2], selectionVerts[3]);

	// 		//for (int i = 0; i < selectionVerts.Length; i++)
	// 		//{
	// 		//	selectionVerts[i] = camera.WorldToScreenPoint(selectionVerts[i]);
	// 		//	selectionVerts[i].z = 0;
	// 		//}
	// 		for (int i = 0; i < selectionVerts.Length; i++)
	// 		{
	// 			selectionVerts[i] = tilemap.CellToWorld(selectionCoords[i]) - worldCellCenter;
	// 			selectionVerts[i].z = 0;
	// 		}

	// 		tempMesh.vertices = selectionVerts;
	// 		tempMesh.triangles = triangles;
	// 		tempMeshrenderer.sharedMaterial = material;

	// 		tempMeshFilter.mesh = tempMesh;
	// 	}

	// 	private void SetupTilemap()
	// 	{
	// 		if (tempGameObject == null)
	// 		{
	// 			tempGameObject = EditorUtility.CreateGameObjectWithHideFlags("Temp GO", HideFlags.HideAndDontSave, typeof(MeshFilter), typeof(MeshRenderer));
	// 		}
	// 		if (tempMesh == null)
	// 		{
	// 			tempMesh = new Mesh();
	// 		}
	// 		if (tempGameObject != null && tilemap != null)
	// 		{
	// 			tempMeshFilter = tempGameObject.GetComponent<MeshFilter>();
	// 			tempMeshrenderer = tempGameObject.GetComponent<MeshRenderer>();
	// 			tempMeshrenderer.sharedMaterial = material;
	// 		}

	// 		triangles = new int[] { 0, 1, 2, 2, 3, 0 };
	// 	}
	}
}
#endif
