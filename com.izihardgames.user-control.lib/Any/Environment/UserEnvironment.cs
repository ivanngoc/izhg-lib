using System;
using IziHardGames.UserControl.Abstractions.NetStd21;
using IziHardGames.UserControl.Abstractions.NetStd21.Environments;
using IziHardGames.UserControl.Lib.UserActions;

namespace IziHardGames.UserControl.Lib.Contexts
{
    public class UserEnvironmentLoop
    {

    }

    /// <summary>
    /// Auto reg in <see cref="IziEnvironment.Environments"/>
    /// </summary>
    public sealed class UserEnvironment : IziHardGames.UserControl.Abstractions.NetStd21.Environments.UserEnvironmentAbstract
    {
        public readonly ContextForUserActionsV2 userActions;
        public readonly ContextForUserModes userModes;
        public UserContextStates? userContextStates;
        public ContextForPointerNonEngine? pointer;
        public ContextForFormsNonEngine? forms;
        public UserEnvironment(User user) : base(user)
        {
            userModes = new ContextForUserModes();
            userActions = new ContextForUserActionsV2();
            userActions.Initilize(this);
            this.Modes = userModes;
        }
        [Stage(0)]
        public void CollectInfo()
        {
            base.CollectStates();
            userActions.actionGrab();
            userModes.actionGrab();
        }

        [Stage(1)]
        public void InternalCalculation()
        {
            userActions.actionInternalCalculation();
        }

        [Stage(2)]
        public void ShareInternalCalculation()
        {
            userActions.actionShareInternalCalculation();
        }

        [Stage(3)]
        public void Filter()
        {
            userActions.actionFilter();
        }

        [Stage(4)]
        public void Execute()
        {
            Refrash();
            userActions.actionExecute();
        }

        /// <summary>
        /// Выполняется в самом конце полного цикла обновления приложения когда данные уже использованы на всех стадиях конвейера и больше не нужны
        /// </summary>
        [Stage(5)]
        public void Clean()
        {
            userActions.Clean();
        }
        /// <summary>
        /// Выполняетя перед стадией выполнения <see cref="Execute"/>. Задает начальные значения по умолчанию.
        /// Необходим в тех случаях когда задаются начальные значения которые могут быть по условию изменены но к моменту каждого выполнения должны иметь
        /// </summary>
        public void Refrash()
        {

        }

        [Stage(6)]
        public void RecordHistory()
        {
            userActions.FillHistory();
        }
        [Stage(7)]
        public void Revert()
        {

        }
        public override T GetContext<T>()
        {
            throw new System.NotImplementedException();
        }
        public void BakeFromMap(UserControlMapAuthoring map)
        {
            userActions.AddActionMap(map.GetActionMap());
        }
    }
}