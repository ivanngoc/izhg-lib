using System;
using UnityEngine;


namespace IziHardGames.Libs.Engine.View
{
	/// <summary>
	/// Базовый класс для наследования. Выполняет все задачи связанный с обнолвением внешнего вида объекта
	/// </summary>
	public class UpdaterView : MonoBehaviour
	{
		#region Unity Message
		public virtual void Reset()
		{

		}
		#endregion
		public void UpdateRotattion()
		{
			throw new NotImplementedException();
		}

		public void UpdatePosition()
		{
			throw new NotImplementedException();
		}
	}
}