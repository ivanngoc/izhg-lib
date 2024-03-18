using System;
using UnityEngine;


namespace IziHardGames.Libs.Engine.ProcessingVertex
{
	/// <summary>
	/// Операции с вершинами
	/// </summary>
	public static class VertexProcessor2D
	{
		#region Unity Message


		#endregion
		/// <summary>
		/// если сумма углов треугольников образованных 2 вершинами из параметров и точка проверки = 360, то точка находится внутри вершин
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <returns></returns>
		public static bool IsVertexInsideTrisByAngle(ref Vector3 point, ref Vector3 a, ref Vector3 b, ref Vector3 c)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// С помощью бараметрической системы координат определить находится ли точка внутри или нет
		/// </summary>
		/// <param name="point"></param>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		/// <see cref="https://github.com/SebLague/Gamedev-Maths/blob/master/PointInTriangle.cs"/>
		public static bool IsVertexInsideTrisByBarycentric(ref Vector2 A, ref Vector2 B, ref Vector2 C, ref Vector2 P)
		{
			float s1 = C.y - A.y;
			float s2 = C.x - A.x;
			float s3 = B.y - A.y;
			float s4 = P.y - A.y;

			float w1 = (A.x * s1 + s4 * s2 - P.x * s1) / (s3 * s2 - (B.x - A.x) * s1);
			float w2 = (s4 - w1 * s3) / s1;

			return w1 >= 0 && w2 >= 0 && (w1 + w2) <= 1;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="point"></param>
		/// <param name="lf"></param>
		/// <returns></returns>
		public static bool IsVertexInsideCube(ref Vector3 point, in Cube cube)
		{
			throw new NotImplementedException();
		}

		public readonly struct Baricentric
		{
			public readonly Vector3 center;
			public readonly Vector3 a;
			public readonly Vector3 b;
		}

		public readonly struct Tris
		{
			public readonly Vector3 a;
			public readonly Vector3 b;
			public readonly Vector3 c;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// down/top; front/back; left/right 
		/// </remarks>
		public readonly struct Cube
		{
			public readonly Vector3 dfl;
			public readonly Vector3 tfl;
			public readonly Vector3 tfr;
			public readonly Vector3 dfr;

			public readonly Vector3 dbl;
			public readonly Vector3 tbl;
			public readonly Vector3 tbr;
			public readonly Vector3 dbr;
		}
	}
}