namespace IziHardGames.UserControl.Abstractions.NetStd21.UserMods
{
    public class UserModeControlAuthoring
    {
        private readonly UserMode mode;
        public UserModeControlAuthoring(UserMode mode)
        {
            this.mode = mode;
        }

        /// <summary>
        /// Отключение режима
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void DisabledBy()
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Включение режима
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void EnabledBy()
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Отмена режима. После чего режим отключается (Неудачное завершение)
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void CanceledBy()
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Завершение режима. После чего режим завершается. (Удачное завершение)
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SuccedBy()
        {
            throw new System.NotImplementedException();
        }
        public void SuccedBy<T>() where T : UserAction
        {
            var action = GetUserAction<T>();
            action.OnSucceed += mode.DisableByUserAction;
        }

        public T GetUserAction<T>() where T : UserAction
        {
            throw new System.NotImplementedException();
        }
    }
}
