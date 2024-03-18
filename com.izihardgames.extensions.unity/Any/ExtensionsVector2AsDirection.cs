namespace UnityEngine
{
	public static partial class ExtensionsVector2AsDirection
	{
		/// <summary>
		/// Rotate Vector. 
		/// If angle positive - counterclockwise, negative - clockwise
		/// </summary>
		/// <param name="value"></param>
		/// <param name="angle"></param>
		/// <returns></returns>
		public static Vector2 RotateByZ(Vector2 value, float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.forward) * value;
		}
	}
}