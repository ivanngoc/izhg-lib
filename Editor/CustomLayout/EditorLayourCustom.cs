#if UNITY_EDITOR
using UnityEngine;


namespace UnityEditor
{
	public class EditorLayourCustom
	{
		public static Vector3 Vector3Field(string label, Vector3 vector3)
		{
			EditorGUILayout.BeginHorizontal();

			GUILayout.Label(label);
			EditorGUILayout.Space();
			GUILayout.Label("X");
			vector3.x = EditorGUILayout.FloatField(vector3.x);
			GUILayout.Label("Y");
			vector3.y = EditorGUILayout.FloatField(vector3.y);
			GUILayout.Label("Z");
			vector3.z = EditorGUILayout.FloatField(vector3.z);

			EditorGUILayout.EndHorizontal();

			return vector3;
		}
		public static Vector2 Vector2Field(string label, Vector2 vector3)
		{
			EditorGUILayout.BeginHorizontal();

			GUILayout.Label(label);
			EditorGUILayout.Space();
			GUILayout.Label("X");
			vector3.x = EditorGUILayout.FloatField(vector3.x);
			GUILayout.Label("Y");
			vector3.y = EditorGUILayout.FloatField(vector3.y);
			EditorGUILayout.EndHorizontal();

			return vector3;
		}
	}
}
#endif
