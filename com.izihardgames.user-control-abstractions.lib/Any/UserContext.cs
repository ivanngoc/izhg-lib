using System;

namespace IziHardGames.UserControl.Abstractions.NetStd21.Contexts
{
    /// <summary>
    /// Part of <see cref="UserEn"/>
    /// </summary>
    public abstract class UserContext : IUserContext
    {
        public readonly Action actionGrab;
        public readonly Action actionExecute;
        public readonly Action actionFilter;
        public readonly Action actionInternalCalculation;
        public readonly Action actionShareInternalCalculation;

        public UserContext()
        {
            actionGrab = Grab;
            actionExecute = Execute;
            actionFilter = Filter;
            actionInternalCalculation = InternalCalculation;
            actionShareInternalCalculation = ShareInternalCalculation;
        }

        /// <summary>
        /// <see cref="UserEnvironment.CollectInfo"/>
        /// </summary>
        protected virtual void Grab()
        {

        }
        /// <summary>
        /// В отдельной стадии обновления выполнить для всех подчиненных объектов проверку на прохождение всех условий заданных в <see cref="FilterByContext"/>
        /// во время разработки (authoring) программы.
        /// Run <see cref="FilterByContext.Execute"/>
        /// <see cref="UserEnvironment.Filter"/>
        /// </summary>
        protected virtual void Filter()
        {

        }
        protected virtual void InternalCalculation()
        {

        }
        protected virtual void ShareInternalCalculation()
        {

        }
        /// <summary>
        /// <see cref="UserEnvironment.Execute"/>
        /// </summary>
        protected virtual void Execute()
        {

        }
        /// <summary>
        /// <see cref="UserEnvironment.Clean"/>
        /// </summary>
        public virtual void Clean()
        {

        }
    }
}