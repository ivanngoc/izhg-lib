using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IziHardGames.Libs.Engine.LevelDesign.Tilemapping
{
	[CreateAssetMenu(menuName = "IziHardGames/Libs/Engine/LevelDesign/Tilemapping/TilemapRegion", fileName = "TilemapRegion")]
	public class TilemapRegionScriptable : ScriptableObject
	{
		[SerializeField] public List<Vector3> vector3s;

		#region Unity Message

		#endregion
	}

	[Serializable]
	public class TilemapRegion
	{
		[SerializeField] public List<Vector3Int> takenTilePoses = new List<Vector3Int>();
		[SerializeField] public List<int> indexesAccordingly = new List<int>();
		[SerializeField] public List<TileBase> tilesAccordingly = new List<TileBase>();
		[SerializeField] public List<int> tilesIdsAccordingly = new List<int>();
		[Space]
		[SerializeField] public int idRegion;
		[SerializeField] public Tilemap tilemapOfRegion;

		public string title;
	}

	public class TilemapRegionGraph
	{
		/// <summary>
		/// Откуда начинать поиск. Точка входа. Всегда 1
		/// </summary>
		public TilemapRegionNode center;
	}

	public class TilemapRegionNode
	{

	}
}