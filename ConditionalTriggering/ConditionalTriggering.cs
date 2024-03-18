using System;
using System.Collections.Generic;


namespace IziHardGames.Libs.NonEngine.ConditionalTriggering
{
	/// <summary>
	/// ’ранилище всех объектов каналов запуска по условию
	/// </summary>
	public class StorageConditionalTriggering
	{

	}
	/// <summary>
	/// √руппа каналов провер¤ющих услови¤
	/// </summary>
	public class ConditionalTriggeringGroupe
	{
		public List<TriggeringChannelActive> channelsByPull;
		public List<TriggeringChannelPassive> channelsByPush;
	}

	public class ConditionalTriggering
	{
		#region Unity Message

		#endregion
	}
	/// <summary>
	///  анал запуска обработчика активным опросом условий 
	/// </summary>
	public class TriggeringChannelActive
	{

	}
	/// <summary>
	///  анал д¤л запуска обработчика(ов) в пассивном режиме. «начение "заталкивает" внешний объект внутрь этого.
	/// </summary>
	public class TriggeringChannelPassive
	{

	}
	/// <summary>
	/// 
	/// </summary>
	public class Trigger
	{
		public int idTrigger;
		public int idTriggerType;
		public int idTriggerGroupe;

		/// <summary>
		/// id действи¤ при срабатывании триггера
		/// </summary>
		public int idAction;
		/// <summary>
		/// id функции дл¤ получени¤ флага <see cref="isTriggered"/>
		/// </summary>
		public int idFunction;
		/// <summary>
		/// ‘лаг определ¤ющий запуск триггера.
		/// </summary>
		public bool isTriggered;

		/// <summary>
		/// <see cref="idAction"/>
		/// </summary>
		[NonSerialized] public Action cachedAction;
		/// <summary>
		/// <see cref="idFunction"/>
		/// </summary>
		[NonSerialized] public Func<bool> cachedFunction;
	}
}