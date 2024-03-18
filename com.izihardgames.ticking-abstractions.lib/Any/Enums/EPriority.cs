namespace IziHardGames.Ticking.Abstractions.Lib
{
    /// <summary>
    /// Predefined Priorites
    /// </summary>
    public enum EPriority : byte
    {
        None = 0,
        /// <summary>
        /// Самый высший приоритет
        /// </summary>
        Input = 1,
        DataCollect,
        DataProcess,
        View,
        /// <summary>
        /// Сброс состояний которые должны быть ByDefault в начале следующего Tick(Update)
        /// </summary>
        ResetLoop = byte.MaxValue,
    }
}
