namespace IziHardGames.Core
{
	/// <summary>
	/// Helps to Determine order in queue. The Less Value the Less order. (Bad English)
	/// Чем больше приоритет, тем раньше положение в очереди.
	/// Объкты с одинаковым приоритетом будут иметь разные порядковые номера и обслуживаться в порядке вставки в очередь.
	/// </summary>
	public interface IPrioritable
	{
		public int Priority { get; }
	}
}