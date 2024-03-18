using IziHardGames.Core;
using IziHardGames.Ticking.Lib.ApplicationLevel;
using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.View
{
	/// <summary>
	/// <see cref=""/>
	/// </summary>
	/// <remarks> 
	/// Пеерключение рендера самый быстрый способ<br/>
	/// Переключение по слоям на 2ом месте<br/>
	/// Переключение по активу гейобжекта самый неэфективный<br/>
	/// </remarks>
	[RequireComponent(typeof(Renderer))]
	public class ControlViewRender : ControlOfVisibility, IziHardGames.Core.IInitializable, IExcluded
	{
		[SerializeField] public new Renderer renderer;
		[SerializeField] List<Behaviour> behaviours;
		/// <summary>
		/// key <see cref="UnityEngine.Object.GetInstanceID"/>
		/// value this instance
		/// </summary>
		public static readonly Dictionary<int, Renderer> renderersById = new Dictionary<int, Renderer>(ConstantsCore.RENDERERS_CONTROL_CAPACITY);

		public Renderer this[int id]
		{
			get => renderersById[id];
		}

		#region Unity Message
		public override void Reset()
		{
			base.Reset();

			renderer = GetComponent<Renderer>();
		}
		#endregion

#if UNITY_EDITOR
		public override void Validate()
		{
			base.Validate();
			isVisible = renderer.enabled;
		}
#endif
		[ContextMenu("Show")]
		public override void Show()
		{
			base.Show();
			renderer.enabled = isVisible;

			foreach (var item in behaviours)
			{
				item.enabled = isVisible;
			}
		}
		[ContextMenu("Hide")]
		public override void Hide()
		{
			base.Hide();
			renderer.enabled = isVisible;

			foreach (var item in behaviours)
			{
				item.enabled = isVisible;
			}
		}
	}
}