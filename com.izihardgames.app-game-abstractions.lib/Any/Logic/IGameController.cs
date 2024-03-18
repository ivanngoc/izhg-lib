using System.Threading.Tasks;
using IziHardGames.Game.Abstractions.Lib;

namespace IziHardGames.Game.Abstractions.Logics
{
    /// <summary>
    /// Конфигурация и настройка игры. Задает <see cref="IGameTrigger"/> и начальное состояние игры
    /// Компилятор игровых элементов, <see cref="IGameElement"/>
    /// </summary>
    public interface IGameConfig
    {
        public void UseGameElement<T>(T element) where T : IGameElement
        {
            throw new System.NotImplementedException();
        }
    }
    public interface IGameState
    {

    }

   
    public interface IGameConrtoller
    {
        public async ValueTask<World> StartGameAsync()
        {
            var world = await Worlds.CreateAsync(GetArchytype());
            world.Start();
            return world;
        }
        public void LoadGame()
        {

        }
        public void CreateGame()
        {

        }
        public void ContinueGame()
        {

        }
        public WorldArchytype GetArchytype();
    }

    public interface IGameProgress
    {

    }

    /// <summary>
    /// Игра хранит данные в <see cref="IGameData"/>. Когда наступает определенное событие через специальные условия - сработывает триггер. 
    /// Причиной триггера может быть все что угодно: время, действие игрока, завершение квеста, праздник, игровое событие
    /// </summary>
    public interface IGameTrigger
    {

    }

}