using System;
using IziHardGames.Game.Abstractions.Lib;
using IziHardGames.Game.Abstractions.Logics;
using IziHardGames.Apps.Abstractions.Lib;

namespace IziHardGames.Apps.Games.Lib
{
    /// <summary>
    /// Сердце игры. 
    /// Запускает начальные состояния. Формирует начальные сцены. Запускает триггеры
    /// </summary>
    public abstract class ControllerForGame : IGameConrtoller
    {
        public abstract WorldArchytype GetArchytype();

        public virtual void Run()
        {
            if (!IIziApp.IsStartupFinished) throw new InvalidOperationException($"IIziApp Startup in not finished! You must explicitly call {nameof(IStartup.FinishStartupGlobal)}");
        }

        public abstract void Itterate();
    }
}
