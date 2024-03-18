using System;

namespace IziHardGames.Libs.NonEngine.Concurrency
{
	public enum EStageOfResolving
	{
		None,
		/// <summary>
		/// Decide from outside condition
		/// <see cref="ConcurrencyItem.isActivated"/>
		/// </summary>
		Activation,
		/// <summary>
		/// <see cref="ConcurrencyItem.isFiltered"/>
		/// </summary>
		Filtration,
	}
	/// <summary>
	/// Служебные данные для разрешения колизий путем выставления состояний включения/исключения и определения порядка выполнения. <br/>
	/// У одного объекта может быть только одна группа <see cref="ConcurrencyGroup"/>. Если <see cref="bind"/> участвует в нескольких группах то для каждой группы создается отдельный объект <see cref="ConcurrencyItem"/>
	/// </summary>
	public class ConcurrencyItem
	{
		/// <summary>
		/// тот кто предоставляет <see cref="actionMain"/> или же выступает от его имени (если поле target делегата не совпадает). Адресат для обратной связи
		/// </summary>
		public object bind;
		/// <summary>
		/// Item pass query at outside 
		/// </summary>
		public bool isQuered;
		/// <summary>
		/// Item filtered at outside. Записывает в поле внешний (вне сборки) метод.
		/// </summary>
		public bool isFiltered;

		/// <summary>
		/// This item is enabled and counted for contests. in case of <see langword="false"/> just skiped
		/// </summary>
		public bool isActivated;
		/// <summary>
		/// During contest beetween concurents this item was disabled (lost contest)
		/// </summary>
		public bool isEliminated;
		/// <summary>
		/// Флаг обозначающий победу в конкуренции и разрешение на выполнение
		/// </summary>
		public bool isWinConcurrency;


		/// <summary>
		/// During condition check this object were activated. This flag is determind when at later stage when objects already eliminated or activated. 
		/// If situatuion is matched then object just "pulled" after some object that gor relation with this object. Scheme sounds like "take me too" or "if you didn't go then i have to"
		/// </summary>
		public bool isPulled;
		/// <summary>
		/// Was passed filter <see cref="ConcurrencyRules.with"/> 
		/// </summary>
		public bool isWith;
		public bool isWithout;
		/// <summary>
		/// Need to perfom some calculations to get value that will be compared to decide order or elimination.
		/// </summary>
		public bool isRecalculateCondition;

		/// <summary>
		/// Вычисляемое поле. Записывается на этапе сортировки
		/// </summary>
		public int order;

		internal void FinishConcurrency()
		{
			isWinConcurrency = isActivated && !isEliminated;
		}

		/// <summary>
		/// ~Weight. Вычисляемое поле. Записывается из вне для расчета <see cref="order"/><br/>
		/// Когда объект добавляется в <see cref="ConcurrencyGroup"/> первым то это значение = 0.<br/>
		/// Если этот объект перед другим то берется <see cref="ConcurrencyItem.priority"/> другого и вычитается 1.<br/>
		/// Если идем за другим то прибавляется 1<br/>
		/// Может использоваться и другая схема расчета но по этому показателю будет идти сортировка для задания <see cref="order"/>
		/// </summary>
		public int priority;

		public ConcurrencyRules concurrencyRules;
		/// <summary>
		/// Next by <see cref="order"/> 
		/// </summary>
		private ConcurrencyItem next;
		/// <summary>
		/// previous by order
		/// </summary>
		private ConcurrencyItem previous;

		public Func<bool> funcIsActivated;
		/// <summary>
		/// In case of need call this method after <see cref="CollectDatas"/> for calculations based on collected datas.
		/// </summary>
		public Action actionLinkedHandler;

		public event Action<ConcurrencyItem> OnInternalCalculationCompleteEvent;

		/// <summary>
		/// Bake datas based on <see cref="concurrencyRules"/>
		/// </summary>
		public void ApplyRules()
		{
			priority = concurrencyRules.GetAproxPriority();
		}

		public static ConcurrencyItem Create(ConcurrencyGroup group, ConcurrencyRules rules, object target)
		{
			ConcurrencyItem concurrencyItem = new ConcurrencyItem();
			concurrencyItem.concurrencyRules = rules;
			concurrencyItem.bind = target;
			group.Add(concurrencyItem);
			return concurrencyItem;
		}

		public void After(ConcurrencyItem concurrencyItem)
		{
			throw new NotImplementedException();
		}
		public void Before(ConcurrencyItem concurrencyItem)
		{
			throw new NotImplementedException();
		}

		public void CollectDatas()
		{
			isActivated = funcIsActivated();
		}
		internal void CallLinkedHandlers()
		{
			actionLinkedHandler?.Invoke();
		}
		/// <summary>
		/// Раздать результат вычисления <see cref="CallLinkedHandlers"/>
		/// </summary>
		internal void PushCalculatedDatas()
		{
			OnInternalCalculationCompleteEvent?.Invoke(this);
		}
		public void Clean()
		{
			isQuered = default;
			isFiltered = default;
			isActivated = default;
			isEliminated = default;
			isPulled = default;
			isRecalculateCondition = default;
			isWinConcurrency = default;
		}
	}
}