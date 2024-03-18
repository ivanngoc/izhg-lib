using UnityEngine;
using UnityEngine.Tilemaps;

namespace IziHardGames.Libs.Engine.Tilemapping
{
	[CreateAssetMenu(menuName = "IziHardGames/Libs/Engine/Tilemapping/TileAtProject", fileName = "TileAtProject")]
	public class TileAtProject : ScriptableObject
	{
		public int id;
		public TileBase tileBase;
	}
}