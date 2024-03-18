namespace UnityEngine
{
	public static class Log
	{
		#region Unity Message
		public static void LogYellow(string message, Object self = default)
		{
#if UNITY_EDITOR
			Debug.Log($"<color=yellow>{message}</color>", self);
#endif
		}
		public static void LogRed(string message, Object self = default)
		{
#if UNITY_EDITOR
			Debug.Log($"<color=red>{message}</color>", self);
#endif
		}
		public static void LogLime(string message, Object self = default)
		{
#if UNITY_EDITOR
			Debug.Log($"<color=lime>{message}</color>", self);
#endif
		}
		public static void LogCyan(string message, Object self = default)
		{
#if UNITY_EDITOR

			Debug.Log($"<color=cyan>{message}</color>", self);
#endif
		}
		public static void LogBlue(string message, Object self = default)
		{
#if UNITY_EDITOR

			Debug.Log($"<color=blue>{message}</color>", self);
#endif
		}
		#endregion
	}

#if UNITY_EDITOR
	public static class ExtensionObjectDebug
	{
		public static void LogRed(this Object self, string message)
		{
			Debug.Log($"<color=red>{message}</color>", self);
		}
		public static void LogLime(this Object self, string message)
		{
			Debug.Log($"<color=lime>{message}</color>", self);
		}
		public static void LogBlue(this Object self, string message)
		{
			Debug.Log($"<color=blue>{message}</color>", self);
		}
	}


#endif

}