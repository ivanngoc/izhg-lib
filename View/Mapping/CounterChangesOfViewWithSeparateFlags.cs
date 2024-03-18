using IziHardGames.View;

namespace IziHardGames.Libs.Engine.View
{
	/// <summary>
	/// Компонент-маркер изменений. Если в одном раунде обнолвений один и тот же объект изменяется несколько раз и по нескольким отдельным параметрам,
	/// то каждый раз добавлять объект в список измененных затратное дело, так как каждый нужно проверять есть ли объект в списке или если в списке изменений разрешены копии,
	/// то когда будет производиться операция Distinct все равно будет производиться та же проверка на уже добавленность в списке.
	/// Для оптимизации этой задачи можно использовать счетчик изменений. Каждое изменение имееетсвой вес. При изменении вес плюсуется, при отмене - минусуется. 
	/// 0 веса означает отсутствие изменений. 
	/// Таким образом в конце каждого раунда проверяется весь список объектов, а в список изменений добавляются те, которые имеют вес больше нуля.
	/// Данный подход не запоминает операции над объектом и не сохраняет историю операций. Лишь маркирует объект как измененный или нет.
	/// Вес так что может быть и 32 битным флагом. Таким образом помечая каждое изменение (максиммум 32 разнличных изменений) отдельным флагом.
	/// </summary>
	public class CounterChangesOfViewWithSeparateFlags
	{
		public bool isChangedPosition;
		public bool isChangedRotation;
		public bool isChangedScale;
		public bool isChangedTransform;

		public IView bound;

		public int countChangesPosition;

		public void CommitChangeOfPosition()
		{
			countChangesPosition++;
			isChangedPosition = true;
		}

		#region Reset
		public void Reset()
		{
			isChangedPosition = default;
			isChangedRotation = default;
			isChangedScale = default;
			isChangedTransform = default;

			countChangesPosition = default;
		}
		#endregion
	}
}