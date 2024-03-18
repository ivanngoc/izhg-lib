#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

//namespace IziHardGame.Cust
[CustomPropertyDrawer(typeof(DrawIntAttribute))]
public class PropDrawerInt : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var tg = attribute as DrawIntAttribute;

		FieldInfo fi = fieldInfo;

		label.image = AssetDatabase.LoadAssetAtPath("Assets/Sprites/SquareWhite.png", typeof(Texture)) as Texture;

		GUIContent gUIContent = EditorGUI.BeginProperty(position, label, property);

		float totalWidth = EditorGUIUtility.currentViewWidth;

		var width = position.width;

		var split = position.RectSplitIntoLineHorizontal(2);
		Rect cache = position;
		int count = 4;
		float pad = 0.01f;
		EditorGUIUtility.labelWidth = cache.RectSplitIntoLineHorizontal(count, 0).width / 2;
		//newRect.width = EditorGUIUtility.currentViewWidth /3f;
		EditorGUI.PropertyField(cache.RectSplitIntoLineHorizontal(count, 0, pad), property);
		EditorGUI.TextField(cache.RectSplitIntoLineHorizontal(count, 1, pad), "Value0");
		EditorGUI.TextField(cache.RectSplitIntoLineHorizontal(count, 2, pad), "Value1");
		EditorGUI.TextField(cache.RectSplitIntoLineHorizontal(count, 3, pad), "Value2");

		Rect last = GUILayoutUtility.GetLastRect();
		EditorGUI.EndProperty();
	}
}

[CustomPropertyDrawer(typeof(DrawStringHexAttribute))]
public class PropDrawStringList : PropertyDrawer
{
	public bool isFoldout;
	Rect[] rects = new Rect[5];
	int[] splitPropotions = new int[] { 20, 5, 20, 10, 45 };
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		//return;
		if (!property.isArray) return;

		var tg = attribute as DrawIntAttribute;
		FieldInfo fi = fieldInfo;
		// 20 10 30 40
		float totalWidth = EditorGUIUtility.currentViewWidth;
		Rect cache = position;
		int count = 2;
		float pad = 0.02f;
		int i = default;

		if (property.isArray)
		{
			//i = property.enumValueIndex;
			string s = property.propertyPath.Remove(0, 19).TrimEnd(']');

			i = int.Parse(s);
			//string s = "strings.Array.data[2]";
			//string s = ;
			//i = property.depth;
			//i = property.intValue;
			//i = property.CountInProperty(); // error
			//i = property.fixedBufferSize;
			//i = property.CountRemaining(); // error
			//i = property.objectReferenceInstanceIDValue;
			EditorGUI.indentLevel++;
		}
		float width = EditorGUIUtility.labelWidth;

		GUIContent gUIContent = EditorGUI.BeginProperty(position, label, property);
		position.GetSplitWithPropotion(pad, rects, splitPropotions);
		EditorGUI.LabelField(rects[0], label);
		EditorGUI.LabelField(rects[1], string.Empty);
		if (property.isArray)
		{
			EditorGUI.TextField(rects[2], i.ToString("X8"));
		}
		else
		{
			EditorGUI.LabelField(rects[2], string.Empty);
		}
		EditorGUI.LabelField(rects[3], string.Empty);
		//EditorGUI.TextField(rects[4], property.GetArrayElementAtIndex(i).stringValue);
		//EditorGUI.TextField(rects[4], property.stringValue);
		EditorGUIUtility.labelWidth = 0;
		EditorGUI.PropertyField(rects[4], property, label, true);
		EditorGUIUtility.labelWidth = width;
		EditorGUI.EndProperty();
	}

	public void Scratch(Rect position, SerializedProperty property, GUIContent label)
	{
		if (!property.isArray) return;

		var tg = attribute as DrawIntAttribute;
		FieldInfo fi = fieldInfo;
		// 20 10 30 40
		float totalWidth = EditorGUIUtility.currentViewWidth;
		Rect cache = position;
		int count = 2;
		float pad = 0.02f;


		GUIContent gUIContent = EditorGUI.BeginProperty(position, label, property);
		EditorGUI.PropertyField(position, property);
		EditorGUI.EndProperty();

		int size = property.arraySize;

		isFoldout = EditorGUILayout.Foldout(isFoldout, "List Of Text With Hex Code");

		if (isFoldout)
		{
			for (int i = 0; i < size; i++)
			{
				position.GetSplitWithPropotion(pad, rects, splitPropotions);
				// label-pad-hex-pad-value              
				EditorGUI.LabelField(rects[0], $"#{i}");
				EditorGUI.LabelField(rects[1], string.Empty);
				EditorGUI.TextField(rects[2], i.ToString("X8"));
				EditorGUI.LabelField(rects[3], string.Empty);
				EditorGUI.TextField(rects[4], property.GetArrayElementAtIndex(i).stringValue);
			}
		}
	}
}
#endif
