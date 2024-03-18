#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IziHardGames.Libs.Engine.LevelDesign.Tilemapping
{
	public class TilemapRegionTool : MonoBehaviour
	{
		[SerializeField] public Tilemap tilemapDebug;
		[SerializeField] public Tilemap tilemapMain;
		[SerializeField] public List<Tilemap> tilemaps;
		[Space]
		[SerializeField] public List<TilemapRegion> tilemapRegions = new List<TilemapRegion>();
		[SerializeField] public List<int> idsCorresponded;
		[SerializeField] public List<TileBase> tilesUsedInProject;
		[SerializeField] public List<TilePerPositionAtTilemap> tilesPerPositionAtTilemap;

		#region Unity Message
		private void OnEnable()
		{
			if (gameObject.tag != "EditorOnly")
			{
				throw new NotSupportedException($"GameObject Must Be Editor Only");
			}
		}

		#endregion
		[ContextMenu("Generate Preset")]
		public void Validate()
		{
			if (tilemaps.Count < 1) throw new ArgumentOutOfRangeException($"There is no any tilemap in [{nameof(tilemaps)}]");

			tilemapDebug.ClearAllTiles();

			idsCorresponded.Clear();
			tilesUsedInProject.Clear();
			tilemapRegions.Clear();
			tilesPerPositionAtTilemap.Clear();

			foreach (var tilemap in tilemaps)
			{
				string chars = tilemap.gameObject.name.LibTakeLast(3).Select(x => x.ToString()).Aggregate((x, y) => x + y);

				int id = int.Parse(chars);

				idsCorresponded.Add(id);
#if UNITY_EDITOR
				int loopProtect = 1000;
#endif
				int currentIndex = default;

				Vector3Int currentItter = tilemap.cellBounds.min;
				Vector3Int positionOfTile = default;
				TileBase foundedTile = default;

				TilemapRegion tilemapRegion = new TilemapRegion()
				{
					idRegion = id,
					title = tilemap.gameObject.name,
				};

				while (tilemap.GetUsedCoordNextXYZ(ref currentIndex, ref currentItter, ref positionOfTile, ref foundedTile))
				{
#if UNITY_EDITOR
					if (loopProtect-- < 0) throw new OverflowException();
#endif
					if (!tilesUsedInProject.Contains(foundedTile))
					{
						tilesUsedInProject.Add(foundedTile);
					}

					tilesPerPositionAtTilemap.Add(new TilePerPositionAtTilemap() { tileBase = foundedTile, position = positionOfTile });

					tilemapDebug.SetTile(positionOfTile, foundedTile);

					tilemapRegion.tilesAccordingly.Add(foundedTile);

					tilemapRegion.indexesAccordingly.Add(currentIndex);

					tilemapRegion.takenTilePoses.Add(positionOfTile);

					tilemapRegion.tilemapOfRegion = tilemap;
				}

				tilemapRegions.Add(tilemapRegion);
			}
			//tilemapMain.ResizeBounds();
		}
	}

	[Serializable]
	public class TilePerPositionAtTilemap
	{
		public TileBase tileBase;
		public Vector3Int position;
	}
}
#endif
