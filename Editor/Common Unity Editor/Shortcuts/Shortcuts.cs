#if UNITY_EDITOR
using UnityEditor;

namespace IziHardGames.CustomEditor
{
	public static class Shortcuts
	{
		[MenuItem("Shortcuts*/Switch Lock Inpector")]
		public static void SwitchLockInpector()
		{
			//EditorWindow inspectorWindow = EditorWindow.GetWindow(typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow"));

			ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;

			ActiveEditorTracker.sharedTracker.ForceRebuild();
		}

	}
}
#endif
