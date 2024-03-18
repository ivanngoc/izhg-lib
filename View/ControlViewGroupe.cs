using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.View
{
	public class ControlViewGroupe : MonoBehaviour, IziHardGames.Core.IInitializable
	{
		[SerializeField] private List<ControlViewRender> controlViews = new List<ControlViewRender>();
		[SerializeField] ControlViewInfo controlViewInfo;

		#region Unity Message
#if UNITY_EDITOR

		private void Reset()
		{
			controlViewInfo = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/[Project] GameProject1/Scriptable Objects/ControlViewInfo.asset", typeof(ControlViewInfo)) as ControlViewInfo;
			this.TryGetComponentsInHierarchy<ControlViewRender>(controlViews);
		}
#endif

		#endregion

		public void Initilize()
		{
			foreach (var item in controlViews)
			{
				item.Initilize();
			}
		}
		public void RenderOn()
		{
			foreach (var item in controlViews)
			{
				item.Show();
			}
		}
		public void RenderOff()
		{
			foreach (var item in controlViews)
			{
				item.Hide();
			}
		}


	}
}