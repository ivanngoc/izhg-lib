using IziHardGames.Core;
using IziHardGames.Libs.Engine.Memory;
using System;
using System.Buffers;
using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.Libs.Engine.PositioningSystem
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TPos"> тип координаты</typeparam>
	/// <typeparam name="TIndexes"> контейнер для индексов (например массив, список и т.п.)</typeparam>
	/// <typeparam name="TValue"> тип значения X,Y,Z </typeparam>
	public class SystemMapping<TPos, TIndexes, TValue> where TIndexes : IList<int>
	{
		#region Unity Message	

		#endregion

		/// <summary>
		/// Хранит данные о занятых ячейках карты 
		/// </summary>
		/// <typeparam name="TPos">
		/// тип координаты
		/// </typeparam>
		/// <remarks>
		/// 1 объект может занимать несколько ячеек
		/// </remarks>
		[Serializable]
		public class ManagerMapping : IInitializable, IDeinitializable
		{
			[SerializeField] public WrapMappingFuncs wrapMappingFuncs;
			/// <summary>
			/// идентификатор экземляра класса
			/// </summary>
			public int idManagerMapping;

			public TPos positionMin;
			public TPos positionMax;
			/// <summary>
			/// minimal size
			/// </summary>
			public TPos size;

			/// <summary>
			/// Значение для прибаки к координате для того чтобы получить координату при которой <see cref="positionMin"/> будет равен 0,0,0
			/// </summary>
			public TPos offsetToZero;

			/// <summary>
			/// Размер наименьшего квадрата карты. Рекомендуется использовать значение в 2 раза меньшее от размера наименьшего объекта (0.5, 0.5, 0.5) 
			/// </summary>
			public TPos minimumMapSize;
			/// <summary>
			/// Максимальный уровень укрупнения элементов начиная от 0 включительно
			/// </summary>
			public int maxRank;
			/// <summary>
			/// Количество уровней укрупнения
			/// </summary>
			public int rankCount;

			public WrapPerRank[] wrapPerRanks;
			public List<ComponentMapablePosition> objects = new List<ComponentMapablePosition>();

			private Func<TPos, TPos, int, int> funcPosToIndexImported;

			public void Initilize()
			{
				funcPosToIndexImported = wrapMappingFuncs.funcPosToIndex;
			}
			public void InitilizeReverse()
			{
				funcPosToIndexImported = default;
			}

			public void CreateWrapsForRanks(int rankCountArg, int mapCellCount)
			{
				rankCount = rankCountArg;

				maxRank = rankCountArg - 1;

				wrapPerRanks = new WrapPerRank[rankCountArg];

				for (int i = 0; i < rankCountArg; i++)
				{
					int mapCellCountForRank = mapCellCount >> (i + 1);

					wrapPerRanks[i] = new WrapPerRank()
					{
						rank = i,
						flags = new bool[mapCellCountForRank],
						mappingCells = new MappingCell<TPos>[mapCellCountForRank],
						objectsInMappingCells = new List<ComponentMapablePosition>[mapCellCountForRank],
					};
				}
			}

			public void InitilizeMap(TPos minArg, TPos maxArg, TPos sizeArg, TPos offsetToZeroArg)
			{
				positionMin = minArg;
				positionMax = maxArg;
				size = sizeArg;
				offsetToZero = offsetToZeroArg;
			}

			public ComponentMapablePosition Add<T>(T mapablePos) where T : IMappable
			{
				ComponentMapablePosition result = PoolObjects<ComponentMapablePosition>.Shared.Rent();

				mapablePos.ComponentMapablePosition = result;

				result.isCalculated = true;

				result.indexInManagerMapping = objects.Count;

				objects.Add(result);

				result.mappablePos = mapablePos;

				return result;
			}

			public ComponentMapablePosition Remove<T>(T mapablePos) where T : IMappable
			{
				ComponentMapablePosition result = mapablePos.ComponentMapablePosition;

				PoolObjects<ComponentMapablePosition>.Shared.Return(mapablePos.ComponentMapablePosition);

				mapablePos.ComponentMapablePosition = default;

				result.isCalculated = default;

				result.indexInManagerMapping = default;

				objects.Remove(result);

				result.mappablePos = default;

				return null;
			}
			/// <summary>
			/// Пропускает объект при <see cref="Remap"/>
			/// </summary>
			/// <param name="transform"></param>
			public void RemapPause(ComponentMapablePosition componentMapablePosition)
			{
				throw new NotImplementedException();
			}
			/// <summary>
			/// Вновь расчитывает объект который был на паузе в <see cref="RemapPause(Transform)"/> в <see cref="Remap"/>
			/// </summary>
			/// <param name="transform"></param>
			public void RemapResume(ComponentMapablePosition componentMapablePosition)
			{
				throw new NotImplementedException();
			}
			/// <summary>
			/// Комплексный Расчет положений объектов. Перемещение между чанками
			/// </summary>
			public void Remap()
			{
				CleanPositions();

				foreach (var item in objects)
				{
					Remap(item);
				}
			}
			/// <summary>
			/// Расчитать положение объектов не полностью, а для графа объектов с указанным началом 
			/// </summary>
			public void RemapGraph(TPos startPos)
			{
				throw new NotImplementedException();
			}
			public void Remap(ComponentMapablePosition componentMapablePosition)
			{
				TPos center = componentMapablePosition.mappablePos.MapPos;

				for (int i = 0; i < rankCount; i++)
				{
					if (componentMapablePosition.mappablePos.FuncGetIndexes(center, i, out int count, out TIndexes indexes))
					{
						WrapPerRank wrapPerRank = wrapPerRanks[i];

						for (int j = 0; j < count; j++)
						{
							List<ComponentMapablePosition> list = wrapPerRank.objectsInMappingCells[indexes[j]];

							if (list == null)
							{
								list = wrapPerRank.objectsInMappingCells[indexes[j]] = new List<ComponentMapablePosition>();
							}

							list.Add(componentMapablePosition);

							wrapPerRank.flags[j] = true;

							componentMapablePosition.indexes.Add(indexes[j]);
						}

						componentMapablePosition.mappablePos.FreeIndexesContainer(indexes);
					}
				}
			}



			#region Seek




			#endregion


			#region Sortings

			private void SortMappedObjects(Memory<ComponentMapablePosition> memory, TPos position)
			{
				Span<ComponentMapablePosition> span = memory.Span;

				for (int i = 0; i < memory.Length; i++)
				{
					span[i].sqrMagnitudeTemp = wrapMappingFuncs.funcGetSqrMagnitude(position, span[i].mappablePos.MapPos);
				}
				span.SortSelectionAscending(x => x.sqrMagnitudeTemp);
			}

			#endregion

			private void CleanPositions()
			{
				foreach (var item in objects)
				{
					item.CleanForRecalculate();
				}

				for (int i = 0; i < wrapPerRanks.Length; i++)
				{
					var temp = wrapPerRanks[i].objectsInMappingCells;

					foreach (var item in temp)
					{
						item?.Clear();
					}
					var flags = wrapPerRanks[i].flags;

					for (int j = 0; j < flags.Length; j++)
					{
						flags[i] = default;
					}
				}
			}
		}
		public class WrapPerRank
		{
			public int rank;
			/// <summary>
			/// Флаги заполнености для <see cref="mappingCells"/> соотвествено<br/>
			/// <see langword="true"/> в ячейке есть хотя бы 1 объект
			/// <see langword="false"/> в ячейке нет ни 1 объекта
			/// </summary>
			public bool[] flags;
			/// <summary>
			/// Массив объектов в каждом квадрате. Индекс массива соответствует индексу квадрата
			/// </summary>
			public List<ComponentMapablePosition>[] objectsInMappingCells;
			/// <summary>
			/// Квадраты карты
			/// </summary>
			public MappingCell<TPos>[] mappingCells;
			/// <summary>
			/// квадрат учтен после обработки в <see cref="TryGetObjectsFromPointClosest"/>. Позднее из помеченного квадрата будут взяты объекты этого квадрата для различных операций
			/// <see langword="true"/> - помечен для выборки<br/>
			/// <see langword="false"/> - не помечен для выьорки <br/>
			/// </summary>
			public bool[] flagsIncludeToSelection;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// 2 способа поиска. 
		/// Первый - от большего к меньшему
		/// Второй - от меньшего к большему
		/// </remarks>
		private class JobSearchClosest
		{
			private int maxRank;
			private int countRequired;
			private TPos position;
			private TPos shiftedPos;
			private TPos offsetToZero;
			private TPos mapSizeOrigin;


			private bool[] flags;
			private ComponentMapablePosition[] targets;
			/// <summary>
			/// включение / исключение для различных целей
			/// </summary>
			private WrapPerRank[] wrapPerRanks;

			[Header("Init Once")]
			private WrapMappingFuncs wrapMappingFuncs;

			public void Initilize(ManagerMapping managerMapping)
			{
				wrapMappingFuncs = managerMapping.wrapMappingFuncs;
			}
			/// <summary>
			/// <inheritdoc cref="Execute"/>
			/// </summary>
			/// <returns>
			/// <see langword="true"/> - было найдено достаточное количество объектов<br/>
			/// <see langword="false"/> - не было найдено достаточное количество объектов<br/>
			/// </returns>
			public bool TryExecute(WrapPerRank[] wrapPerRanksArg,
								   IList<ComponentMapablePosition> componentMapablePositions,
								   TPos positionArg,
								   TPos shiftedPosArg,
								   TPos offsetToZeroArg,
								   int countArg,
								   int maxRankArg,
								   out Memory<ComponentMapablePosition> memory)
			{
				memory = Execute(wrapPerRanksArg, componentMapablePositions, positionArg, shiftedPosArg, offsetToZeroArg, countArg, maxRankArg);

				return memory.Length == countArg;
			}
			/// <summary>
			/// Получить объекты от указанного квадрата. количество объектов не может быть ниже указанного.
			/// Поиск происходит от большего к меньшему. Сначала проверяется вся карта целиком, затем масштаб постоянно уменьшается, 
			/// до тех пор пока не будет найденно достаточное количество объектов при минимально возможном масштабе
			/// </summary>
			/// <remarks>
			/// Недостаток: трудно объединить соседние квадраты того же ранга. для поиска в ширину (индуктивно из центра) лучше использовать другой или комбинированный с этим подход
			/// для более точных расчетов нужно также каждый раз брать 6*N квадратов на внешнем контуре, где N - расстояние до внешнего контура
			/// </remarks>

			public Memory<ComponentMapablePosition> Execute(WrapPerRank[] wrapPerRanksArg,
															IList<ComponentMapablePosition> componentMapablePositions,
															TPos positionArg,
															TPos shiftedPosArg,
															TPos offsetToZeroArg,
															int countArg,
															int maxRankArg)
			{
				wrapPerRanks = wrapPerRanksArg;
				maxRank = maxRankArg;
				position = positionArg;
				shiftedPos = shiftedPosArg;
				offsetToZero = offsetToZeroArg;

				countRequired = countArg;

				targets = ArrayPool<ComponentMapablePosition>.Shared.Rent(componentMapablePositions.Count);
				flags = ArrayPool<bool>.Shared.Rent(componentMapablePositions.Count);

				Filtering();

				CompressFiltered();

				throw new NotImplementedException();
			}
			/// <summary>
			/// Сужение области поиска
			/// </summary>
			private void ShrinkSearchingArea()
			{
				throw new NotImplementedException();
			}

			private void SelectClosest()
			{
				int rankCount = maxRank + 1;

				for (int i = 0; i < rankCount; i++)
				{
					int rank = maxRank - i;

					WrapPerRank wrapPerRank = wrapPerRanks[rank];

					int countProceeded = default;

					int objSum = default;

					while (countProceeded < wrapPerRank.mappingCells.Length)
					{
						countProceeded += SelectObjectsAtRadius(rank, rank);

						if (objSum >= countRequired) break;
					}
					throw new NotImplementedException();
				}
			}

			private int SelectObjectsAtRadius(int rank, int radius)
			{
				int countFiltered = default;

				int circleCount = Mathf.Clamp(6 * radius, 1, int.MaxValue);

				TPos sizeForRank = wrapMappingFuncs.funcGetSizeForRank(mapSizeOrigin, rank);

				int indexCellMapCurrent = wrapMappingFuncs.funcPosToIndex(position, sizeForRank, rank);

				WrapPerRank wrapPerRank = wrapPerRanks[rank];

				for (int k = 0; k < circleCount; k++)
				{
					ParamsGetIndexOfCell paramsGetIndexOfCell = new ParamsGetIndexOfCell(shiftedPos, sizeForRank, indexCellMapCurrent, k, radius);

					if (wrapMappingFuncs.funcGetCellIndex(in paramsGetIndexOfCell, out int indexCell))
					{
						List<ComponentMapablePosition> targets = wrapPerRank.objectsInMappingCells[indexCell];

						foreach (var item in targets)
						{
							if (!item.isFiltered)
							{
								item.isFiltered = true;
								countFiltered++;
							}
						}
					}
				}
				return countFiltered;
			}
			/// <summary>
			/// Разделить отфильтрованные объекты и неотфильтрованные. Создать <see cref="Memory{T}"/>
			/// </summary>
			private void CompressFiltered()
			{
				throw new NotImplementedException();
			}


			private void Filtering()
			{
				SelectClosest();
			}

			public void Clean()
			{
				foreach (var item in targets)
				{
					item.isFiltered = false;
				}

				ArrayPool<ComponentMapablePosition>.Shared.Return(targets);
				targets = default;
				position = default;
			}
		}
		/// <summary>
		/// Хранит данные о расчетах положения в сетках разного масштаба. Необходимо для поиска объектов по сегментам/областям/квадратам/кубам
		/// </summary>
		public class ComponentMapablePosition : IPoolable, IEquatable<ComponentMapablePosition>
		{
			/// <summary>
			/// <see langword="true"/>- не будет пропущен в <see cref="ManagerMapping{T}.Remap"/><br/>
			/// <see langword="false"/> - будет пропущен в <see cref="ManagerMapping{T}.Remap"/><br/>
			/// </summary>
			public bool isCalculated;
			/// <summary>
			/// <see langword="true"/> - прошел проверку, включен в обработку<br/>
			/// <see langword="false"/> - не прошел провекру, исключен из обработки
			/// </summary>
			public bool isFiltered;

			public int indexMapCurrent;
			/// <summary>
			/// Для сортировки
			/// </summary>
			public float sqrMagnitudeTemp;
			/// <summary>
			/// <see cref=""/>
			/// </summary>
			public int indexInManagerMapping;

			public IMappable mappablePos;
			/// <summary>
			/// Индексы квадратов карты которые занимает объект
			/// </summary>
			public List<int> indexes = new List<int>();

			public void CleanForRecalculate()
			{
				indexes.Clear();
			}

			public void CleanToReuse()
			{
				throw new NotImplementedException();
			}

			public void ReturnToPool()
			{
				throw new NotImplementedException();
			}

			public bool Equals(ComponentMapablePosition other)
			{
				return this == other;
			}
		}

		/// <summary>
		/// Объект может быть ассоциирован с квадартами или кубам на карте для группировки объектов в некой области размченной карты. 
		/// </summary>
		public interface IMappable
		{
			TPos MapPos { get; }
			ComponentMapablePosition ComponentMapablePosition { get; set; }
			TryGetCellIndexes FuncGetIndexes { get; }
			void FreeIndexesContainer(TIndexes container);
		}

		[Serializable]
		public struct MappingCell<T>
		{
			/// <summary>
			/// индекс связанных объектов в <see cref="ManagerMapping{T}.transforms"/>
			/// </summary>
			public int indexSelf;
			/// <summary>
			/// coordinate to index
			/// </summary>
			public int indexCell;
			/// <summary>
			/// Степень уплотнения. 
			/// 0 - равен минимальному размеру карты<br/>
			/// 1 - равен размеру 2х2, то есть 4 минимальных размеров объединеных в квадрат<br/>
			/// 2 - 
			/// </summary>
			public int rank;

			public T coord;
		}

		public readonly struct ParamsGetIndexOfCell
		{
			/// <summary>
			/// Позиция относительно min или origin = 0,0,0. Все координаты всегда положительные
			/// </summary>
			public readonly TPos posShifted;
			/// <summary>
			/// Размер карты относительно ращзмера ячеек
			/// </summary>
			public readonly TPos size;

			/// <summary>
			/// Индекс <see cref="posShifted"/>
			/// </summary>
			public readonly int indexPosShifted;
			public readonly int indexCircle;
			public readonly int radius;

			public ParamsGetIndexOfCell(TPos posShifted, TPos size, int indexPosShifted, int indexCircle, int radius)
			{
				this.posShifted = posShifted;
				this.size = size;
				this.indexPosShifted = indexPosShifted;
				this.indexCircle = indexCircle;
				this.radius = radius;
			}
		}

		/// <summary>
		/// Базовый класс обертка для функций получения индекса из координаты
		/// </summary>
		/// <typeparam name="T"></typeparam>
		//public class WrapMappingFuncs<T, TIndexes> : ScriptableObject where TIndexes : IList<int>
		public class WrapMappingFuncs : ScriptableObject
		{
			//public TryGetMapIndexes funcGetIndexes;
			public Func<TPos, TPos, int, int> funcPosToIndex;
			public Func<TPos, int, TPos> funcGetSizeForRank;
			public Func<TPos, TPos, TPos> funcSum;
			public TryGetCellIndexe funcGetCellIndex;
			public Func<TPos, TPos, float> funcGetSqrMagnitude;

			protected virtual void OnEnable()
			{//[{nameof(funcGetIndexes)}]
				throw new NotImplementedException($"Must be overrided and fields [{nameof(funcPosToIndex)}] &  must be initilized");
			}
		}


		/// <summary>
		/// Обработчик для получения индексов квадратов карты с указанием количества найденных ячеек
		/// </summary>
		/// <typeparam name="TIndexes">
		/// Структура с последовательностью чисел
		/// </typeparam>
		/// <param name="countIndex"></param>
		/// <param name=""></param>
		/// <returns></returns>
		public delegate bool TryGetCellIndexes(TPos center, int rank, out int countIndex, out TIndexes mapCellIndexes);
		public delegate bool TryGetCellIndexe(in ParamsGetIndexOfCell paramsGetIndexOfCell, out int index);


		//public delegate T GetPosValue<T>(ref TPos pos);
		public delegate T GetPosValue<T>(ref TPos pos);
	}
}