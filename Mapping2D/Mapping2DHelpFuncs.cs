namespace IziHardGames.NonEngine.Mapping.Mapping2D
{
	public static class Mapping2DHelpFuncs
	{
		#region Unity Message

		#endregion


		/// <summary>
		/// такой же принцип как и в <see cref="ExtensionVector3IntQuad.GetAtRadiusQuadsLayerPosition"/><br/>
		/// баунд мерится от 0 до sizeX/sizeY. Это значит если currentX|currentY будет отрицательным то условия не выполнится. 
		/// Поэтому нужно задавать смещение текущей координаты при передаче в параметре как если бы карта начиналась с 0,0<br/>
		/// </summary>
		/// <param name="indexCenter"></param>
		/// <param name="radius"></param>
		/// <param name="i">круговой индекс внешнего периметра дя указанного радисуа. Количество элементов всегда 6*radius. Индекс начинается с нижнего левого элемента и далее по часовой стрелке</param>
		/// <param name="currentCenterX">X со смещение в нулевую координату</param>
		/// <param name="currentCenterY">Y со смещение в нулевую координату</param>
		/// <param name="sizeX"> - размер границы карты по горизонтали от 0 до этого числа</param>
		/// <param name="sizeY">- размер границы карты по вертикали от 0 до этого числа</param>
		/// <param name="indexMap"></param>
		/// <returns></returns>
		public static bool TryGetIndexBound(int indexCenter, int radius, int i, int currentCenterX, int currentCenterY, int sizeX, int sizeY, out int indexMap)
		{
			var cortage = GetIndexBound(indexCenter, radius, sizeX, i);

			indexMap = cortage.Item1;

			int actualX = currentCenterX + cortage.Item2;

			if (0 <= actualX && actualX < sizeX)
			{
				int actualY = currentCenterY + cortage.Item3;

				if (0 <= actualY && actualY < sizeY)
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// По сути нужен для выявления количества переходов отдельно по X а затем по Y от указанного стартового индекса в таблице до указанного радиального конечного.
		/// Индекс внутри таблицы с началом в 0,0,0 <br/>.
		/// <inheritdoc cref="TryGetIndexBound"/>
		/// Принцип основан на нахождении смещения по каждой отдельной оси относительно центра
		/// </summary>
		/// <remarks>
		/// Без разницы от смещения координат в нулевую точку
		/// </remarks>
		/// <param name="indexCenter"></param>
		/// <param name="radius"></param>
		/// <param name="countColumnX">количество столбиков</param>
		/// <param name="indexTarget">Кольцевой индекс, индекс ячейки по периметру</param>
		/// <returns>
		/// Index of cell with offset from parameters<br/>
		/// Item0: index of cell in table with origin at 0,0,0<br/>
		/// Item1: offset by x-axis <br/>
		/// Item2: offset by y-axis <br/>
		/// </returns>
		public static (int, int, int) GetIndexBound(int indexCenter, int radius, int countColumnX, int indexTarget)
		{
			if (radius == 0)
			{
				//offsetVertical = default;
				//offsetHorizontal = default;

				return (indexCenter, 0, 0);
			}
			int circleCount = radius * 8;

			int oneSideCount = 2 * radius + 1;
			// иднекс верхнего правого угла
			int half = circleCount >> 1;
			// сколько нужно отсчитать от любого крайнего угла до прилежащей середины
			int oneSideMid = oneSideCount >> 1;

			int offsetVertical;
			int offsetHorizontal;

			if (indexTarget < half)
			{
				if (indexTarget < oneSideCount)
				{
					offsetVertical = indexTarget - oneSideMid;
					offsetHorizontal = -radius;
				}
				else
				{
					offsetVertical = oneSideMid;
					offsetHorizontal = indexTarget - oneSideCount + 1 - oneSideMid;
				}
			}
			else
			{   // индекс нижнего правого углас
				int third = half + (half >> 1);

				if (indexTarget > third)
				{
					offsetVertical = -radius;
					offsetHorizontal = circleCount - indexTarget - oneSideMid;
				}
				else
				{
					offsetVertical = oneSideMid - indexTarget + half;
					offsetHorizontal = radius;
				}
			}
			return (indexCenter + (countColumnX * offsetVertical) + offsetHorizontal, offsetHorizontal, offsetVertical);
		}

	}
}