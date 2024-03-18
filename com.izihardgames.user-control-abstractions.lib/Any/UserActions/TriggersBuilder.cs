using System;
using System.Collections.Generic;


namespace IziHardGames.UserControl.Abstractions.NetStd21.Builders
{
    public static class TriggersBuilder
    {
        public static TriggersBuilderMonada From => new TriggersBuilderMonada() { triggers = new List<TriggerSource>() };
    }

    public sealed class TriggersBuilderMonada
    {
        internal List<TriggerSource> triggers;
        public readonly List<SetOfTriggers> sets = new List<SetOfTriggers>();
        /// <summary>
        /// Когда Pointer наведен на объект который в <see cref="IUserEnvironment"/> определяется с типом T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public TriggersBuilderMonada Hover<T>()
        {
            throw new System.NotImplementedException();
        }
        public TriggersBuilderMonada KeyPress<TContainer, TDevice>(int keyCodeAsInt, Device device, InputCollectTask task) where TContainer : InputContainer
        {
            var pair = RegistryForInputSystem.Pairs.GetOrCreate<TContainer, TDevice>(task, device);

            TriggerByKeyPress triggerByKeyPress = RegistryForTriggers.GetOrCreate<TriggerByKeyPress>(x => x.keyCode == keyCodeAsInt, () =>
            {
                var trigger = new TriggerByKeyPress()
                {
                    keyCode = keyCodeAsInt,
                };
                trigger.SetPair(pair);
                trigger.SetTask(task);
                return trigger;
            });
            triggerByKeyPress.SetKey(keyCodeAsInt);
            triggers.Add(triggerByKeyPress);
            return this;
        }
        public TriggersBuilderMonada KeyPress<TContainer, TDevice>(int code, EDeviceSeelctor deviceSeelctor) where TContainer : InputContainer
        {
            var task = RegistryForInputSystem.EnsureTaskCreated<TDevice>(code);
            task.SetValue(code);

            switch (deviceSeelctor)
            {
                case EDeviceSeelctor.None:
                    break;
                case EDeviceSeelctor.Any:
                    {
                        foreach (var device in RegistryForDevices.devices)
                        {
                            if (device.target is TDevice)
                            {
                                KeyPress<TContainer, TDevice>(code, device, task);
                                CompleteSet();
                            }
                        }
                        break;
                    }
                case EDeviceSeelctor.All:
                    {
                        foreach (var device in RegistryForDevices.devices)
                        {
                            if (device.target is TDevice)
                            {
                                KeyPress<TContainer, TDevice>(code, device, task);
                            }
                        }
                        CompleteSet();
                    }
                    break;
                case EDeviceSeelctor.Specific: throw new InvalidOperationException("You must call another method that specified device");

                default:
                    break;
            }
            return this;
        }
        public TriggersBuilderMonada KeyPressOn(int code)
        {
            throw new System.NotImplementedException();
        }
        public TriggersBuilderMonada KeyPressOff(int code)
        {
            throw new System.NotImplementedException();
        }
        public TriggersBuilderMonada PointerMove<TContainer, TDevice>(EDeviceSeelctor deviceSeelctor) where TContainer : InputContainer
        {   // по умолчанию допускается только 1 устройство поинтера поэтому берем хэш код как ID
            var task = RegistryForInputSystem.EnsureTaskCreated<TDevice>(typeof(TDevice).GetHashCode());

            switch (deviceSeelctor)
            {
                case EDeviceSeelctor.None: goto default;
                case EDeviceSeelctor.Any:
                    {
                        foreach (var device in RegistryForDevices.devices)
                        {
                            PointerMove<TContainer, TDevice>(device, task);
                            CompleteSet();
                        }
                        break;
                    }
                case EDeviceSeelctor.All:
                    {
                        foreach (var device in RegistryForDevices.devices)
                        {
                            PointerMove<TContainer, TDevice>(device, task);
                        }
                        CompleteSet();
                        break;
                    }
                case EDeviceSeelctor.Specific:
                    {
                        throw new InvalidOperationException("You must call another method that specified device");
                    }
                default: throw new System.NotImplementedException();
            }

            return this;
        }
        public TriggersBuilderMonada PointerMove<TContainer, TDevice>(Device device, InputCollectTask task) where TContainer : InputContainer
        {
            var pair = RegistryForInputSystem.Pairs.GetOrCreate<TContainer, TDevice>(task, device);
            TriggerByPointerMove triggerByPointerMove = RegistryForTriggers.GetOrCreate<TriggerByPointerMove>((x) => x.pair.device == device, () =>
            {
                var trigger = new TriggerByPointerMove();
                trigger.SetTask(task);
                trigger.SetPair(pair);
                return trigger;
            });
            triggers.Add(triggerByPointerMove);
            return this;
        }
        private TriggersBuilderMonada CompleteSet()
        {
            SetOfTriggers setOfTriggers = RegistryForSetOfTriggers.Sets.GetOrCreate(ref triggers);
            if (triggers != null)
            {
                triggers.Clear();
            }
            else
            {
                triggers = new List<TriggerSource>();    // ensure because if set existed it won't take ownership of field triggers
            }
            sets.Add(setOfTriggers);
            return this;
        }
    }
}
