using IziHardGames.Libs.NonEngine.Graphs.QuadTile.Ground;
using IziHardGames.UserControl.InputSystem.Unity;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IziHardGames.UserControl.InputSystem.Unity.Raycasting
{
	/// <summary>
	/// 1 Raycaster per 1 tilemap
	/// </summary>
	[CreateAssetMenu(menuName = "IziHardGames/UserControl/Raycasting/Tilemap", fileName = "RaycastResultForTilemap")]
	public class RaycasterTilemap //: RaycasterBase
	{
		// /// <summary>
		// /// Min Depth of tile to search <see cref="TileBase"/>
		// /// </summary>
		// [SerializeField] public int tileZMin;
		// /// <summary>
		// /// Max Depth of tile to search <see cref="TileBase"/>
		// /// </summary>
		// [SerializeField] public int tileZMax;

		// [NonSerialized] public Vector3Int tilePosForDetect;
		// [NonSerialized] public TileBase[] tileBases;
		// [NonSerialized] public int countTileBases;
		// [NonSerialized] Tilemap tilemap;

		// [SerializeField] DataInput dataInput;

		// public DataGroundDynamic dataGroundDynamic;
		// /// <summary>
		// /// provide array for func to write objects into. Specify limitations of array and get as result count of founded objects in given pos.
		// /// </summary>
		// public Func<Transform[], Vector3Int, int, int> funcGetObjectByPos;

		// #region Unity Message

		// #endregion

		// public override void Initilize()
		// {
		// 	base.Initilize();

		// 	int depth = tileZMax - tileZMin;

		// 	SetDepth(depth, depth);

		// 	tileBases = new TileBase[depth];
		// }

		// public void Detect(Vector3 screenPos)
		// {
		// 	Clear();

		// 	countTileBases = default;
		// 	countHitsComponents = default;

		// 	var tilePos = tilemap.WorldToCell(cam.ScreenToWorldPoint(screenPos));

		// 	tilePosForDetect = tilePos;

		// 	for (int i = 0; i < hitDepth; i++)
		// 	{
		// 		TileBase tileBase = tilemap.GetTile(new Vector3Int(tilePosForDetect.x, tilePosForDetect.y, tileZMin + i));

		// 		if (tileBase != null)
		// 		{
		// 			tileBases[countTileBases] = tileBase;
		// 			countTileBases++;
		// 		}
		// 	}
		// 	int count = funcGetObjectByPos(hitArray, tilePos, hitDepth);
		// 	countHitsTransforms = count;

		// 	for (int i = 0; i < count; i++)
		// 	{
		// 		componentsPerHit[i] = hitArray[i].GetComponents<Component>();
		// 		countHitsComponents += componentsPerHit[i].Length;
		// 	}
		// }

		// public void Enable(Tilemap tilemap)
		// {
		// 	Enable();
		// 	this.tilemap = tilemap;
		// }
		// public void Disable(Tilemap tilemap)
		// {
		// 	Disable();
		// 	this.tilemap = default;
		// }

		// public override void ExecuteUpdate()
		// {
		// 	base.ExecuteUpdate();

		// 	if (!IsRaycastAllowed()) return;

		// 	Detect(dataInput.pointerPos3dNew);
		// }
	}
}