#if UNITY_EDITOR
using System;
using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace IziHardGames.MenuEditor
{
	public class MenuIsometric
	{
		#region Rotate Selected
		[MenuItem("*Isometric/Rotate/35/-Y")]
		public static void RotateGlobalnY35()
		{
			var selection = Selection.activeGameObject;

			if (selection != null)
			{
				Undo.RecordObject(selection.transform, "Change Rotation");
				selection.transform.Rotate(0f, -35f, 0f, Space.World);
			}
		}
		[MenuItem("*Isometric/Rotate/35/+Y")]
		public static void RotateGlobalY35()
		{
			var selection = Selection.activeGameObject;

			if (selection != null)
			{
				Undo.RecordObject(selection.transform, "Change Rotation");
				selection.transform.Rotate(0f, 35f, 0f, Space.World);
			}
		}


		[MenuItem("*Isometric/Rotate/Diff/1|3 Pi X")]
		public static void RotateGlobalY30()
		{
			var selection = Selection.activeGameObject;

			if (selection != null)
			{
				Undo.RecordObject(selection.transform, "Change Rotation");
				selection.transform.Rotate(Mathf.Rad2Deg * Mathf.PI * (1f / 3f), 0f, 0f, Space.World);
			}
		}
		[MenuItem("*Isometric/Rotate/30/X")]
		public static void RotateGlobalX30()
		{
			var selection = Selection.activeGameObject;

			if (selection != null)
			{
				Undo.RecordObject(selection.transform, "Change Rotation");
				selection.transform.Rotate(30f, 0f, 0f, Space.World);
			}
		}
		[MenuItem("*Isometric/Rotate/30/+Z")]
		public static void RotateGlobalZ30()
		{
			var selection = Selection.activeGameObject;

			if (selection != null)
			{
				Undo.RecordObject(selection.transform, "Change Rotation");
				selection.transform.Rotate(0f, 0f, 30f, Space.World);
			}
		}
		[MenuItem("*Isometric/Rotate/30/-Z")]
		public static void RotateGlobalnZ30()
		{
			var selection = Selection.activeGameObject;

			if (selection != null)
			{
				Undo.RecordObject(selection.transform, "Change Rotation");
				selection.transform.Rotate(0f, 0f, -30f, Space.World);
			}
		}

		#region 45
		[MenuItem("*Isometric/Rotate/45/+X")]
		public static void RotateGlobalX45()
		{
			var selection = Selection.activeGameObject;

			if (selection != null)
			{
				Undo.RecordObject(selection.transform, "Change Rotation");
				selection.transform.Rotate(45f, 0f, 0f, Space.World);
			}
		}
		[MenuItem("*Isometric/Rotate/45/-X")]
		public static void RotateGlobalnX45()
		{
			var selection = Selection.activeGameObject;

			if (selection != null)
			{
				Undo.RecordObject(selection.transform, "Change Rotation");
				selection.transform.Rotate(-45f, 0f, 0f, Space.World);
			}
		}
		[MenuItem("*Isometric/Rotate/45/+Y")]
		public static void RotateGlobalY45()
		{
			var selection = Selection.activeGameObject;

			if (selection != null)
			{
				Undo.RecordObject(selection.transform, "Change Rotation");
				selection.transform.Rotate(0f, 45f, 0f, Space.World);
			}
		}
		[MenuItem("*Isometric/Rotate/45/-Y")]
		public static void RotateGlobalnY45()
		{
			var selection = Selection.activeGameObject;

			if (selection != null)
			{
				Undo.RecordObject(selection.transform, "Change Rotation");
				selection.transform.Rotate(0f, 45f, 0f, Space.World);
			}
		}
		#endregion
		#endregion

		[MenuItem("*Isometric/2D/Align with Gravity")]
		public static void AlignWithGravity()
		{
			var selection = Selection.gameObjects;

			if (selection != null)
			{
				foreach (var item in selection)
				{
					item.transform.rotation = Quaternion.LookRotation(-item.transform.forward, Vector3.up);
				}
			}
		}

		[MenuItem("*Isometric/2D/Print Rotation")]
		public static void PrintRotation()
		{
			var selection = Selection.gameObjects;

			//PrintVector3(ToSignedRotation(new Vector3(179, 370, -380)));

			if (selection != null)
			{
				foreach (var item in selection)
				{
					var backward = (-item.transform.up);

					Vector3 signed = ToSignedRotation(item.transform.rotation.eulerAngles);
					PrintVector3(signed, item);
					PrintVector3(item.transform.rotation.eulerAngles, item);
					PrintVector3(backward, item);
					var projectDown = Vector3.Project(backward, Vector3.down);
					var projectForwads = Vector3.Project(backward, Vector3.forward);

					PrintVector3(new Vector3(0, projectDown.y, projectForwads.z), item);
				}
			}
		}

		[MenuItem("*Isometric/2D/X,Y,Z | Set Origin | Look At Right")]
		public static void IsometricAlignRight()
		{
			var selections = Selection.gameObjects;

			if (selections != null)
			{
				foreach (var item in selections)
				{
					Undo.RecordObject(item.transform, "Change Rotation");

					Vector3 pos = item.transform.position;
					item.transform.position = Vector3.zero;
					item.transform.rotation = Quaternion.identity;
					item.transform.Rotate(Vector3.right, -45, Space.World);
					item.transform.Rotate(Vector3.up, -35, Space.World);
					item.transform.Rotate(Vector3.forward, 30, Space.World);
					item.transform.position = pos;
					//item.transform.rotation = Quaternion.identity * Quaternion.Euler(new Vector3(0, 0, 30)) * Quaternion.Euler(new Vector3(0, 45, 0)) * Quaternion.Euler(new Vector3(30, 0, 0));
				}
			}
		}

		#region Y Rotation
		[MenuItem("*Isometric/2D/Y Rotation/[90] Z,Y,!X")]
		public static void IsometricAlign_Z_Y_nX()
		{
			var selection = Selection.gameObjects;

			if (selection != null)
			{
				foreach (var item in selection)
				{
					Undo.RecordObject(item.transform, "Change Rotation");

					Vector3 pos = item.transform.position;
					item.transform.position = Vector3.zero;
					item.transform.rotation = Quaternion.identity;
					item.transform.Rotate(Vector3.right, -45, Space.World);
					item.transform.Rotate(Vector3.up, -35, Space.World);
					item.transform.Rotate(Vector3.forward, 30, Space.World);
					item.transform.Rotate(Vector3.up, 90, Space.Self);
					item.transform.position = pos;
					//item.transform.rotation = Quaternion.identity * Quaternion.Euler(new Vector3(0, 0, 30)) * Quaternion.Euler(new Vector3(0, 45, 0)) * Quaternion.Euler(new Vector3(30, 0, 0));
				}
			}
		}
		[MenuItem("*Isometric/2D/Y Rotation/[180] !X,Y,!Z")]
		public static void IsometricAlign_nX_Y_nZ()
		{
			var selection = Selection.gameObjects;

			if (selection != null)
			{
				foreach (var item in selection)
				{
					Undo.RecordObject(item.transform, "Change Rotation");

					Vector3 pos = item.transform.position;
					item.transform.position = Vector3.zero;
					item.transform.rotation = Quaternion.identity;
					item.transform.Rotate(Vector3.right, -45, Space.World);
					item.transform.Rotate(Vector3.up, -35, Space.World);
					item.transform.Rotate(Vector3.forward, 30, Space.World);
					item.transform.Rotate(Vector3.up, 180, Space.Self);
					item.transform.position = pos;
				}
			}
		}
		[MenuItem("*Isometric/2D/Y Rotation/[270] !Z,Y,X")]
		public static void IsometricAlign_nZ_Y_X()
		{
			var selection = Selection.gameObjects;

			if (selection != null)
			{
				foreach (var item in selection)
				{
					Undo.RecordObject(item.transform, "Change Rotation");

					Vector3 pos = item.transform.position;
					item.transform.position = Vector3.zero;
					item.transform.rotation = Quaternion.identity;
					item.transform.Rotate(Vector3.right, -45, Space.World);
					item.transform.Rotate(Vector3.up, -35, Space.World);
					item.transform.Rotate(Vector3.forward, 30, Space.World);
					item.transform.Rotate(Vector3.up, 270, Space.Self);
					item.transform.position = pos;
				}
			}
		}
		#endregion

		#region X Rotations
		[MenuItem("*Isometric/2D/X Rotatios/[90] X,Z,!Y")]
		public static void IsometricAlign_X_Z_nY()
		{
			var selection = Selection.gameObjects;

			if (selection != null)
			{
				foreach (var item in selection)
				{
					Vector3 pos = item.transform.position;
					item.transform.position = Vector3.zero;
					item.transform.rotation = Quaternion.identity;
					item.transform.Rotate(Vector3.right, -45, Space.World);
					item.transform.Rotate(Vector3.up, -35, Space.World);
					item.transform.Rotate(Vector3.forward, 30, Space.World);
					item.transform.Rotate(Vector3.right, -90, Space.Self);
					item.transform.position = pos;
					//item.transform.rotation = Quaternion.identity * Quaternion.Euler(new Vector3(0, 0, 30)) * Quaternion.Euler(new Vector3(0, 45, 0)) * Quaternion.Euler(new Vector3(30, 0, 0));
				}
			}
		}
		[MenuItem("*Isometric/2D/X Rotatios/[180] X,!Y,!Z")]
		public static void IsometricAlign_X_nY_nZ()
		{
			var selection = Selection.gameObjects;

			if (selection != null)
			{
				foreach (var item in selection)
				{
					Vector3 pos = item.transform.position;
					item.transform.position = Vector3.zero;
					item.transform.rotation = Quaternion.identity;
					item.transform.Rotate(Vector3.right, -45, Space.World);
					item.transform.Rotate(Vector3.up, -35, Space.World);
					item.transform.Rotate(Vector3.forward, 30, Space.World);
					item.transform.Rotate(Vector3.right, -180, Space.Self);
					item.transform.position = pos;
					//item.transform.rotation = Quaternion.identity * Quaternion.Euler(new Vector3(0, 0, 30)) * Quaternion.Euler(new Vector3(0, 45, 0)) * Quaternion.Euler(new Vector3(30, 0, 0));
				}
			}
		}

		[MenuItem("*Isometric/2D/X Rotatios/[270] X,!Z,Y")]
		public static void IsometricAlign_X_nZ_Y()
		{
			var selection = Selection.gameObjects;

			if (selection != null)
			{
				foreach (var item in selection)
				{
					Vector3 pos = item.transform.position;
					item.transform.position = Vector3.zero;
					item.transform.rotation = Quaternion.identity;
					item.transform.Rotate(Vector3.right, -45, Space.World);
					item.transform.Rotate(Vector3.up, -35, Space.World);
					item.transform.Rotate(Vector3.forward, 30, Space.World);
					item.transform.Rotate(Vector3.right, -270, Space.Self);
					item.transform.position = pos;
					//item.transform.rotation = Quaternion.identity * Quaternion.Euler(new Vector3(0, 0, 30)) * Quaternion.Euler(new Vector3(0, 45, 0)) * Quaternion.Euler(new Vector3(30, 0, 0));
				}
			}
		}

		#endregion


		[MenuItem("*Isometric/2D/X,Z,Y | Look At Left")]
		public static void IsometricAlignLeft()
		{
			var selection = Selection.gameObjects;

			if (selection != null)
			{
				foreach (var item in selection)
				{
					Undo.RecordObject(item.transform, "Change Rotation");

					Vector3 pos = item.transform.position;
					item.transform.position = Vector3.zero;
					item.transform.rotation = Quaternion.identity;
					item.transform.Rotate(Vector3.right, -45, Space.World);
					item.transform.Rotate(Vector3.up, 35, Space.World);
					item.transform.Rotate(Vector3.forward, 30, Space.World);
					item.transform.position = pos;
					// not working
					//item.transform.rotation = Quaternion.identity * Quaternion.Euler(new Vector3(-45, 0, 0)) * Quaternion.Euler(new Vector3(0, 35, 0)) * Quaternion.Euler(new Vector3(0, 0, 30));
				}
			}
		}



		[MenuItem("*Isometric/2D/Y,!Z,!X | Look At Top ")]
		public static void IsometricAlignTop()
		{
			var selection = Selection.gameObjects;

			if (selection != null)
			{
				foreach (var item in selection)
				{
					Vector3 pos = item.transform.position;
					item.transform.position = Vector3.zero;
					item.transform.rotation = Quaternion.identity;
					item.transform.Rotate(Vector3.right, 45, Space.World);
					item.transform.Rotate(Vector3.up, 35, Space.World);
					item.transform.Rotate(Vector3.forward, -30, Space.World);
					item.transform.position = pos;
					//item.transform.rotation = Quaternion.identity * Quaternion.Euler(new Vector3(0, 0, 30)) * Quaternion.Euler(new Vector3(0, 45, 0)) * Quaternion.Euler(new Vector3(30, 0, 0));
				}
			}
		}
		[MenuItem("*Isometric/2D/Y,Z,!X | Align To Left (Look At Bot) ")]
		public static void IsometricAlignBot()
		{
			var selection = Selection.gameObjects;

			if (selection != null)
			{
				foreach (var item in selection)
				{
					Vector3 pos = item.transform.position;
					item.transform.position = Vector3.zero;
					item.transform.rotation = Quaternion.identity;
					item.transform.Rotate(Vector3.right, -45, Space.World);
					item.transform.Rotate(Vector3.up, -35, Space.World);
					item.transform.Rotate(Vector3.forward, -30, Space.World);
					item.transform.position = pos;
					//item.transform.rotation = Quaternion.identity * Quaternion.Euler(new Vector3(0, 0, 30)) * Quaternion.Euler(new Vector3(0, 45, 0)) * Quaternion.Euler(new Vector3(30, 0, 0));
				}
			}
		}



		private static Vector3 ToSignedRotation(Vector3 rotation)
		{
			return new Vector3(ToSignedAngle(rotation.x), ToSignedAngle(rotation.y), ToSignedAngle(rotation.z));
		}

		private static void PrintVector3(Vector3 vector, UnityEngine.Object context = default)
		{
			string x = vector.x.ToString("G17", CultureInfo.InvariantCulture);
			string y = vector.y.ToString("G17", CultureInfo.InvariantCulture);
			string z = vector.z.ToString("G17", CultureInfo.InvariantCulture);

			//string xAngle = item.transform.rotation.x.ToString("G17", CultureInfo.InvariantCulture);
			//string yAngle = item.transform.rotation.y.ToString("G17", CultureInfo.InvariantCulture);
			//string zAngle = item.transform.rotation.z.ToString("G17", CultureInfo.InvariantCulture);

			if (context != null)
			{
				Debug.Log($"{context.name}{Environment.NewLine}({x}, {y}, {z})", context);
			}
			else
			{
				Debug.Log($"({x}, {y}, {z})");
			}

		}

		private static float ToSignedAngle(float value)
		{
			if (value > 0)
			{
				while (value > 360)
				{
					value -= 360;
				}
			}

			if (value < 0)
			{
				while (value < -360)
				{
					value += 360;
				}
			}

			if (value > 180)
			{
				return -180 + (value - 180);
			}

			if (value < -180)
			{
				return 180 + (value + 180);
			}

			return value;
		}
	}
}
#endif
