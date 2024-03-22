using System;

namespace IziHardGames.UserControl.Abstractions.NetStd21
{
    [Flags]
    public enum EUserActionOptions
    {
        /// <summary>
        /// Например при тандемной вызове. НЕ путать с активацией по количеству сработанных триггеров
        /// </summary>
        AllowMultipleExecution,
        /// <summary>
        /// Количество выполнений будет равно количеству тригеров внутри активатора <see cref="UserActionActivator"/>?
        /// </summary>
        ExecutionPerTrigger
    }

    public enum EUserActionStage
    {
        CollectInput,
        Triggering,
        CombineTriggers,

        Filtering,
        Resolve,

        BeforeExecute,
        Execute,
        AfterExecute
    }
}
