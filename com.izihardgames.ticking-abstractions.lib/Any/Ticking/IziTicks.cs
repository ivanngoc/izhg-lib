using System;
using IziHardGames.Attributes;
using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.Ticking.SlotBased;
using System.Collections.Generic;

namespace IziHardGames.Libs.NonEngine.Game.Abstractions
{
    /// <summary>
    /// Так - это единица итерации кадра. Внутри кадра существуют якорные точки или по другому точки синхронизации. 
    /// Порядок выполнения этих точек имеет строгую последовательность и порядок. Чаще всего эти точки предоставляются движком.
    /// </summary>
    [StaticAbstraction]
    public static class IziTicks
    {
        public const string GROUPE_INITILIZATION = "Initilization";
        public const string GROUPE_BEFORE_PHYSICS = "BeforePhysics";
        public const string GROUPE_FIXED = "Fixed";
        public const string GROUPE_NORMAL = "Normal";
        public const string GROUPE_LATE = "Late";
        public const string GROUPE_UI = "UI";
        public const string GROUPE_USER_CONTROL = "UserControl";
        public const string GROUPE_USER_INPUT = "UserInput";
        public const string GROUPE_RESET = "Reset";

        /// <summary>
        /// As Soon as possible for custom calls
        /// </summary>
        [UpdateOrder(0)] public static TickGroupe Initilization { get => groups[GROUPE_INITILIZATION] ?? throw new NullReferenceException(); }
        [UpdateOrder(1)] public static TickGroupe BeforePhysics { get => groups[GROUPE_BEFORE_PHYSICS] ?? throw new NullReferenceException(); }
        [UpdateOrder(2)] public static TickGroupe Fixed { get => groups[GROUPE_FIXED] ?? throw new NullReferenceException(); }
        [UpdateOrder(3)] public static TickGroupe Normal { get => groups[GROUPE_NORMAL] ?? throw new NullReferenceException(); }
        [UpdateOrder(4)] public static TickGroupe UI { get => groups[GROUPE_UI] ?? throw new NullReferenceException(); }
        [UpdateOrder(5)] public static TickGroupe Late { get => groups[GROUPE_LATE] ?? throw new NullReferenceException(); }
        /// <summary>
        /// <see cref="IziHardGames.Ticking.Abstractions.Lib.EPriority.ResetLoop"/>. End Of Frame
        /// </summary>
        [UpdateOrder(uint.MaxValue)] public static TickGroupe Reset { get => groups[GROUPE_RESET] ?? throw new NullReferenceException(); }

        public static SelectorForTickChannel Channel => channel ?? throw new NullReferenceException();
        public static TickScheduler Scheduler => scheduler ?? throw new NullReferenceException();

        private static SelectorForTickChannel? channel;
        private static TickScheduler? scheduler;

        private readonly static Dictionary<string, TickGroupe> groups = new Dictionary<string, TickGroupe>();
        private readonly static Dictionary<string, TickSlot> slots = new Dictionary<string, TickSlot>();

        [UnityHotReloadEditor]
        public static void CleanupStatic()
        {
            channel = default;
            scheduler = default;
            groups.Clear();
            slots.Clear();
        }

        public static void RegistGroupe(TickGroupe grp)
        {
            throw new NotImplementedException();
        }

        public static void RegistSlot(TickSlot tickSlot)
        {
            throw new NotImplementedException();
        }
    }
}
