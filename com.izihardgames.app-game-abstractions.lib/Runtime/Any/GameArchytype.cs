using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using IziHardGames.Collections.Abstractions.NetStd21;
using IziHardGames.Libs.NonEngine.Applications;

namespace IziHardGames.Game.Abstractions.Lib
{
    public readonly struct Entity
    {

    }

    public class GameArchytype
    {
        private List<IGameElement> elements = new List<IGameElement>();
    }

    public interface IWorldControl
    {

    }

    public interface IGameElement
    {

    }

    public static class Worlds
    {
        internal static async Task<World> CreateAsync(WorldArchytype worldArchytype)
        {
            World world = new World();
            await worldArchytype.ExecuteAsync(world);
            return world;
        }
    }

    /// <summary>
    /// Логическое разделение игры на мир в котором свои сервисы и свои данные.
    /// Мир существует вне приложения? на случай если мир один мир будут изменять несколько процессов или даже несколько машин.
    /// Существует либо на серверной стороне либо на самодостаточном приложении
    /// </summary>
    public sealed class World
    {
        public readonly int id;
        private IVector<Entity> enteties;
        public World()
        {
            id = GetHashCode();
        }
        internal void Start()
        {
            Logging.Log($"<color=green>World Started!</color> id:{id}", this);
        }
    }

    /// <summary>
    /// Архитип (схема) мира. Определяет какими элементами будет обладать мир. 
    /// Например если текстовая игра, ей не нужна система позиционирования юнитов. А нужен текстовый плеер и переход по стадиям. 
    /// Или например открытый мир в котором есть NPC будет иметь систему создающую NPC.
    /// Также этот объект собирает пресеты для генерации мира <see cref="World"/>.
    /// По сути это объект должен содержать задачи который должны быть выполнены для создания мира. Загрузить текстуры/создать и разместить объекты. Отрендерить их и т.д.
    /// </summary>
    public class WorldArchytype
    {
        private List<WorldCreatingTask> tasks = new List<WorldCreatingTask>();
        /// <summary>
        /// Добавить задачу для последовательного асинхронного выполнения
        /// </summary>
        /// <param name="task"></param>
        public void AddTaskToAsyncQueue(Func<Task> task)
        {
            tasks.Add(new FromTask(task));
        }
        internal async Task ExecuteAsync(World world)
        {
            foreach (var item in tasks)
            {
                await ExecuteCenter.Run(item);
            }
        }
    }

    internal class FromTask : WorldCreatingTask
    {
        private Func<Task> task;
        public FromTask(Func<Task> task)
        {
            this.task = task;
        }
        internal override async Task ExecuteAsync()
        {
            await task();
        }
    }

    internal abstract class WorldCreatingTask : IExecutable
    {
        internal abstract Task ExecuteAsync();
    }

    internal interface IExecutable
    {

    }
    internal static class ExecuteCenter
    {
        internal static async Task Run(WorldCreatingTask item)
        {
            await item.ExecuteAsync();
        }
    }
}