using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IziHardGames.Libs.Engine.Storages
{
	/*
    Есть карта. Карта разбивается на ячейки(квадраты) как на шахматном поле
    В зависимости от уровня (Precision Grade) карта делится по соотвествующим размерам квадратов
    Например если уровень 0 то делится на квадрат размером 1, если 2 то 4, если 3 то 8, то есть соответсвтует уровню двойки (связано с размером карты обычно кратному двум а также из за особенностей двоичного счисления)
    Для деления в меньшу сторону используются отрицательные числа, -1 = 2 в степени -1 = 0.5f, -2 = 0.25, -3 = 0.125
    Каждый квадрат преобразовывается в индекс. Если карта начаинается в нулевой координате то первая ячейка будет с координатой 0.0.0. 
    Порядок счисления индекса идет по порядку координат. то есть сначала по X, потом по Y потом по Z для системы XYZ. (есть и другие, например YXZ, XZY и т.д.)
    Объекты в одном квадрате помещаются в одну группу. Каждые 4 квадрата образующие квадрат начиная от начальной точки образуют квадрат более высокго Precision Grade

    Памятка для работы: для точной работы минимально необходимый размер квадрата = четверть от самого маленького объекта. То есть в самый маленький квадрат можно разместить 4 самых маленьних объекта
    Для поиска объектов в квадрате сначала выбирается размер равный самому себе, затем Precision Grade увеличиватеся до требуемого условия. Таким образом происходит индукционный поиск от квадрата
    
    Поиск может быть как с укрупнением Precision Grade, так и без него переходами по соседям. Тогда расширением будет происходи по принципу увеличения размера квадрата от центра на 1. 
    Размер = 0 будет поиск в самом центре. 
    Размер равный 1 будет происходить во всех 8 прилегающих к центру ячейках. 
    Размер равный 2 = во всех 16 ячейкаx внешнего кольца (периметра) соприкасающихся с ячейками размера 1 и т.д.
     
    Разметка и группировка объектов подобным образом позволяет быстро получать доступ к объектам на позициях 
    */

	/// <summary>
	/// 
	/// </summary>
	public class StorageByMappedPos
	{
		#region Unity Message
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public class StorageByMappedPosInt
	{
		public Vector3Int boundMin;
		public Vector3Int boundMax;
		public Vector3Int size;
		/// <summary>
		/// Сколько надо прибавить 
		/// </summary>
		public Vector3Int offsetToZero;

		public Tilemap tilemap;

		public Dictionary<int, TransformsAtIndex> transformsAtIndexes;

		public void Initilize(Vector3Int min, Vector3Int max)
		{
			boundMin = min;
			boundMax = max;
			size = max - min;
		}
		public void Initilize_De()
		{

		}

		public void MoveFromGroupeOneToGroupeTwo(TransformsAtIndex one, TransformsAtIndex two, Transform toMove)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Расчитаь индекс для размещения
		/// </summary>
		/// <param name="transform"></param>
		public void CalculateIndexToPlace(Transform transform)
		{
			throw new NotImplementedException();
		}

		public void CalculateIndexToPlace(Vector3 position)
		{

		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="position"></param>
		/// <param name="precisionGrade">
		/// 0 - 1 к 1, 1 ячейка размер 1x1 <br/>
		/// 1 - 1 к 2, 1 ячейка размер 2х2 и т.д.<br/>
		/// степен точности, степень укрупнения. Является показателем степени двойки
		/// </param>
		public int CalculateIndexToPlace2D(Vector3Int position, int precisionGrade)
		{
			Vector3Int minToPos = position - boundMin;

			if (precisionGrade > 0)
			{
				Vector3Int newPos = new Vector3Int(minToPos.x >> precisionGrade, minToPos.y >> precisionGrade, default);

				return newPos.ToCellIndex(ref size, ref minToPos);
			}
			else
			{
				if (precisionGrade < 0)
				{
					return minToPos.ToCellIndex(ref size, ref minToPos);
				}
				else
				{
					Vector3Int newPos = new Vector3Int(minToPos.x << precisionGrade, minToPos.y << precisionGrade, default);

					return newPos.ToCellIndex(ref size, ref minToPos);
				}
			}
		}
	}
	public class TransformsAtIndex
	{
		/// <summary>
		/// pos to index of square or cube
		/// </summary>
		public int index;

		public List<Transform> transforms;
	}
}