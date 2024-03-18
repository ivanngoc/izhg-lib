using System;
using IziHardGames.UserControl.Abstractions.NetStd21.Environments;


namespace IziHardGames.UserControl.Abstractions.NetStd21
{
    public class UserActionControlAuthoring
    {
        private UserAction action;

        public UserActionControlAuthoring(UserAction action)
        {
            this.action = action;
        }
    }

    public abstract class UserAction : IUserAction
    {
        private string description;
        protected EUserActionCompletionStatus completionStatus;
        protected EUserActionInvokationFlags invokationFlags;
        public readonly UserActionActivator activator = new UserActionActivator();

        public EUserActionInvokationFlags InvokationFlags => invokationFlags;
        public EUserActionCompletionStatus CompletionStatus => completionStatus;

        public event Action? OnSucceed;
        public event Action? OnFailed;

        protected UserAction()
        {
            description = GetType().Name;
        }
        public abstract void Execute();

        public string ToStringInfo()
        {
            return $"Type:{GetType().FullName}. desc:{description}";
        }
        public void Clean()
        {
            activator.Clean();
            completionStatus = EUserActionCompletionStatus.None;
            invokationFlags = EUserActionInvokationFlags.None;
        }
        internal void SetUserEnvironment(UserEnvironmentAbstract userEnvironment)
        {
            activator.BindEnvironemnt(userEnvironment);
        }
        public void AddFlag(EUserActionInvokationFlags flag)
        {
            this.invokationFlags |= flag;
        }
        public void SetStatus(EUserActionCompletionStatus status)
        {
            this.completionStatus = status;
        }
    }

    [Flags]
    public enum EUserActionInvokationFlags : uint
    {
        /// <summary>
        ///  can't be all?
        /// </summary>
        Error = uint.MaxValue,

        None = 0,
        /// <summary>
        /// Invoked directly
        /// </summary>
        Manual = 1 << 0,
        /// <summary>
        /// Scheduled to be executed later in current loop
        /// </summary>
        Scheduled = 1 << 1,
        /// <summary>
        /// Executed were performed in special UI loop
        /// </summary>
        ExecutedInUiLoop = 1 << 2,
        /// <summary>
        /// Executed once during current frame?
        /// </summary>
        Executed = 1 << 3,
        /// <summary>
        /// Invoked by rule as binded
        /// </summary>
        CoInvokated = 1 << 4,
        /// <summary>
        /// Supre
        /// </summary>
        Supressed = 1 << 4,
    }

    public enum EUserActionCompletionStatus
    {
        None,
        /// <summary>
        /// Were Triggered and <see cref="EUserActionInvokationFlags.Executed"/>
        /// </summary>
        Succeed,
        /// <summary>
        /// Were Triggered but failed to complete
        /// </summary>
        Exception,
        /// <summary>
        /// Were triggered and completed with any status
        /// </summary>
        Finished,
    }
}
