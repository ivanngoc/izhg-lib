using System;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;

namespace IziHardGames.View
{
	// [RequireComponent(typeof(Image))]
	public class ControlForVisibilityOfImage : ControlOfVisibility
	{
		// [SerializeField] public Image image;
		// [SerializeField] public RectTransform rectTransform;
		// [SerializeField] public MaskableGraphic[] maskableGraphics;

		// #region Unity Message
		// public override void Reset()
		// {
		// 	base.Reset();

		// 	image = GetComponent<Image>();

		// 	rectTransform = GetComponent<RectTransform>();

		// 	List<MaskableGraphic> list = new List<MaskableGraphic>();

		// 	transform.GetComponentsBeneath<MaskableGraphic>(list);

		// 	maskableGraphics = list.ToArray();
		// }
		// #endregion

		// [ContextMenu("Show")]
		// public override void Show()
		// {
		// 	base.Show();

		// 	image.enabled = isVisible;
		// 	maskableGraphics.ForEach(x => x.enabled = true);
		// }

		// [ContextMenu("Hide")]
		// public override void Hide()
		// {
		// 	base.Hide();

		// 	image.enabled = isVisible;
		// 	maskableGraphics.ForEach(x => x.enabled = false);
		// }
	}
}