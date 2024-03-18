namespace UnityEngine
{
	/// <summary>
	/// 
	/// </summary>
	public static class ExtensionsVector2ForScreen
	{
		public static Vector2 ScreenOppositeY(this Vector2 vector3)
		{
			return new Vector2(vector3.x, Screen.height - vector3.y);
		}
	}
}