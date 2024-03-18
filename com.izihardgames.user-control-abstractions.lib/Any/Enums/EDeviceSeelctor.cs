namespace IziHardGames.UserControl.Abstractions.NetStd21
{
    public enum EDeviceSeelctor
    {
        None,
        /// <summary>
        /// Например если 2 клваиатуры то нажатие клавиши Space на любой из них будет запускать тригер от нажатия Space
        /// </summary>
        Any,
        /// <summary>
        /// На всех устройствах должен сработать триггер
        /// </summary>
        All,
        /// <summary>
        /// Конкретное утройство
        /// </summary>
        Specific,
    }
}
