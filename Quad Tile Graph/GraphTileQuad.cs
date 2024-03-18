using System;
using System.Collections.Generic;



namespace IziHardGames.Libs.NonEngine.Graphs.QuadTile
{
	[Serializable]
	public struct QuadGraphTileNode
	{
		/// <summary>
		/// index in array as ID
		/// </summary>
		public int index;
		/// <summary>
		/// 0000 - 0
		/// 0001 - 1 
		/// 0010 - 2
		/// 0011 - 3
		/// 0100 - 4
		/// 0101 - 5
		/// 0110 - 6
		/// 0111 - 7
		/// 1000 - 8
		/// 1001 - 9
		/// 1010 - 10
		/// 1011 - 11
		/// 1100 - 12
		/// 1101 - 13
		/// 1110 - 14
		/// 1111 - 15
		/// </summary>
		public int neighbourhoodType;
		public int state;
	}
	[Serializable]
	public struct QuadGraphTileCoord
	{
		public int x;
		public int y;
		public int z;
	}
	[Serializable]
	public struct QuadGraphTileNeighbourhood
	{
		public int indexTop;
		public int indexRight;
		public int indexBot;
		public int indexLeft;
	}

	[Serializable]
	public class GraphTile
	{
		public QuadGraphTileNode[] graphTileNodes;
		public QuadGraphTileCoord[] graphTileCoords;
		public QuadGraphTileNeighbourhood[] graphTileNeighbourhoods;

		public int countCells;

		public void CreateGraph2D(int xOrigin, int yOrigin, int sizeX, int sizeY)
		{
			int count = countCells = sizeX * sizeY;

			graphTileNodes = new QuadGraphTileNode[count];
			graphTileCoords = new QuadGraphTileCoord[count];

			int index = default;

			for (int y = 0; y < sizeY; y++)
			{
				for (int x = 0; x < sizeX; x++)
				{
					graphTileNodes[index] = new QuadGraphTileNode()
					{
						index = index,
					};

					graphTileCoords[index] = new QuadGraphTileCoord()
					{
						x = x + xOrigin,
						y = y + yOrigin,
						z = default,
					};

					index++;
				}
			}
		}
	}
	/// <summary>
	/// Смысл в индукционном поиске 
	/// по форме креста то есть движению по 4 сторонам от ячейки,без переходов по диаганали <br/>
	/// или по форме квадрата, то есть своего рода квдаратная рамка образованная внешним контуром вокруг ячейки при равном удалении от ячейки во все стороны включая диагональные переходы<br/>
	/// переход производится не по наращиванию координаты а по переходам по соседям узлов которые имеют определенную форму <see cref="QuadGraphTileNode.neighbourhoodType"/>
	/// Например если 4 соотвествует паттерну когда с 4 примыкающих прямых (не диагональных) сторон, то в открытый список следующего уровня индукции (радиуса/дистанции) добавляются индексы в массиве этих 4 соседних узлов.
	/// Если бы паттерн был бы 3 к примеру который бы показывал что есть сверху/снизу/справа сосед, то в открытый список попали бы эти 3 индекса узлов<br/>
	/// Суть в том чтобы экономить с переходов не смещением координаты а предопределенным переходом. Таким образом не придется проверять входит ли координата в карту
	/// Размер кода при этом вырастает так как надо свичить добавление в открытый список в зависимости от значение паттерна <see cref="QuadGraphTileNode.neighbourhoodType"/>
	/// </summary>
	/// <remarks>
	/// Проблема в том что если есть 2 замкнутых конутра корты не связанных друг с другом ячейками. 
	/// Тогда затрудняется переход так как неизвестен радиус от места индукции до ячеек за разрывом<br/>
	/// Процесс продвижения выглядит следующим образом: каждая индукция добавляет в <see cref="indexes"/> все индексы ячеек карты этого радиуса. 
	/// Затем передается на внешнее управление, где могут поэлеметно перебрать все ячейки этого радисуа. Если поиск не успешен, индукция расширяется на следующий радиус инкрементно
	/// и цикл внешнего перебора продолжаеься.<br/>
	/// 2 режима индукции: по связям или по смещениям координат. По связям ищет только по текущему контуру, а по смещениям - по всей области
	/// В этом классе сделан по переходам по связям
	/// </remarks>
	public class GraphTileJobByTransition
	{
		public List<int> openList;
		public List<int> closeList;
		public GraphTile graphTile;
		public int[] indexes;
		public int circleCountCurrent;

		public GraphTileJobByTransition(GraphTile graphTileArg)
		{
			Initilize(graphTileArg);
		}
		public void Initilize(GraphTile graphTileArg)
		{
			indexes = new int[graphTileArg.graphTileNodes.Length];
			circleCountCurrent = default;
		}
		public void Clean() { }

		public void GetPositionsAtDistance_Quad()
		{
			throw new NotImplementedException();
		}
		public void GetPositionsAtDistance_Cross()
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class GraphTileJobByExpansion
	{
		//public List<int> openList;
		//public List<int> closeList;
		//public GraphTile graphTile;
		//public int[] indexes;
		//public int circleCountCurrent;

		//public int indexCenter;

		//public int countRawY;
		//public int countColumnX;

		//public int xMax;
		//public int yMax;
		//public int zMax;

		//public int xMin;
		//public int yMin;
		//public int zMin;

		public GraphTileJobByExpansion(int xArg, int yArg)
		{
			//countColumnX = xArg;
			//countRawY = yArg;
		}

		public void SetRadius(int radius)
		{

		}
		public void ExpandIncremental()
		{

		}
		/// <summary>
		/// По часовой стрелке<br/>
		/// Общее количество иттераций соотвествует Radius * 8<br/>
		/// Иттерация из нижнего левого угла в верхний левый, затем вернхий правый, зетем нижний правый, затем возврат в нижний левый
		/// </summary>
		/// <param name="index"></param>
		/// <returns>
		/// is gona be next itteration?
		/// </returns>
		public bool TryItterateNext(ref int indexCircleExpansion, out int indexResult)
		{
			/// преообразовать индекс <see cref="indexCircleExpansion"/> в индекс карты с заданным размером 
			indexCircleExpansion++;

			throw new NotImplementedException();
		}

		public void Formulas()
		{
			// offset horizontal [-radisu, + radius]
			int offsetHorizontal = default;

			int index = default;
			// top raw
			//int topRawIndex = index + countRaw + offsetHorizontal;

		}
	}

}