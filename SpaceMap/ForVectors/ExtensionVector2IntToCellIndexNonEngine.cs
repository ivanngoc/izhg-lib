using Vector2Int = IziHardGames.Libs.NonEngine.Vectors.Point2Int;

namespace IziHardGames.Libs.NonEngine.Vectors.Extensions
{
	public static partial class ExtensionVector2IntToCellIndexNonEngine
	{
		public static int ToCellIndex(this ref Vector2Int self, ref Vector2Int size, int precisionGrade)
		{
			Vector2Int shifted = new Vector2Int(self.x >> precisionGrade, self.y >> precisionGrade);

			return shifted.ToCellIndex(ref size);
		}
		public static int ToCellIndex(this ref Vector2Int v, ref Vector2Int sizeMapIn)
		{
			return v.y * sizeMapIn.x + v.x;
		}
		public static int ToCellIndex(this ref Vector2Int self, ref Vector2Int size, ref Vector2Int origin)
		{
			var temp = (self - origin);
			return temp.ToCellIndex(ref size);
		}
	}
}