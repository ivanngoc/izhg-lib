using System;
using UnityEngine;

namespace IziHardGames.View
{
	public class ControlViewLayer : ControlOfVisibility
	{
		[SerializeField] public int layerCached;

		#region UntyMessage
		public override void Reset()
		{
			base.Reset();

			layerCached = gameObject.layer;
		}
		#endregion
		public override void Show()
		{
			base.Show();
			if (isVisible) SetLayer(layerCached);
		}
		public override void Hide()
		{
			base.Hide();
			if (isVisible) transform.SetHierarchyLayer7();
		}

		public void SetLayer(int layer)
		{
			switch (layer)
			{
				case 0: transform.SetHierarchyLayer0(); break;
				case 1: transform.SetHierarchyLayer1(); break;
				case 2: transform.SetHierarchyLayer2(); break;
				case 3: transform.SetHierarchyLayer3(); break;
				case 4: transform.SetHierarchyLayer4(); break;
				case 5: transform.SetHierarchyLayer5(); break;
				case 6: transform.SetHierarchyLayer6(); break;
				case 7: transform.SetHierarchyLayer7(); break;
				default: throw new NotImplementedException($"Нет метода для установки слоя [{layer}]");
			}
		}
	}
}