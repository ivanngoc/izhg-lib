using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using IziHardGames.Libs.Engine.Helpers;
using IziHardGaems.Libs.Engine.ForEditor;
using IziHardGames.Libs.NonEngine.Vectors;

namespace IziHardGames.Libs.NonEngine.SpaceMap.ForEditor
{
	[CustomPropertyDrawer(typeof(Point3))]
	public class PropertyDrawerForVector3Sur : PropertyDrawer
	{
		private float xPad = 2;
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);


			RectSlicing rectSlicing  = new RectSlicing(position,xPad);



			var rectPropX                = property.FindPropertyRelative(nameof(Point3.x));
			var rectPropY                = property.FindPropertyRelative(nameof(Point3.y));
			var rectPropZ                = property.FindPropertyRelative(nameof(Point3.z));
			var rectPropMagnitude        = property.FindPropertyRelative(nameof(Point3.magnitudeCachedByDemand));

			//// Draw fields - pass GUIContent.none to each so they are drawn without labels
			//EditorGUI.PropertyField(propX, rectPropX, new GUIContent("X"));
			//EditorGUI.PropertyField(propY, rectPropY, new GUIContent("Y"));
			//EditorGUI.PropertyField(propZ, rectPropZ, new GUIContent("Z"));
			//EditorGUI.PropertyField(propMagnitude, rectPropMagnitude, new GUIContent("Mag"));
			float fieldWidth = ConstantsForCustomProperty.WIDTH_FOR_FLOAT_FIELD_F4;

			EditorGUI.LabelField(rectSlicing.GetSliceAlongXWithWidthAndPad(ConstantsForCustomProperty.WIDTH_FOR_LABEL_STANDART), property.displayName);


			//EditorGUI.LabelField(rectSlicing.GetSliceAlongXWithWidth(20), nameof(Vector3Sur.x));
			//EditorGUI.LabelField(rectSlicing.GetSliceAlongXWithWidth(10f), nameof(Vector3Sur.y));

			var propX              = rectSlicing.GetSliceAlongXWithWidth(fieldWidth);
			var propY              = rectSlicing.GetSliceAlongXWithWidth(fieldWidth);
			var propZ              = rectSlicing.GetSliceAlongXWithWidth(fieldWidth);
			var propMagnitude      = rectSlicing.GetReminder(ConstantsForCustomProperty.PAD_PROPERTY_END);

			EditorGUI.PropertyField(propX, rectPropX, GUIContent.none);
			EditorGUI.PropertyField(propY, rectPropY, GUIContent.none);
			EditorGUI.PropertyField(propZ, rectPropZ, GUIContent.none);
			EditorGUI.PropertyField(propMagnitude, rectPropMagnitude, GUIContent.none);


			EditorGUI.EndProperty();

			property.serializedObject.ApplyModifiedProperties();
		}
	}
}