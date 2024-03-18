#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


namespace IziHardGames.CustomEditor.Window
{
	public class EditorWindowSplit : EditorWindow
	{
		// Add menu named "My Window" to the Window menu
		[MenuItem("Window/Custom/2D/Split")]
		static void Init()
		{
			// Get existing open window or if none, make a new one:
			EditorWindowSplit window = (EditorWindowSplit)EditorWindow.GetWindow(typeof(EditorWindowSplit));

			window.titleContent = new GUIContent("Split");

			window.Show();
		}

		private void OnGUI()
		{

		}
	}

}
#endif
