using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

namespace IziHardGames.Region.Language
{
	[CustomEditor(typeof(DataLang))]
	public class DataLangInspector : Editor
	{
		string str = default;
		bool isPressed = false;

		public override void OnInspectorGUI()
		{
			//if (isPressed) 
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("count"));
			EditorGUILayout.LabelField("Labellll");
			EditorGUILayout.EndHorizontal();

			DataLang dataLang = target as DataLang;

			GUI.enabled = false;
			EditorGUILayout.ObjectField("Script Editor:", MonoScript.FromScriptableObject(this), typeof(DataLang), true);
			GUI.enabled = true;

			DrawDefaultInspector();

			GUI.SetNextControlName("TextField");
			str = EditorGUILayout.TextField(str);
			//EditorGUILayout.Input
			if (isPressed = GUILayout.Button("Push"))
			{
				//serializedObject.Update();
				//SerializedProperty serializedProperty = serializedObject.FindProperty("strings");
				//int index = serializedProperty.arraySize;
				//serializedProperty.InsertArrayElementAtIndex(index);
				//serializedProperty.GetArrayElementAtIndex(index).objectReferenceValue = str;
				//serializedObject.ApplyModifiedProperties();

				dataLang.strings.Add(str);
				//serializedProperty.list
			}

			if (isPressed) EditorGUI.FocusTextInControl("TextField");
			dataLang.count = dataLang.strings.Count;
		}

	}
}
#endif