using System;
using IziHardGames.UserControl.Abstractions.NetStd21.Environments;

namespace IziHardGames.UserControl.Abstractions.NetStd21.UserMods
{
    /// <summary>
    /// Состояние пользовательской среды. Может быть в двух состояниях: активен/не активен. нужен для разрешения/запрещения пользовательских действий пока активен/неактивен
    /// </summary>
    public abstract class UserMode : IUserMode, IEnvironmentState
    {
        protected bool isActive;

        public event Action? OnEnableEvent;
        public event Action? OnDisableEvent;
        protected virtual Type Key => GetType();

        private void AddToEnvironment(UserEnvironmentAbstract userEnvironment)
        {
            EnvironmentState environmentState = new EnvironmentState(() => this.isActive);
            userEnvironment.AddState(Key, environmentState);
        }

        public virtual void Enable()
        {
            isActive = true;
            OnEnableEvent?.Invoke();
        }
        public virtual void Disable()
        {
            isActive = false;
            OnDisableEvent?.Invoke();
        }

        internal void DisableByUserAction()
        {
            throw new NotImplementedException();
        }
    }
}
