namespace UnityEngine
{
	/// <summary>
	/// Left/Center/Right ||| Bot/Mid/Top ||| Front/Half/Back
	/// </summary>
	public static class ExtensionRenderer
	{
		/// <summary>
		/// World render top center bound
		/// </summary>
		/// <param name="renderer"></param>
		/// <returns></returns>
		public static Vector3 GetCenterTopFront(this Renderer renderer)
		{
			return renderer.bounds.GetCenterTopFront();
		}
	}

}