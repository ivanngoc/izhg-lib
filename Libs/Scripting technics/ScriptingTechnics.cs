#pragma warning disable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace IziHardGames.Develop.Technics
{
	public class ScriptingTechnics : MonoBehaviour
	{
		/// <summary>
		/// Циклы For
		/// </summary>
		public void ItteratingsFor()
		{

		}
		/// <summary>
		/// Циклы 
		/// </summary>
		public void Itteratings()
		{

			// каждый с каждым c выдачей результата
			// intersections = count * (count - 1) - колво иттераций c учетом перевертывания то есть  A vs B != B vs A
			// прямоугольник
			{
				int[] source = new int[6];

				List<string> result = new List<string>();

				for (int i = 0; i < source.Length; i++)
				{
					for (int j = 0; j < source.Length; j++)
					{
						if (i == j) continue;

						int perfom = source[i] + source[j];

						result.Add("result of intersection");
					}
				}
			}

			// пирамидка
			// каждый встретится с каждым только 1 раз
			{
				int[] source = new int[] { 0, 1, 2, 3, 4, 5 };

				int tempCount = default;

				int iCount = source.Length - 1;

				for (int i = 0; i < iCount; i++)
				{
					tempCount++;

					for (int j = 0; j < tempCount; j++)
					{
						Debug.Log($"{source[j]} | {source[tempCount]}");
					}
					Debug.LogError("End");
				}
			}





			// 2^n - где n количество факторов
		}

		private void ItteratingsWithPad()
		{
			float padX = default;
			float padz = default;

			int x = 10;
			int z = 10;

			Vector3 coord = default;

			float xPad = padX;
			float zpad = padz;

			for (int i = 0; i < x; i++)
			{
				for (int j = 0; j < z; j++)
				{
					coord = new Vector3(i + xPad, 0, j + zpad);

					transform.position = coord;

					zpad += padz;
				}
				xPad += padX;
				zpad = padz;
			}
		}

		[Flags]
		public enum EEnum : int
		{
			Value0,
			Value1,
			Value2,
			Value3,
			Value4,
			Value5,
		}

		public void Enums()
		{
			EEnum eEnum = default;
			// multiflag 
			eEnum = EEnum.Value0 | EEnum.Value2 | EEnum.Value4;
			// except

			// add
		}
	}
}