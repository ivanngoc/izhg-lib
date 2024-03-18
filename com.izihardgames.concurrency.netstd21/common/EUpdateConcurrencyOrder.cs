namespace IziHardGames.Libs.NonEngine.Concurrency
{
	/// <summary>
	/// Cycles of each update phase. 
	/// Rules to resolve concurency.
	/// </summary>
	public enum EUpdateConcurrencyOrder
	{
		None,
		/// <summary>
		/// Search which objects to proceed. Which one is activated
		/// Stage is optional in case of dynamic changes (if objects enabled/disable)
		/// </summary>
		SearchActiveObjects,
		/// <summary>
		/// This stage will find solo (0 relations) objects to fast handle. Rest will be determind which relations they have (from count of active). 
		/// It will looks like graph between active objects each update. 
		/// Stage is optional in case of dynamic changes (if objects enabled/disable)
		/// </summary>
		ScanRelations,
		/// <summary>
		/// Pull datas
		/// </summary>
		CollectDatas,
		/// <summary>
		/// Calculate internal datas and conditions
		/// </summary>
		Calculate,
		/// <summary>
		/// Copy results from internal to external storages (caching, local copy) in case of need
		/// </summary>
		ExchangeWithInternalResults,
		/// <summary>
		/// Compare results of internal datas to each other in case of dependecies
		/// </summary>
		Compare,
		/// <summary>
		/// Based on comparisons in case of need set order for concurents (Decide order)
		/// </summary>
		Sort,
		/// <summary>
		/// Exclude concurents that are not compatible (Decide winner)
		/// </summary>
		Elimination,
		/// <summary>
		/// Perfom main handler
		/// </summary>
		Execute,
	}
}