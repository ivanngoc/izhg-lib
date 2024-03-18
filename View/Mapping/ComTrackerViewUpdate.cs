using UnityEngine;


namespace IziHardGames.Libs.Engine.View
{
	/// <summary>
	/// Содержит дельта-флаги ассоциированного представления.
	/// Обновление представления может быть по слежующим направлениеям:<br/>
	/// -обновление рендерера<br/>
	/// -обновление трансформа вью <br/>
	/// -обнолвение UI контента<br/>
	/// -обновление/триггер анимации<br/>
	/// -триггер спецэффектов<br/>
	/// </summary>
	/// <remarks>
	/// обновдение происходит методом пула или менеджер в цикле опрашивает состояния каждого объекта и если требуется делает триггер на оболвение<br/>
	/// так как многие вычислительные задачи помещают результат в массив структур и то лучше пробегая по этим массивам копировать их данные в месть запроса<br/>
	/// </remarks>
	[RequireComponent(typeof(UpdaterView))]
	public class ComTrackerViewUpdate : MonoBehaviour
	{
		/// <summary>
		/// Данные были изменены
		/// </summary>
		[SerializeField] public bool isDataModified;
		/// <summary>
		/// Требуется оюновить представление
		/// </summary>
		[SerializeField] public bool isViewUpdateNeeded;


		[SerializeField] UpdaterView updaterView;
		#region Unity Message
		public virtual void Reset()
		{
			updaterView = GetComponent<UpdaterView>();
		}
		#endregion
	}
}