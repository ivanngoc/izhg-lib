using IziHardGames.Core;
using System;
using UnityEngine;

namespace IziHardGames.UserControl.Unity.ScrollWithScreenEdge
{
	/// <summary>
	/// Данные о текущем дисплее. Вызов событий изменения размера экрана. Подгонка / подстройка после ресайза экрана
	/// </summary>
	public class ManagerDisplay : MonoBehaviour, IInitializable, IExcluded
	{
		[SerializeField] Vector2 screenRect;
		[SerializeField] int width;
		[SerializeField] int height;

		[SerializeField] int widthDelta;
		[SerializeField] int heightDelta;
		[SerializeField] bool isResized;


		public static event Action OnScreenResizedEvent;
		public static event Action<Vector2> OnScreenResizedDeltaEvent;
		public int Priority { get; }

		#region Unity Message
		#endregion

		public void Initilize()
		{

		}
		public void Initilize_De()
		{
			OnScreenResizedEvent?.Clean();
			OnScreenResizedDeltaEvent?.Clean();
		}

		public void ExecuteUpdate()
		{
			if (width != Screen.width)
			{
				isResized = true;
			}
			if (height != Screen.height)
			{
				isResized = true;
			}
			if (isResized)
			{
				OnScreenResizedEvent?.Invoke();

				widthDelta = width - Screen.width;

				heightDelta = height - Screen.height;

				OnScreenResizedDeltaEvent?.Invoke(new Vector2(widthDelta, heightDelta));
			}

			width = Screen.width;
			height = Screen.height;

			screenRect.x = width;
			screenRect.y = height;
		}


		public void ExecuteUpdateLate()
		{
			widthDelta = default;
			heightDelta = default;

			isResized = default;
		}
	}
}