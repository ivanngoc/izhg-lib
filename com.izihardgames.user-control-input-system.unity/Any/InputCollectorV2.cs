using IziHardGames.Attributes;
using IziHardGames.UserControl.InputSystem.ForUnity;
using IziHardGames.UserControl.Abstractions.NetStd21;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using System.Runtime.CompilerServices;

namespace IziHardGames.UserControl.InputSystem.ForUnity
{
    internal class IziInputSystemMono : IziInputSystem
    {
        public readonly InputCollectorV2 collector = new InputCollectorV2();
        private readonly DataInput dataInput;
        public IziInputSystemMono() : base()
        {
            this.iCollector = collector;
        }
        public override T GetCollectorAs<T>()
        {
            return collector as T;
        }
    }

    public class UnityPointerContainer : InputContainer
    {
        private Vector2 value;
        private Vector2 valuePrev;
        private Vector2 delta;

        internal UnityPointerContainer(int code, Device device) : base(code, device)
        {

        }
        public void Update(Vector2 value)
        {
            this.valuePrev = this.value;
            this.value = value;
            this.delta = this.value - this.valuePrev;
            data.SetChanged(delta.sqrMagnitude > 0);
        }
    }

    public class UnityInputKeyContainer : InputContainer
    {
        internal float value;
        internal float valuePrevious;
        internal Key key;
        internal UnityInputKeyContainer(int code, Device device) : base(code, device)
        {

        }
        public void UpdateValue(float value)
        {
            this.valuePrevious = this.value;
            this.value = value;
            data.SetChanged(this.value != this.valuePrevious);  // float compare with '==' is allowed. No need to use float.Epsilon
            data.SetActive(value != default);
            UpdateValueFloat(value);
        }
    }
    public static class ExtensionsForInputContainerAsUnityInputKeyContainer
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdatValueAsUnityInputKeyContainer(this InputContainer container, float value)
        {
            (container as UnityInputKeyContainer)?.UpdateValue(value);
        }
    }

    [InputCollector]
    public class InputCollectorV2 : IInputCollector
    {
        public InputCollectorV2()
        {
            Initilize();
            UnityEngine.InputSystem.InputSystem.onDeviceChange += HandleDeviceChanging;
        }
        private void Initilize()
        {
            var devices = UnityEngine.InputSystem.InputSystem.devices;
            foreach (var unityDevice in devices)
            {
                var device = RegistryForDevices.Regist(unityDevice);
                FillInputData(device, unityDevice);
            }
        }
        private void HandleDeviceChanging(InputDevice device, InputDeviceChange change)
        {
            //Debug.Log($"[{Time.frameCount}] Device changed. State:{change}; Device: prod:{device.description.product}; serial:{device.description.serial}; manufactor:{device.description.manufacturer}; interfaceName:{device.description.interfaceName};");
            switch (change)
            {
                case InputDeviceChange.Added:
                    {
                        break;
                    }
                case InputDeviceChange.Removed:
                    {
                        break;
                    }
                case InputDeviceChange.Disconnected:
                    {
                        break;
                    }

                case InputDeviceChange.Reconnected:
                    {
                        break;
                    }
                case InputDeviceChange.UsageChanged:
                    {
                        break;
                    }
                case InputDeviceChange.ConfigurationChanged:
                    {
                        break;
                    }

                case InputDeviceChange.HardReset:
                    {
                        break;
                    }

                case InputDeviceChange.SoftReset:
                    {
                        break;
                    }
                case InputDeviceChange.Enabled:
                    {
                        break;
                    }
                case InputDeviceChange.Disabled:
                    {
                        break;
                    }

                default: throw new System.NotImplementedException(change.ToString());
            }
            RegistryForDevices.RegistOrUpdateDevice(device);
        }

        public void CollectAtNormalUpdate()
        {
            // list currently connected devices
            var devices = UnityEngine.InputSystem.InputSystem.devices;

            foreach (var unityDevice in devices)
            {
                if (unityDevice is Keyboard keyboard)
                {
                    var device = RegistryForDevices.Find.ByTarget(unityDevice);

                    foreach (var task in RegistryForInputSystem.tasks.Values)
                    {
                        if (task.deviceType == typeof(Keyboard))
                        {
                            var code = task.valueAsInt;
                            var keyCode = (Key)task.valueAsInt;
                            var val = keyboard[keyCode].ReadValue();
                            // считывает неправильно. Залипает. клавиша отжата но он продолжает считывать как =1
                            //var valPrev = keyboard[keyCode].ReadValueFromPreviousFrame();

                            foreach (var pair in task.Pairs)
                            {
                                if (pair.device == device)
                                {
                                    var container = pair.container;
                                    if (container is UnityInputKeyContainer unityContainer)
                                    {
                                        unityContainer.UpdateValue(val);
                                    }
                                    else
                                    {
                                        container.UpdateValueFloat(val);
                                    }
                                    var isChanged = container.data.IsChanged;
                                    //if (isChanged) Debug.Log($"Is changed key: {keyCode}. Value:{val};");
                                }
                            }
                        }
                    }
                }
                else if (unityDevice is Touchscreen touchscreen) // Touchscreen is Pointer Also
                {

                }
                else if (unityDevice is Mouse mouse) // Mouse is Pointer Also
                {
                    ReadPointer(unityDevice, mouse);
                }
                else if (unityDevice is Pointer pointer)
                {
                    ReadPointer(unityDevice, pointer);
                }
            }
        }

        private static void ReadPointer(InputDevice unityDevice, Pointer pointer)
        {
            foreach (var item in Users.All)
            {
                var value = pointer.position.ReadValue();
                if (value != default)
                {
                    var dataInput = item.GetInputData<DataInput>();
                    dataInput.CalculatePointerGenerics(pointer);
                }
            }

            var device = RegistryForDevices.Find.ByTarget(unityDevice);

            foreach (var task in RegistryForInputSystem.tasks.Values)
            {
                if (task.deviceType == typeof(Pointer))
                {
                    var value = pointer.position.ReadValue();

                    foreach (var pair in task.Pairs)
                    {
                        if (pair.device == device)
                        {
                            var container = pair.container;
                            if (container is UnityPointerContainer unityContainer)
                            {
                                unityContainer.Update(value);
                            }
                        }
                    }
                }
            }
        }

        public void CollectAtFixedUpdate()
        {
            var devices = UnityEngine.InputSystem.InputSystem.devices;

            foreach (var device in devices)
            {

            }
        }
        public void ResetForNextLoop()
        {

        }

        private static void FillInputData(Device device, InputDevice unityDevice)
        {
            var descr = unityDevice.description;
            device.serial = descr.serial;
            device.id = unityDevice.GetType().GetHashCode();
        }
    }
}