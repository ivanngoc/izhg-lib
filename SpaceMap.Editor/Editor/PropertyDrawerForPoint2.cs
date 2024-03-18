using UnityEngine;
using UnityEditor;
using IziHardGames.Libs.Engine.Helpers;
using IziHardGaems.Libs.Engine.ForEditor;
using IziHardGames.Libs.NonEngine.Vectors;

namespace IziHardGames.Libs.NonEngine.SpaceMap.ForEditor
{
	[CustomPropertyDrawer(typeof(Point2))]
	public class PropertyDrawerForPoint2 : PropertyDrawer
	{
		private float xPad = 2;
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			RectSlicing rectSlicing = new RectSlicing(position, xPad);

			var rectPropX = property.FindPropertyRelative(nameof(Point2.x));
			var rectPropY = property.FindPropertyRelative(nameof(Point2.y));
		
			float fieldWidth = ConstantsForCustomProperty.WIDTH_FOR_FLOAT_FIELD_F4;

			EditorGUI.LabelField(rectSlicing.GetSliceAlongXWithWidthAndPad(ConstantsForCustomProperty.WIDTH_FOR_LABEL_STANDART), property.displayName);


			var propX = rectSlicing.GetSliceAlongXWithWidth(fieldWidth);
			var propY = rectSlicing.GetSliceAlongXWithWidth(fieldWidth);
			var propZ = rectSlicing.GetSliceAlongXWithWidth(fieldWidth);
			var propMagnitude = rectSlicing.GetReminder(ConstantsForCustomProperty.PAD_PROPERTY_END);

			EditorGUI.PropertyField(propX, rectPropX, GUIContent.none);
			EditorGUI.PropertyField(propY, rectPropY, GUIContent.none);
			EditorGUI.EndProperty();
			property.serializedObject.ApplyModifiedProperties();
		}
	}
}