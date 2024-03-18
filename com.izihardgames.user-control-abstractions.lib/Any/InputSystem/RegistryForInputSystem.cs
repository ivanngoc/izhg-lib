using System;
using System.Collections.Generic;
using IziHardGames.UserControl.Abstractions.NetStd21.Attributes;

namespace IziHardGames.UserControl.Abstractions.NetStd21
{
    [Registry]
    public static class RegistryForInputSystem
    {
        public readonly static Dictionary<int, InputCollectTask> tasks = new Dictionary<int, InputCollectTask>();
        public static RegistryForPairContainerAndDevice Pairs { get; set; }
        public static readonly FactorySelector inputContainerFactory = new FactorySelector();
        public static InputCollectTask EnsureTaskCreated<TDevice>(int idTask)
        {
            if (!tasks.TryGetValue(idTask, out var task))
            {
                task = new InputCollectTask(typeof(TDevice), idTask);
                task.idTask = idTask;
                tasks.Add(idTask, task);
            }
            return task;
        }
    }

    public class FactorySelector
    {
        private Dictionary<Type, InputIContainerFactory> keyValuePairs = new Dictionary<Type, InputIContainerFactory>();
        public InputIContainerFactory this[Type type] => keyValuePairs[type];
        public void Regist<Type>(InputIContainerFactory inputIContainerFactory)
        {
            keyValuePairs.Add(typeof(Type), inputIContainerFactory);
        }
        public void Cleanup()
        {
            keyValuePairs.Clear();
        }
    }
}
