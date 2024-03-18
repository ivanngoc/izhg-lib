using IziHardGames.NonEngine.Mapping.Mapping2D;
using UnityEngine;
using Vector2Int = IziHardGames.Libs.NonEngine.Vectors.Point2Int;
using static IziHardGames.Libs.Engine.PositioningSystem.SystemMapping<IziHardGames.Libs.NonEngine.Vectors.Point2Int, int[], int>;
using IziHardGames.Libs.NonEngine.Vectors.Extensions;

namespace IziHardGames.Libs.Engine.PositioningSystem
{
	/// <summary>
	/// Набор функций получения индексов квадратов карты занятыми объектом
	/// </summary>
	[CreateAssetMenu(menuName = "IziHardGames/Libs/Engine/PositioningSystem/FuncsMappingTilemapQuad", fileName = "FuncsMappingTilemapQuad")]
	//public class FuncsMappingTilemapQuad : WrapMappingFuncs<Vector2Int, Vector1<int>>
	public class FuncsMappingTilemapQuad : WrapMappingFuncs
	{
		protected override void OnEnable()
		{
			//funcGetIndexes = TryGetMapIndexes;
			funcPosToIndex = PosToIndex;
			funcSum = Sum;
			funcGetCellIndex = TryGetMapIndex;
		}

		//public bool TryGetMapIndexes(out int countIndex, out Vector1<int> mapCellIndexes)
		//{
		//    countIndex = 1;

		//    throw new NotImplementedException();
		//}

		public int PosToIndex(Vector2Int vector2Int, Vector2Int size, int rank)
		{
			return vector2Int.ToCellIndex(ref size, rank);
		}

		public Vector2Int Sum(Vector2Int a, Vector2Int b)
		{
			return a + b;
		}

		public int GetX(ref Vector2Int vector2Int)
		{
			return vector2Int.x;
		}
		public int GetY(ref Vector2Int vector2Int)
		{
			return vector2Int.x;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="paramsGetIndexOfCell"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public bool TryGetMapIndex(in ParamsGetIndexOfCell paramsGetIndexOfCell, out int index)
		{
			Vector2Int pos = paramsGetIndexOfCell.posShifted;
			Vector2Int size = paramsGetIndexOfCell.size;

			if (Mapping2DHelpFuncs.TryGetIndexBound(paramsGetIndexOfCell.indexPosShifted, paramsGetIndexOfCell.radius, paramsGetIndexOfCell.indexCircle, pos.x, pos.y, size.x, size.y, out index))
			{
				return true;
			}
			return false;
		}
	}
}