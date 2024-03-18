#if UNITY_EDITOR

#pragma warning disable
using IziHardGames.CustomEditor.Window;
using IziHardGames.Libs.Engine.CustomTypes;
using IziHardGames.Libs.Engine.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace IziHardGames.CustomEditor.Window
{
	public class EditorWindowAlign : EditorWindow
	{
		private Object selectionObjectKey;
		private EAlignType eAlignType;
		private EDirection eDirection;
		private Vector2 vector2;
		private Vector3 vector3;
		private RectPosGlobal rectPosGlobalSelection;
		private float spaceSize;

		///<summary>
		///<see cref="EAction"/><br/>
		///0 AlignObjectsHorizontalLeft<br/>
		///1 AlignObjectsHorizontalMid<br/>
		///2 AlignObjectsHorizontalRight<br/>
		///3 AlignObjectsVerticalBot<br/>
		///4 AlignObjectsVerticalMid<br/>
		///5 AlignObjectsVerticalTop<br/>
		///6 DistributeObjectsHorizontalLeft<br/>
		///7 DistributeObjectsHorizontalMid<br/>
		///8 DistributeObjectsHorizontalRight<br/>
		///9 DistributeObjectsVerticalBot<br/>
		///10 DistributeObjectsVerticalMid<br/>
		///11 DistributeObjectsVerticalTop<br/>
		///12 AnchoringFit<br/>
		///13 DistributeSpaceHorizontal<br/>
		///14 DistributeSpaceVertical<br/>
		///15 SplitHorizontal<br/>
		///16 SplitVertical<br/> 
		/// </summary>
		public Dictionary<int, Texture> buttonIcons;
		private RectTransform[] cache;
		private Dictionary<RectTransform, int> cachedSiblingIndex = new Dictionary<RectTransform, int>(100);
		private Dictionary<RectTransform, Transform> cachedParents = new Dictionary<RectTransform, Transform>(100);

		private bool isGrouped;
		private bool isOrderByPositionNotHierarchy;

		// Add menu named "My Window" to the Window menu
		[MenuItem("Window/IziHardGames/2D/AlignTool")]
		static void Init()
		{
			// Get existing open window or if none, make a new one:
			EditorWindowAlign window = (EditorWindowAlign)EditorWindow.GetWindow(typeof(EditorWindowAlign));
			//window.name = "Align";
			window.InitIcons();

			window.titleContent = new GUIContent("Align");

			window.Show();
		}


		#region Unity Message  
		//private void OnDisable()
		//{
		//    SceneShowRect.isShow = false;
		//}
		//private void OnFocus()
		//{
		//    SceneShowRect.isShow = eAlignType == EAlignType.SelectionRect;
		//}
		//private void OnLostFocus()
		//{
		//    SceneShowRect.isShow = false;
		//}
		//private void OnEnable()
		//{
		//    SceneShowRect.isShow = eAlignType == EAlignType.SelectionRect;
		//}
		private void OnGUI()
		{
			GUI.enabled = false;
			EditorGUILayout.ObjectField("Script Editor:", MonoScript.FromScriptableObject(this), typeof(EditorWindowAlign), true);
			GUI.enabled = true;

			if (buttonIcons == null) InitIcons();

			#region Rect type selector
			EditorGUILayout.BeginHorizontal();

			GUILayout.Label("Align Type");

			eAlignType = (EAlignType)EditorGUILayout.EnumPopup(eAlignType);

			switch (eAlignType)
			{
				case EAlignType.SelectionBorderInnter: selectionObjectKey = EditorGUILayout.ObjectField(selectionObjectKey, typeof(RectTransform), true); break;
				case EAlignType.SelectionBorderOuter: selectionObjectKey = EditorGUILayout.ObjectField(selectionObjectKey, typeof(RectTransform), true); break;
				case EAlignType.SelectionRect:
					{
						RectTransform[] rectTransforms = default;

						if ((rectTransforms = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered)) != null && rectTransforms.Length > 0)
						{
							// SceneShowRect.rectPosGlobal = HelperRect.CalculateBounds(rectTransforms);
						}
						break;
					}
				case EAlignType.Vector2:
					//vector2 = EditorGUILayout.Vector2Field("Vector2", vector2);
					vector2 = EditorLayourCustom.Vector2Field("Vector2:", vector2);
					break;
				case EAlignType.Vector3:
					//vector3 = EditorGUILayout.Vector3Field("Vector3:", vector3);
					vector3 = EditorLayourCustom.Vector3Field("Vector3:", vector3);
					break;
				case EAlignType.SpacingAdd:
					//vector3 = EditorGUILayout.Vector3Field("Vector3:", vector3);
					vector3 = EditorLayourCustom.Vector3Field("Vector3:", vector3);
					break;
				case EAlignType.Spacing:
					//vector3 = EditorGUILayout.Vector3Field("Vector3:", vector3);
					vector3 = EditorLayourCustom.Vector3Field("Vector3:", vector3);
					break;
				default: break;
			}
			EditorGUILayout.EndHorizontal();
			#endregion

			#region Ascending / Descending selector + left/right
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Направление ");
			eDirection = (EDirection)EditorGUILayout.EnumPopup(eDirection);

			GUILayout.Label("Space Size");
			spaceSize = EditorGUILayout.FloatField(spaceSize);

			EditorGUILayout.EndHorizontal();

			#endregion

			//EditorGUILayout.PrefixLabel("As Group");

			isGrouped = EditorGUILayout.Toggle("As Group", isGrouped);
			isOrderByPositionNotHierarchy = EditorGUILayout.Toggle("Order By Pos Not By Hierarchy", isOrderByPositionNotHierarchy);

			EditorGUILayout.Space();

			Split();

			EditorGUILayout.Space();

			AlignObjects();

			DistributeObjects();

			Spacing();

			if (GUILayout.Button("Switch Selection Rect"))
			{
				// SceneShowRect.isShow = !SceneShowRect.isShow;
			}
		}
		#endregion
		public void Split()
		{
			GUILayout.Label("Split");

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button(buttonIcons[(int)EAction.SplitHorizontalToLeft], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectPosGlobal rectPosGlobal = default;

				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

				cache = new RectTransform[rects.Length];

				HelperRectTransform.PartingHorizontalCloneToLeft(rects, 0, rects.Length, 0, cache);

				Selection.objects = cache.Union(rects).Select(x => x.gameObject).ToArray();
			}
			if (GUILayout.Button(buttonIcons[(int)EAction.SplitHorizontalToRight], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectPosGlobal rectPosGlobal = default;

				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

				cache = new RectTransform[rects.Length];

				HelperRectTransform.PartingHorizontalCloneToRight(rects, 0, rects.Length, 0, cache);

				Selection.objects = cache.Union(rects).Select(x => x.gameObject).ToArray();
			}
			if (GUILayout.Button(buttonIcons[(int)EAction.SplitVerticalToTop], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectPosGlobal rectPosGlobal = default;

				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

				cache = new RectTransform[rects.Length];

				HelperRectTransform.PartingVerticalCloneToTop(rects, 0, rects.Length, 0, cache);

				Selection.objects = cache.Union(rects).Select(x => x.gameObject).ToArray();
			}
			if (GUILayout.Button(buttonIcons[(int)EAction.SplitVerticalToBot], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectPosGlobal rectPosGlobal = default;

				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

				cache = new RectTransform[rects.Length];

				HelperRectTransform.PartingVerticalCloneToBot(rects, 0, rects.Length, 0, cache);

				Selection.objects = cache.Union(rects).Select(x => x.gameObject).ToArray();
			}
			EditorGUILayout.Space();

			if (GUILayout.Button(buttonIcons[(int)EAction.AnchoringFit], GUILayout.Width(32), GUILayout.Height(32)))
			{
				MenuUtility.SetAnchors();
			}
			EditorGUILayout.Space();
			// distribute space horizontal
			if (GUILayout.Button(buttonIcons[(int)EAction.DistributeSpaceHorizontal], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

				RectPosGlobal rectPosGlobal = default;

				if (eAlignType == EAlignType.SelectionBorderInnter || eAlignType == EAlignType.SelectionBorderOuter) rectPosGlobal = new RectPosGlobal(selectionObjectKey as RectTransform);

				if (eAlignType == EAlignType.SelectionRect) rectPosGlobal = HelperRect.CalculateBounds(rects);

				if (eAlignType == EAlignType.Vector2) rectPosGlobal = new RectPosGlobal(vector2, vector2);

				if (eAlignType == EAlignType.Vector3) rectPosGlobal = new RectPosGlobal(vector3, vector3);

				HelperRectTransform.DestributeHorizontalSpaces(rects, in rectPosGlobal, isOrderByPositionNotHierarchy);
			}
			// distribute space vertical
			if (GUILayout.Button(buttonIcons[(int)EAction.DistributeSpaceVertical], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

				RectPosGlobal rectPosGlobal = default;

				if (eAlignType == EAlignType.SelectionBorderInnter || eAlignType == EAlignType.SelectionBorderOuter) rectPosGlobal = new RectPosGlobal(selectionObjectKey as RectTransform);

				if (eAlignType == EAlignType.SelectionRect) rectPosGlobal = HelperRect.CalculateBounds(rects);

				if (eAlignType == EAlignType.Vector2) rectPosGlobal = new RectPosGlobal(vector2, vector2);

				if (eAlignType == EAlignType.Vector3) rectPosGlobal = new RectPosGlobal(vector3, vector3);

				HelperRectTransform.DestributeVerticalSpaces(rects, in rectPosGlobal, isOrderByPositionNotHierarchy);
			}
			EditorGUILayout.EndHorizontal();
		}
		public void Spacing()
		{
			GUILayout.Label("Pading & Spacing");
			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button(buttonIcons[(int)EAction.SpaceRemoveHorizontalToLeft], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

				RectPosGlobal rectPosGlobal = default;

				if (eAlignType == EAlignType.SelectionBorderInnter || eAlignType == EAlignType.SelectionBorderOuter) rectPosGlobal = new RectPosGlobal(selectionObjectKey as RectTransform);

				if (eAlignType == EAlignType.SelectionRect) rectPosGlobal = HelperRect.CalculateBounds(rects);

				if (eAlignType == EAlignType.Vector2) rectPosGlobal = new RectPosGlobal(vector2, vector2);

				if (eAlignType == EAlignType.Vector3) rectPosGlobal = new RectPosGlobal(vector3, vector3);

				HelperRectTransform.SpaceRemoveHorizontalToLeft(rects, 0, rects.Length);
			}
			if (GUILayout.Button(buttonIcons[(int)EAction.SpaceRemoveHorizontalToRight], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

				RectPosGlobal rectPosGlobal = default;

				if (eAlignType == EAlignType.SelectionBorderInnter || eAlignType == EAlignType.SelectionBorderOuter) rectPosGlobal = new RectPosGlobal(selectionObjectKey as RectTransform);

				if (eAlignType == EAlignType.SelectionRect) rectPosGlobal = HelperRect.CalculateBounds(rects);

				if (eAlignType == EAlignType.Vector2) rectPosGlobal = new RectPosGlobal(vector2, vector2);

				if (eAlignType == EAlignType.Vector3) rectPosGlobal = new RectPosGlobal(vector3, vector3);

				HelperRectTransform.SpaceRemoveHorizontalToRight(rects, 0, rects.Length);
			}
			if (GUILayout.Button(buttonIcons[(int)EAction.SpaceRemoveVerticalToBot], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

				RectPosGlobal rectPosGlobal = default;

				if (eAlignType == EAlignType.SelectionBorderInnter || eAlignType == EAlignType.SelectionBorderOuter) rectPosGlobal = new RectPosGlobal(selectionObjectKey as RectTransform);

				if (eAlignType == EAlignType.SelectionRect) rectPosGlobal = HelperRect.CalculateBounds(rects);

				if (eAlignType == EAlignType.Vector2) rectPosGlobal = new RectPosGlobal(vector2, vector2);

				if (eAlignType == EAlignType.Vector3) rectPosGlobal = new RectPosGlobal(vector3, vector3);

				HelperRectTransform.SpaceRemoveVerticalToBot(rects, 0, rects.Length);
			}
			if (GUILayout.Button(buttonIcons[(int)EAction.SpaceRemoveVerticalToTop], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

				RectPosGlobal rectPosGlobal = default;

				if (eAlignType == EAlignType.SelectionBorderInnter || eAlignType == EAlignType.SelectionBorderOuter) rectPosGlobal = new RectPosGlobal(selectionObjectKey as RectTransform);

				if (eAlignType == EAlignType.SelectionRect) rectPosGlobal = HelperRect.CalculateBounds(rects);

				if (eAlignType == EAlignType.Vector2) rectPosGlobal = new RectPosGlobal(vector2, vector2);

				if (eAlignType == EAlignType.Vector3) rectPosGlobal = new RectPosGlobal(vector3, vector3);

				HelperRectTransform.SpaceRemoveVerticalToTop(rects, 0, rects.Length);
			}

			EditorGUILayout.Space();

			if (GUILayout.Button(buttonIcons[(int)EAction.PadVerticalAdd], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered).OrderBy(x => x.transform.GetSiblingIndex()).ToArray();

				if (eDirection == EDirection.Ascending)
				{
					HelperRectTransform.SpaceAddAscending(rects, Vector3.up, 0, rects.Length, spaceSize);
				}
				else
				{
					HelperRectTransform.SpaceAddAscending(rects, Vector3.down, 0, rects.Length, spaceSize);
				}
			}
			if (GUILayout.Button(buttonIcons[(int)EAction.PadVerticalSub], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered).OrderBy(x => x.transform.GetSiblingIndex()).ToArray();

				if (eDirection == EDirection.Ascending)
				{
					HelperRectTransform.SpaceSubAscending(rects, Vector3.up, 0, rects.Length, spaceSize);
				}
				else
				{
					HelperRectTransform.SpaceSubDescending(rects, Vector3.down, 0, rects.Length, spaceSize);
				}
			}
			if (GUILayout.Button(buttonIcons[(int)EAction.PadHorizontalAdd], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered).OrderBy(x => x.transform.GetSiblingIndex()).ToArray();

				if (eDirection == EDirection.Ascending)
				{
					HelperRectTransform.SpaceAddAscending(rects, Vector3.right, 0, rects.Length, spaceSize);
				}
				else
				{
					HelperRectTransform.SpaceAddAscending(rects, Vector3.left, 0, rects.Length, spaceSize);
				}
			}
			if (GUILayout.Button(buttonIcons[(int)EAction.PadHorizontalSub], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered).OrderBy(x => x.transform.GetSiblingIndex()).ToArray();

				if (eDirection == EDirection.Ascending)
				{
					HelperRectTransform.SpaceSubAscending(rects, Vector3.right, 0, rects.Length, spaceSize);
				}
				else
				{
					HelperRectTransform.SpaceSubDescending(rects, Vector3.left, 0, rects.Length, spaceSize);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button(buttonIcons[(int)EAction.HierarchySortAscendingX], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

				HelperTransforms.SetSiblingIndexByAxisXAscending(rects, 0, rects.Length);
			}

			if (GUILayout.Button(buttonIcons[(int)EAction.HierarchySortDescendingX], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

				HelperTransforms.SetSiblingIndexByAxisXDescending(rects, 0, rects.Length);
			}
			if (GUILayout.Button(buttonIcons[(int)EAction.HierarchySortAscendingY], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

				HelperTransforms.SetSiblingIndexByAxisYAscending(rects, 0, rects.Length);
			}

			if (GUILayout.Button(buttonIcons[(int)EAction.HierarchySortDescendingY], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

				HelperTransforms.SetSiblingIndexByAxisYDescending(rects, 0, rects.Length);
			}
			EditorGUILayout.EndHorizontal();
		}
		public void AlignObjects()
		{
			GUILayout.Label("Align Objects");

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button(buttonIcons[(int)EAction.AlignObjectsHorizontalLeft], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

				if (rects.Length < 1) return;

				RectPosGlobal rectPosGlobal = default;

				if (eAlignType == EAlignType.SelectionBorderInnter || eAlignType == EAlignType.SelectionBorderOuter) rectPosGlobal = new RectPosGlobal(selectionObjectKey as RectTransform);

				if (eAlignType == EAlignType.SelectionRect) rectPosGlobal = HelperRect.CalculateBounds(rects);

				if (eAlignType == EAlignType.Vector2) rectPosGlobal = new RectPosGlobal(vector2, vector2);

				if (eAlignType == EAlignType.Vector3) rectPosGlobal = new RectPosGlobal(vector3, vector3);

				if (isGrouped)
				{
					GameObject temp = new GameObject("Temp", typeof(RectTransform));

					RectTransform canvas = rects.First().GetComponentInParent<Canvas>().transform as RectTransform;

					RectTransform parentTemp = temp.transform as RectTransform;

					temp.transform.SetParent(canvas);

					var groupRect = HelperRect.CalculateBounds(rects);

					parentTemp.SetRect(in groupRect);

					parentTemp.SetPositionCentered(in groupRect);

					HierarchyPositionCache(rects);

					parentTemp.SetChilds(rects);

					if (eAlignType == EAlignType.SelectionBorderOuter)
					{
						HelperRectTransform.AlignHorizontalRight(new RectTransform[] { parentTemp }, in rectPosGlobal, true);
					}
					else
					{
						HelperRectTransform.AlignHorizontalLeft(new RectTransform[] { parentTemp }, in rectPosGlobal);
					}

					HierarchyPositionRestore();

					DestroyImmediate(temp);
				}
				else
				{
					if (eAlignType == EAlignType.SelectionBorderOuter)
					{
						HelperRectTransform.AlignHorizontalRight(rects, in rectPosGlobal, true);
					}
					else
					{
						HelperRectTransform.AlignHorizontalLeft(rects, in rectPosGlobal);
					}
				}
			}
			if (GUILayout.Button(buttonIcons[(int)EAction.AlignObjectsHorizontalMid], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

				RectPosGlobal rectPosGlobal = default;

				if (eAlignType == EAlignType.SelectionBorderInnter || eAlignType == EAlignType.SelectionBorderOuter) rectPosGlobal = new RectPosGlobal(selectionObjectKey as RectTransform);

				if (eAlignType == EAlignType.SelectionRect) rectPosGlobal = HelperRect.CalculateBounds(rects);

				if (eAlignType == EAlignType.Vector2) rectPosGlobal = new RectPosGlobal(vector2, vector2);

				if (eAlignType == EAlignType.Vector3) rectPosGlobal = new RectPosGlobal(vector3, vector3);

				if (isGrouped)
				{
					GameObject temp = new GameObject("Temp", typeof(RectTransform));

					RectTransform canvas = rects.First().GetComponentInParent<Canvas>().transform as RectTransform;

					RectTransform parentTemp = temp.transform as RectTransform;

					temp.transform.SetParent(canvas);

					var groupRect = HelperRect.CalculateBounds(rects);

					parentTemp.SetRect(in groupRect);

					parentTemp.SetPositionCentered(in groupRect);

					HierarchyPositionCache(rects);

					parentTemp.SetChilds(rects);

					if (eAlignType == EAlignType.SelectionBorderOuter)
					{
						HelperRectTransform.AlignHorizontalMid(new RectTransform[] { parentTemp }, in rectPosGlobal);
					}
					else
					{
						HelperRectTransform.AlignHorizontalMid(new RectTransform[] { parentTemp }, in rectPosGlobal);
					}

					HierarchyPositionRestore();

					DestroyImmediate(temp);
				}
				else
				{
					if (eAlignType == EAlignType.SelectionBorderOuter)
					{
						HelperRectTransform.AlignHorizontalMid(rects, in rectPosGlobal);
					}
					else
					{
						HelperRectTransform.AlignHorizontalMid(rects, in rectPosGlobal);
					}
				}
			}
			if (GUILayout.Button(buttonIcons[(int)EAction.AlignObjectsHorizontalRight], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

				RectPosGlobal rectPosGlobal = default;

				if (eAlignType == EAlignType.SelectionBorderInnter || eAlignType == EAlignType.SelectionBorderOuter) rectPosGlobal = new RectPosGlobal(selectionObjectKey as RectTransform);

				if (eAlignType == EAlignType.SelectionRect) rectPosGlobal = HelperRect.CalculateBounds(rects);

				if (eAlignType == EAlignType.Vector2) rectPosGlobal = new RectPosGlobal(vector2, vector2);

				if (eAlignType == EAlignType.Vector3) rectPosGlobal = new RectPosGlobal(vector3, vector3);

				if (isGrouped)
				{
					GameObject temp = new GameObject("Temp", typeof(RectTransform));

					RectTransform canvas = rects.First().GetComponentInParent<Canvas>().transform as RectTransform;

					RectTransform parentTemp = temp.transform as RectTransform;

					temp.transform.SetParent(canvas);

					var groupRect = HelperRect.CalculateBounds(rects);

					parentTemp.SetRect(in groupRect);

					parentTemp.SetPositionCentered(in groupRect);

					HierarchyPositionCache(rects);

					parentTemp.SetChilds(rects);

					if (eAlignType == EAlignType.SelectionBorderOuter)
					{
						HelperRectTransform.AlignHorizontalLeft(new RectTransform[] { parentTemp }, in rectPosGlobal, true);
					}
					else
					{
						HelperRectTransform.AlignHorizontalRight(new RectTransform[] { parentTemp }, in rectPosGlobal);
					}

					HierarchyPositionRestore();

					DestroyImmediate(temp);
				}
				else
				{
					if (eAlignType == EAlignType.SelectionBorderOuter)
					{
						HelperRectTransform.AlignHorizontalLeft(rects, in rectPosGlobal, true);
					}
					else
					{
						HelperRectTransform.AlignHorizontalRight(rects, in rectPosGlobal);
					}
				}
			}

			EditorGUILayout.Space();

			if (GUILayout.Button(buttonIcons[(int)EAction.AlignObjectsVerticalTop], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

				RectPosGlobal rectPosGlobal = default;

				if (eAlignType == EAlignType.SelectionBorderInnter || eAlignType == EAlignType.SelectionBorderOuter) rectPosGlobal = new RectPosGlobal(selectionObjectKey as RectTransform);

				if (eAlignType == EAlignType.SelectionRect) rectPosGlobal = HelperRect.CalculateBounds(rects);

				if (eAlignType == EAlignType.Vector2) rectPosGlobal = new RectPosGlobal(vector2, vector2);

				if (eAlignType == EAlignType.Vector3) rectPosGlobal = new RectPosGlobal(vector3, vector3);

				if (isGrouped)
				{
					GameObject temp = new GameObject("Temp", typeof(RectTransform));

					RectTransform canvas = rects.First().GetComponentInParent<Canvas>().transform as RectTransform;

					RectTransform parentTemp = temp.transform as RectTransform;

					temp.transform.SetParent(canvas);

					var groupRect = HelperRect.CalculateBounds(rects);

					parentTemp.SetRect(in groupRect);

					parentTemp.SetPositionCentered(in groupRect);

					HierarchyPositionCache(rects);

					parentTemp.SetChilds(rects);

					if (eAlignType == EAlignType.SelectionBorderOuter)
					{
						HelperRectTransform.AlignVerticalBot(new RectTransform[] { parentTemp }, in rectPosGlobal, true);
					}
					else
					{
						HelperRectTransform.AlignVerticalTop(new RectTransform[] { parentTemp }, in rectPosGlobal);
					}

					HierarchyPositionRestore();

					DestroyImmediate(temp);
				}
				else
				{
					if (eAlignType == EAlignType.SelectionBorderOuter)
					{
						HelperRectTransform.AlignVerticalBot(rects, in rectPosGlobal, true);
					}
					else
					{
						HelperRectTransform.AlignVerticalTop(rects, in rectPosGlobal);
					}
				}
			}
			if (GUILayout.Button(buttonIcons[(int)EAction.AlignObjectsVerticalMid], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

				RectPosGlobal rectPosGlobal = default;

				if (eAlignType == EAlignType.SelectionBorderInnter || eAlignType == EAlignType.SelectionBorderOuter) rectPosGlobal = new RectPosGlobal(selectionObjectKey as RectTransform);

				if (eAlignType == EAlignType.SelectionRect) rectPosGlobal = HelperRect.CalculateBounds(rects);

				if (eAlignType == EAlignType.Vector2) rectPosGlobal = new RectPosGlobal(vector2, vector2);

				if (eAlignType == EAlignType.Vector3) rectPosGlobal = new RectPosGlobal(vector3, vector3);

				if (isGrouped)
				{
					GameObject temp = new GameObject("Temp", typeof(RectTransform));

					RectTransform canvas = rects.First().GetComponentInParent<Canvas>().transform as RectTransform;

					RectTransform parentTemp = temp.transform as RectTransform;

					temp.transform.SetParent(canvas);

					var groupRect = HelperRect.CalculateBounds(rects);

					parentTemp.SetRect(in groupRect);

					parentTemp.SetPositionCentered(in groupRect);

					HierarchyPositionCache(rects);

					parentTemp.SetChilds(rects);

					if (eAlignType == EAlignType.SelectionBorderOuter)
					{
						HelperRectTransform.AlignVerticalMid(new RectTransform[] { parentTemp }, in rectPosGlobal);
					}
					else
					{
						HelperRectTransform.AlignVerticalMid(new RectTransform[] { parentTemp }, in rectPosGlobal);
					}

					HierarchyPositionRestore();

					DestroyImmediate(temp);
				}
				else
				{
					if (eAlignType == EAlignType.SelectionBorderOuter)
					{
						HelperRectTransform.AlignVerticalMid(rects, in rectPosGlobal);
					}
					else
					{
						HelperRectTransform.AlignVerticalMid(rects, in rectPosGlobal);
					}
				}
			}
			if (GUILayout.Button(buttonIcons[(int)EAction.AlignObjectsVerticalBot], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered);

				RectPosGlobal rectPosGlobal = default;

				if (eAlignType == EAlignType.SelectionBorderInnter || eAlignType == EAlignType.SelectionBorderOuter) rectPosGlobal = new RectPosGlobal(selectionObjectKey as RectTransform);

				if (eAlignType == EAlignType.SelectionRect) rectPosGlobal = HelperRect.CalculateBounds(rects);

				if (eAlignType == EAlignType.Vector2) rectPosGlobal = new RectPosGlobal(vector2, vector2);

				if (eAlignType == EAlignType.Vector3) rectPosGlobal = new RectPosGlobal(vector3, vector3);

				if (isGrouped)
				{
					GameObject temp = new GameObject("Temp", typeof(RectTransform));

					RectTransform canvas = rects.First().GetComponentInParent<Canvas>().transform as RectTransform;

					RectTransform parentTemp = temp.transform as RectTransform;

					temp.transform.SetParent(canvas);

					var groupRect = HelperRect.CalculateBounds(rects);

					parentTemp.SetRect(in groupRect);

					parentTemp.SetPositionCentered(in groupRect);

					HierarchyPositionCache(rects);

					parentTemp.SetChilds(rects);

					if (eAlignType == EAlignType.SelectionBorderOuter)
					{
						HelperRectTransform.AlignVerticalTop(new RectTransform[] { parentTemp }, in rectPosGlobal, true);
					}
					else
					{
						HelperRectTransform.AlignVerticalBot(new RectTransform[] { parentTemp }, in rectPosGlobal);
					}

					HierarchyPositionRestore();

					DestroyImmediate(temp);
				}
				else
				{
					if (eAlignType == EAlignType.SelectionBorderOuter)
					{
						HelperRectTransform.AlignVerticalTop(rects, in rectPosGlobal, true);
					}
					else
					{
						HelperRectTransform.AlignVerticalBot(rects, in rectPosGlobal);
					}
				}
			}

			EditorGUILayout.EndHorizontal();
		}
		public void DistributeObjects()
		{
			GUILayout.Label("Distribute Objects");
			EditorGUILayout.BeginHorizontal();
			// distribute horizontal left
			if (GUILayout.Button(buttonIcons[(int)EAction.DistributeObjectsHorizontalLeft], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered).OrderBy(x => x.transform.GetSiblingIndex()).ToArray();

				RectPosGlobal rectPosGlobal = default;

				if (eAlignType == EAlignType.SelectionBorderInnter || eAlignType == EAlignType.SelectionBorderOuter) rectPosGlobal = new RectPosGlobal(selectionObjectKey as RectTransform);

				if (eAlignType == EAlignType.SelectionRect) rectPosGlobal = HelperRect.CalculateBounds(rects);

				if (eAlignType == EAlignType.Vector2) rectPosGlobal = new RectPosGlobal(vector2, vector2);

				if (eAlignType == EAlignType.Vector3) rectPosGlobal = new RectPosGlobal(vector3, vector3);

				HelperRectTransform.DestributeHorizontalLeft(rects, in rectPosGlobal, isOrderByPositionNotHierarchy);

				if (HelperTransforms.IsSiblings(rects, 0, rects.Length, out Transform parent) && isOrderByPositionNotHierarchy)
				{
					HelperTransforms.SetSiblingIndexByAxisXAscending(rects, 0, rects.Length);
				}
			}
			// distribute horizontal mid
			if (GUILayout.Button(buttonIcons[(int)EAction.DistributeObjectsHorizontalMid], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered).OrderBy(x => x.transform.GetSiblingIndex()).ToArray();

				RectPosGlobal rectPosGlobal = default;

				if (eAlignType == EAlignType.SelectionBorderInnter || eAlignType == EAlignType.SelectionBorderOuter) rectPosGlobal = new RectPosGlobal(selectionObjectKey as RectTransform);

				if (eAlignType == EAlignType.SelectionRect) rectPosGlobal = HelperRect.CalculateBounds(rects);

				if (eAlignType == EAlignType.Vector2) rectPosGlobal = new RectPosGlobal(vector2, vector2);

				if (eAlignType == EAlignType.Vector3) rectPosGlobal = new RectPosGlobal(vector3, vector3);

				HelperRectTransform.DestributeHorizontalMid(rects, in rectPosGlobal, isOrderByPositionNotHierarchy);

				if (HelperTransforms.IsSiblings(rects, 0, rects.Length, out Transform parent) && isOrderByPositionNotHierarchy)
				{
					HelperTransforms.SetSiblingIndexByAxisXDescending(rects, 0, rects.Length);
				}
			}
			// distribute horizontal right
			if (GUILayout.Button(buttonIcons[(int)EAction.DistributeObjectsHorizontalRight], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered).OrderBy(x => x.transform.GetSiblingIndex()).ToArray();

				RectPosGlobal rectPosGlobal = default;

				if (eAlignType == EAlignType.SelectionBorderInnter || eAlignType == EAlignType.SelectionBorderOuter) rectPosGlobal = new RectPosGlobal(selectionObjectKey as RectTransform);

				if (eAlignType == EAlignType.SelectionRect) rectPosGlobal = HelperRect.CalculateBounds(rects);

				if (eAlignType == EAlignType.Vector2) rectPosGlobal = new RectPosGlobal(vector2, vector2);

				if (eAlignType == EAlignType.Vector3) rectPosGlobal = new RectPosGlobal(vector3, vector3);

				HelperRectTransform.DestributeHorizontalRight(rects, in rectPosGlobal, isOrderByPositionNotHierarchy);

				if (HelperTransforms.IsSiblings(rects, 0, rects.Length, out Transform parent) && isOrderByPositionNotHierarchy)
				{
					HelperTransforms.SetSiblingIndexByAxisXDescending(rects, 0, rects.Length);
				}
			}

			EditorGUILayout.Space();
			// distrubute vertical top
			if (GUILayout.Button(buttonIcons[(int)EAction.DistributeObjectsVerticalTop], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered).OrderBy(x => x.transform.GetSiblingIndex()).ToArray();

				RectPosGlobal rectPosGlobal = default;

				if (eAlignType == EAlignType.SelectionBorderInnter || eAlignType == EAlignType.SelectionBorderOuter) rectPosGlobal = new RectPosGlobal(selectionObjectKey as RectTransform);

				if (eAlignType == EAlignType.SelectionRect) rectPosGlobal = HelperRect.CalculateBounds(rects);

				if (eAlignType == EAlignType.Vector2) rectPosGlobal = new RectPosGlobal(vector2, vector2);

				if (eAlignType == EAlignType.Vector3) rectPosGlobal = new RectPosGlobal(vector3, vector3);

				HelperRectTransform.DestributeVerticalTop(rects, in rectPosGlobal, isOrderByPositionNotHierarchy);

				if (HelperTransforms.IsSiblings(rects, 0, rects.Length, out Transform parent) && isOrderByPositionNotHierarchy)
				{
					HelperTransforms.SetSiblingIndexByAxisYDescending(rects, 0, rects.Length);
				}
			}
			// distribute vertical mid
			if (GUILayout.Button(buttonIcons[(int)EAction.DistributeObjectsVerticalMid], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered).OrderBy(x => x.transform.GetSiblingIndex()).ToArray();

				RectPosGlobal rectPosGlobal = default;

				if (eAlignType == EAlignType.SelectionBorderInnter || eAlignType == EAlignType.SelectionBorderOuter) rectPosGlobal = new RectPosGlobal(selectionObjectKey as RectTransform);

				if (eAlignType == EAlignType.SelectionRect) rectPosGlobal = HelperRect.CalculateBounds(rects);

				if (eAlignType == EAlignType.Vector2) rectPosGlobal = new RectPosGlobal(vector2, vector2);

				if (eAlignType == EAlignType.Vector3) rectPosGlobal = new RectPosGlobal(vector3, vector3);

				HelperRectTransform.DestributeVerticalMid(rects, in rectPosGlobal, isOrderByPositionNotHierarchy);

				if (HelperTransforms.IsSiblings(rects, 0, rects.Length, out Transform parent) && isOrderByPositionNotHierarchy)
				{
					HelperTransforms.SetSiblingIndexByAxisYDescending(rects, 0, rects.Length);
				}
			}
			// distribute vertical bot
			if (GUILayout.Button(buttonIcons[(int)EAction.DistributeObjectsVerticalBot], GUILayout.Width(32), GUILayout.Height(32)))
			{
				RectTransform[] rects = Selection.GetFiltered<RectTransform>(SelectionMode.Unfiltered).OrderBy(x => x.transform.GetSiblingIndex()).ToArray();

				RectPosGlobal rectPosGlobal = default;

				if (eAlignType == EAlignType.SelectionBorderInnter || eAlignType == EAlignType.SelectionBorderOuter) rectPosGlobal = new RectPosGlobal(selectionObjectKey as RectTransform);

				if (eAlignType == EAlignType.SelectionRect) rectPosGlobal = HelperRect.CalculateBounds(rects);

				if (eAlignType == EAlignType.Vector2) rectPosGlobal = new RectPosGlobal(vector2, vector2);

				if (eAlignType == EAlignType.Vector3) rectPosGlobal = new RectPosGlobal(vector3, vector3);

				HelperRectTransform.DestributeVerticalBot(rects, in rectPosGlobal, isOrderByPositionNotHierarchy);

				if (HelperTransforms.IsSiblings(rects, 0, rects.Length, out Transform parent) && isOrderByPositionNotHierarchy)
				{
					HelperTransforms.SetSiblingIndexByAxisYAscending(rects, 0, rects.Length);
				}
			}

			EditorGUILayout.EndHorizontal();
		}
		private void InitIcons()
		{
			buttonIcons = new Dictionary<int, Texture>(12);
			//buttonIcons.Clear();
			string path = "Assets/Library/_UnityEditor/WindowAlign/Icons/";

			buttonIcons.Add((int)EAction.AlignObjectsHorizontalLeft, AssetDatabase.LoadAssetAtPath(path + "Align_Objects_Horizontal_Left@4x.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.AlignObjectsHorizontalMid, AssetDatabase.LoadAssetAtPath(path + "Align_Objects_Horizontal_Mid@4x.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.AlignObjectsHorizontalRight, AssetDatabase.LoadAssetAtPath(path + "Align_Objects_Horizontal_Right@4x.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.AlignObjectsVerticalBot, AssetDatabase.LoadAssetAtPath(path + "Align_Objects_Vertical_Bot@4x.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.AlignObjectsVerticalMid, AssetDatabase.LoadAssetAtPath(path + "Align_Objects_Vertical_Mid@4x.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.AlignObjectsVerticalTop, AssetDatabase.LoadAssetAtPath(path + "Align_Objects_Vertical_Top@4x.png", typeof(Texture)) as Texture);

			buttonIcons.Add((int)EAction.DistributeObjectsHorizontalLeft, AssetDatabase.LoadAssetAtPath(path + "Distribute_Objects_Horizontal_Left@4x.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.DistributeObjectsHorizontalMid, AssetDatabase.LoadAssetAtPath(path + "Distribute_Objects_Horizontal_Mid@4x.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.DistributeObjectsHorizontalRight, AssetDatabase.LoadAssetAtPath(path + "Distribute_Objects_Horizontal_Right@4x.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.DistributeObjectsVerticalBot, AssetDatabase.LoadAssetAtPath(path + "Distribute_Objects_Vertical_Bot@4x.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.DistributeObjectsVerticalMid, AssetDatabase.LoadAssetAtPath(path + "Distribute_Objects_Vertical_Mid@4x.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.DistributeObjectsVerticalTop, AssetDatabase.LoadAssetAtPath(path + "Distribute_Objects_Vertical_Top@4x.png", typeof(Texture)) as Texture);

			buttonIcons.Add((int)EAction.AnchoringFit, AssetDatabase.LoadAssetAtPath(path + "Anchoring_Fit@4x.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.DistributeSpaceHorizontal, AssetDatabase.LoadAssetAtPath(path + "Distribute_Space_Horizontal@4x.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.DistributeSpaceVertical, AssetDatabase.LoadAssetAtPath(path + "Distribute_Space_Vertical@4x.png", typeof(Texture)) as Texture);

			buttonIcons.Add((int)EAction.SpaceRemoveHorizontalToLeft, AssetDatabase.LoadAssetAtPath(path + "SpaceRemoveHorizontalToLeft@4x.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.SpaceRemoveHorizontalToRight, AssetDatabase.LoadAssetAtPath(path + "SpaceRemoveHorizontalToRight@4x.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.SpaceRemoveVerticalToBot, AssetDatabase.LoadAssetAtPath(path + "SpaceRemoveVerticalToBot@4x.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.SpaceRemoveVerticalToTop, AssetDatabase.LoadAssetAtPath(path + "SpaceRemoveVerticalToTop@4x.png", typeof(Texture)) as Texture);

			buttonIcons.Add((int)EAction.SplitHorizontalToLeft, AssetDatabase.LoadAssetAtPath(path + "SplitHorizontalToLeft@4x.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.SplitHorizontalToRight, AssetDatabase.LoadAssetAtPath(path + "SplitHorizontalToRight@4x.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.SplitVerticalToTop, AssetDatabase.LoadAssetAtPath(path + "SplitVerticalToTop@4x.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.SplitVerticalToBot, AssetDatabase.LoadAssetAtPath(path + "SplitVerticalToBot@4x.png", typeof(Texture)) as Texture);

			buttonIcons.Add((int)EAction.HierarchySortAscendingX, AssetDatabase.LoadAssetAtPath(path + "SortAscendingX.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.HierarchySortDescendingX, AssetDatabase.LoadAssetAtPath(path + "SortDescendingX.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.HierarchySortAscendingY, AssetDatabase.LoadAssetAtPath(path + "SortAscendingY.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.HierarchySortDescendingY, AssetDatabase.LoadAssetAtPath(path + "SortDescendingY.png", typeof(Texture)) as Texture);

			buttonIcons.Add((int)EAction.PadHorizontalAdd, AssetDatabase.LoadAssetAtPath(path + "PadHorizontalAdd.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.PadHorizontalSub, AssetDatabase.LoadAssetAtPath(path + "PadHorizontalSub.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.PadVerticalAdd, AssetDatabase.LoadAssetAtPath(path + "PadVerticalAdd.png", typeof(Texture)) as Texture);
			buttonIcons.Add((int)EAction.PadVerticalSub, AssetDatabase.LoadAssetAtPath(path + "PadVerticalSub.png", typeof(Texture)) as Texture);
		}
		private void HierarchyPositionCache(RectTransform[] rectTransforms)
		{
			cachedParents.Clear();
			cachedSiblingIndex.Clear();

			for (int i = 0; i < rectTransforms.Length; i++)
			{
				Transform parent = rectTransforms[i].parent;

				int siblingIndex = rectTransforms[i].GetSiblingIndex();

				cachedParents.Add(rectTransforms[i], parent);

				cachedSiblingIndex.Add(rectTransforms[i], siblingIndex);
			}
		}

		private void HierarchyPositionRestore()
		{
			foreach (var item in cachedParents)
			{
				item.Key.SetParent(item.Value);

				item.Key.SetSiblingIndex(cachedSiblingIndex[item.Key]);
			}

			cachedParents.Clear();
			cachedSiblingIndex.Clear();
		}


		private enum EDirection
		{
			Ascending,
			Descending,
			TopToBot = Ascending,
			BotToTop = Descending,
			LeftToRight = Ascending,
			RightToLeft = Descending
		}

		private enum EAlignType
		{
			SelectionBorderInnter,
			SelectionBorderOuter,
			SelectionRect,
			Vector2,
			Vector3,
			SpacingAdd,
			Spacing,
		}

		private enum EAction
		{
			AlignObjectsHorizontalLeft,
			AlignObjectsHorizontalMid,
			AlignObjectsHorizontalRight,
			AlignObjectsVerticalBot,
			AlignObjectsVerticalMid,
			AlignObjectsVerticalTop,
			DistributeObjectsHorizontalLeft,
			DistributeObjectsHorizontalMid,
			DistributeObjectsHorizontalRight,
			DistributeObjectsVerticalBot,
			DistributeObjectsVerticalMid,
			DistributeObjectsVerticalTop,
			AnchoringFit,
			DistributeSpaceHorizontal,
			DistributeSpaceVertical,
			SplitHorizontalToLeft,
			SplitHorizontalToRight,
			SplitVerticalToTop,
			SplitVerticalToBot,

			SpaceRemoveHorizontalToLeft,
			SpaceRemoveHorizontalToRight,
			SpaceRemoveVerticalToBot,
			SpaceRemoveVerticalToTop,

			PadVerticalAdd, // non
			PadVerticalSub, // non
			PadHorizontalAdd,   // non
			PadHorizontalSub, // non

			HierarchySortAscendingX,   // non
			HierarchySortDescendingX,   // non
			HierarchySortAscendingY,   // non
			HierarchySortDescendingY,   // non
		}
	}


}
#endif
