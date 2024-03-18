using System;
using IziHardGames.Attributes;
using IziHardGames.Apps.Abstractions.Lib;

namespace IziHardGames.Libs.NonEngine.Game.Abstractions
{
    /// <summary>
    /// Так - это единица итерации кадра. Внутри кадра существуют якорные точки или по другому точки синхронизации. 
    /// Порядок выполнения этих точек имеет строгую последовательность и порядок. Чаще всего эти точки предоставляются движком.
    /// </summary>
    [StaticAbstraction]
    public static class IziTicks
    {
        public static TickChannel? UI { get; set; }
        /// <summary>
        /// As Soon as possible for custom calls
        /// </summary>
        [UpdateOrder(0)] public static TickChannel? Initilization { get; set; }
        [UpdateOrder(1)] public static TickChannel? BeforePhysics { get; set; }
        [UpdateOrder(2)] public static TickChannel? Fixed { get; set; }
        [UpdateOrder(3)] public static TickChannel? Normal { get; set; }
        [UpdateOrder(4)] public static TickChannel? Late { get; set; }
        /// <summary>
        /// <see cref="IziHardGames.Ticking.Abstractions.Lib.EPriority.ResetLoop"/>
        /// </summary>
        [UpdateOrder(uint.MaxValue)] public static TickChannel? Reset { get; set; }


        public static SelectorForTickChannel? Channel;
        public static TickScheduler? scheduler;


#if DEBUG
        internal static void Examples()
        {
            Action action = () => { };
            Late.Regist(action);
            Channel["MyChannel"].Regist(action);
        }
#endif

        [UnityHotReloadEditor]
        public static void CleanupStatic()
        {
            UI = default;
            Initilization = default;
            Normal = default;
            Late = default;
            Fixed = default;
            Reset = default;

            Channel = default;
            scheduler = default;
        }
    }

    public class UpdateOrderAttribute : Attribute
    {
        public UpdateOrderAttribute(uint order)
        {

        }
    }
}
