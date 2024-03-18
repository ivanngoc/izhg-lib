using System;
using System.Collections.Generic;
using System.Linq;
using IziHardGames.UserControl.Abstractions.NetStd21.Attributes;

namespace IziHardGames.UserControl.Abstractions.NetStd21
{
    [Registry]
    public class RegistryForPairContainerAndDevice
    {
        private readonly List<PairContainerAndDevice> pairs = new List<PairContainerAndDevice>();

        public T ForDevice<T>(int code, Device device) where T : InputContainer
        {
            throw new NotImplementedException();
        }
        internal PairContainerAndDevice Create<TContainer>(InputCollectTask task, Device device) where TContainer : InputContainer
        {
            var pair = task.CreatePair();
            TContainer cont = RegistryForInputSystem.inputContainerFactory[typeof(TContainer)].GetNew(pair.id, task.valueAsInt, device) as TContainer;
            pair.Bind(device, cont);
#if DEBUG
            if (cont == null) throw new NullReferenceException($"Factory:{RegistryForInputSystem.inputContainerFactory.GetType().FullName} Can't created container typeof:{typeof(TContainer).FullName}");
#endif
            pairs.Add(pair);
            return pair;
        }
        internal PairContainerAndDevice GetOrCreate<TContainer, TDevice>(InputCollectTask task, Device device) where TContainer : InputContainer
        {
            if (!task.TryFindPair<TContainer>(device, out var pair))
            {
                pair = Create<TContainer>(task, device);
            }
            return pair;
        }
        internal PairContainerAndDevice GetOrCreate<TContainer, TDevice>(int idTask, Device device) where TContainer : InputContainer
        {
            var task = RegistryForInputSystem.EnsureTaskCreated<TDevice>(idTask);
            return GetOrCreate<TContainer, TDevice>(task, device);
        }
    }
}
