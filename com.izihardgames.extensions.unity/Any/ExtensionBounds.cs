namespace UnityEngine
{
	/// <summary>
	/// Именование идет по координате
	/// Left/Center/Right ||| Bot/Mid/Top ||| Front/Half/Back
	/// </summary>
	public static class ExtensionBounds
	{
		public static Vector3 GetCenterTopFront(this Bounds bounds)
		{
			return new Vector3(bounds.min.x + (bounds.max.x - bounds.min.x) * 0.5f, bounds.max.y, bounds.min.z);
		}
		public static Vector3 GetLeftBotFront(this Bounds bounds)
		{
			return new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
		}
		public static Vector3 GetLeftTopFront(this Bounds bounds)
		{
			return new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
		}
		public static Vector3 GetRightBotFront(this Bounds bounds)
		{
			return new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
		}
		public static Vector3 GetRightTopFront(this Bounds bounds)
		{
			return new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
		}
		public static Vector3 GetLeftBotBack(this Bounds bounds)
		{
			return new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
		}
		public static Vector3 GetLeftTopBack(this Bounds bounds)
		{
			return new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
		}
		public static Vector3 GetRightBotBack(this Bounds bounds)
		{
			return new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
		}
		public static Vector3 GetRightTopBack(this Bounds bounds)
		{
			return new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
		}
	}	

}