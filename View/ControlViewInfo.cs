using IziHardGames.Ticking.Lib.ApplicationLevel;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IziHardGames.View
{
	[CreateAssetMenu(fileName = "ControlViewInfo", menuName = "IziHardGames/View/Control View Info")]
	public class ControlViewInfo : ScriptableObject
	{
		[NonSerialized] public List<int> ids = new List<int>(ConstantsCore.RENDERERS_CONTROL_CAPACITY);
		[NonSerialized] public List<Renderer> renderers = new List<Renderer>(ConstantsCore.RENDERERS_CONTROL_CAPACITY);
		[NonSerialized] public List<ControlOfVisibility> controlViews = new List<ControlOfVisibility>(ConstantsCore.RENDERERS_CONTROL_CAPACITY);

		private Dictionary<int, GroupeView> groupeViews = new Dictionary<int, GroupeView>();

		public Renderer this[int id]
		{
			get => renderers[id];
		}

		public void Show(Scene scene)
		{
			foreach (var item in renderers)
			{
				if (item.gameObject.scene == scene)
				{
					item.enabled = true;
				}
			}
		}
		public void Hide(Scene scene)
		{
			foreach (var item in renderers)
			{
				if (item.gameObject.scene == scene)
				{
					item.enabled = false;
				}
			}
		}
		public void GroupeHide(int idGroupe)
		{
			groupeViews[idGroupe].Hide();
		}
		public void GroupeShow(int idGroupe)
		{
			groupeViews[idGroupe].Show();
		}
		public void Regist(ControlOfVisibility controlView)
		{
			ids.Add(controlView.GetInstanceID());
			controlViews.Add(controlView);

			if (controlView is ControlViewRender)
			{
				renderers.Add((controlView as ControlViewRender).renderer);
			}
		}
		public void RegistDe(ControlOfVisibility controlView)
		{
			int index = controlViews.IndexOf(controlView);

			if (index < 0) return;
			//ids.Remove(controlView.GetInstanceID());
			//controlViews.Remove(controlView);
			//renderers.Remove(controlView.renderer); 

			ids.RemoveAt(index);
			controlViews.RemoveAt(index);

			if (controlView is ControlViewRender)
			{
				renderers.RemoveAt(index);
			}
		}
		private int GroupeAdd(ControlOfVisibility controlView)
		{
			if (groupeViews.TryGetValue(controlView.idGroupeMain, out GroupeView groupeView))
			{
				groupeView.controlViews.Add(controlView);
			}
			else
			{
				groupeViews.Add(controlView.idGroupeMain,
					new GroupeView()
					{
						idGrpupe = controlView.idGroupeMain,
						controlViews = new List<ControlOfVisibility>() { controlView },
					});
			}

			return controlView.idGroupeMain;
		}
		private int GroupeRemove(ControlOfVisibility controlView)
		{
			GroupeView groupeView = groupeViews[controlView.idGroupeMain];

			groupeView.controlViews.Remove(controlView);

			return controlView.idGroupeMain;
		}

		public void Clean()
		{
			ids.Clear();
			renderers.Clear();
			controlViews.Clear();

			groupeViews.Clear();
		}

		private class GroupeView
		{
			public int idGrpupe;
			public bool isVisible;
			public List<ControlOfVisibility> controlViews;

			public void Hide()
			{
				foreach (var item in controlViews)
				{
					item.Hide();
				}
				isVisible = false;
			}

			public void Show()
			{
				foreach (var item in controlViews)
				{
					item.Show();
				}
				isVisible = true;
			}

			public void Switch()
			{
				isVisible = !isVisible;

				if (isVisible)
				{
					Show();
				}
				else
				{
					Hide();
				}
			}
		}

		#region Test

		[ContextMenu("Test")]
		public void Test()
		{
			Debug.Log("", this);
		}
		[ContextMenu("Swtich all renders")]
		public void SwitchAllRenders()
		{
			foreach (var item in renderers)
			{
				item.enabled = !item.enabled;
			}
		}

		#endregion
	}
}