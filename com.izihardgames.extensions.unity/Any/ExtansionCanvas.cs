namespace UnityEngine
{
	public static class ExtensionsForCanvas
	{

		public static void SetParent(this Canvas canvas, Transform container)
		{
			RectTransform rectTransform = canvas.transform as RectTransform;

			rectTransform.SetParent(container);
			rectTransform.localScale = Vector3.one;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.position = container.position;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
		}
	}

}