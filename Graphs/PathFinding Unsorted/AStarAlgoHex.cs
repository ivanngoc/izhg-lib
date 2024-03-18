using IziHardGames.Extensions.PremetiveTypes;
using IziHardGames.Tile.Hex.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IziHardGames.PathFinding
{
	/// <summary>
	/// Single Core A* Path finding
	/// </summary>
	[Serializable]
	public class AStarAlgoHex
	{
#if UNITY_EDITOR
		Dictionary<Vector3Int, GameObject> keyValuePairs = new Dictionary<Vector3Int, GameObject>();
#endif
		public bool isInteger;

		[SerializeField] public NodeHex[] nodes;

		[SerializeField] public Vector3Int start;
		[SerializeField] public Vector3Int end;
		[SerializeField] public Vector3Int processingCoord;
		[SerializeField] public Vector2Int sizeMap;
		[SerializeField] public int processingId;
		public Tilemap tilemap;

#if UNITY_EDITOR
		public GameObject prefab;
		public bool isDebug = false;
#endif
		[SerializeField] List<int> openList = new List<int>();
		[SerializeField] List<int> openListTier2 = new List<int>();

		[SerializeField] List<Vector3Int> result = new List<Vector3Int>();

		public void Initilize(Vector2Int sizeIn, Tilemap tilemapIn)
		{
#if UNITY_EDITOR

#endif
			NodeHex.defaultSizeMap = sizeIn;

			tilemap = tilemapIn;

			sizeMap = sizeIn;

			int count = sizeIn.x * sizeIn.y;

			nodes = new NodeHex[count];

			Clear();

			for (int i = 0; i < count; i++)
			{
				Vector3Int vector3Int = NodeHex.GetCoordFromId(i);

				//Debug.Log($"{i}{Environment.NewLine}{vector3Int}");

				nodes[i] = new NodeHex()
				{
					id = i,
					coord = vector3Int,
					isAllowed = true,
					costG = float.MaxValue,
					costF = float.MaxValue,
					previousNodeId = int.MinValue,
				};
				nodes[i].FillNeighbourIds(vector3Int);
			}
		}
		/// <summary>
		/// Межитерационная очистка
		/// </summary>
		public void Clear()
		{
			openList.Clear();
			openListTier2.Clear();
		}
		/// <summary>
		/// Очитстка данных для последующего формирования маршрута
		/// </summary>
		public void ReSet()
		{
			Clear();

			result = new List<Vector3Int>();

			start = default;
			end = default;

			processingCoord = default;
			processingId = default;

			for (int i = 0; i < nodes.Length; i++)
			{
				nodes[i].costG = float.MaxValue;
				nodes[i].costH = default;
				nodes[i].costF = float.MaxValue;

				nodes[i].previousNodeId = int.MinValue;
				nodes[i].isAllowed = true;
			}
		}
		public Vector3Int[] FindPathPerStep(Vector3Int startIn, Vector3Int endIn, List<Vector3Int> obstacles)
		{
			if (ShortPathCheck(startIn, endIn))
			{
				throw new NotImplementedException();
			}

			if (Start(ref startIn, ref endIn))
			{
				SetObstacles(obstacles);

				Next();
			}

			return default;
		}

		public Vector3Int[] FindPathInstant(Vector3Int startIn, Vector3Int endIn)
		{
			if (ShortPathCheck(startIn, endIn))
			{
				throw new NotImplementedException();
			}

			if (Start(ref startIn, ref endIn))
			{
				while (Next())
				{

				}

				return result.ToArray();
			}
			throw new NullReferenceException("Rout is empty");
		}

		public List<Vector3Int> FindPathWithObstacles(Vector3Int startIn, Vector3Int endIn, IEnumerable<Vector3Int> ignore)
		{
			if (ShortPathCheck(startIn, endIn, ignore))
			{
				return new List<Vector3Int>() { startIn, endIn };
			}

			if (Start(ref startIn, ref endIn))
			{
				foreach (var obstacle in ignore)
				{
					nodes[NodeHex.GetIdFromCoordXYZ(obstacle)].isAllowed = false;
				}

				int count = sizeMap.x * sizeMap.y - ignore.Count();

				while (Next())
				{
					count--;

					if (count < 0)
					{
						Debug.LogError($"Количество иттераций превыисло количество узлов. Не смог найти путь{Environment.NewLine}" +
							$"{startIn} => {endIn}{Environment.NewLine}" +
							$"{ignore.Select(x => x.ToString()).Aggregate((z, y) => z + y + "|")}{Environment.NewLine}" +
							$"");

						//EditorApplication.isPaused = true;

						Debug.Break();

						Debug.LogError($"skipped");

						return null;
					}
				}
				return result;
			}

			throw new NullReferenceException("Rout is empty");
		}
		/// <summary>
		/// Проверка на координаты в пределах поля <br/>
		/// Настройка стартового узла
		/// </summary>
		/// <param name="startIn"></param>
		/// <param name="endIn"></param>
		/// <returns></returns>
		private bool Start(ref Vector3Int startIn, ref Vector3Int endIn)
		{
			start = startIn;
			end = endIn;

			bool isSuccedStat = startIn.IsInRect(sizeMap) && endIn.IsInRect(sizeMap);

			if (isSuccedStat)
			{
				int id = NodeHex.GetIdFromCoordXYZ(startIn);

				processingCoord = start;
				processingId = id;

				nodes[id].costG = default;
				nodes[id].costH = default;
				//nodes[id].costH = (startIn - endIn).magnitude;
				//nodes[id].costF = nodes[id].costG + nodes[id].costH;
				nodes[id].costF = default;
				nodes[id].isAllowed = false;
				nodes[id].previousNodeId = id;
			}
			else
			{
				Debug.LogError($"Out of battleground. {startIn}|{endIn}|{sizeMap}");
			}

			return isSuccedStat;
		}
		/// <summary>
		/// Проверка на окончание (успешное только)<br/>
		/// Вызыв посешения следующего узла <br/>
		/// Вызов формирования результата при достижении цели (успешно тольео)<br/>
		/// </summary>
		/// <returns></returns>
		private bool Next()
		{
			if (processingCoord != end)
			{
				NodeExplore(processingId);
			}

			if (processingCoord == end)
			{
				FormResult();
			}

			return processingCoord != end;
		}
		/// <summary>
		/// Note: 20201117
		/// </summary>
		private void SetNext()
		{
#if UNITY_EDITOR
			if (isDebug) Recolor(Color.yellow, processingCoord);
#endif
			openListTier2.Clear();

			float min1 = openList.Select(x => nodes[x].costF).Min();

			openListTier2.AddRange(openList.Where(x => nodes[x].costF.IsEqualMili(min1)));


			if (openListTier2.Count == 0)
			{
				Debug.LogError("Нет пути");

				return;
			}

			if (openListTier2.Count > 1)
			{
				//always clockwise
				float min2 = openListTier2.Select(x => nodes[x].costF).Min();

				var opentListTier3 = openListTier2.Where(x => nodes[x].costF == min2).ToList();

				int centerId = nodes[opentListTier3.First()].previousNodeId;

				var pack = nodes[centerId].coord.GetNeigbours();
				// если логика целых чисел
				if (isInteger)
				{   // выбираем узлы от которых пришли к узлам в откротом списке
					var roots = openListTier2.Select(x => nodes[x].previousNodeId).Distinct().ToArray();
					// стартовое значение id ячейки которая будет выбрана в качестве центра для выборки следующего узла
					int root = int.MinValue;
					// если в списке 2 тира больше двух узлов от которых эти элементы списка пришли то делаем выборку в пользу предыдущего узла
					if (roots.Count() > 1)
					{   // это исключительный случай возможен только для симметричных путей (их участков)
						//if (roots.Count() > 2) throw new NotImplementedException("В списке второго тира не может быть больше 3 узлов. Непредвиденная ситуация");
						// выбираем тот узел в качестве центра из которого перешли в прошлый раз
						//if (false) root = nodes[openListTier2.First(x => nodes[x].previousNodeId == processingId)].previousNodeId;

						root = nodes[openListTier2.First()].previousNodeId;

						//Debug.LogError($"Multiple roots. [{root}] were selected");
					}
					else
					{
						root = nodes[openListTier2.First()].previousNodeId;
					}
					// выбираем узлы вокруг цетра (НЕ ПУСТЫЕ) и выбираем их направления от центра к ним
					// т.е. выбираем не пустующие направления от центра (root)
					var positionsWindRose = openListTier2
						.Where(z => nodes[z].previousNodeId == root)
						.Select(x => TileHexPack.WindRose6To8(nodes[x].hexPosForPrevious))
						.ToArray();
					// из непустующих напрвлений выбираем то которое по направлению взгляда в конечную точку первую сторону по часовой стрелке
					int dir = nodes[root].coord.GetDirectionClockWiseRoseWind6(ref end, positionsWindRose);
					// извлекаем коорлдинату
					processingCoord = nodes[nodes[root][dir]].coord;

					processingId = NodeHex.GetIdFromCoordXYZ(processingCoord);
				}
				else
				{
					Span<int> span = stackalloc int[2];

					int spanIndex = default;

					for (int i = 0; i < 6; i++)
					{
						int n = NodeHex.GetIdFromCoordXYZ(pack[i]);

						if (opentListTier3.Contains(n))
						{
							span[spanIndex] = i;

							spanIndex++;
						}
					}
					// span=2
					// исключительный случай когда иттерация начинается с северной ячейки а выбор идет между северной и северо западной ячейкой 
					// которая по часовой идет первой
					if (span[0] == 0 && span[1] == 5)
					{
						processingId = NodeHex.GetIdFromCoordXYZ(pack[5]);
					}
					else
					{   // в остальных случаях берем кого настигла стрелка часов первой при вращении
						processingId = NodeHex.GetIdFromCoordXYZ(pack[span[0]]);
					}
					processingCoord = nodes[processingId].coord;
				}
			}
			else
			{
				processingId = openListTier2.First();
				processingCoord = nodes[processingId].coord;
			}
#if UNITY_EDITOR
			if (isDebug) Recolor(Color.red, nodes[processingId].coord);
#endif
		}

		private void NodeExplore(int nodeId)
		{
			//Debug.LogError($"Explorer  node {processingCoord}");

			if (nodeId == processingId)
			{
				//Debug.LogError($"Зациклился");
			}

			openList.Remove(nodeId);

			nodes[nodeId].isAllowed = false;

			for (int i = 0; i < 6; i++)
			{
				int n = nodes[nodeId][i];

				if (n > -1)
				{
					/// по идексатору <see cref="NodeHex"/>
					NodeCompare(ref nodes[nodeId], ref nodes[n], i);
				}
			}

			SetNext();
		}
		private void NodeCompare(ref NodeHex origin, ref NodeHex target, int i)
		{
			if (!target.isAllowed || origin.previousNodeId == target.id) return;

			if (!openList.Contains(target.id))
			{
				openList.Add(target.id);
			}
			/// var1
			//float newG = (start - target.coord).magnitude;
			//float newH = (end - target.coord).magnitude;
			//float newF = newG + newH;
			/// var2
			float newG = default;
			//float newG = (tilemap.CellToWorld(start) - tilemap.CellToWorld(target.coord)).magnitude.SetPrecision(2);
			float newH = default;

			if (isInteger)
			{
				newH = target.coord.GetRadius(end);
				newG = origin.costG + 1;
			}
			else
			{
				newH = (tilemap.CellToWorld(end) - tilemap.CellToWorld(target.coord)).magnitude.SetPrecision(2);
			}
			//float newH = target.coord.GetRadius(end);
			/// var3
			//float newG = (tilemap.CellToWorld(start - target.coord)).magnitude;
			//float newH = (tilemap.CellToWorld(end - target.coord)).magnitude;

			float newF = newG + newH;

			if (newF < target.costF)
			{
				target.hexPosForPrevious = i;
#if UNITY_EDITOR
				if (isDebug)
				{
					Recolor(Color.green, target.coord);
				}
#endif
				target.costG = newG;
				target.costH = newH;
				target.costF = newF;
				target.previousNodeId = origin.id;
			}
#if UNITY_EDITOR
			if (isDebug)
			{
				if (!keyValuePairs.TryGetValue(target.coord, out GameObject gameObject))
				{
					gameObject = GameObject.Instantiate(prefab, Component.FindObjectsOfType<Canvas>().First(x => x.renderMode == RenderMode.WorldSpace).transform);

					keyValuePairs.Add(target.coord, gameObject);

					gameObject.transform.position = tilemap.CellToWorld(target.coord);
				}

				// gameObject.GetComponent<TestPathFindingAStar>().SetTitles(ref target, start);

				gameObject.name = $"G={target.costG}|H={target.costH}|F={target.costF}";

				gameObject.SetActive(true);
			}
#endif
		}

		private void SetObstacles(List<Vector3Int> obstacles)
		{
			foreach (var coord in obstacles)
			{
				nodes[NodeHex.GetIdFromCoordXYZ(coord)].isAllowed = false;
			}
		}
#if UNITY_EDITOR
		private void Recolor(Color color, Vector3Int v)
		{
			tilemap.SetTileFlags(v, TileFlags.None);

			tilemap.SetColor(v, color);
		}
#endif
		/// <summary>
		/// Проверка на короткий путь из соседних клеток
		/// </summary>
		/// <returns></returns>
		private bool ShortPathCheck(Vector3Int startIn, Vector3Int endIn, IEnumerable<Vector3Int> ignore = default)
		{
			if ((ignore?.Contains(startIn) ?? false) || (ignore?.Contains(endIn) ?? false))
			{
				throw new Exception($"Одна из координат входи в список исключения поиска {Environment.NewLine}" +
						  $"{startIn} => {endIn}{Environment.NewLine}" +
							$"{ignore.Select(x => x.ToString()).Aggregate((z, y) => z + y + "|")}");
			}

			if (startIn.IsHexIntersect(endIn))
			{
				return true;
			}

			return default;
		}
		private void FormResult()
		{
			int safeCount = default;

			NodeHex node = nodes[processingId];

			while (node.previousNodeId != node.id)
			{
				safeCount++;

				if (safeCount > 10000)
				{
					throw new Exception($"Loop over 10000");
				}

				result.Add(node.coord);

				node = nodes[node.previousNodeId];
			}

			result.Add(start);

			result.Reverse();

#if UNITY_EDITOR
			if (isDebug) result.ForEach(x => Recolor(Color.magenta, x));
#endif
		}

		/// <summary>
		/// Вершина графа представленного в виде вымащенной шестигранниками плоскостью.
		/// Каждая вершина всегда имеет 6 связяей. по умолчанию тип хекса - flat top. Top=North. То есть прямые связи только по вертикали. 
		/// А по горизонтали по 2 диагональные связи для каждой стороы.
		/// </summary>
		[Serializable]
		public struct NodeHex
		{
			public static Vector2Int defaultSizeMap;

			public int id;
			/// <summary>
			/// От старта до текущего узла
			/// </summary>
			public float costG;
			/// <summary>
			/// От конца до текущего узла
			/// </summary>
			public float costH;
			/// <summary>
			/// costG+costH
			/// </summary>
			public float costF;

			public int previousNodeId;
			public int hexPosForPrevious;

			public bool isAllowed;

			public Vector3Int coord;

			public int center;
			public int n;
			public int ne;
			public int se;
			public int s;
			public int sw;
			public int nw;
			/// <summary>
			/// ClockWise по розе ветров
			/// </summary>
			/// <param name="i"></param>
			/// <returns></returns>
			public int this[int i]
			{
				get
				{
					switch (i)
					{
						case 0: return n;
						case 1: return ne;
						case 2: return se;
						case 3: return s;
						case 4: return sw;
						case 5: return nw;
						default:
							throw new Exception($"For this type max index is 5!!!");
					}
				}
				set
				{
					switch (i)
					{
						case 0: n = value; break;
						case 1: ne = value; break;
						case 2: se = value; break;
						case 3: s = value; break;
						case 4: sw = value; break;
						case 5: nw = value; break;
						default:
							throw new Exception($"For this type max index is 5!!!");
					}
				}
			}

			/// <summary>
			/// Получить идентификатор из координаты
			/// </summary>
			/// <param name="v"></param>
			/// <returns></returns>
			public static int GetIdFromCoordXYZ(Vector3Int v)
			{
				//int id = x * sizeIn.x + y;

				return v.y * defaultSizeMap.x + v.x;
			}

			public static Vector3Int GetCoordFromId(int id)
			{
				int x = id % defaultSizeMap.x;
				int y = id / defaultSizeMap.x;

				return new Vector3Int(x, y, 0);
			}

			public void FillNeighbourIds(Vector3Int vector3Int)
			{
				var v1 = vector3Int.GetNorth();
				var v2 = vector3Int.GetSouth();
				var v3 = vector3Int.GetNorthWest();
				var v4 = vector3Int.GetNorthEast();
				var v5 = vector3Int.GetSouthWest();
				var v6 = vector3Int.GetSouthEast();

				n = v1.IsInRect(defaultSizeMap) ? NodeHex.GetIdFromCoordXYZ(vector3Int.GetNorth()) : -1;
				s = v2.IsInRect(defaultSizeMap) ? NodeHex.GetIdFromCoordXYZ(vector3Int.GetSouth()) : -1;
				nw = v3.IsInRect(defaultSizeMap) ? NodeHex.GetIdFromCoordXYZ(vector3Int.GetNorthWest()) : -1;
				ne = v4.IsInRect(defaultSizeMap) ? NodeHex.GetIdFromCoordXYZ(vector3Int.GetNorthEast()) : -1;
				sw = v5.IsInRect(defaultSizeMap) ? NodeHex.GetIdFromCoordXYZ(vector3Int.GetSouthWest()) : -1;
				se = v6.IsInRect(defaultSizeMap) ? NodeHex.GetIdFromCoordXYZ(vector3Int.GetSouthEast()) : -1;
			}
		}

		/// <summary>
		/// To Convert into Job
		/// </summary>
		public struct NodeNeighbour
		{
			public int id;
			public int center;
			public int n;
			public int s;
			public int nw;
			public int ne;
			public int sw;
			public int se;

			public int this[int i]
			{
				get
				{
					switch (i)
					{
						case 0: return n;
						case 1: return s;
						case 2: return nw;
						case 3: return ne;
						case 4: return sw;
						case 5: return se;
						default:
							throw new Exception($"For this type max index is 5!!!");
					}
				}
				set
				{
					switch (i)
					{
						case 0: n = value; break;
						case 1: s = value; break;
						case 2: nw = value; break;
						case 3: ne = value; break;
						case 4: sw = value; break;
						case 5: se = value; break;
						default:
							throw new Exception($"For this type max index is 5!!!");
					}
				}
			}
		}
	}
}