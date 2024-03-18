using IziHardGames.Extensions.PremetiveTypes;
using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace UnityEngine
{

	public static partial class ExtensionVector3IntHex
	{
		#region Common
		public static void Abs(this Vector3Int v)
		{
			if (v.x < 0)
			{
				if (v.y.IsEven())
				{
					v.x = -v.x;
				}
				else
				{
					v.x = -v.x - 1;
				}
			}
			v.y = Mathf.Abs(v.y);
			v.z = Mathf.Abs(v.z);
		}
		public static Vector3Int SetRelativeToZero(this Vector3Int origin, Vector3Int endpoint)
		{
			// odd - odd
			// 4, 3, 0
			// 2, 1, 0
			// diff 2,2 + 

			// even - odd
			// 4 3 0
			// 2 0 0
			// 2 3 0 diff +

			// odd - even (0.5+0.5)
			// 7 4 0
			// 2 1 0
			// 5 3 0 diff - (right 4 3 0)

			// even even
			// 6 6 0
			// 3 2 0
			// 3 4 0 diff +

			if (origin.y.IsOdd() && endpoint.y.IsEven())
			{
				return endpoint - origin + Vector3Int.left;
			}
			else
			{
				return endpoint - origin;
			}
		}
		#endregion

		#region Neigbour
		/// <summary>
		/// Хексагоны имеют общую сторону или стоят в одной точке
		/// </summary>
		/// <param name="self"></param>
		/// <param name="compareTo"></param>
		/// <returns></returns>
		public static bool IsHexIntersect(this Vector3Int self, Vector3Int compareTo)
		{
			// не работает
			//float distance = (self - compareTo).magnitude;
			//return distance.IsEqualDeci(1f) || distance.IsEqualMicro(1.414214f);
			var diff = compareTo - self;

			var absY = Mathf.Abs(diff.y);

			if (self.y.IsEven())
			{
				if (absY < 2 && !(diff.x < -1 || 1 < absY + diff.x)) return true;
			}
			else
			{
				if (absY < 2 && !(diff.x < 1 || 1 < absY + Mathf.Abs(diff.x))) return true;
			}

			return false;
		}
		public static void GetNeigbours(this Vector3Int self, ref List<Vector3Int> result)
		{
			result.Add(self.GetNorth());
			result.Add(self.GetNorthEast());
			result.Add(self.GetSouthEast());
			result.Add(self.GetSouth());
			result.Add(self.GetSouthWest());
			result.Add(self.GetNorthWest());
		}

		/// <summary>
		/// Ячейка граничи с ячейкой. Вернет False если оба стоят в одном месте
		/// </summary>
		/// <param name="self"></param>
		/// <param name="with"></param>
		/// <returns></returns>
		public static bool IsSideBySideWith(this Vector3Int self, Vector3Int with)
		{
			var yDiff = Math.Abs((self - with).y);

			var isEven = self.y.IsEven();

			var diff = with - self;

			if (yDiff > 1) return false;

			var xySumAbs = Mathf.Abs(diff.x + diff.y);

			if (isEven)
			{
				if (diff.x == -1)
					return true;
				else
				{
					if (xySumAbs == 1) return true;
				}
			}
			else
			{
				if (diff.x == 1)
					return true;
				else
				{
					if (xySumAbs == 1) return true;
				}
			}
			// Note: Vector3 не может быть использован так как при изменении размера ячейки изменяются кординаты
			//float distance = (tilemap.CellToWorld(self) - tilemap.CellToWorld(with)).magnitude;
			//return distance < 0.5531;
			return false;
		}

		#endregion

		#region Radius

		/// <summary>
		/// Получить радиус ячейки - количество минимальных переходов до ячейки
		/// </summary>
		/// <param name="center"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static int GetRadius(this Vector3Int target, Vector3Int center)
		{
			var diff = target - center;
			var diffAbs = new Vector3Int(Mathf.Abs(diff.x), Mathf.Abs(diff.y), 0);

			var isCenterYOdd = center.y.IsOdd();

			if (isCenterYOdd)
			{
				if (diff.x > 0 && diff.y != 0 && diff.y.IsOdd())
					diffAbs.x--;
			}
			else
			{
				if (diff.x < 0 && diff.y != 0 && diff.y.IsOdd())
					diffAbs.x--;
			}
			// количество диоганальных переходов без вычета по X 
			var timesNoX = diffAbs.y / 2;

			var xTransitions = Mathf.Clamp(diffAbs.x - timesNoX, 0, diffAbs.x);

			return diffAbs.y + xTransitions;
		}

		/// <summary>
		/// Размер входного массива должен быть равен произведению радиуса на 6
		/// </summary>
		/// <param name="center"></param>
		/// <param name="toFill"></param>
		/// <param name="rad"></param>
		/// <param name="indexFrom"></param>
		/// <returns></returns>
		public static bool GetCircleAtRadiusNonAlloc(this Vector3Int center, Vector3Int[] toFill, int rad, int indexFrom = default)
		{
			//Debug.Log($"GetAllAtRadius {center} rad = {rad}");

			var countTotal = rad * 6;

			if (countTotal > toFill.Length - indexFrom) throw new ArgumentException($"Size is not fitting. Recieve {toFill.Length}. Must be >{countTotal + indexFrom}. From Index {indexFrom}");

			var xShift = center.x;
			var yShift = center.y;

			var index = indexFrom;

			toFill[index] = new Vector3Int(rad + xShift, yShift, 0);

			for (var i = 0; i < rad; i++)
			{
				toFill[index + 1] = toFill[index].GetSouthEast();
				index++;
			}
			for (var i = 0; i < rad; i++)
			{
				toFill[index + 1] = toFill[index].GetSouth();
				index++;
			}
			for (var i = 0; i < rad; i++)
			{
				toFill[index + 1] = toFill[index].GetSouthWest();
				index++;
			}
			for (var i = 0; i < rad; i++)
			{
				toFill[index + 1] = toFill[index].GetNorthWest();
				index++;
			}
			for (var i = 0; i < rad; i++)
			{
				toFill[index + 1] = toFill[index].GetNorth();
				index++;
			}
			for (var i = 0; i < rad - 1; i++)
			{
				toFill[index + 1] = toFill[index].GetNorthEast();
				index++;
			}

			return true;
		}
		public static Vector3Int[] GetCircleAtRadius(this Vector3Int center, int rad)
		{
			Debug.Log($"GetAllAtRadius {center} rad = {rad}");

			var count = rad * 6;

			var result = new Vector3Int[count];

			var xShift = center.x;
			var yShift = center.y;

			result[0] = new Vector3Int(rad + xShift, yShift, 0);

			int index = default;

			for (var i = 0; i < rad; i++)
			{
				result[index + 1] = result[index].GetSouthEast();
				index++;
			}
			for (var i = 0; i < rad; i++)
			{
				result[index + 1] = result[index].GetSouth();
				index++;
			}
			for (var i = 0; i < rad; i++)
			{
				result[index + 1] = result[index].GetSouthWest();
				index++;
			}
			for (var i = 0; i < rad; i++)
			{
				result[index + 1] = result[index].GetNorthWest();
				index++;
			}
			for (var i = 0; i < rad; i++)
			{
				result[index + 1] = result[index].GetNorth();
				index++;
			}
			for (var i = 0; i < rad - 1; i++)
			{
				result[index + 1] = result[index].GetNorthEast();
				index++;
			}

			return result;
		}

		/// <summary>
		///  Получить координату на заданном радиусе от точки со смещением индекса по часовой стрелке
		/// </summary>
		/// <param name="self"></param>
		/// <param name="radius"></param>
		/// <param name="index"></param>
		/// <remarks>
		///  Движение идет по правильному шестиугольнику с плоским верхом. Север - 3 часа <br/>
		/// 
		/// </remarks>
		/// <returns></returns>
		public static Vector3Int GetCoordAtRadiusByIndex(this Vector3Int self, int radius, int index)
		{
			var countToltal = radius * 6;
			var sides = countToltal / radius;
#if UNITY_EDITOR
			if (index > countToltal) throw new ArgumentException("index > countToltal");
			if (radius < 1) throw new ArgumentException("radius < 1");
#endif
			if (radius == 1) return self.GetCoordByDirection(index);

			if (index == 0) return self + new Vector3Int(radius, 0, 0);

			var result = self + new Vector3Int(radius, 0, 0);

			var indexRem = index - 1;
			var isEven = self.y.IsEven();
			// side 1 right to bot
			for (var i = 0; i < radius; i++)
			{
				if (indexRem < 0) return result;
				if (isEven) result.x -= 1;
				result.y--;
				indexRem--;
				isEven = !isEven;
			}

			isEven = result.y.IsEven();
			// side 2 bot
			for (var i = 0; i < radius; i++)
			{
				if (indexRem < 0) return result;
				result.x--;
				indexRem--;
				isEven = !isEven;
			}

			isEven = result.y.IsEven();
			// side 3 bot to left
			for (var i = 0; i < radius; i++)
			{
				if (indexRem < 0) return result;
				if (isEven) result.x -= 1;
				result.y++;
				indexRem--;
				isEven = !isEven;
			}

			isEven = result.y.IsEven();
			// side 4 left to top
			for (var i = 0; i < radius; i++)
			{
				if (indexRem < 0) return result;
				if (!isEven) result.x += 1;
				result.y++;
				indexRem--;
				isEven = !isEven;
			}

			// side 5 top
			for (var i = 0; i < radius; i++)
			{
				if (indexRem < 0) return result;
				result.x += 1;
				indexRem--;
			}


			isEven = result.y.IsEven();
			// side 6 top to right
			for (var i = 0; i < radius; i++)
			{
				if (indexRem < 0) return result;
				if (!isEven) result.x += 1;
				result.y--;
				indexRem--;
				isEven = !isEven;
			}

			throw new Exception($" не должен доходить");
		}
		/// <summary>
		/// Получить кольцевой индекс на указанном радиусе для указанной ячейки
		/// </summary>
		/// <param name="self"></param>
		/// <param name="rad"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static int GetIndexAtRadius(this Vector3Int self, int rad, ref Vector3Int target)
		{
			var count = rad * 6;

			for (var i = 0; i < count; i++)
			{
				var compare = self.GetCoordAtRadiusByIndex(rad, i);

				if (compare == target) return i;
			}
			return -1;
		}

		#endregion

		#region Area
		/// <summary>
		/// NONOPT: Optimize
		/// </summary>
		/// <param name="self"></param>
		/// <param name="count"></param>
		/// <param name="dir"></param>
		/// <returns></returns>
		public static void GetAreaAround(this Vector3Int center, Vector3Int[] toFill, int radius, int indexFrom = default)
		{
			var countTotal = radius.DecrementSumOfMul(6);

			if (countTotal > toFill.Length - indexFrom)
				throw new ArgumentException($"Size is not fitting. Recieve {toFill.Length}. Must be >{countTotal + indexFrom}. From Index {indexFrom}");

			int takenCount = default;

			for (var i = 0; i < radius; i++)
			{
				var radCurrent = radius - i;

				var countIter = radCurrent * 6;

				center.GetCircleAtRadiusNonAlloc(toFill, radCurrent, indexFrom + takenCount);

				takenCount += countIter;
			}
		}

		#endregion

		#region Direction (6)

		/// <summary>
		/// <see cref="ETileDirectionClockWise"/>
		/// </summary>
		/// <param name="self"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static Vector3Int GetCoordByDirection(this Vector3Int self, int direction)
		{
			switch (direction)
			{
				case 0: return self.GetNorth();
				case 1: return self.GetNorthEast();
				case 2: return self.GetSouthEast();
				case 3: return self.GetSouth();
				case 4: return self.GetSouthWest();
				case 5: return self.GetNorthWest();

				default:
					throw new ArgumentException($"Must be in range [0,5]. Recived {direction}");
			}
		}

		/// <summary>
		/// Север - направление в сторону ячейки по оси x: x+1 <br/>
		/// North Dir = x + 1
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to">Должен быть прилегающий к from</param>
		/// <returns></returns>
		public static int GetDirectionNeigbour(this Vector3Int from, Vector3Int to)
		{
			var dir = to - from;

			var isYEven = from.y.IsEven();

			if (dir == Vector3Int.right) return 0;
			if (dir == Vector3Int.left) return 3;

			if (isYEven)
			{
				if (dir == Vector3Int.down) return 1;
				if (dir == new Vector3Int(-1, -1, 0)) return 2;
				if (dir == new Vector3Int(-1, 1, 0)) return 4;
				if (dir == Vector3Int.up) return 5;
			}
			else
			{
				if (dir == new Vector3Int(1, -1, 0)) return 1;
				if (dir == Vector3Int.down) return 2;
				if (dir == Vector3Int.up) return 4;
				if (dir == new Vector3Int(1, 1, 0)) return 5;
			}
			//throw new ArgumentException($"Wron method execution. Wrong parameter to. Mast be side-by-side cell");
			return -1;
		}
		/// <summary>
		/// Север - направление в сторону ячейки по оси x: x+1 <br/>
		/// North Dir = x + 1
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to">Должен быть прилегающий к from</param>
		/// <returns></returns>

		/// <summary>
		/// Получить направление от точки к точке по розе ветров с 6ью направлениями (без востока и запада)<br/>
		/// В параметре передаются направления из которых делать выборку. Оставшиеся игнорируются
		/// </summary>
		/// <param name="self"></param>
		/// <param name="headTo"></param>
		/// <param name="include"></param>
		/// <returns></returns>

		/// <summary>
		/// Получить смещение по одной из 6 сторон для указанной координаты
		/// </summary>
		/// <param name="self"></param>
		/// <param name="number"></param>
		/// <returns></returns>
		public static Vector3Int GetDirectionOffsetBySideNumber(this Vector3Int self, int number)
		{
			if (self.y.IsEven())
			{
				switch (number)
				{
					case 0: return Vector3Int.right;
					case 1: return new Vector3Int(0, -1, 0);
					case 2: return new Vector3Int(-1, -1, 0);
					case 3: return Vector3Int.left;
					case 4: return new Vector3Int(-1, 1, 0);
					case 5: return new Vector3Int(0, 1, 0);

					default:
						throw new ArgumentException($"Must be 0-5 but recived [{number}]");
				}
			}
			else
			{
				switch (number)
				{
					case 0: return Vector3Int.right;
					case 1: return new Vector3Int(1, -1, 0);
					case 2: return new Vector3Int(0, -1, 0);
					case 3: return Vector3Int.left;
					case 4: return new Vector3Int(0, 1, 0);
					case 5: return new Vector3Int(1, 1, 0);

					default:
						throw new ArgumentException($"Must be 0-5 but recived [{number}]");
				}
			}
		}
		/// <summary>
		/// Получить направление по розе ветров <br/>
		/// По двум осям можно определить только 8 состояний <br/>
		/// Результат округляется в зависимости от секторов расположения <br/>
		/// При нахождении на осях выдается точынй результат
		/// </summary>
		/// <param name="self"></param>
		/// <param name="headTo"></param>
		/// <returns></returns>
		public static Vector3Int Forward(this Vector3Int self, int count, int dir)
		{
			var result = self;

			for (var i = 0; i < count; i++)
			{
				result = result.GetCoordByDirection(dir);
			}
			return result;
		}


		/// <summary>
		/// Сосед сверху
		/// North - Flat top. South - Flat Bottom
		/// </summary>
		/// <param name="self"></param>
		/// <returns></returns>
		public static Vector3Int GetNorth(this Vector3Int self)
		{
			return self + Vector3Int.right;
		}
		/// <summary>
		/// Сосед снизу
		/// North - Flat top. South - Flat Bottom 
		/// </summary>
		/// <param name="self"></param>
		/// <returns></returns>
		public static Vector3Int GetSouth(this Vector3Int self)
		{
			return self + Vector3Int.left;
		}
		/// <summary>
		/// Сосед сверху слева
		/// North - Flat top. South - Flat Bottom 
		/// </summary>
		/// <param name="self"></param>
		/// <returns></returns>
		public static Vector3Int GetNorthWest(this Vector3Int self)
		{
			return self + (self.y.IsEven() ? Vector3Int.up : new Vector3Int(1, 1, 0));
		}
		/// <summary>
		/// Сосед сверху справа
		/// North - Flat top. South - Flat Bottom
		/// </summary>
		/// <param name="self"></param>
		/// <returns></returns>
		public static Vector3Int GetNorthEast(this Vector3Int self)
		{
			return self + (self.y.IsEven() ? Vector3Int.down : new Vector3Int(1, -1, 0));
		}
		/// <summary>
		/// Сосед снизу слева
		/// North - Flat top. South - Flat Bottom
		/// </summary>
		/// <param name="self"></param>
		/// <returns></returns>
		public static Vector3Int GetSouthWest(this Vector3Int self)
		{
			return self + (self.y.IsEven() ? new Vector3Int(-1, 1, 0) : Vector3Int.up);
		}
		/// <summary>
		/// 
		/// North - Flat top. South - Flat Bottom
		/// </summary>
		/// <param name="self"></param>
		/// <returns></returns>
		public static Vector3Int GetSouthEast(this Vector3Int self)
		{
			return self + (self.y.IsEven() ? new Vector3Int(-1, -1, 0) : Vector3Int.down);
		}

		#endregion

		#region Linear (Hex point top)
		/// <summary>
		/// Между двумя точками можно провести прямую линию которая пересечет ровно по центру по одной или более ячейке на каждой строке (строка - ось x, столбец - ось y) между двумя точками<br/>
		/// Точки не должны совпадать
		/// </summary>
		/// <param name="self"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static bool IsLinear(this Vector3Int self, ref Vector3Int target)
		{
			var diff = self - target;

			diff.Abs();

			if (diff.y < 2) return true;

			int foldTimes = (diff.x / diff.y);

			bool isVertiacal = diff.y == 0;

			if (isVertiacal) return true;

			bool isHorizontal = diff.x == 0;

			if (isHorizontal) return true;

			bool isStackedLinear = (diff.x * 2 + (self.y.IsOdd() ? 1 : 0)) == diff.y;

			if (isStackedLinear) return true;

			bool isLinear = (diff.x - (diff.y / 2)) % diff.y == 0;

			return isLinear;

#if UNITY_EDITOR
#pragma warning disable
			if (false)
			{
				// линии по ячейкам второй строки
				var line3 = new Vector3Int[] {
				new Vector3Int(0,2,0),
				new Vector3Int(1,2,0),  //1
                new Vector3Int(3,2,0),
				new Vector3Int(5,2,0),
				new Vector3Int(7,2,0),
				 };

				// +0.5x
				var line4 = new Vector3Int[] {
				new Vector3Int(0,3,0),
				new Vector3Int(1,3,0),  // 1.5
                new Vector3Int(4,3,0),
				new Vector3Int(7,3,0),
			};

				var line5 = new Vector3Int[] {
				new Vector3Int(0,4,0),
				new Vector3Int(2,4,0),  //4/2=2
                new Vector3Int(6,4,0),
				new Vector3Int(10,4,0),
			};
				// +0.5x
				var line6 = new Vector3Int[] {
				new Vector3Int(0,5,0),
				new Vector3Int(2,5,0),  //5/2=2.5
                new Vector3Int(7,5,0),
				new Vector3Int(12,5,0),
				new Vector3Int(17,5,0),
			};
				// +0.5x
				var line7 = new Vector3Int[] {
				new Vector3Int(0,6,0),
				new Vector3Int(3,6,0),
				new Vector3Int(9,6,0),
			};
			}


#pragma warning restore
#endif
		}
		/// <summary>
		/// can exceed <see cref="Vector3Int.y"/> of target point
		/// </summary>
		/// <param name="self"></param>
		/// <param name="target"></param>
		/// <param name="yLevel"></param>
		/// <param name="yFrequence">шаг встречи с гексагоном (у которого линия проходит по середине) по оси Y</param>
		/// <returns>
		/// если вертикальный столб - то возвращает на каждой строке 0ую ячейку (переделать? / модифицировать? чтобы отбирал только четные строки)
		/// в минус по иксу работает неправильно
		/// в минус по y не работает
		/// 0 0 0 - 16 8 0 работает не правильно
		/// 0 0 0 - 1 7 0 работает правильно 
		/// 0 0 0 - 10 0 0 работает не правильно  (цикл for i)
		/// </returns>
		public static bool GetLinearOffset(this Vector3Int self, ref Vector3Int target, int yLevel, int yFrequence, out Vector3Int result)
		{
#if UNITY_EDITOR
			//if (Mathf.Abs(self.y - target.y) < yLevel) throw new ArgumentException($"argument yLevel exceed range");
#endif

			var diff = self.SetRelativeToZero(target);

			bool isNegativeX = diff.x < 0;

			diff.Abs();

			/// найти смещение по оси X
			float temp = ((diff.y.IsEven() ? 0 : 0.5f) + diff.x) / diff.y * yLevel;

			//bool isGotIntermediatePoint = temp.IsWholeNumber();
			bool isGotIntermediatePoint = temp.IsWholeNumber() || temp.GetDecimical().IsEqualEpsiolon(0.5f);
			// linear formula: y = x*offset
			int x = (int)(temp);

			result = new Vector3Int(x, yLevel, 0) + self;

			return isGotIntermediatePoint;

#if UNITY_EDITOR
#pragma warning disable
			/// EVEN
			if (false)
			{
				/// линии по ячейкам 2ой строки
				//0.5X
				var line20 = new Vector3Int[] {
				new Vector3Int(0,0,0),  //0
                new Vector3Int(0,1,0),  //0.5
                new Vector3Int(1,2,0),  //1
                new Vector3Int(1,3,0),  //1.5
                new Vector3Int(2,4,0),  //2
                new Vector3Int(2,5,0),  //2.5
                 };

				// +1.5x
				var line21 = new Vector3Int[] {
				new Vector3Int(0,0,0),  //0
                new Vector3Int(1,1,0),  //1.5
                new Vector3Int(3,2,0),  //3
                new Vector3Int(4,3,0),  //4.5
                new Vector3Int(6,4,0),  //6
            };
				//+2.5
				var line22 = new Vector3Int[] {
				new Vector3Int(0,0,0),  //0
                new Vector3Int(2,1,0),  //2.5
                new Vector3Int(5,2,0),  //5
                new Vector3Int(7,3,0),  //7.5
                new Vector3Int(10,4,0), //10
            };
				// +3.5x
				var line23 = new Vector3Int[] {
				new Vector3Int(0,0,0),  //0
                new Vector3Int(3,1,0),  //3.5
                new Vector3Int(7,2,0),  //7
                new Vector3Int(10,3,0), //10.5
                new Vector3Int(14,4,0), //14
            };
			}
			/// ODD // скругление типа <see cref="Mathf.Floor(float)(float)"/>
			if (false)
			{   //0.5X
				var line3 = new Vector3Int[] {
				new Vector3Int(0,1,0),  //0.5
                new Vector3Int(1,2,0),  //1
                new Vector3Int(1,3,0),  //1.5
                new Vector3Int(2,4,0),  //2
                new Vector3Int(2,5,0),  //2.5
                new Vector3Int(3,6,0),  //3
                 };

				// +1.5x
				var line4 = new Vector3Int[] {
				new Vector3Int(0,1,0),  //0.5
                new Vector3Int(2,2,0),  //2
                new Vector3Int(3,3,0),  //3.5
                new Vector3Int(5,4,0),  //5
                new Vector3Int(6,5,0),  //6.5
            };
				//+2.5
				var line5 = new Vector3Int[] {
				new Vector3Int(0,1,0),  //0.5
                new Vector3Int(3,2,0),  //3
                new Vector3Int(5,3,0),  //5.5
                new Vector3Int(8,4,0),  //8
                new Vector3Int(10,5,0), //10.5
            };
				// +3.5x
				var line6 = new Vector3Int[] {
				new Vector3Int(0,1,0),  //0.5
                new Vector3Int(4,2,0),  //4
                new Vector3Int(7,3,0),  //7.5
                new Vector3Int(11,4,0), //11
                new Vector3Int(14,5,0), //14.5
            };

				/// линии по ячейка 3ей строки
				var line33 = new Vector3Int[]
				{
				new Vector3Int(0,0,0),
				new Vector3Int(2,2,0),
				new Vector3Int(4,4,0),
				new Vector3Int(6,6,0),

			};
				var line34 = new Vector3Int[]
				{
				new Vector3Int(0,0,0),
				new Vector3Int(1,1,0),
				new Vector3Int(4,4,0),
				new Vector3Int(6,6,0),

			};
			}
#pragma warning restore
#endif
		}
		/// <summary>
		/// Получить все ячейки, которые пересекает отрезок
		/// </summary>
		/// <param name="self"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static Vector3Int[] GetCellsIntersectWithLinear(this Vector3Int self, ref Vector3Int target)
		{
			throw new NotImplementedException();
		}
		public static bool GetCellsIntersectWithLinear(this Vector3Int self, ref Vector3Int target, Tilemap tilemap, ref List<Vector3Int> result)
		{
			Vector3 worldStart = tilemap.CellToWorld(self);
			Vector3 worldEnd = tilemap.CellToWorld(target);
			Vector3Int diff = self.SetRelativeToZero(target);

			diff.Abs();

			int count = (diff.x > diff.y ? diff.x : diff.y) * 3;
			//int count = (diff.x > diff.y ? diff.x  : diff.y) ;
			float f = count - 1;
			bool isGotOne = default;

			for (int i = 0; i < count; i++)
			{
				Vector3 lerp = Vector3.Lerp(worldStart, worldEnd, i / f);

				Vector3Int pos = tilemap.WorldToCell(lerp);

				//if (result.Any(x => x.y == pos.y) || result.Contains(pos))
				if (result.Contains(pos))
				{
					continue;
				}
				else
				{
					result.Add(pos);

					isGotOne = true;
				}
			}

			return isGotOne;
		}
		/// <summary>
		/// Поиск линии по форме двутавара (только для вертикальных или 30 градусных направлений)
		/// </summary>
		/// <param name="self"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static Vector3Int[] GetCellsIntersectWithLinearIBeam(this Vector3Int self, ref Vector3Int target)
		{
			throw new NotImplementedException();
		}

		#endregion

#if UNITY_EDITOR
#pragma warning disable
		#region Incomlete
		static Vector3Int GetCoordAtRadiusWithOffesetV2(this Vector3Int self, int radius, int offset)
		{
			var totalCount = radius * 6;

			var remainder = radius % 2;
			var countEven = radius / 2;
			var countOdd = countEven + remainder;

			int diff = default;

			int changeCount = default;

			// суммируем изменения по x для всех сторон
			var x = radius * changeCount + radius + radius + radius + radius + radius;

			var y = radius * totalCount - offset;

			// x: -radius..0..radius
			// y: -radius..0..radius

			// x: rad..0..-rad..0..rad sin(x)
			// y: 0..-rad..0..rad..0 -cos(y)

			Span<int> spanX = stackalloc int[6 * radius];
			Span<int> spanY = stackalloc int[6 * radius];

			//var x = default ;

			return default;
		}
		static Vector3Int GetCoordAtRadiusWithOffeset(this Vector3Int self, int radius, int index)
		{
			var totalCount = radius * 6;

			int tempCount = default;

			var radiusTimesFull = totalCount / radius;
			var radiusTimesLeft = totalCount % radius;
			// even odd относиттся к ячейке от которого идет переход
			// ниже список шифтов для этой ячейки
			// каждый переход это всегда изменение x и/или y
			var result = new Vector3Int(radius, 0, 0) + self;

			var countEven = radius / 2;
			var left = radius % 2;
			var countOdd = countEven + left;

			int changeX = default;
			int changeXNot = default;


			if (radiusTimesFull > 1)
			{
				if (result.y.IsEven())
				{
					changeXNot = radius / 2;
					changeX = radius % 2 + changeXNot;
				}
				else
				{
					changeX = radius / 2;
					changeXNot = radius % 2 + changeXNot;
				}
				//var N2NeOdd =
				result += new Vector3Int(0, -1, 0) * changeXNot;
				//var N2NeEven =
				result += new Vector3Int(-1, -1, 0) * changeX;
				if (radiusTimesFull > 2)
				{
					//var Ne2Se =
					result += new Vector3Int(-1, 0, 0) * radius;

					if (radiusTimesFull > 3)
					{
						//var Se2SEven =
						result += new Vector3Int(-1, 1, 0) * changeX;
						//var Se2SOdd =
						result += new Vector3Int(0, 1, 0) * changeXNot;
						if (radiusTimesFull > 4)
						{
							//var S2SwOdd =
							result += new Vector3Int(1, 1, 0) * changeX;
							//var S2SwEven =
							result += new Vector3Int(0, 1, 0) * changeXNot;

							if (radiusTimesFull > 5)
							{
								//var Sw2Nw =
								result += new Vector3Int(1, 0, 0) * radius;

								//var Nw2NOdd =
								result += new Vector3Int(1, -1, 0) * changeX;
								//var Nw2NEven =
								result += new Vector3Int(0, -1, 0) * changeXNot;
							}
						}
					}
				}
			}

			return result;

			if (false)
			{
				var N2NeOdd = new Vector3Int(0, -1, 0) * tempCount;
				var N2NeEven = new Vector3Int(-1, -1, 0) * tempCount;

				var Ne2Se = new Vector3Int(-1, 0, 0) * radius;

				var Se2SEven = new Vector3Int(-1, 1, 0) * tempCount;
				var Se2SOdd = new Vector3Int(0, 1, 0) * tempCount;

				var S2SwOdd = new Vector3Int(1, 1, 0) * tempCount;
				var S2SwEven = new Vector3Int(0, 1, 0) * tempCount;

				var Sw2Nw = new Vector3Int(1, 0, 0) * radius;

				var Nw2NOdd = new Vector3Int(1, -1, 0) * tempCount;
				var Nw2NEven = new Vector3Int(0, -1, 0) * tempCount;

				return
			N2NeOdd +
			N2NeEven +
			Ne2Se +
			Se2SEven +
			Se2SOdd +
			S2SwOdd +
			S2SwEven +
			Sw2Nw +
			Nw2NOdd +
			Nw2NEven;
			}
		}
		static (Vector3Int, int) GetCoordAtRadiusWithOffesetV4(this Vector3Int self, int radius, int offset)
		{
			if (radius == 1)
			{
				//return self.GetCoordByDirection(offset);
				return default;
			}

			Vector2 projection = default;

			var totalCount = radius * 6;

			var lerp = offset / (float)totalCount;

			var sin = Mathf.Sin(Mathf.PI * 2 * lerp);
			var cos = Mathf.Cos(Mathf.PI * 2 * lerp);

			var x = cos * radius;
			var y = sin * radius;


			var half = totalCount / 2;

			var countQuad = totalCount / 4;

			var countAxis = radius * 2 + 1;

			// сопоставить countAxis с half: короткую с длинной
			//int x = offset;

			// x: -radius..0..radius
			// y: -radius..0..radius

			// x: rad..0..-rad..0..rad sin(x)
			// y: 0..-rad..0..rad..0 -cos(y)


			var v1 = offset / (float)totalCount;
			var sides = offset / radius; // (0-5)
			var rem = offset % radius;

			return default;
		}
		#endregion
#pragma warning restore
#endif
	}
}