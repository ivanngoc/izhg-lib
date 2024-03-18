using UnityEngine;


namespace IziHardGames.Libs.Engine.Math.Trigonometry
{
	public static partial class HelperTrigonometry
	{
		/// <summary>
		/// Расчитать вращение для проекции. При сусловии что вращение осуществляется только по 2 осям: X и Z
		/// Только для аксонометрических проекций
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="degreeWidth"></param>
		/// <param name="degreeHeight"></param>
		/// <returns></returns>
		public static Vector3 GetRotattionFromProjection(float width, float originalWidth, float height, float originalHeight)
		{
			float degreeX = Mathf.Acos(width / originalWidth) * Mathf.Rad2Deg;

			float degreeZ = Mathf.Acos(height / originalHeight) * Mathf.Rad2Deg;

			return new Vector3(degreeX, 0, degreeZ);
		}
	}
}