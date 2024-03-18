using IziHardGames.Libs.NonEngine.Vectors;

namespace UnityEngine
{
	public static partial class ExtensionsVectorConversionsToIndex
	{
		public static int ToCellIndex(this Vector3Int val, Point3Int sizeOfTable, Point3Int origin)
		{
			return (val.y - origin.y) * sizeOfTable.x + (val.x - origin.x);
		}
	}

}